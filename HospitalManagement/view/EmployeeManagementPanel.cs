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
                e.Cancel = true; // ✅ chặn dialog mặc định
            };


            btnSearch.Click += (_, _) => ApplyFilters();
            btnRefresh.Click += (_, _) => { txtKeyword.Clear(); LoadData(); };

            btnDetail.Click += (_, _) => ShowDetail();
            btnEdit.Click += (_, _) => UpdateSelected();
            btnDelete.Click += (_, _) => DeleteSelected();

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

            // TODO: mở dialog update thật
            MessageBox.Show($"Chức năng update nhân viên [{e.Code}] đang được phát triển", 
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
