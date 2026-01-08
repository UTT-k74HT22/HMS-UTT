using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.entity;
using HospitalManagement.view.@base;

namespace HospitalManagement.view
{
    /// <summary>
    /// Panel quản lý tài khoản (Account Management)
    /// Extends từ BaseManagementPanel để có sẵn table, filters, actions
    /// Tính năng: CRUD accounts, search/filter theo role và status
    /// </summary>
    public class AccountManagementPanel : BaseManagementPanel<Account>
    {
        // ========== Dependencies ==========
        private readonly AccountController _accountController;

        // ========== Filter Controls ==========
        private TextBox _searchBox = null!;
        private ComboBox _roleFilter = null!;
        private ComboBox _statusFilter = null!;

        // ========== Constructor ==========
        public AccountManagementPanel(AccountController accountController)
        {
            this._accountController = accountController;
            
            Reload();
        }

        // ========== Implement Abstract Methods ==========

        protected override string TitleTotal()
        {
            return "Tổng số tài khoản";
        }

        protected override (string PropertyName, string HeaderText, int Width)[] GetColumns()
        {
            return new[]
            {
                ("Id", "ID", 60),
                ("Username", "Tài khoản", 150),
                ("Role", "Vai trò", 120),
                ("IsActive", "Trạng thái", 100),
                ("LastLoginAt", "Đăng nhập cuối", 150),
                ("CreatedAt", "Ngày tạo", 130)
            };
        }

        protected override List<Account> FetchData()
        {
            // Designer mode - return empty list
            if (_accountController == null)
            {
                return new List<Account>();
            }
            
            return _accountController.GetAccounts();
        }

        // ========== Override Optional Hooks ==========

        protected override Panel BuildFilters()
        {
            var panel = UiFactory.CreateTransparentPanel();
            panel.AutoSize = true;

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            // Search box
            layout.Controls.Add(UiFactory.CreateLabel("Tìm kiếm:"));
            _searchBox = UiFactory.CreateTextField(250);
            _searchBox.PlaceholderText = "Nhập username...";
            layout.Controls.Add(_searchBox);

            // Search button
            var searchBtn = UiFactory.CreateButton("🔍 Tìm kiếm", UiTheme.PRIMARY, (s, e) => ApplyFilters());
            layout.Controls.Add(searchBtn);

            // Spacer
            layout.Controls.Add(new Panel { Width = 20, BackColor = Color.Transparent });

            // Role filter
            layout.Controls.Add(UiFactory.CreateLabel("Vai trò:"));
            _roleFilter = UiFactory.CreateComboBox(
                new[] { "Tất cả", "ADMIN", "EMPLOYEE", "CUSTOMER" },
                130
            );
            _roleFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            layout.Controls.Add(_roleFilter);

            // Status filter
            layout.Controls.Add(UiFactory.CreateLabel("Trạng thái:"));
            _statusFilter = UiFactory.CreateComboBox(
                new[] { "Tất cả", "Hoạt động", "Khóa" },
                120
            );
            _statusFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            layout.Controls.Add(_statusFilter);

            panel.Controls.Add(layout);
            return panel;
        }

        protected override Panel BuildActions()
        {
            var panel = UiFactory.CreateTransparentPanel();
            panel.AutoSize = true;

            var layout = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            // Utility buttons (right side)
            layout.Controls.Add(UiFactory.CreateButton("📄 Export", UiTheme.PURPLE, OnExportExcel));
            layout.Controls.Add(UiFactory.CreateButton("🔄 Làm mới", UiTheme.SECONDARY, (s, e) => Reload()));

            // Spacer
            layout.Controls.Add(new Panel { Width = 12, Height = 1, BackColor = Color.Transparent });

            // CRUD buttons
            layout.Controls.Add(UiFactory.CreateButton("🗑️ Xóa", UiTheme.DANGER, OnDelete));
            layout.Controls.Add(UiFactory.CreateButton("🔒 Khóa/Mở", UiTheme.ORANGE, OnToggleStatus));
            layout.Controls.Add(UiFactory.CreateButton("✏️ Sửa", UiTheme.WARNING, OnEdit));
            layout.Controls.Add(UiFactory.CreateButton("👁 Xem", UiTheme.INFO, OnViewDetail));
            layout.Controls.Add(UiFactory.CreateButton("➕ Thêm", UiTheme.SUCCESS, OnAdd));

            panel.Controls.Add(layout);
            return panel;
        }

        protected override void AfterTableCreated()
        {
            // Format cột IsActive thành "Hoạt động"/"Khóa"
            if (Table.Columns.Contains("IsActive"))
            {
                Table.Columns["IsActive"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                // Custom cell formatting
                Table.CellFormatting += (s, e) =>
                {
                    if (e.ColumnIndex == Table.Columns["IsActive"]!.Index && e.Value != null)
                    {
                        var isActive = (bool)e.Value;
                        e.Value = isActive ? "✓ Hoạt động" : "✗ Khóa";
                        e.CellStyle.ForeColor = isActive ? UiTheme.SUCCESS : UiTheme.DANGER;
                        e.FormattingApplied = true;
                    }
                };
            }

            // Format cột LastLoginAt
            if (Table.Columns.Contains("LastLoginAt"))
            {
                Table.Columns["LastLoginAt"]!.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                Table.Columns["LastLoginAt"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                // Handle null values
                Table.CellFormatting += (s, e) =>
                {
                    if (e.ColumnIndex == Table.Columns["LastLoginAt"]!.Index && e.Value == null)
                    {
                        e.Value = "Chưa đăng nhập";
                        e.CellStyle.ForeColor = Color.Gray;
                        e.FormattingApplied = true;
                    }
                };
            }

            // Format cột CreatedAt
            if (Table.Columns.Contains("CreatedAt"))
            {
                Table.Columns["CreatedAt"]!.DefaultCellStyle.Format = "dd/MM/yyyy";
                Table.Columns["CreatedAt"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Format cột Role với màu sắc
            if (Table.Columns.Contains("Role"))
            {
                Table.CellFormatting += (s, e) =>
                {
                    if (e.ColumnIndex == Table.Columns["Role"]!.Index && e.Value != null)
                    {
                        var role = e.Value.ToString();
                        e.CellStyle.ForeColor = role switch
                        {
                            "ADMIN" => UiTheme.DANGER,
                            "EMPLOYEE" => UiTheme.INFO,
                            "CUSTOMER" => UiTheme.SUCCESS,
                            _ => UiTheme.TEXT
                        };
                    }
                };
            }

            // Double click để xem chi tiết
            Table.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    OnViewDetail(s, e);
                }
            };
        }

        protected override void ApplyFilters()
        {
            var filters = new List<string>();

            // Filter by username
            if (!string.IsNullOrWhiteSpace(_searchBox?.Text))
            {
                var keyword = _searchBox.Text.Trim();
                filters.Add($"Username LIKE '%{keyword}%'");
            }

            // Filter by role
            if (_roleFilter?.SelectedIndex > 0)
            {
                var role = _roleFilter.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(role))
                    filters.Add($"Role = '{role}'");
            }

            // Filter by status
            if (_statusFilter?.SelectedIndex > 0)
            {
                var statusText = _statusFilter.SelectedItem?.ToString();
                var isActive = statusText == "Hoạt động";
                filters.Add($"IsActive = {isActive}");
            }

            // Apply combined filters
            if (filters.Any())
            {
                ApplyTextFilter(string.Join(" AND ", filters));
            }
            else
            {
                ClearFilter();
            }
        }

        // ========== Event Handlers ==========

        private void OnAdd(object? sender, EventArgs e)
        {
            // TODO: Open Add Account Dialog
            var message = "Chức năng Thêm tài khoản\n\n" +
                         "Dialog sẽ bao gồm:\n" +
                         "- Username (unique)\n" +
                         "- Password\n" +
                         "- Role (ADMIN/EMPLOYEE/CUSTOMER)\n" +
                         "- IsActive\n\n" +
                         "TODO: Implement AddAccountDialog";
            
            MessageBox.Show(message, "Thêm tài khoản",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // After save: Reload();
        }

        private void OnViewDetail(object? sender, EventArgs e)
        {
            var selected = GetSelectedItem();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var info = $"═══════════════════════════════\n" +
                      $"CHI TIẾT TÀI KHOẢN\n" +
                      $"═══════════════════════════════\n\n" +
                      $"ID: {selected.Id}\n" +
                      $"Username: {selected.Username}\n" +
                      $"Vai trò: {selected.Role}\n" +
                      $"Trạng thái: {(selected.IsActive ? "✓ Hoạt động" : "✗ Khóa")}\n" +
                      $"Đăng nhập cuối: {(selected.LastLoginAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa đăng nhập")}\n" +
                      $"Ngày tạo: {selected.CreatedAt:dd/MM/yyyy HH:mm}\n" +
                      $"Cập nhật: {selected.UpdatedAt:dd/MM/yyyy HH:mm}";

            MessageBox.Show(info, "Chi tiết tài khoản",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnEdit(object? sender, EventArgs e)
        {
            var selected = GetSelectedItem();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Open Edit Dialog
            MessageBox.Show($"Chức năng Sửa tài khoản\n\nUsername: {selected.Username}\nTODO: Implement EditAccountDialog",
                "Sửa tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // After save: Reload();
        }

        private void OnToggleStatus(object? sender, EventArgs e)
        {
            var selected = GetSelectedItem();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var action = selected.IsActive ? "khóa" : "mở khóa";
            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn {action} tài khoản:\n{selected.Username}?",
                $"Xác nhận {action}",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // TODO: Call service to toggle status
                // _controller.ToggleAccountStatus(selected.Id);
                
                MessageBox.Show($"Đã {action} tài khoản thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                Reload();
            }
        }

        private void OnDelete(object? sender, EventArgs e)
        {
            var selected = GetSelectedItem();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prevent deleting ADMIN accounts
            if (selected.Role == "ADMIN")
            {
                MessageBox.Show("Không thể xóa tài khoản ADMIN!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn XÓA tài khoản:\n{selected.Username}?\n\n⚠️ Hành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // TODO: Call delete service
                // _controller.DeleteAccount(selected.Id);
                
                MessageBox.Show("Xóa tài khoản thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                Reload();
            }
        }

        private void OnExportExcel(object? sender, EventArgs e)
        {
            // TODO: Implement Excel export
            MessageBox.Show("Chức năng Export Excel\n\nTODO: Implement export accounts to Excel",
                "Export Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
