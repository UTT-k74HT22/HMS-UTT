namespace HospitalManagement.WinUI.Views;

public partial class MainForm : Form
{
    public event Action? OnLogoutClicked;

    public MainForm(string username, string role)
    {
        InitializeComponent();
        headerControl.SetUserInfo(username, role);
        sidebarControl.OnLogoutClicked += () => OnLogoutClicked?.Invoke();
    }

    public Panel ContentPanel => panelContent;
}
