﻿using HospitalManagement.entity;
using HospitalManagement.service;
using HospitalManagement.controller;
using HospitalManagement.entity.enums;

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

    // Constructor cho runtime (DI)
    public LoginForm(IAuthService authService, AccountController accountController, EmployeeController employeeController) : this()
    {
        _authService = authService;
        _accountController = accountController;
        _employeeController = employeeController;
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        lblError.Visible = false; // Hide error 
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
            
            // Debug: Check if controllers are null
            if (_accountController == null)
            {
                MessageBox.Show("WARNING: AccountController is NULL!", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (_employeeController == null)
            {
                MessageBox.Show("WARNING: EmployeeController is NULL!", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            // Open MainFrame
            var mainFrame = new MainFrame(account.Username, account.Role.ToString(), _accountController, _employeeController);
            mainFrame.FormClosed += (_, _) => Application.Exit();
            mainFrame.Show();
            
            // Hide login form
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
            this.Cursor = Cursors.Default;
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
    
    // Helpers
    private void ShowError(string error)
    {
        lblError.Text = error;
        lblError.Visible = true;
    }
}

