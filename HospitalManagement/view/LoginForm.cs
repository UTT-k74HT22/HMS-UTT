﻿using HospitalManagement.entity;
using HospitalManagement.service;
using HospitalManagement.controller;
using HospitalManagement.entity.enums;
using HospitalManagement.router;
using HospitalManagement.service.impl;

namespace HospitalManagement.view;
public partial class LoginForm : Form
{
    private IAuthService? _authService;
        private AccountController? _accountController;
        private EmployeeController? _employeeController;

        // Constructor cho Designer
        public LoginForm()
        {
            InitializeComponent();
            this.ActiveControl = tbUsername;
        }

        // Constructor runtime (DI)
        public LoginForm(
            IAuthService authService,
            AccountController accountController,
            EmployeeController employeeController
        ) : this()
        {
            _authService = authService;
            _accountController = accountController;
            _employeeController = employeeController;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            if (_authService == null)
            {
                ShowError("AuthService chưa được khởi tạo");
                return;
            }

            string username = tbUsername.Text.Trim();
            string password = tbPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Vui lòng nhập tên đăng nhập");
                tbUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Vui lòng nhập mật khẩu");
                tbPassword.Focus();
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Đang đăng nhập...";

            try
            {
                Account account = _authService.authenticate(username, password);
                
                long userId = AuthServiceImpl.GetCurrentUserProfileId()
                              ?? throw new Exception("Không lấy được user profile");
                
                AuthContextManager.SetUser(
                    userId,
                    account.Role.ToString(),
                    account.Username
                );
                var mainFrame = new MainFrame(
                    account.Username,
                    account.Role.ToString(),
                    _accountController,
                    _employeeController
                );

                mainFrame.FormClosed += (_, _) => Application.Exit();
                mainFrame.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
            }
        }



        private void tbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnLogin.PerformClick();
            }
        }

        private void ShowError(string error)
        {
            lblError.Text = error;
            lblError.Visible = true;
        }
}

