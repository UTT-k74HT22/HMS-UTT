using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HospitalManagement.view.@base;

namespace HospitalManagement.view.layouts
{
    /// <summary>
    /// Header component hiển thị module title và user info
    /// Chiều cao cố định 70px
    /// </summary>
    public class Header : Panel
    {
        private readonly Label _moduleLabel;
        private Label _userInfoLabel = null!;
        private Button _btnProfile = null!;

        public Header(string username, string role)
        {
            InitializeHeader();
            
            // Module title (left)
            _moduleLabel = new Label
            {
                Text = "Hospital Management System",
                ForeColor = Color.FromArgb(72, 69, 130),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Padding = new Padding(24, 0, 0, 0),
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Right panel (user info + buttons)
            var rightPanel = CreateRightPanel(username, role);
            
            Controls.Add(_moduleLabel);
            Controls.Add(rightPanel);
        }

        private void InitializeHeader()
        {
            Height = 70;
            Dock = DockStyle.Top;
            BackColor = Color.FromArgb(248, 249, 255);
            Padding = new Padding(0);
            
            // Custom paint để vẽ border dưới
            Paint += (sender, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 232, 245), 1))
                {
                    e.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);
                }
            };
        }

        private Panel CreateRightPanel(string username, string role)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 300,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 24, 10)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Color.Transparent
            };
            
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // User info row
            _userInfoLabel = new Label
            {
                Text = $"{username} ({role})",
                ForeColor = Color.FromArgb(120, 122, 150),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                AutoSize = false
            };

            // Action button row
            var actionRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            _btnProfile = CreateGhostButton("Profile");
            actionRow.Controls.Add(_btnProfile);

            layout.Controls.Add(_userInfoLabel, 0, 0);
            layout.Controls.Add(actionRow, 0, 1);

            panel.Controls.Add(layout);
            return panel;
        }

        private Button CreateGhostButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(90, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = UiTheme.PRIMARY,
                Font = UiTheme.FONT_BASE,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderColor = UiTheme.PRIMARY;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 255);

            // Custom paint để bo tròn góc
            btn.Paint += (sender, e) =>
            {
                var button = sender as Button;
                if (button == null) return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                using (var path = GetRoundedRectangle(button.ClientRectangle, 8))
                using (var brush = new SolidBrush(button.BackColor))
                using (var pen = new Pen(button.Enabled ? UiTheme.PRIMARY : Color.Gray, 1))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }

                TextRenderer.DrawText(e.Graphics, button.Text, button.Font,
                    button.ClientRectangle, button.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            return btn;
        }

        private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            var diameter = radius * 2;
            var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            // Top left arc
            path.AddArc(arc, 180, 90);

            // Top right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        // ========== Public API ==========

        public void SetModuleTitle(string title)
        {
            _moduleLabel.Text = title;
        }

        public void SetUserInfo(string username, string role)
        {
            _userInfoLabel.Text = $"{username} ({role})";
        }

        public Button ProfileButton => _btnProfile;
    }
}
