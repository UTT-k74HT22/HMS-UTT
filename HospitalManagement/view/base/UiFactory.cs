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
                    using (var pen = new Pen(Color.FromArgb(220, 223, 230), 1))
                    {
                        var rect = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
                        e.Graphics.DrawRectangle(pen, rect);
                    }
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
                Margin = new Padding(0, 2, 6, 2)
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
                Font = new Font("Segoe UI", 9.25F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Padding = new Padding(10, 6, 10, 6),
                AutoSize = true,
                Height = 32,
                MinimumSize = new Size(78, 32),
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 6, 0)
            };

            button.FlatAppearance.BorderSize = 0;

            var hoverColor = ControlPaint.Light(bgColor, 0.2f);
            var pressColor = ControlPaint.Dark(bgColor, 0.05f);
            button.FlatAppearance.MouseOverBackColor = hoverColor;
            button.FlatAppearance.MouseDownBackColor = pressColor;

            if (clickHandler != null) button.Click += clickHandler;
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
                Margin = new Padding(0, 4, 6, 4),
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
                Margin = new Padding(0, 2, 6, 2)
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
            table.GridColor = Color.FromArgb(235, 237, 242);
            table.BackgroundColor = Color.White;
            table.BorderStyle = BorderStyle.None;
            table.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            table.RowHeadersVisible = false;
            table.AllowUserToAddRows = false;
            table.AllowUserToDeleteRows = false;
            table.AllowUserToResizeRows = false;
            table.ReadOnly = true;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.MultiSelect = false;
            table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            table.EnableHeadersVisualStyles = false;
            table.ShowCellToolTips = false;
            table.Cursor = Cursors.Hand;
            table.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 236, 255);
            table.DefaultCellStyle.SelectionForeColor = Color.FromArgb(45, 45, 70);
            table.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            table.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            table.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            table.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            table.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            table.StandardTab = true;
            table.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
            table.DefaultCellStyle.BackColor = Color.White;
            table.DefaultCellStyle.ForeColor = Color.FromArgb(45, 45, 70);
            table.DoubleBuffered(true);
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
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(8, 10, 8, 10),

                SelectionBackColor = UiTheme.PRIMARY,
                SelectionForeColor = Color.White
            };

            table.ColumnHeadersDefaultCellStyle = headerStyle;
            table.ColumnHeadersHeight = 44;
            table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            table.EnableHeadersVisualStyles = false;
        }

        /// <summary>
        /// Apply zebra striping (màu xen kẽ) cho DataGridView
        /// </summary>
        public static void ApplyZebraStripes(DataGridView table)
        {
            table.RowsDefaultCellStyle.BackColor = Color.White;
            table.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 255);
            table.DefaultCellStyle.ForeColor = Color.FromArgb(45, 45, 70);
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
        
        /// <summary>
        /// Enable double buffering cho control để giảm flicker
        /// </summary>
        private static void DoubleBuffered(this Control control, bool enable)
        {
            var property = typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            property?.SetValue(control, enable, null);
        }
    }
}
