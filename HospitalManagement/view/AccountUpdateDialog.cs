using HospitalManagement.entity.enums;

namespace HospitalManagement.view
{
    /// <summary>
    /// Dialog để cập nhật role và trạng thái active của tài khoản
    /// </summary>
    public partial class AccountUpdateDialog : Form
    {
        public RoleType SelectedRole { get; private set; }
        public bool IsActive { get; private set; }
        public bool Updated { get; private set; }

        private readonly string _username;
        private readonly RoleType _currentRole;
        private readonly bool _currentActive;

        public AccountUpdateDialog(string username, RoleType currentRole, bool currentActive)
        {
            _username = username;
            _currentRole = currentRole;
            _currentActive = currentActive;

            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            Text = $"Cập nhật tài khoản: {_username}";

            // Initialize role combobox
            cboRole.Items.Clear();
            cboRole.Items.Add(new ComboBoxItem { Text = "Admin", Value = RoleType.ADMIN });
            cboRole.Items.Add(new ComboBoxItem { Text = "Nhân viên", Value = RoleType.EMPLOYEE });
            cboRole.Items.Add(new ComboBoxItem { Text = "Khách hàng", Value = RoleType.CUSTOMER });
            cboRole.DisplayMember = "Text";
            cboRole.ValueMember = "Value";

            // Set current values
            for (int i = 0; i < cboRole.Items.Count; i++)
            {
                var item = (ComboBoxItem)cboRole.Items[i];
                if ((RoleType)item.Value == _currentRole)
                {
                    cboRole.SelectedIndex = i;
                    break;
                }
            }

            chkActive.Checked = _currentActive;

            // Events
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (_, _) => { Updated = false; Close(); };
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cboRole.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn vai trò", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedItem = (ComboBoxItem)cboRole.SelectedItem;
                SelectedRole = (RoleType)selectedItem.Value;
                IsActive = chkActive.Checked;

                // Warning nếu đang deactivate account
                if (_currentActive && !IsActive)
                {
                    var result = MessageBox.Show(
                        "Bạn có chắc chắn muốn vô hiệu hóa tài khoản này?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                // Warning nếu thay đổi role
                if (_currentRole != SelectedRole)
                {
                    var result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn thay đổi vai trò từ {_currentRole} sang {SelectedRole}?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                Updated = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class ComboBoxItem
        {
            public string Text { get; set; } = "";
            public object Value { get; set; } = null!;
        }

        private void InitializeComponent()
        {
            var lblUsername = new Label 
            { 
                Text = $"Tài khoản: {_username}", 
                Location = new Point(20, 20), 
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblRole = new Label 
            { 
                Text = "Vai trò:", 
                Location = new Point(20, 60), 
                AutoSize = true 
            };
            
            cboRole = new ComboBox 
            { 
                Location = new Point(140, 57), 
                Width = 250, 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };

            chkActive = new CheckBox 
            { 
                Text = "Tài khoản hoạt động", 
                Location = new Point(140, 100), 
                AutoSize = true 
            };

            btnSave = new Button 
            { 
                Text = "Cập nhật", 
                Location = new Point(140, 140), 
                Width = 100,
                Height = 35
            };
            
            btnCancel = new Button 
            { 
                Text = "Hủy", 
                Location = new Point(250, 140), 
                Width = 100,
                Height = 35
            };

            Controls.AddRange(new Control[] {
                lblUsername,
                lblRole, cboRole,
                chkActive,
                btnSave, btnCancel
            });

            ClientSize = new Size(430, 200);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private ComboBox cboRole = null!;
        private CheckBox chkActive = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
    }
}
