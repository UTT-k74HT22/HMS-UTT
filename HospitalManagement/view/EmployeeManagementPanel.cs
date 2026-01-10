using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.dto.response;

namespace HospitalManagement.view
{
    public partial class EmployeeManagementPanel : UserControl
    {
        private readonly EmployeeController _controller;
        private readonly BindingSource _bs = new();
        private List<EmployeeProfileResponse> _all = new();

        public EmployeeManagementPanel(EmployeeController controller)
        {
            _controller = controller;
            
            InitializeComponent();

            dgvEmployees.DataSource = _bs;

            InitGrid();
            InitEvents();

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                _all = _controller.GetAllEmployees();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitGrid()
        {
            dgvEmployees.AutoGenerateColumns = false;
            dgvEmployees.AllowUserToResizeColumns = false;
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.RowHeadersVisible = false;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.MultiSelect = false;
            dgvEmployees.ReadOnly = true;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvEmployees.Columns.Clear();

            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                Width = 60,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EmployeeProfileResponse.Id),
                DataPropertyName = nameof(EmployeeProfileResponse.Id),
                HeaderText = "ID",
                FillWeight = 15
            });

            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EmployeeProfileResponse.Code),
                DataPropertyName = nameof(EmployeeProfileResponse.Code),
                HeaderText = "Mã NV",
                FillWeight = 18
            });

            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EmployeeProfileResponse.FullName),
                DataPropertyName = nameof(EmployeeProfileResponse.FullName),
                HeaderText = "Họ tên",
                FillWeight = 35
            });

            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EmployeeProfileResponse.Phone),
                DataPropertyName = nameof(EmployeeProfileResponse.Phone),
                HeaderText = "SĐT",
                FillWeight = 20
            });

            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(EmployeeProfileResponse.Status),
                DataPropertyName = nameof(EmployeeProfileResponse.Status),
                HeaderText = "Trạng thái",
                FillWeight = 12
            });

            dgvEmployees.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0) return;

                if (dgvEmployees.Columns[e.ColumnIndex].Name == "STT")
                {
                    e.Value = (e.RowIndex + 1).ToString().ToString();
                    e.FormattingApplied = true;
                }
            };
        }

        private void InitEvents()
        {
            dgvEmployees.DataError += (s, e) =>
            {
                var colName = dgvEmployees.Columns[e.ColumnIndex].Name;
                MessageBox.Show(
                    $"DataError ở cột: {colName}\nRow: {e.RowIndex}\n{e.Exception.Message}",
                    "Grid Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                e.ThrowException = false;
                e.Cancel = true; 
            };


            btnSearch.Click += (_, _) => ApplyFilters();
            btnRefresh.Click += (_, _) => { txtKeyword.Clear(); LoadData(); };

            btnDetail.Click += (_, _) => ShowDetail();
            btnEdit.Click += (_, _) => UpdateSelected();
            btnDelete.Click += (_, _) => DeleteSelected();
            btnExport.Click += (_, _) => ExportToExcel();

            txtKeyword.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    ApplyFilters();
                }
            };
        }
        
        private void ApplyFilters()
        {
            var kw = (txtKeyword.Text ?? "").Trim().ToLower();

            var filtered = _all.Where(x =>
                    string.IsNullOrEmpty(kw)
                    || x.Id.ToString().Contains(kw)
                    || x.Code.ToLower().Contains(kw)
                    || x.FullName.ToLower().Contains(kw)
                    || x.Phone.ToLower().Contains(kw)
                )
                .ToList();

            _bs.DataSource = filtered;
            lblTotal.Text = $"Tổng số: {filtered.Count}";
        }

        private EmployeeProfileResponse? GetSelected()
            => dgvEmployees.CurrentRow?.DataBoundItem as EmployeeProfileResponse;

        private void ShowDetail()
        {
            var e = GetSelected();
            if (e == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var detail = _controller.GetEmployeeByCode(e.Code);
                MessageBox.Show(
                    $"Mã: {detail.Code}\n" +
                    $"Họ tên: {detail.FullName}\n" +
                    $"SĐT: {detail.Phone}\n" +
                    $"Email: {detail.Email}\n" +
                    $"Chức vụ: {detail.Position}\n" +
                    $"Phòng ban: {detail.Department}\n" +
                    $"Ngày vào: {detail.HiredDate:yyyy-MM-dd}\n" +
                    $"Lương: {detail.Salary:N0} VNĐ\n" +
                    $"Trạng thái: {detail.Status}",
                    "Chi tiết nhân viên",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSelected()
        {
            var e = GetSelected();
            if (e == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Console.WriteLine($"[UI] UpdateEmployee: Loading detail for code={e.Code}");
                
                // Lấy chi tiết đầy đủ của nhân viên
                var detail = _controller.GetEmployeeByCode(e.Code);
                
                Console.WriteLine($"[UI] UpdateEmployee: Opening dialog for employee={detail.Code}");
                var dialog = new EmployeeUpdateDialog(
                    detail.Code,
                    detail.FullName,
                    detail.Phone ?? "",
                    detail.Email ?? "",
                    detail.Address ?? "",
                    detail.Position,
                    detail.Department,
                    detail.HiredDate,
                    detail.Salary,
                    detail.Status
                );
                
                dialog.ShowDialog();

                if (dialog.Updated && dialog.Result != null)
                {
                    Console.WriteLine($"[UI] UpdateEmployee: Updating employee code={e.Code}");
                    _controller.UpdateEmployeeDetail(e.Code, dialog.Result);
                    Console.WriteLine("[UI] UpdateEmployee: Success!");
                    
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    Console.WriteLine("[UI] UpdateEmployee: Cancelled");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UI] UpdateEmployee: ERROR - {ex}");
                MessageBox.Show($"Lỗi: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteSelected()
        {
            var e = GetSelected();
            if (e == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa nhân viên [{e.Code}] ?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // TODO: Implement delete trong controller
                    MessageBox.Show("Chức năng xóa nhân viên đang được phát triển", 
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportToExcel()
        {
            try
            {
                Console.WriteLine("[UI] ExportToExcel: Starting export...");

                var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Export danh sách nhân viên",
                    FileName = $"NhanVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine($"[UI] ExportToExcel: Exporting to {saveDialog.FileName}");

                    // Lấy dữ liệu chi tiết đầy đủ
                    var details = _controller.GetAllProfileDetails();
                    Console.WriteLine($"[UI] ExportToExcel: Got {details.Count} records");

                    using (var workbook = new ClosedXML.Excel.XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Nhân viên");

                        // Headers
                        worksheet.Cell(1, 1).Value = "STT";
                        worksheet.Cell(1, 2).Value = "Mã NV";
                        worksheet.Cell(1, 3).Value = "Họ tên";
                        worksheet.Cell(1, 4).Value = "SĐT";
                        worksheet.Cell(1, 5).Value = "Email";
                        worksheet.Cell(1, 6).Value = "Địa chỉ";
                        worksheet.Cell(1, 7).Value = "Chức vụ";
                        worksheet.Cell(1, 8).Value = "Phòng ban";
                        worksheet.Cell(1, 9).Value = "Ngày vào";
                        worksheet.Cell(1, 10).Value = "Lương";
                        worksheet.Cell(1, 11).Value = "Trạng thái";

                        // Style header
                        var headerRange = worksheet.Range("A1:K1");
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightBlue;
                        headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                        // Data
                        int row = 2;
                        foreach (var emp in details)
                        {
                            worksheet.Cell(row, 1).Value = row - 1;
                            worksheet.Cell(row, 2).Value = emp.Code;
                            worksheet.Cell(row, 3).Value = emp.FullName;
                            worksheet.Cell(row, 4).Value = emp.Phone ?? "";
                            worksheet.Cell(row, 5).Value = emp.Email ?? "";
                            worksheet.Cell(row, 6).Value = emp.Address ?? "";
                            worksheet.Cell(row, 7).Value = emp.Position;
                            worksheet.Cell(row, 8).Value = emp.Department;
                            worksheet.Cell(row, 9).Value = emp.HiredDate.ToString("yyyy-MM-dd");
                            worksheet.Cell(row, 10).Value = emp.Salary;
                            worksheet.Cell(row, 11).Value = emp.Status.ToString();
                            row++;
                        }

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        workbook.SaveAs(saveDialog.FileName);
                    }

                    Console.WriteLine("[UI] ExportToExcel: Success!");
                    MessageBox.Show($"Export thành công!\n\nFile: {saveDialog.FileName}", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Console.WriteLine("[UI] ExportToExcel: Cancelled");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UI] ExportToExcel: ERROR - {ex}");
                MessageBox.Show($"Lỗi khi export: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
