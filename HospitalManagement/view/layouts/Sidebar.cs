using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HospitalManagement.view.@base;

namespace HospitalManagement.view.layouts
{
    /// <summary>
    /// Sidebar navigation menu với dynamic menu theo role
    /// Width: 240px
    /// </summary>
    public class Sidebar : Panel
    {
        // Menu keys - consistent với backend routing
        public const string MENU_DASHBOARD = "DASHBOARD";
        public const string MENU_ACCOUNTS = "ACCOUNTS";
        public const string MENU_EMPLOYEE = "EMPLOYEE";
        public const string MENU_CUSTOMER = "CUSTOMER";
        public const string MENU_CATEGORIES = "CATEGORIES";
        public const string MENU_MANUFACTURERS = "MANUFACTURERS";
        public const string MENU_PRODUCTS = "PRODUCTS";
        public const string MENU_WAREHOUSES = "WAREHOUSES";
        public const string MENU_BATCHES = "BATCHES";
        public const string MENU_INVENTORY = "INVENTORY";
        public const string MENU_STOCK_MOVEMENTS = "STOCK_MOVEMENTS";
        public const string MENU_ORDERS = "ORDERS";
        public const string MENU_INVOICES = "INVOICES";
        public const string MENU_PAYMENTS = "PAYMENTS";
        public const string MENU_REPORT_SUMMARY = "REPORT_SUMMARY";
        public const string MENU_REPORT_DETAIL = "REPORT_DETAIL";
        public const string MENU_LOGOUT = "LOGOUT";

        private readonly Dictionary<string, Button> _menuButtons = new Dictionary<string, Button>();
        private string? _activeKey = null;
        private readonly FlowLayoutPanel _menuContainer;

        public event EventHandler<MenuClickedEventArgs>? MenuClicked;

        // Constructor mặc định cho Designer
        public Sidebar() : this("ADMIN")
        {
        }

        public Sidebar(string role)
        {
            InitializeSidebar();

            // App logo/name
            var appLabel = new Label
            {
                Text = "HMS - Hospital",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 64,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 12),
                BackColor = Color.Transparent
            };


            _menuContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };


            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(0, 0, 0, 0),
                BackColor = Color.Transparent
            };




            _menuContainer.Dock = DockStyle.Top;
            appLabel.Dock = DockStyle.Top;
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.Controls.Add(_menuContainer);
            Controls.Add(scrollPanel);
            Controls.Add(appLabel);

            // Build menu theo role
            BuildMenus(role);

            // Set default active menu
            var defaultMenu = GetDefaultMenu(role);
            SetActiveMenu(defaultMenu);
        }

        private void InitializeSidebar()
        {
            Width = 240;
            Dock = DockStyle.Left;
            BackColor = UiTheme.PRIMARY;
            Padding = new Padding(18, 20, 18, 18);
        }

        private void BuildMenus(string role)
        {
            role = role.ToUpper();

            if (role == "ADMIN")
            {
                AddSection("Tổng quan");
                AddMenu("Dashboard", MENU_DASHBOARD);

                AddSection("Quản trị hệ thống");
                AddMenu("Tài khoản", MENU_ACCOUNTS);
                AddMenu("Quản lý nhân viên", MENU_EMPLOYEE);
                AddMenu("Quản lý khách hàng", MENU_CUSTOMER);

                AddSection("Danh mục");
                AddMenu("Danh mục sản phẩm", MENU_CATEGORIES);
                AddMenu("Nhà sản xuất", MENU_MANUFACTURERS);
                AddMenu("Sản phẩm", MENU_PRODUCTS);

                AddSection("Kho & Tồn");
                AddMenu("Kho hàng", MENU_WAREHOUSES);
                AddMenu("Lô hàng", MENU_BATCHES);
                AddMenu("Tồn kho", MENU_INVENTORY);
                AddMenu("Xuất/Nhập kho", MENU_STOCK_MOVEMENTS);

                AddSection("Bán hàng & Thanh toán");
                AddMenu("Đơn hàng", MENU_ORDERS);
                AddMenu("Hóa đơn", MENU_INVOICES);
                AddMenu("Thanh toán", MENU_PAYMENTS);

                AddSection("Thống kê");
                AddMenu("Báo cáo tóm tắt", MENU_REPORT_SUMMARY);
                AddMenu("Thống kê chi tiết", MENU_REPORT_DETAIL);
            }
            else if (role == "EMPLOYEE")
            {
                AddSection("Tổng quan");
                AddMenu("Dashboard", MENU_DASHBOARD);

                AddSection("Danh mục");
                AddMenu("Danh mục sản phẩm", MENU_CATEGORIES);
                AddMenu("Nhà sản xuất", MENU_MANUFACTURERS);
                AddMenu("Sản phẩm", MENU_PRODUCTS);

                AddSection("Kho & Tồn");
                AddMenu("Kho hàng", MENU_WAREHOUSES);
                AddMenu("Lô hàng", MENU_BATCHES);
                AddMenu("Tồn kho", MENU_INVENTORY);
                AddMenu("Xuất/Nhập kho", MENU_STOCK_MOVEMENTS);

                AddSection("Bán hàng & Thanh toán");
                AddMenu("Đơn hàng", MENU_ORDERS);
                AddMenu("Hóa đơn", MENU_INVOICES);
                AddMenu("Thanh toán", MENU_PAYMENTS);

                AddSection("Khách hàng");
                AddMenu("Tra cứu khách hàng", MENU_CUSTOMER);
            }
            else // CUSTOMER
            {
                AddSection("Mua hàng");
                AddMenu("Sản phẩm", MENU_PRODUCTS);

                AddSection("Đơn của tôi");
                AddMenu("Đơn hàng của tôi", MENU_ORDERS);
                AddMenu("Hóa đơn của tôi", MENU_INVOICES);
                AddMenu("Thanh toán của tôi", MENU_PAYMENTS);
            }

            // Logout button at bottom
            AddSpacer(12);
            var logoutBtn = CreateMenuButton("Đăng xuất", MENU_LOGOUT);
            _menuButtons[MENU_LOGOUT] = logoutBtn;
            _menuContainer.Controls.Add(logoutBtn);
        }

        private void AddSection(string title)
        {
            AddSpacer(14);

            var label = new Label
            {
                Text = title,
                ForeColor = Color.FromArgb(215, 215, 245),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 6)
            };

            _menuContainer.Controls.Add(label);
        }

        private void AddMenu(string text, string key)
        {
            var btn = CreateMenuButton(text, key);
            _menuButtons[key] = btn;
            _menuContainer.Controls.Add(btn);
            AddSpacer(6);
        }



        private void AddSpacer(int height)
        {
            _menuContainer.Controls.Add(new Panel
            {
                Height = height,
                Width = 186, // <= sửa từ 1 lên full width
                BackColor = Color.Transparent
            });
        }



        private Button CreateMenuButton(string text, string key)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(186, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                Tag = key,
                UseVisualStyleBackColor = false,
                Padding = new Padding(16, 0, 0, 0)
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 255, 255, 255);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 255, 255, 255);

            btn.Click += (s, e) =>
            {
                SetActiveMenu(key);
                MenuClicked?.Invoke(this, new MenuClickedEventArgs(key, text));
            };

            // Custom paint for active state with rounded background
            btn.Paint += (sender, e) =>
            {
                var isActive = key == _activeKey;
                if (!isActive) return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
                using var path = GetRoundedRectangle(btn.ClientRectangle, 8);
                e.Graphics.FillPath(brush, path);
            };

            return btn;
        }

        private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            var diameter = radius * 2;
            var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        private string GetDefaultMenu(string role)
        {
            return MENU_DASHBOARD;
        }

        // ========== Public API ==========

        public void SetActiveMenu(string key)
        {
            _activeKey = key;

            // Refresh all buttons
            foreach (var btn in _menuButtons.Values)
            {
                btn.Invalidate();
            }
        }

        public string? ActiveMenu => _activeKey;
    }

    public class MenuClickedEventArgs : EventArgs
    {
        public string MenuKey { get; }
        public string MenuText { get; }

        public MenuClickedEventArgs(string menuKey, string menuText)
        {
            MenuKey = menuKey;
            MenuText = menuText;
        }
    }
}
