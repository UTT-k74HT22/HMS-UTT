namespace HospitalManagement.WinUI.Views;

public partial class LoginForm : Form
{
    public event Action<string, string>? OnLoginClicked;
    public event Action? OnGoRegisterClicked;

    public LoginForm()
    {
        InitializeComponent();
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        
        if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            ShowError("Vui lòng nh?p ??y ?? thông tin");
            return;
        }

        OnLoginClicked?.Invoke(txtUsername.Text.Trim(), txtPassword.Text);
    }

    private void BtnGoRegister_Click(object? sender, EventArgs e)
    {
        OnGoRegisterClicked?.Invoke();
    }

    public void ShowError(string message)
    {
        lblError.Text = message;
    }

    public void ClearForm()
    {
        txtUsername.Clear();
        txtPassword.Clear();
        lblError.Text = string.Empty;
    }
}
