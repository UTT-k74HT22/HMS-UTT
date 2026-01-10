using System.ComponentModel;
using System.Windows.Forms;

namespace HospitalManagement.view
{
    partial class ReportDetailManagementPanel
    {
        private IContainer components = null;

        private TabControl tabControl;
        private TabPage tabBestSelling;
        private TabPage tabInventory;
        private TabPage tabCustomers;
        private TabPage tabOrders;

        private DataGridView dgvBestSelling;
        private DataGridView dgvInventory;
        private DataGridView dgvCustomers;
        private DataGridView dgvOrders;

        private Button btnReloadBestSelling;
        private Button btnReloadInventory;
        private Button btnReloadCustomers;
        private Button btnReloadOrders;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();

            tabControl = new TabControl();
            tabBestSelling = new TabPage("Best Selling");
            tabInventory = new TabPage("Inventory");
            tabCustomers = new TabPage("Customers");
            tabOrders = new TabPage("Orders");

            dgvBestSelling = new DataGridView();
            dgvInventory = new DataGridView();
            dgvCustomers = new DataGridView();
            dgvOrders = new DataGridView();

            btnReloadBestSelling = new Button { Text = "Reload" };
            btnReloadInventory = new Button { Text = "Reload" };
            btnReloadCustomers = new Button { Text = "Reload" };
            btnReloadOrders = new Button { Text = "Reload" };

            // Layout TabControl
            tabControl.Dock = DockStyle.Fill;
            tabControl.Controls.Add(tabBestSelling);
            tabControl.Controls.Add(tabInventory);
            tabControl.Controls.Add(tabCustomers);
            tabControl.Controls.Add(tabOrders);

            // DataGridViews
            dgvBestSelling.Dock = dgvInventory.Dock = dgvCustomers.Dock = dgvOrders.Dock = DockStyle.Top;
            dgvBestSelling.Height = dgvInventory.Height = dgvCustomers.Height = dgvOrders.Height = 400;

            // Buttons positions
            btnReloadBestSelling.Top = btnReloadInventory.Top = btnReloadCustomers.Top = btnReloadOrders.Top = 410;
            btnReloadBestSelling.Left = btnReloadInventory.Left = btnReloadCustomers.Left = btnReloadOrders.Left = 10;

            // Add controls to Tabs
            tabBestSelling.Controls.Add(dgvBestSelling); tabBestSelling.Controls.Add(btnReloadBestSelling);
            tabInventory.Controls.Add(dgvInventory); tabInventory.Controls.Add(btnReloadInventory);
            tabCustomers.Controls.Add(dgvCustomers); tabCustomers.Controls.Add(btnReloadCustomers);
            tabOrders.Controls.Add(dgvOrders); tabOrders.Controls.Add(btnReloadOrders);

            this.Controls.Add(tabControl);
            this.Dock = DockStyle.Fill;
        }
    }
}
