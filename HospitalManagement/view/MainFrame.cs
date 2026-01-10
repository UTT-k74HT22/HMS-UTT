using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.view.layouts;
using HospitalManagement.view.@base;

namespace HospitalManagement.view
{
    /// <summary>
    /// Main application frame với Header, Sidebar, Footer và Content area
    /// </summary>
    public class MainFrame : Form
    {
        private readonly string _username;
        private readonly string _role;
        private readonly AccountController? _accountController;

        private Sidebar _sidebar = null!;
        private Header _header = null!;
        private Footer _footer = null!;
        private Panel _contentPanel = null!;

        // Constructor mặc định cho Designer (REQUIRED for WinForms designer)
        public MainFrame() : this("Designer", "ADMIN", null!)
        {
        }

        public MainFrame(string username, string role, AccountController accountController)
        {
            _username = username;
            _role = role;
            _accountController = accountController;

            InitializeForm();
            CreateLayout();
            SetupEvents();
            
            // Show dashboard by default
            ShowPanel(Sidebar.MENU_DASHBOARD);
        }

        private void InitializeForm()
        {
            Text = "Hospital Management System - HMS";
            Size = new Size(1400, 900);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1200, 700);
            BackColor = UiTheme.BG;
            
            // Icon (optional)
            // Icon = new Icon("path/to/icon.ico");
        }

        private void CreateLayout()
        {
            // Create components
            _sidebar = new Sidebar(_role);
            _header = new Header(_username, _role);
            _footer = new Footer();
            
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.BG,
                Padding = new Padding(0)
            };

            // Add to form (order matters for docking!)
            Controls.Add(_contentPanel);
            Controls.Add(_footer);
            Controls.Add(_header);
            Controls.Add(_sidebar);
        }

        private void SetupEvents()
        {
            _sidebar.MenuClicked += OnMenuClicked;
            _header.ProfileButton.Click += OnProfileClick;
            
            // Close confirmation
            FormClosing += (s, e) =>
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thoát?",
                    "Xác nhận thoát",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            };
        }

        private void OnMenuClicked(object? sender, MenuClickedEventArgs e)
        {
            if (e.MenuKey == Sidebar.MENU_LOGOUT)
            {
                HandleLogout();
                return;
            }

            ShowPanel(e.MenuKey);
        }

        private void OnProfileClick(object? sender, EventArgs e)
        {
            var info = $"Thông tin người dùng\n\n" +
                      $"Username: {_username}\n" +
                      $"Vai trò: {_role}\n" +
                      $"Đăng nhập lúc: {DateTime.Now:dd/MM/yyyy HH:mm}";

            MessageBox.Show(info, "Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowPanel(string menuKey)
        {
            _contentPanel.Controls.Clear();

            Control? panel = menuKey switch
            {
                Sidebar.MENU_DASHBOARD => new DashboardPanel(),
                Sidebar.MENU_ACCOUNTS => _accountController != null
                    ? new AccountManagementPanel(_accountController)
                    : CreateComingSoonPanel("Quản lý tài khoản (Cần DI)"),

                Sidebar.MENU_EMPLOYEE => new EmployeeManagementPanel(),

                Sidebar.MENU_CUSTOMER => CreateComingSoonPanel("Quản lý khách hàng"),
                Sidebar.MENU_CATEGORIES => CreateComingSoonPanel("Danh mục sản phẩm"),
                Sidebar.MENU_MANUFACTURERS => new ManufacturerManagementForm(),
                Sidebar.MENU_PRODUCTS => new ProductManagementPanel(),

                Sidebar.MENU_WAREHOUSES => CreateComingSoonPanel("Kho hàng"),
                Sidebar.MENU_BATCHES => CreateComingSoonPanel("Lô hàng"),
                Sidebar.MENU_INVENTORY => CreateComingSoonPanel("Tồn kho"),
                Sidebar.MENU_STOCK_MOVEMENTS => CreateComingSoonPanel("Xuất/Nhập kho"),
                Sidebar.MENU_ORDERS => CreateComingSoonPanel("Đơn hàng"),
                Sidebar.MENU_INVOICES => CreateComingSoonPanel("Hóa đơn"),
                Sidebar.MENU_PAYMENTS => new PaymentManagementForm(),
                Sidebar.MENU_REPORT_SUMMARY => CreateComingSoonPanel("Báo cáo tóm tắt"),
                Sidebar.MENU_REPORT_DETAIL => CreateComingSoonPanel("Thống kê chi tiết"),

                _ => new DashboardPanel()
            };

            if (panel != null)
            {
                panel.Dock = DockStyle.Fill;
                _contentPanel.Controls.Add(panel);

                _header.SetModuleTitle(GetModuleTitle(menuKey));
                _sidebar.SetActiveMenu(menuKey);
            }
        }


        private Panel CreateComingSoonPanel(string moduleName)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.BG,
                Padding = new Padding(50)
            };

            var container = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            container.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, container.ClientRectangle,
                    UiTheme.BORDER, 1, ButtonBorderStyle.Solid,
                    UiTheme.BORDER, 1, ButtonBorderStyle.Solid,
                    UiTheme.BORDER, 1, ButtonBorderStyle.Solid,
                    UiTheme.BORDER, 1, ButtonBorderStyle.Solid);
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));

            var iconLabel = new Label
            {
                Text = "🚧",
                Font = new Font("Segoe UI", 72F),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            var titleLabel = new Label
            {
                Text = $"{moduleName}\n\nĐang phát triển...",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = UiTheme.TEXT,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Dock = DockStyle.Fill
            };

            layout.Controls.Add(iconLabel, 0, 0);
            layout.Controls.Add(titleLabel, 0, 1);

            container.Controls.Add(layout);
            panel.Controls.Add(container);

            return panel;
        }

        private string GetModuleTitle(string menuKey)
        {
            return menuKey switch
            {
                Sidebar.MENU_DASHBOARD => "Dashboard",
                Sidebar.MENU_ACCOUNTS => "Quản lý tài khoản",
                Sidebar.MENU_EMPLOYEE => "Quản lý nhân viên",
                Sidebar.MENU_CUSTOMER => "Quản lý khách hàng",
                Sidebar.MENU_CATEGORIES => "Danh mục sản phẩm",
                Sidebar.MENU_MANUFACTURERS => "Nhà sản xuất",
                Sidebar.MENU_PRODUCTS => "Sản phẩm",
                Sidebar.MENU_WAREHOUSES => "Kho hàng",
                Sidebar.MENU_BATCHES => "Lô hàng",
                Sidebar.MENU_INVENTORY => "Tồn kho",
                Sidebar.MENU_STOCK_MOVEMENTS => "Xuất/Nhập kho",
                Sidebar.MENU_ORDERS => "Đơn hàng",
                Sidebar.MENU_INVOICES => "Hóa đơn",
                Sidebar.MENU_PAYMENTS => "Thanh toán",
                Sidebar.MENU_REPORT_SUMMARY => "Báo cáo tóm tắt",
                Sidebar.MENU_REPORT_DETAIL => "Thống kê chi tiết",
                _ => "Hospital Management System"
            };
        }

        private void HandleLogout()
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Close main frame and restart application
                Application.Restart();
            }
        }
    }
}
