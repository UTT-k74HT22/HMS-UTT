using HospitalManagement.dto.request;
using HospitalManagement.entity.enums;

namespace HospitalManagement.view
{
    public partial class AccountFormDialog : Form
    {
        public CreateAccountRequest? Result { get; private set; }

        private readonly long? _editAccountId;
        private readonly bool _isEditMode;

        public AccountFormDialog(long? accountId = null)
        {
            InitializeComponent();
            _editAccountId = accountId;
            _isEditMode = accountId.HasValue;

            InitForm();
            
            if (_isEditMode)
            {
                Text = "Cập nhật tài khoản";
                LoadAccountData();
            }
            else
            {
                Text = "Tạo tài khoản mới";
            }
        }

        private void InitForm()
        {
            // Initialize role combobox
            cboRole.Items.Clear();
            cboRole.Items.Add(new ComboBoxItem { Text = "Nhân viên", Value = RoleType.EMPLOYEE });
            cboRole.Items.Add(new ComboBoxItem { Text = "Khách hàng", Value = RoleType.CUSTOMER });
            cboRole.DisplayMember = "Text";
            cboRole.ValueMember = "Value";
            cboRole.SelectedIndex = 0;

            chkActive.Checked = true;

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        }

        private void LoadAccountData()
        {
            // TODO: Load account data when edit mode
            // For now, just disable username field
            txtUsername.ReadOnly = true;
            txtPassword.Enabled = false;
            txtConfirmPassword.Enabled = false;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Vui lòng nhập username", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (!_isEditMode)
                {
                    if (string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        MessageBox.Show("Vui lòng nhập password", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.Focus();
                        return;
                    }

                    if (txtPassword.Text != txtConfirmPassword.Text)
                    {
                        MessageBox.Show("Password và Confirm Password không khớp", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtConfirmPassword.Focus();
                        return;
                    }

                    if (txtPassword.Text.Length < 6)
                    {
                        MessageBox.Show("Password phải có ít nhất 6 ký tự", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.Focus();
                        return;
                    }
                }

                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFullName.Focus();
                    return;
                }

                // Build result
                Result = new CreateAccountRequest
                {
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text,
                    ConfirmPassword = txtConfirmPassword.Text,
                    Role = (RoleType)((ComboBoxItem)cboRole.SelectedItem).Value,
                    Active = chkActive.Checked,
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class ComboBoxItem
        {
            public string Text { get; set; } = "";
            public object Value { get; set; } = null!;
        }

        private void InitializeComponent()
        {
            var lblUsername = new Label { Text = "Username:", Location = new Point(20, 20), AutoSize = true };
            txtUsername = new TextBox { Location = new Point(140, 17), Width = 300 };

            var lblPassword = new Label { Text = "Password:", Location = new Point(20, 50), AutoSize = true };
            txtPassword = new TextBox { Location = new Point(140, 47), Width = 300, PasswordChar = '*' };

            var lblConfirmPassword = new Label { Text = "Confirm Password:", Location = new Point(20, 80), AutoSize = true };
            txtConfirmPassword = new TextBox { Location = new Point(140, 77), Width = 300, PasswordChar = '*' };

            var lblFullName = new Label { Text = "Họ tên:", Location = new Point(20, 110), AutoSize = true };
            txtFullName = new TextBox { Location = new Point(140, 107), Width = 300 };

            var lblEmail = new Label { Text = "Email:", Location = new Point(20, 140), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(140, 137), Width = 300 };

            var lblPhone = new Label { Text = "SĐT:", Location = new Point(20, 170), AutoSize = true };
            txtPhone = new TextBox { Location = new Point(140, 167), Width = 300 };

            var lblAddress = new Label { Text = "Địa chỉ:", Location = new Point(20, 200), AutoSize = true };
            txtAddress = new TextBox { Location = new Point(140, 197), Width = 300, Height = 60, Multiline = true };

            var lblRole = new Label { Text = "Vai trò:", Location = new Point(20, 270), AutoSize = true };
            cboRole = new ComboBox { Location = new Point(140, 267), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };

            chkActive = new CheckBox { Text = "Kích hoạt", Location = new Point(140, 300), AutoSize = true };

            btnSave = new Button { Text = "Lưu", Location = new Point(140, 330), Width = 100 };
            btnCancel = new Button { Text = "Hủy", Location = new Point(250, 330), Width = 100 };

            Controls.AddRange(new Control[] {
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                lblConfirmPassword, txtConfirmPassword,
                lblFullName, txtFullName,
                lblEmail, txtEmail,
                lblPhone, txtPhone,
                lblAddress, txtAddress,
                lblRole, cboRole,
                chkActive,
                btnSave, btnCancel
            });

            ClientSize = new Size(470, 380);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmPassword = null!;
        private TextBox txtFullName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPhone = null!;
        private TextBox txtAddress = null!;
        private ComboBox cboRole = null!;
        private CheckBox chkActive = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
    }
}
