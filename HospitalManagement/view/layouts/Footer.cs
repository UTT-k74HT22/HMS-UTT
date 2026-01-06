using System;
using System.Drawing;
using System.Windows.Forms;

namespace HospitalManagement.view.layouts
{
    /// <summary>
    /// Footer component hiển thị copyright info
    /// Chiều cao cố định 32px
    /// </summary>
    public class Footer : Panel
    {
        public Footer()
        {
            InitializeFooter();

            var label = new Label
            {
                Text = $"© {DateTime.Now.Year} - Hospital Management System - Minh Châu",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(150, 150, 180),
                AutoSize = true,
                Padding = new Padding(24, 0, 0, 0),
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(label);
        }

        private void InitializeFooter()
        {
            Height = 32;
            Dock = DockStyle.Bottom;
            BackColor = Color.FromArgb(248, 249, 255);
            Padding = new Padding(0);

            // Custom paint để vẽ border trên
            Paint += (sender, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 232, 245), 1))
                {
                    e.Graphics.DrawLine(pen, 0, 0, Width, 0);
                }
            };
        }
    }
}
