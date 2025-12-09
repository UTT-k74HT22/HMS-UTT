namespace HospitalManagement.WinUI.Views;

public partial class RegisterForm : Form
{
    public event Action<string, string, string>? OnRegisterClicked;
    public event Action? OnBackToLoginClicked;

    public RegisterForm()
    {
        InitializeComponent();
        cboRole.SelectedIndex = 0;
    }

    private void BtnRegister_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(txtUsername.Text) || 
            string.IsNullOrWhiteSpace(txtPassword.Text) ||
            cboRole.SelectedItem == null)
        {
            ShowError("Vui lòng nh?p ??y ?? thông tin");
            return;
        }

        OnRegisterClicked?.Invoke(
            txtUsername.Text.Trim(), 
            txtPassword.Text, 
            cboRole.SelectedItem.ToString() ?? "ADMIN"
        );
    }

    private void BtnBackToLogin_Click(object? sender, EventArgs e)
    {
        OnBackToLoginClicked?.Invoke();
    }

    public void ShowError(string message)
    {
        lblError.Text = message;
    }

    public void ClearForm()
    {
        txtUsername.Clear();
        txtPassword.Clear();
        cboRole.SelectedIndex = 0;
        lblError.Text = string.Empty;
    }
}
