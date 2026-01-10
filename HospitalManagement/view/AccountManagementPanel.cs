using HospitalManagement.controller;
using HospitalManagement.dto.response;
using HospitalManagement.entity.enums;

namespace HospitalManagement.view
{
    public partial class AccountManagementPanel : UserControl
    {
        private readonly AccountController _controller;

        private readonly BindingSource _bs = new();
        private List<AccountResponse> _all = new();

        public AccountManagementPanel(AccountController controller)
        {
            _controller = controller;
            
            InitializeComponent();

            dgvAccounts.DataSource = _bs;

            InitGrid();
            InitEvents();

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                _all = _controller.GetAllAccounts();
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
            dgvAccounts.AutoGenerateColumns = false;
            dgvAccounts.AllowUserToResizeColumns = false;
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.MultiSelect = false;
            dgvAccounts.ReadOnly = true;
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvAccounts.Columns.Clear();

            // STT (unbound)
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                Width = 60,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            // ID
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(AccountResponse.Id),
                DataPropertyName = nameof(AccountResponse.Id),
                HeaderText = "ID",
                FillWeight = 18
            });

            // Username
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(AccountResponse.Username),
                DataPropertyName = nameof(AccountResponse.Username),
                HeaderText = "Username",
                FillWeight = 34
            });

            // Role
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(AccountResponse.Role),
                DataPropertyName = nameof(AccountResponse.Role),
                HeaderText = "Role",
                FillWeight = 24
            });

            // Active
            var activeCol = new DataGridViewCheckBoxColumn
            {
                Name = nameof(AccountResponse.Active),
                DataPropertyName = nameof(AccountResponse.Active),
                HeaderText = "Active",
                FillWeight = 16
            };
            dgvAccounts.Columns.Add(activeCol);

            dgvAccounts.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0) return;

                // STT
                if (dgvAccounts.Columns[e.ColumnIndex].Name == "STT")
                {
                    e.Value = (e.RowIndex + 1).ToString().ToString();
                    e.FormattingApplied = true;
                }
            };
        }

        private void InitEvents()
        {
            btnSearch.Click += (_, _) => ApplyFilters();
            btnRefresh.Click += (_, _) => { txtKeyword.Clear(); LoadData(); };

            btnAdd.Click += (_, _) => CreateAccount();
            btnEdit.Click += (_, _) => UpdateAccount();
            btnDelete.Click += (_, _) => DeleteAccount();
            btnDetail.Click += (_, _) => ShowDetail();
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

        private void CreateAccount()
        {
            Console.WriteLine("[UI] CreateAccount: Opening dialog...");
            var dialog = new AccountFormDialog();
            if (dialog.ShowDialog() == DialogResult.OK && dialog.Result != null)
            {
                try
                {
                    Console.WriteLine($"[UI] CreateAccount: Calling controller with Username={dialog.Result.Username}, Role={dialog.Result.Role}");
                    _controller.CreateAccount(dialog.Result);
                    Console.WriteLine("[UI] CreateAccount: Success!");
                    MessageBox.Show("Tạo tài khoản thành công!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UI] CreateAccount: ERROR - {ex}");
                    MessageBox.Show($"Lỗi: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Console.WriteLine("[UI] CreateAccount: Dialog cancelled or no result");
            }
        }

        private void UpdateAccount()
        {
            var account = GetSelected();
            if (account == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Console.WriteLine($"[UI] UpdateAccount: Opening dialog for account={account.Username}");
            var dialog = new AccountUpdateDialog(account.Username, account.Role, account.Active);
            dialog.ShowDialog();

            if (dialog.Updated)
            {
                try
                {
                    Console.WriteLine($"[UI] UpdateAccount: Updating account ID={account.Id}, Role={dialog.SelectedRole}, Active={dialog.IsActive}");
                    _controller.UpdateAccount(account.Id, dialog.SelectedRole, dialog.IsActive);
                    Console.WriteLine("[UI] UpdateAccount: Success!");
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UI] UpdateAccount: ERROR - {ex}");
                    MessageBox.Show($"Lỗi: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Console.WriteLine("[UI] UpdateAccount: Cancelled");
            }
        }

        private void DeleteAccount()
        {
            var account = GetSelected();
            if (account == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xác nhận xóa tài khoản [{account.Username}]?", 
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _controller.DeleteAccount(account.Id);
                    MessageBox.Show("Xóa tài khoản thành công!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowDetail()
        {
            var account = GetSelected();
            if (account == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(
                $"ID: {account.Id}\n" +
                $"Username: {account.Username}\n" +
                $"Role: {account.Role}\n" +
                $"Active: {account.Active}\n" +
                $"Last Login: {account.LastLoginAt?.ToString("yyyy-MM-dd HH:mm") ?? "Chưa đăng nhập"}",
                "Chi tiết tài khoản",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void ApplyFilters()
        {
            var kw = (txtKeyword.Text ?? "").Trim().ToLower();

            var filtered = _all.Where(x =>
                    string.IsNullOrEmpty(kw)
                    || x.Username.ToLower().Contains(kw)
                    || x.Role.ToString().Contains(kw)
                    || x.Id.ToString().Contains(kw)
                )
                .ToList();

            _bs.DataSource = filtered;
            lblTotal.Text = $"Tổng số: {filtered.Count}";
        }

        private AccountResponse? GetSelected()
            => dgvAccounts.CurrentRow?.DataBoundItem as AccountResponse;

        private void ExportToExcel()
        {
            try
            {
                Console.WriteLine("[UI] ExportToExcel: Starting export...");

                var saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Export danh sách tài khoản",
                    FileName = $"Accounts_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine($"[UI] ExportToExcel: Exporting to {saveDialog.FileName}");

                    using (var workbook = new ClosedXML.Excel.XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Tài khoản");

                        // Headers
                        worksheet.Cell(1, 1).Value = "STT";
                        worksheet.Cell(1, 2).Value = "ID";
                        worksheet.Cell(1, 3).Value = "Username";
                        worksheet.Cell(1, 4).Value = "Role";
                        worksheet.Cell(1, 5).Value = "Active";
                        worksheet.Cell(1, 6).Value = "Last Login";

                        // Style header
                        var headerRange = worksheet.Range("A1:F1");
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightBlue;
                        headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;

                        // Data
                        int row = 2;
                        foreach (var acc in _all)
                        {
                            worksheet.Cell(row, 1).Value = row - 1;
                            worksheet.Cell(row, 2).Value = acc.Id;
                            worksheet.Cell(row, 3).Value = acc.Username;
                            worksheet.Cell(row, 4).Value = acc.Role.ToString();
                            worksheet.Cell(row, 5).Value = acc.Active ? "Có" : "Không";
                            worksheet.Cell(row, 6).Value = acc.LastLoginAt?.ToString("yyyy-MM-dd HH:mm") ?? "Chưa đăng nhập";
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
