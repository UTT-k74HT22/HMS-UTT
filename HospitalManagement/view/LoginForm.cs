using HospitalManagement.entity;
using HospitalManagement.service;

namespace HospitalManagement.view;
public partial class LoginForm : Form
{
    private readonly IAuthService _authService;
    private bool _isLoggedIn = false;
    
    public LoginForm(IAuthService authService)
    {
        _authService = authService;
        InitializeComponent();
        this.ActiveControl = tbUsername;
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
            
            // Mark as logged in
            _isLoggedIn = true;
            
            // Open MainFrame
            var mainFrame = new MainFrame(account.Username, account.Role);
            mainFrame.FormClosed += (s, args) => Application.Exit();
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

        // // Nếu đã login thì không cần confirm
        // if (_isLoggedIn)
        // {
        //     return;
        // }
        //
        // if (e.CloseReason == CloseReason.UserClosing)
        // {
        //     var result = MessageBox.Show(
        //         "Bạn có chắc chắn muốn thoát?", 
        //         "Xác nhận thoát", 
        //         MessageBoxButtons.YesNo, 
        //         MessageBoxIcon.Question);
        //         
        //     if (result == DialogResult.No)
        //     {
        //         e.Cancel = true;
        //     }
        //     else
        //     {
        //         Application.Exit()
        //     var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //     if (result == DialogResult.No)
        //     {
        //         e.Cancel = true;
        //     }
        // }
    }
