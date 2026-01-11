using System.Windows.Forms;

namespace HospitalManagement.view
{
    partial class ReportDetailManagementPanel
    {
        private System.ComponentModel.IContainer components = null;

        private TabControl tabControl;
        private TabPage tabBestSelling;
        private TabPage tabInventory;
        private TabPage tabCustomers;
        private TabPage tabOrders;

        // ===== BEST SELLING =====
        private Panel pnlBestSellingTop;
        private Button btnReloadBestSelling;
        private DataGridView dgvBestSelling;

        // ===== INVENTORY =====
        private Panel pnlInventoryTop;
        private Button btnReloadInventory;
        private DataGridView dgvInventory;

        // ===== CUSTOMERS =====
        private Panel pnlCustomersTop;
        private Button btnReloadCustomers;
        private DataGridView dgvCustomers;

        // ===== ORDERS =====
        private Panel pnlOrdersTop;
        private Button btnReloadOrders;
        private DataGridView dgvOrders;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            tabControl = new TabControl();
            tabBestSelling = new TabPage();
            tabInventory = new TabPage();
            tabCustomers = new TabPage();
            tabOrders = new TabPage();

            // ================= TAB CONTROL =================
            tabControl.Dock = DockStyle.Fill;
            tabControl.Controls.Add(tabBestSelling);
            tabControl.Controls.Add(tabInventory);
            tabControl.Controls.Add(tabCustomers);
            tabControl.Controls.Add(tabOrders);

            // =================================================
            // ================ BEST SELLING ===================
            // =================================================
            pnlBestSellingTop = new Panel();
            btnReloadBestSelling = new Button();
            dgvBestSelling = CreateGrid();

            pnlBestSellingTop.Dock = DockStyle.Top;
            pnlBestSellingTop.Height = 40;

            btnReloadBestSelling.Text = "Reload";
            btnReloadBestSelling.Dock = DockStyle.Left;
            btnReloadBestSelling.Width = 100;

            pnlBestSellingTop.Controls.Add(btnReloadBestSelling);

            tabBestSelling.Text = "Best Selling";
            tabBestSelling.Controls.Add(dgvBestSelling);
            tabBestSelling.Controls.Add(pnlBestSellingTop);

            // =================================================
            // ================= INVENTORY =====================
            // =================================================
            pnlInventoryTop = new Panel();
            btnReloadInventory = new Button();
            dgvInventory = CreateGrid();

            pnlInventoryTop.Dock = DockStyle.Top;
            pnlInventoryTop.Height = 40;

            btnReloadInventory.Text = "Reload";
            btnReloadInventory.Dock = DockStyle.Left;
            btnReloadInventory.Width = 100;

            pnlInventoryTop.Controls.Add(btnReloadInventory);

            tabInventory.Text = "Inventory";
            tabInventory.Controls.Add(dgvInventory);
            tabInventory.Controls.Add(pnlInventoryTop);

            // =================================================
            // ================= CUSTOMERS =====================
            // =================================================
            pnlCustomersTop = new Panel();
            btnReloadCustomers = new Button();
            dgvCustomers = CreateGrid();

            pnlCustomersTop.Dock = DockStyle.Top;
            pnlCustomersTop.Height = 40;

            btnReloadCustomers.Text = "Reload";
            btnReloadCustomers.Dock = DockStyle.Left;
            btnReloadCustomers.Width = 100;

            pnlCustomersTop.Controls.Add(btnReloadCustomers);

            tabCustomers.Text = "Customers";
            tabCustomers.Controls.Add(dgvCustomers);
            tabCustomers.Controls.Add(pnlCustomersTop);

            // =================================================
            // ================== ORDERS =======================
            // =================================================
            pnlOrdersTop = new Panel();
            btnReloadOrders = new Button();
            dgvOrders = CreateGrid();

            pnlOrdersTop.Dock = DockStyle.Top;
            pnlOrdersTop.Height = 40;

            btnReloadOrders.Text = "Reload";
            btnReloadOrders.Dock = DockStyle.Left;
            btnReloadOrders.Width = 100;

            pnlOrdersTop.Controls.Add(btnReloadOrders);

            tabOrders.Text = "Orders";
            tabOrders.Controls.Add(dgvOrders);
            tabOrders.Controls.Add(pnlOrdersTop);

            // ================= ROOT =================
            Controls.Add(tabControl);
            Name = "ReportDetailManagementPanel";
            Size = new System.Drawing.Size(900, 600);
        }
        #endregion

        // ================= HELPER =================
        private DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                MultiSelect = false
            };
        }
    }
}
