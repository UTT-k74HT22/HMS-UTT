using System;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.entity.dto;

namespace HospitalManagement.view.Auth
{
    public partial class LoginForm : Form
    {
        private readonly AuthController _controller;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnGoRegister;

        public LoginForm(AuthController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtUsername = new TextBox() { Left = 100, Top = 30, Width = 200 };
            txtPassword = new TextBox() { Left = 100, Top = 70, Width = 200, PasswordChar = '*' };

            btnLogin = new Button() { Text = "Login", Left = 100, Top = 110, Width = 200 };
            btnGoRegister = new Button() { Text = "Register", Left = 100, Top = 150, Width = 200 };

            btnLogin.Click += BtnLogin_Click;
            btnGoRegister.Click += BtnGoRegister_Click;

            Controls.Add(new Label() { Text = "Username", Left = 20, Top = 30 });
            Controls.Add(new Label() { Text = "Password", Left = 20, Top = 70 });

            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnGoRegister);

            Text = "Login";
            Size = new System.Drawing.Size(350, 250);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                var request = new LoginRequest(txtUsername.Text, txtPassword.Text);
                var result = _controller.Login(request);

                if (result)
                {
                    Console.WriteLine("? LOGIN SUCCESS");
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? LOGIN FAILED: {ex.Message}");
                MessageBox.Show($"Login failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGoRegister_Click(object? sender, EventArgs e)
        {
            this.Hide();
            new RegisterForm(_controller).Show();
        }
    }
}
