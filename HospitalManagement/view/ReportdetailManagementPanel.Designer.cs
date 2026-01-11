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
        private DataGridViewTextBoxColumn bsCol1;
        private DataGridViewTextBoxColumn bsCol2;
        private DataGridViewTextBoxColumn bsCol3;
        private DataGridViewTextBoxColumn bsCol4;

        // ===== INVENTORY =====
        private Panel pnlInventoryTop;
        private Button btnReloadInventory;
        private DataGridView dgvInventory;
        private DataGridViewTextBoxColumn invCol1;
        private DataGridViewTextBoxColumn invCol2;
        private DataGridViewTextBoxColumn invCol3;
        private DataGridViewTextBoxColumn invCol4;

        // ===== CUSTOMERS =====
        private Panel pnlCustomersTop;
        private Button btnReloadCustomers;
        private DataGridView dgvCustomers;
        private DataGridViewTextBoxColumn cusCol1;
        private DataGridViewTextBoxColumn cusCol2;
        private DataGridViewTextBoxColumn cusCol3;
        private DataGridViewTextBoxColumn cusCol4;

        // ===== ORDERS =====
        private Panel pnlOrdersTop;
        private Button btnReloadOrders;
        private DataGridView dgvOrders;
        private DataGridViewTextBoxColumn ordCol1;
        private DataGridViewTextBoxColumn ordCol2;
        private DataGridViewTextBoxColumn ordCol3;
        private DataGridViewTextBoxColumn ordCol4;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Designer generated code
        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabBestSelling = new TabPage();
            tabInventory = new TabPage();
            tabCustomers = new TabPage();
            tabOrders = new TabPage();

            // ================= TAB CONTROL =================
            tabControl.Dock = DockStyle.Fill;
            tabControl.Controls.AddRange(new Control[]
            {
                tabBestSelling, tabInventory, tabCustomers, tabOrders
            });

            // =================================================
            // =============== BEST SELLING ====================
            // =================================================
            pnlBestSellingTop = new Panel { Dock = DockStyle.Top, Height = 40 };
            btnReloadBestSelling = new Button { Text = "Reload", Left = 10, Top = 8, Width = 80 };
            pnlBestSellingTop.Controls.Add(btnReloadBestSelling);

            dgvBestSelling = CreateGrid();
            bsCol1 = CreateCol("STT");
            bsCol2 = CreateCol("Product Code");
            bsCol3 = CreateCol("Product Name");
            bsCol4 = CreateCol("Sold Quantity");
            dgvBestSelling.Columns.AddRange(bsCol1, bsCol2, bsCol3, bsCol4);

            tabBestSelling.Text = "Best Selling";
            tabBestSelling.Controls.Add(dgvBestSelling);
            tabBestSelling.Controls.Add(pnlBestSellingTop);

            // =================================================
            // ================= INVENTORY =====================
            // =================================================
            pnlInventoryTop = new Panel { Dock = DockStyle.Top, Height = 40 };
            btnReloadInventory = new Button { Text = "Reload", Left = 10, Top = 8, Width = 80 };
            pnlInventoryTop.Controls.Add(btnReloadInventory);

            dgvInventory = CreateGrid();
            invCol1 = CreateCol("STT");
            invCol2 = CreateCol("Product Code");
            invCol3 = CreateCol("Product Name");
            invCol4 = CreateCol("Stock Quantity");
            dgvInventory.Columns.AddRange(invCol1, invCol2, invCol3, invCol4);

            tabInventory.Text = "Inventory";
            tabInventory.Controls.Add(dgvInventory);
            tabInventory.Controls.Add(pnlInventoryTop);

            // =================================================
            // ================= CUSTOMERS =====================
            // =================================================
            pnlCustomersTop = new Panel { Dock = DockStyle.Top, Height = 40 };
            btnReloadCustomers = new Button { Text = "Reload", Left = 10, Top = 8, Width = 80 };
            pnlCustomersTop.Controls.Add(btnReloadCustomers);

            dgvCustomers = CreateGrid();
            cusCol1 = CreateCol("STT");
            cusCol2 = CreateCol("Customer Code");
            cusCol3 = CreateCol("Customer Name");
            cusCol4 = CreateCol("Total Orders");
            dgvCustomers.Columns.AddRange(cusCol1, cusCol2, cusCol3, cusCol4);

            tabCustomers.Text = "Customers";
            tabCustomers.Controls.Add(dgvCustomers);
            tabCustomers.Controls.Add(pnlCustomersTop);

            // =================================================
            // ================= ORDERS ========================
            // =================================================
            pnlOrdersTop = new Panel { Dock = DockStyle.Top, Height = 40 };
            btnReloadOrders = new Button { Text = "Reload", Left = 10, Top = 8, Width = 80 };
            pnlOrdersTop.Controls.Add(btnReloadOrders);

            dgvOrders = CreateGrid();
            ordCol1 = CreateCol("STT");
            ordCol2 = CreateCol("Status");
            ordCol3 = CreateCol("Orders Count");
            ordCol4 = CreateCol("Total Amount");
            dgvOrders.Columns.AddRange(ordCol1, ordCol2, ordCol3, ordCol4);

            tabOrders.Text = "Orders";
            tabOrders.Controls.Add(dgvOrders);
            tabOrders.Controls.Add(pnlOrdersTop);

            // ================= ROOT =================
            Controls.Add(tabControl);
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
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
        }

        private DataGridViewTextBoxColumn CreateCol(string header)
        {
            return new DataGridViewTextBoxColumn
            {
                HeaderText = header,
                ReadOnly = true
            };
        }
    }
}
