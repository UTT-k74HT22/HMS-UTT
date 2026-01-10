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
            var dialog = new AccountFormDialog();
            if (dialog.ShowDialog() == DialogResult.OK && dialog.Result != null)
            {
                try
                {
                    _controller.CreateAccount(dialog.Result);
                    MessageBox.Show("Tạo tài khoản thành công!", "Success", 
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

        private void UpdateAccount()
        {
            var account = GetSelected();
            if (account == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TODO: Implement update dialog
            MessageBox.Show($"Chức năng update tài khoản [{account.Username}] đang được phát triển", 
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
