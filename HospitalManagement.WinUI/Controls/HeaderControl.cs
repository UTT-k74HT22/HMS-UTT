namespace HospitalManagement.WinUI.Controls;

public partial class HeaderControl : UserControl
{
    public Button AddButton => btnAdd;
    public Button EditButton => btnEdit;
    public Button ExportExcelButton => btnExportExcel;

    public HeaderControl()
    {
        InitializeComponent();
    }

    public void SetUserInfo(string username, string role)
    {
        lblUserInfo.Text = $"{username} ({role})";
    }

    public void SetModuleName(string moduleName)
    {
        lblModuleName.Text = moduleName;
    }
}
