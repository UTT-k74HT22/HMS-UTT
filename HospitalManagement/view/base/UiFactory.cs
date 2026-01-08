using System;
using System.Drawing;
using System.Windows.Forms;

namespace HospitalManagement.view.@base
{
    /// <summary>
    /// Factory class cung cấp các helper methods để tạo styled UI components
    /// Đảm bảo tính nhất quán về UI/UX trong toàn bộ ứng dụng
    /// </summary>
    public static class UiFactory
    {
        /// <summary>
        /// Tạo một Panel với style card (viền và padding)
        /// </summary>
        public static Panel CreateCardPanel()
        {
            var panel = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(0),
                BorderStyle = BorderStyle.None
            };
            
            // Custom paint để tạo border đẹp hơn
            panel.Paint += (sender, e) =>
            {
                var p = sender as Panel;
                if (p != null)
                {
                    ControlPaint.DrawBorder(e.Graphics, p.ClientRectangle,
                        Color.FromArgb(235, 237, 242), 1, ButtonBorderStyle.Solid,
                        Color.FromArgb(235, 237, 242), 1, ButtonBorderStyle.Solid,
                        Color.FromArgb(235, 237, 242), 1, ButtonBorderStyle.Solid,
                        Color.FromArgb(235, 237, 242), 1, ButtonBorderStyle.Solid);
                }
            };
            
            return panel;
        }

        /// <summary>
        /// Tạo TextBox với style chuẩn
        /// </summary>
        public static TextBox CreateTextField(int width = 200)
        {
            return new TextBox
            {
                Width = width,
                Font = UiTheme.FONT_BASE,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 3, 8, 3)
            };
        }

        /// <summary>
        /// Tạo Button với màu background tùy chỉnh
        /// </summary>
        public static Button CreateButton(string text, Color bgColor, EventHandler? clickHandler = null)
        {
            var button = new Button
            {
                Text = text,
                BackColor = bgColor,
                ForeColor = Color.White,
                Font = UiTheme.FONT_BOLD,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Padding = new Padding(12, 6, 12, 6),
                AutoSize = true,
                Height = 32,
                MinimumSize = new Size(80, 32),
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 6, 0)
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(bgColor, 0.1f);
            button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.1f);

            if (clickHandler != null)
            {
                button.Click += clickHandler;
            }

            return button;
        }

        /// <summary>
        /// Tạo Label với style chuẩn
        /// </summary>
        public static Label CreateLabel(string text, bool isBold = false)
        {
            return new Label
            {
                Text = text,
                Font = isBold ? UiTheme.FONT_BOLD : UiTheme.FONT_BASE,
                ForeColor = UiTheme.TEXT,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 5, 8, 5),
                Padding = new Padding(0)
            };
        }

        /// <summary>
        /// Tạo ComboBox với style chuẩn
        /// </summary>
        public static ComboBox CreateComboBox(string[] items, int width = 150)
        {
            var combo = new ComboBox
            {
                Width = width,
                Font = UiTheme.FONT_BASE,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 3, 8, 3)
            };

            if (items != null && items.Length > 0)
            {
                combo.Items.AddRange(items);
                combo.SelectedIndex = 0;
            }

            return combo;
        }

        /// <summary>
        /// Style cho DataGridView (Table) - base styling
        /// </summary>
        public static void StyleTable(DataGridView table)
        {
            table.RowTemplate.Height = 40;
            table.Font = UiTheme.FONT_BASE;
            table.GridColor = Color.FromArgb(230, 230, 230);
            table.BackgroundColor = Color.White;
            table.BorderStyle = BorderStyle.None;
            table.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            table.RowHeadersVisible = false;
            table.AllowUserToAddRows = false;
            table.AllowUserToDeleteRows = false;
            table.AllowUserToResizeRows = false;
            table.ReadOnly = true;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            table.EnableHeadersVisualStyles = false;
            
            // Selection colors
            table.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            table.DefaultCellStyle.SelectionForeColor = UiTheme.TEXT;
            
            // Better padding for cells
            table.DefaultCellStyle.Padding = new Padding(8, 5, 8, 5);
        }

        /// <summary>
        /// Style cho header của DataGridView
        /// </summary>
        public static void StyleHeader(DataGridView table)
        {
            var headerStyle = new DataGridViewCellStyle
            {
                BackColor = UiTheme.PRIMARY,
                ForeColor = Color.White,
                Font = UiTheme.FONT_BOLD,
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(8, 10, 8, 10)
            };

            table.ColumnHeadersDefaultCellStyle = headerStyle;
            table.ColumnHeadersHeight = 45;
            table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }

        /// <summary>
        /// Apply zebra striping (màu xen kẽ) cho DataGridView
        /// </summary>
        public static void ApplyZebraStripes(DataGridView table)
        {
            table.RowsDefaultCellStyle.BackColor = Color.White;
            table.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);
            table.DefaultCellStyle.ForeColor = Color.FromArgb(52, 58, 70);
            table.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        /// <summary>
        /// Tạo một styled panel trong suốt (transparent)
        /// </summary>
        public static Panel CreateTransparentPanel()
        {
            return new Panel
            {
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };
        }

        /// <summary>
        /// Tạo vertical spacer
        /// </summary>
        public static Panel CreateVerticalSpacer(int height)
        {
            return new Panel
            {
                Height = height,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent
            };
        }

        /// <summary>
        /// Tạo horizontal spacer
        /// </summary>
        public static Panel CreateHorizontalSpacer(int width)
        {
            return new Panel
            {
                Width = width,
                Dock = DockStyle.Left,
                BackColor = Color.Transparent
            };
        }
    }
}
