using System;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.entity.dto;

namespace HospitalManagement.view.Auth
{
    public partial class RegisterForm : Form
    {
        private readonly AuthController _controller;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtFullname;
        private Button btnRegister;
        private Button btnBackLogin;

        public RegisterForm(AuthController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtUsername = new TextBox() { Left = 100, Top = 30, Width = 200 };
            txtPassword = new TextBox() { Left = 100, Top = 70, Width = 200, PasswordChar = '*' };
            txtFullname = new TextBox() { Left = 100, Top = 110, Width = 200 };

            btnRegister = new Button() { Text = "Register", Left = 100, Top = 160, Width = 200 };
            btnBackLogin = new Button() { Text = "Back to Login", Left = 100, Top = 200, Width = 200 };

            btnRegister.Click += BtnRegister_Click;
            btnBackLogin.Click += BtnBackLogin_Click;

            Controls.Add(new Label() { Text = "Username", Left = 20, Top = 30 });
            Controls.Add(new Label() { Text = "Password", Left = 20, Top = 70 });
            Controls.Add(new Label() { Text = "Fullname", Left = 20, Top = 110 });

            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(txtFullname);
            Controls.Add(btnRegister);
            Controls.Add(btnBackLogin);

            Text = "Register";
            Size = new System.Drawing.Size(370, 300);
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            try
            {
                var request = new RegisterRequest(
                    txtUsername.Text,
                    txtPassword.Text,
                    txtFullname.Text
                );

                _controller.Register(request);
                
                Console.WriteLine("? REGISTER SUCCESS");
                MessageBox.Show("Register successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.Hide();
                new LoginForm(_controller).Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? REGISTER FAILED: {ex.Message}");
                MessageBox.Show($"Register failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBackLogin_Click(object? sender, EventArgs e)
        {
            this.Hide();
            new LoginForm(_controller).Show();
        }
    }
}
