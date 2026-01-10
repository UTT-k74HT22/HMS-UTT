using HospitalManagement.dto.request;
using HospitalManagement.entity;

namespace HospitalManagement.view
{
    /// <summary>
    /// Dialog để cập nhật chi tiết nhân viên
    /// </summary>
    public partial class EmployeeUpdateDialog : Form
    {
        public UpdateEmployeeProfileDetailRequest? Result { get; private set; }
        public bool Updated { get; private set; }

        private readonly string _code;
        private readonly string _currentFullName;
        private readonly string _currentPhone;
        private readonly string _currentEmail;
        private readonly string _currentAddress;
        private readonly string _currentPosition;
        private readonly string _currentDepartment;
        private readonly DateTime _currentHiredDate;
        private readonly decimal _currentSalary;
        private readonly ProfileStatus _currentStatus;

        public EmployeeUpdateDialog(
            string code,
            string fullName,
            string phone,
            string email,
            string address,
            string position,
            string department,
            DateTime hiredDate,
            decimal salary,
            ProfileStatus status)
        {
            _code = code;
            _currentFullName = fullName;
            _currentPhone = phone;
            _currentEmail = email;
            _currentAddress = address;
            _currentPosition = position;
            _currentDepartment = department;
            _currentHiredDate = hiredDate;
            _currentSalary = salary;
            _currentStatus = status;

            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            Text = $"Cập nhật nhân viên: {_code}";

            // Set current values
            txtFullName.Text = _currentFullName;
            txtPhone.Text = _currentPhone;
            txtEmail.Text = _currentEmail;
            txtAddress.Text = _currentAddress;
            txtPosition.Text = _currentPosition;
            txtDepartment.Text = _currentDepartment;
            dtpHiredDate.Value = _currentHiredDate;
            numSalary.Value = _currentSalary;

            // Initialize status combobox
            cboStatus.Items.Clear();
            cboStatus.Items.Add(new ComboBoxItem { Text = "Hoạt động", Value = ProfileStatus.ACTIVE });
            cboStatus.Items.Add(new ComboBoxItem { Text = "Không hoạt động", Value = ProfileStatus.INACTIVE });
            cboStatus.Items.Add(new ComboBoxItem { Text = "Tạm ngưng", Value = ProfileStatus.SUSPENDED });
            cboStatus.DisplayMember = "Text";
            cboStatus.ValueMember = "Value";

            // Set current status
            for (int i = 0; i < cboStatus.Items.Count; i++)
            {
                var item = (ComboBoxItem)cboStatus.Items[i];
                if ((ProfileStatus)item.Value == _currentStatus)
                {
                    cboStatus.SelectedIndex = i;
                    break;
                }
            }

            // Events
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (_, _) => { Updated = false; Close(); };
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFullName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPosition.Text))
                {
                    MessageBox.Show("Vui lòng nhập chức vụ", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPosition.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDepartment.Text))
                {
                    MessageBox.Show("Vui lòng nhập phòng ban", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartment.Focus();
                    return;
                }

                if (dtpHiredDate.Value > DateTime.Now)
                {
                    MessageBox.Show("Ngày vào làm không thể trong tương lai", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpHiredDate.Focus();
                    return;
                }

                if (numSalary.Value < 0)
                {
                    MessageBox.Show("Lương phải lớn hơn hoặc bằng 0", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numSalary.Focus();
                    return;
                }

                if (cboStatus.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedStatus = (ProfileStatus)((ComboBoxItem)cboStatus.SelectedItem).Value;

                // Build result
                Result = new UpdateEmployeeProfileDetailRequest
                {
                    FullName = txtFullName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Position = txtPosition.Text.Trim(),
                    Department = txtDepartment.Text.Trim(),
                    HiredDate = dtpHiredDate.Value,
                    Salary = numSalary.Value,
                    Status = selectedStatus
                };

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
            var lblCode = new Label 
            { 
                Text = $"Mã NV: {_code}", 
                Location = new Point(20, 15), 
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblFullName = new Label { Text = "Họ tên:", Location = new Point(20, 50), Width = 120 };
            txtFullName = new TextBox { Location = new Point(140, 47), Width = 300 };

            var lblPhone = new Label { Text = "Số điện thoại:", Location = new Point(20, 80), Width = 120 };
            txtPhone = new TextBox { Location = new Point(140, 77), Width = 300 };

            var lblEmail = new Label { Text = "Email:", Location = new Point(20, 110), Width = 120 };
            txtEmail = new TextBox { Location = new Point(140, 107), Width = 300 };

            var lblAddress = new Label { Text = "Địa chỉ:", Location = new Point(20, 140), Width = 120 };
            txtAddress = new TextBox { Location = new Point(140, 137), Width = 300, Height = 50, Multiline = true };

            var lblPosition = new Label { Text = "Chức vụ:", Location = new Point(20, 200), Width = 120 };
            txtPosition = new TextBox { Location = new Point(140, 197), Width = 300 };

            var lblDepartment = new Label { Text = "Phòng ban:", Location = new Point(20, 230), Width = 120 };
            txtDepartment = new TextBox { Location = new Point(140, 227), Width = 300 };

            var lblHiredDate = new Label { Text = "Ngày vào làm:", Location = new Point(20, 260), Width = 120 };
            dtpHiredDate = new DateTimePicker { Location = new Point(140, 257), Width = 300, Format = DateTimePickerFormat.Short };

            var lblSalary = new Label { Text = "Lương (VNĐ):", Location = new Point(20, 290), Width = 120 };
            numSalary = new NumericUpDown 
            { 
                Location = new Point(140, 287), 
                Width = 300,
                Maximum = 999999999,
                Minimum = 0,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };

            var lblStatus = new Label { Text = "Trạng thái:", Location = new Point(20, 320), Width = 120 };
            cboStatus = new ComboBox 
            { 
                Location = new Point(140, 317), 
                Width = 300, 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };

            btnSave = new Button { Text = "Cập nhật", Location = new Point(140, 360), Width = 100, Height = 35 };
            btnCancel = new Button { Text = "Hủy", Location = new Point(250, 360), Width = 100, Height = 35 };

            Controls.AddRange(new Control[] {
                lblCode,
                lblFullName, txtFullName,
                lblPhone, txtPhone,
                lblEmail, txtEmail,
                lblAddress, txtAddress,
                lblPosition, txtPosition,
                lblDepartment, txtDepartment,
                lblHiredDate, dtpHiredDate,
                lblSalary, numSalary,
                lblStatus, cboStatus,
                btnSave, btnCancel
            });

            ClientSize = new Size(470, 420);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private TextBox txtFullName = null!;
        private TextBox txtPhone = null!;
        private TextBox txtEmail = null!;
        private TextBox txtAddress = null!;
        private TextBox txtPosition = null!;
        private TextBox txtDepartment = null!;
        private DateTimePicker dtpHiredDate = null!;
        private NumericUpDown numSalary = null!;
        private ComboBox cboStatus = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
    }
}
