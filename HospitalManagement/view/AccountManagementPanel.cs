using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using HospitalManagement.view.@base;

namespace HospitalManagement.view
{
    /// <summary>
    /// Panel quản lý tài khoản (Account Management)
    /// Extends từ BaseManagementPanel để có sẵn table, filters, actions
    /// Tính năng: CRUD accounts, search/filter theo role và status
    /// </summary>
    public class AccountManagementPanel : BaseManagementPanel<AccountResponse>
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
                ("Id", "ID", 70),
                ("Username", "Tài khoản", 180),
                ("Role", "Vai trò", 130),
                ("IsActive", "Trạng thái", 130),
                ("LastLoginAt", "Đăng nhập cuối", 170),
                ("CreatedAt", "Ngày tạo", 140)
            };
        }

        protected override List<AccountResponse> FetchData()
        {
            // Designer mode - return empty list
            if (_accountController == null)
            {
                return new List<AccountResponse>();
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
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,

                Padding = new Padding(0),
                Margin = new Padding(0)
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
            layout.Controls.Add(new Panel { Width = 10, BackColor = Color.Transparent });

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
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,

                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            // CRUD buttons (left side)
            layout.Controls.Add(UiFactory.CreateButton("➕ Thêm", UiTheme.SUCCESS, OnAdd));
            layout.Controls.Add(UiFactory.CreateButton("✏️ Sửa", UiTheme.WARNING, OnEdit));
            layout.Controls.Add(UiFactory.CreateButton("🗑️ Xóa", UiTheme.DANGER, OnDelete));
            layout.Controls.Add(UiFactory.CreateButton("🔒 Khóa/Mở", UiTheme.ORANGE, OnToggleStatus));

            // Spacer
            layout.Controls.Add(new Panel { Width = 20, Height = 1, BackColor = Color.Transparent });

            // Utility buttons (right side)
            layout.Controls.Add(UiFactory.CreateButton("🔄 Làm mới", UiTheme.SECONDARY, (s, e) => Reload()));
            layout.Controls.Add(UiFactory.CreateButton("📄 Export", UiTheme.PURPLE, OnExportExcel));

            panel.Controls.Add(layout);
            return panel;
        }

        protected override void AfterTableCreated()
        {
            // Căn giữa cho các cột ID, Role, IsActive, LastLoginAt, CreatedAt
            if (Table.Columns.Contains("Id"))
                Table.Columns["Id"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            if (Table.Columns.Contains("Role"))
                Table.Columns["Role"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            if (Table.Columns.Contains("IsActive"))
                Table.Columns["IsActive"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            if (Table.Columns.Contains("LastLoginAt"))
                Table.Columns["LastLoginAt"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            if (Table.Columns.Contains("CreatedAt"))
                Table.Columns["CreatedAt"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            // Format cột IsActive thành "Hoạt động"/"Khóa"
            if (Table.Columns.Contains("IsActive"))
            {
                
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
            if (selected.Role == RoleType.ADMIN)
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
