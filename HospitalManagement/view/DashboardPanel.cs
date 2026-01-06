using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalManagement.view.@base;

namespace HospitalManagement.view
{
    /// <summary>
    /// Dashboard panel - màn hình tổng quan
    /// </summary>
    public class DashboardPanel : Panel
    {
        public DashboardPanel()
        {
            Dock = DockStyle.Fill;
            BackColor = UiTheme.BG;
            Padding = new Padding(20);

            var container = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30)
            };

            // Custom paint border
            container.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, container.ClientRectangle,
                    UiTheme.BORDER, 1, ButtonBorderStyle.Solid,
                    UiTheme.BORDER, 1, ButtonBorderStyle.Solid,
                    UiTheme.BORDER, 1, ButtonBorderStyle.Solid,
                    UiTheme.BORDER, 1, ButtonBorderStyle.Solid);
            };

            // Welcome header
            var welcomeLabel = new Label
            {
                Text = "🏥 Chào mừng đến với Hệ thống Quản lý Bệnh viện",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = UiTheme.PRIMARY,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var descLabel = new Label
            {
                Text = "Sử dụng menu bên trái để điều hướng đến các chức năng khác nhau",
                Font = UiTheme.FONT_LARGE,
                ForeColor = UiTheme.TEXT,
                AutoSize = true,
                Location = new Point(0, 50)
            };

            // Stats cards
            var statsPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 100),
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };

            statsPanel.Controls.Add(CreateStatCard("👥 Tài khoản", "50", UiTheme.PRIMARY));
            statsPanel.Controls.Add(CreateStatCard("👨‍⚕️ Nhân viên", "35", UiTheme.INFO));
            statsPanel.Controls.Add(CreateStatCard("🏥 Khách hàng", "120", UiTheme.SUCCESS));
            statsPanel.Controls.Add(CreateStatCard("💊 Sản phẩm", "200", UiTheme.ORANGE));

            container.Controls.Add(welcomeLabel);
            container.Controls.Add(descLabel);
            container.Controls.Add(statsPanel);

            Controls.Add(container);
        }

        private Panel CreateStatCard(string title, string value, Color color)
        {
            var card = new Panel
            {
                Width = 200,
                Height = 120,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 20, 20),
                Padding = new Padding(20)
            };

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    color, 2, ButtonBorderStyle.Solid,
                    color, 2, ButtonBorderStyle.Solid,
                    color, 2, ButtonBorderStyle.Solid,
                    color, 2, ButtonBorderStyle.Solid);
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = UiTheme.FONT_BASE,
                ForeColor = UiTheme.TEXT,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = color,
                AutoSize = true,
                Location = new Point(0, 30)
            };

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);

            return card;
        }
    }
}
