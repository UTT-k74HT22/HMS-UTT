using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.entity;
using HospitalManagement.view.@base;

namespace HospitalManagement.view
{
    /// <summary>
    /// Ví dụ cụ thể về cách sử dụng BaseManagementPanel
    /// Panel quản lý danh sách nhân viên với đầy đủ tính năng:
    /// - Search/Filter
    /// - CRUD operations
    /// - Export Excel
    /// </summary>
    public class EmployeeManagementPanel : BaseManagementPanel<EmployeeProfile>
    {
        // ========== Dependencies ==========
        // TODO: Inject controller/service khi đã có
        // private readonly EmployeeController _controller;

        // ========== Filter Controls ==========
        private TextBox _searchBox = null!;
        private ComboBox _departmentFilter = null!;

        // ========== Constructor ==========
        public EmployeeManagementPanel()
        {
            // TODO: Inject dependencies
            // this._controller = controller ?? throw new ArgumentNullException(nameof(controller));
            
            // Load data ngay khi khởi tạo
            Reload();
        }

        // ========== Implement Abstract Methods ==========

        protected override string TitleTotal()
        {
            return "Tổng số nhân viên";
        }

        protected override (string PropertyName, string HeaderText, int Width)[] GetColumns()
        {
            return new[]
            {
                ("Id", "ID", 60),
                ("ProfileId", "Profile ID", 100),
                ("Position", "Chức vụ", 150),
                ("Department", "Phòng ban", 150),
                ("HiredDate", "Ngày vào làm", 120),
                ("BaseSalary", "Lương cơ bản", 130)
            };
        }

        protected override List<EmployeeProfile> FetchData()
        {
            // TODO: Replace with actual service call
            // return _controller.GetAllEmployees();
            
            // Mock data for demonstration
            return GenerateMockData();
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
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            // Search box
            layout.Controls.Add(UiFactory.CreateLabel("Từ khóa:"));
            _searchBox = UiFactory.CreateTextField(250);
            layout.Controls.Add(_searchBox);

            // Search button
            var searchBtn = UiFactory.CreateButton("🔍 Tìm kiếm", UiTheme.PRIMARY, (s, e) => ApplyFilters());
            layout.Controls.Add(searchBtn);

            // Spacer
            layout.Controls.Add(new Panel { Width = 20, BackColor = Color.Transparent });

            // Department filter
            layout.Controls.Add(UiFactory.CreateLabel("Phòng ban:"));
            _departmentFilter = UiFactory.CreateComboBox(
                new[] { "Tất cả", "Kế toán", "Kinh doanh", "Kỹ thuật", "Nhân sự" },
                150
            );
            _departmentFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            layout.Controls.Add(_departmentFilter);



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
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            // Các action buttons
            layout.Controls.Add(UiFactory.CreateButton("➕ Thêm mới", UiTheme.SUCCESS, OnAdd));
            layout.Controls.Add(UiFactory.CreateButton("👁 Xem chi tiết", UiTheme.INFO, OnViewDetail));
            layout.Controls.Add(UiFactory.CreateButton("✏️ Sửa", UiTheme.WARNING, OnEdit));
            layout.Controls.Add(UiFactory.CreateButton("🗑️ Xóa", UiTheme.DANGER, OnDelete));
            
            // Spacer
            layout.Controls.Add(new Panel { Width = 20, BackColor = Color.Transparent });
            
            layout.Controls.Add(UiFactory.CreateButton("🔄 Refresh", UiTheme.SECONDARY, (s, e) => Reload()));
            layout.Controls.Add(UiFactory.CreateButton("📄 Export Excel", UiTheme.PURPLE, OnExportExcel));

            panel.Controls.Add(layout);
            return panel;
        }

        protected override void AfterTableCreated()
        {
            // Custom format cho các cột đặc biệt
            
            // Format cột Salary thành currency
            if (Table.Columns.Contains("BaseSalary"))
            {
                Table.Columns["BaseSalary"]!.DefaultCellStyle.Format = "N0";
                Table.Columns["BaseSalary"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Format cột Date
            if (Table.Columns.Contains("HiredDate"))
            {
                Table.Columns["HiredDate"]!.DefaultCellStyle.Format = "dd/MM/yyyy";
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

            // Filter by search text
            if (!string.IsNullOrWhiteSpace(_searchBox?.Text))
            {
                var keyword = _searchBox.Text.Trim();
                filters.Add($"(Position LIKE '%{keyword}%' OR Department LIKE '%{keyword}%')");
            }

            // Filter by department
            if (_departmentFilter?.SelectedIndex > 0)
            {
                var dept = _departmentFilter.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(dept))
                    filters.Add($"Department = '{dept}'");
            }

            // Combine filters
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
            // TODO: Open Add Employee Dialog
            MessageBox.Show("Chức năng Thêm nhân viên\n\nTODO: Implement dialog thêm mới",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // After save successfully:
            // Reload();
        }

        private void OnViewDetail(object? sender, EventArgs e)
        {
            var selected = GetSelectedItem();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Open Detail Dialog
            var info = $"ID: {selected.Id}\n" +
                      $"Profile ID: {selected.ProfileId}\n" +
                      $"Chức vụ: {selected.Position}\n" +
                      $"Phòng ban: {selected.Department}\n" +
                      $"Ngày vào làm: {selected.HiredDate:dd/MM/yyyy}\n" +
                      $"Lương: {selected.BaseSalary:N0} VNĐ";

            MessageBox.Show(info, "Chi tiết nhân viên",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnEdit(object? sender, EventArgs e)
        {
            var selected = GetSelectedItem();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Open Edit Dialog
            MessageBox.Show($"Chức năng Sửa nhân viên ID: {selected.Id}\n\nTODO: Implement dialog sửa",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // After save successfully:
            // Reload();
        }

        private void OnDelete(object? sender, EventArgs e)
        {
            var selected = GetSelectedItem();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nhân viên:\n{selected.Position} - {selected.Department}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // TODO: Call delete service
                // _controller.DeleteEmployee(selected.Id);
                
                MessageBox.Show("Xóa thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                Reload();
            }
        }

        private void OnExportExcel(object? sender, EventArgs e)
        {
            // TODO: Implement Excel export
            MessageBox.Show("Chức năng Export Excel\n\nTODO: Implement export",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ========== Mock Data for Demo ==========

        private List<EmployeeProfile> GenerateMockData()
        {
            var random = new Random();
            var positions = new[] { "Nhân viên", "Trưởng phòng", "Phó phòng", "Giám đốc", "Kỹ sư" };
            var departments = new[] { "Kế toán", "Kinh doanh", "Kỹ thuật", "Nhân sự" };
            var statuses = new[] { "Đang làm", "Đã nghỉ" };

            var employees = new List<EmployeeProfile>();

            for (int i = 1; i <= 50; i++)
            {
                employees.Add(new EmployeeProfile
                {
                    ProfileId = 1000 + i,
                    Position = positions[random.Next(positions.Length)],
                    Department = departments[random.Next(departments.Length)],
                    HiredDate = DateTime.Now.AddDays(-random.Next(1, 3650)),
                    BaseSalary = random.Next(8, 50) * 1000000,
                    CreatedAt = DateTime.Now.AddDays(-random.Next(1, 365)),
                    UpdatedAt = DateTime.Now
                });
            }

            return employees;
        }
    }
}
