using HospitalManagement.Domain.Interfaces;
using HospitalManagement.WinUI.Views;

namespace HospitalManagement.WinUI.Controllers;

public class AuthController : IAuthController
{
    private readonly IAuthService _authService;
    private readonly LoginForm _loginForm;
    private readonly RegisterForm _registerForm;
    private readonly Func<string, string, MainForm> _mainFormFactory;
    private MainForm? _currentMainForm;

    public AuthController(
        IAuthService authService, 
        LoginForm loginForm, 
        RegisterForm registerForm,
        Func<string, string, MainForm> mainFormFactory)
    {
        _authService = authService;
        _loginForm = loginForm;
        _registerForm = registerForm;
        _mainFormFactory = mainFormFactory;

        _loginForm.OnLoginClicked += async (username, password) => 
            await HandleLoginAsync(username, password);
        _loginForm.OnGoRegisterClicked += ShowRegister;

        _registerForm.OnRegisterClicked += async (username, password, role) => 
            await HandleRegisterAsync(username, password, role);
        _registerForm.OnBackToLoginClicked += ShowLogin;
    }

    public void ShowLogin()
    {
        _loginForm.ClearForm();
        _registerForm.Hide();
        if (_currentMainForm != null)
        {
            _currentMainForm.Close();
            _currentMainForm = null;
        }
        _loginForm.Show();
    }

    public void ShowRegister()
    {
        _registerForm.ClearForm();
        _loginForm.Hide();
        _registerForm.Show();
    }

    public async Task HandleLoginAsync(string username, string password)
    {
        try
        {
            var account = await _authService.LoginAsync(username, password);
            
            if (account == null)
            {
                _loginForm.ShowError("Sai tài kho?n ho?c m?t kh?u");
                return;
            }

            _currentMainForm = _mainFormFactory(account.Username, account.Role);
            _currentMainForm.OnLogoutClicked += ShowLogin;
            _currentMainForm.Show();
            _loginForm.Hide();
        }
        catch (Exception ex)
        {
            _loginForm.ShowError($"L?i ??ng nh?p: {ex.Message}");
        }
    }

    public async Task HandleRegisterAsync(string username, string password, string role)
    {
        try
        {
            await _authService.RegisterAsync(username, password, role);
            
            MessageBox.Show(
                "??ng ký thành công! Vui lòng ??ng nh?p.", 
                "Thành công", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information
            );
            
            ShowLogin();
        }
        catch (Exception ex)
        {
            _registerForm.ShowError(ex.Message);
        }
    }
}
