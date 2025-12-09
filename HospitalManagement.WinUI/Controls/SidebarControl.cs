using System.Drawing.Drawing2D;

namespace HospitalManagement.WinUI.Controls;

public partial class SidebarControl : UserControl
{
    public event Action? OnLogoutClicked;

    public SidebarControl()
    {
        InitializeComponent();
    }

    private void BtnLogout_Click(object? sender, EventArgs e)
    {
        OnLogoutClicked?.Invoke();
    }

    private void SidebarControl_Paint(object? sender, PaintEventArgs e)
    {
        using var brush = new LinearGradientBrush(
            new Point(0, 0),
            new Point(0, Height),
            Color.FromArgb(103, 58, 183),
            Color.FromArgb(33, 150, 243)
        );
        e.Graphics.FillRectangle(brush, ClientRectangle);
    }
}
