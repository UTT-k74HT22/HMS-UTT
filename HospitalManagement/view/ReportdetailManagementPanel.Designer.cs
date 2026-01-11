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
            pnlBestSellingTop = new Panel();
            btnReloadBestSelling = new Button();
            tabInventory = new TabPage();
            pnlInventoryTop = new Panel();
            btnReloadInventory = new Button();
            tabCustomers = new TabPage();
            pnlCustomersTop = new Panel();
            btnReloadCustomers = new Button();
            tabOrders = new TabPage();
            pnlOrdersTop = new Panel();
            btnReloadOrders = new Button();
            dataGridView1 = new DataGridView();
            dataGridView2 = new DataGridView();
            dataGridView3 = new DataGridView();
            dataGridView4 = new DataGridView();
            tabControl.SuspendLayout();
            tabBestSelling.SuspendLayout();
            pnlBestSellingTop.SuspendLayout();
            tabInventory.SuspendLayout();
            pnlInventoryTop.SuspendLayout();
            tabCustomers.SuspendLayout();
            pnlCustomersTop.SuspendLayout();
            tabOrders.SuspendLayout();
            pnlOrdersTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabBestSelling);
            tabControl.Controls.Add(tabInventory);
            tabControl.Controls.Add(tabCustomers);
            tabControl.Controls.Add(tabOrders);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(900, 600);
            tabControl.TabIndex = 0;
            // 
            // tabBestSelling
            // 
            tabBestSelling.Controls.Add(pnlBestSellingTop);
            tabBestSelling.Location = new Point(4, 24);
            tabBestSelling.Name = "tabBestSelling";
            tabBestSelling.Size = new Size(892, 572);
            tabBestSelling.TabIndex = 0;
            tabBestSelling.Text = "Best Selling";
            // 
            // pnlBestSellingTop
            // 
            pnlBestSellingTop.Controls.Add(dataGridView1);
            pnlBestSellingTop.Controls.Add(btnReloadBestSelling);
            pnlBestSellingTop.Location = new Point(0, 0);
            pnlBestSellingTop.Name = "pnlBestSellingTop";
            pnlBestSellingTop.Size = new Size(839, 547);
            pnlBestSellingTop.TabIndex = 0;
            // 
            // btnReloadBestSelling
            // 
            btnReloadBestSelling.Location = new Point(0, 0);
            btnReloadBestSelling.Name = "btnReloadBestSelling";
            btnReloadBestSelling.Size = new Size(75, 23);
            btnReloadBestSelling.TabIndex = 0;
            // 
            // tabInventory
            // 
            tabInventory.Controls.Add(pnlInventoryTop);
            tabInventory.Location = new Point(4, 24);
            tabInventory.Name = "tabInventory";
            tabInventory.Size = new Size(892, 572);
            tabInventory.TabIndex = 1;
            tabInventory.Text = "Inventory";
            // 
            // pnlInventoryTop
            // 
            pnlInventoryTop.Controls.Add(dataGridView2);
            pnlInventoryTop.Controls.Add(btnReloadInventory);
            pnlInventoryTop.Location = new Point(0, 0);
            pnlInventoryTop.Name = "pnlInventoryTop";
            pnlInventoryTop.Size = new Size(841, 550);
            pnlInventoryTop.TabIndex = 0;
            // 
            // btnReloadInventory
            // 
            btnReloadInventory.Location = new Point(0, 0);
            btnReloadInventory.Name = "btnReloadInventory";
            btnReloadInventory.Size = new Size(75, 23);
            btnReloadInventory.TabIndex = 0;
            // 
            // tabCustomers
            // 
            tabCustomers.Controls.Add(pnlCustomersTop);
            tabCustomers.Location = new Point(4, 24);
            tabCustomers.Name = "tabCustomers";
            tabCustomers.Size = new Size(892, 572);
            tabCustomers.TabIndex = 2;
            tabCustomers.Text = "Customers";
            // 
            // pnlCustomersTop
            // 
            pnlCustomersTop.Controls.Add(dataGridView3);
            pnlCustomersTop.Controls.Add(btnReloadCustomers);
            pnlCustomersTop.Location = new Point(0, 0);
            pnlCustomersTop.Name = "pnlCustomersTop";
            pnlCustomersTop.Size = new Size(849, 543);
            pnlCustomersTop.TabIndex = 0;
            // 
            // btnReloadCustomers
            // 
            btnReloadCustomers.Location = new Point(0, 0);
            btnReloadCustomers.Name = "btnReloadCustomers";
            btnReloadCustomers.Size = new Size(75, 23);
            btnReloadCustomers.TabIndex = 0;
            // 
            // tabOrders
            // 
            tabOrders.Controls.Add(pnlOrdersTop);
            tabOrders.Location = new Point(4, 24);
            tabOrders.Name = "tabOrders";
            tabOrders.Size = new Size(892, 572);
            tabOrders.TabIndex = 3;
            tabOrders.Text = "Orders";
            // 
            // pnlOrdersTop
            // 
            pnlOrdersTop.Controls.Add(dataGridView4);
            pnlOrdersTop.Controls.Add(btnReloadOrders);
            pnlOrdersTop.Location = new Point(0, 0);
            pnlOrdersTop.Name = "pnlOrdersTop";
            pnlOrdersTop.Size = new Size(842, 547);
            pnlOrdersTop.TabIndex = 0;
            // 
            // btnReloadOrders
            // 
            btnReloadOrders.Location = new Point(0, 0);
            btnReloadOrders.Name = "btnReloadOrders";
            btnReloadOrders.Size = new Size(75, 23);
            btnReloadOrders.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 29);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(855, 515);
            dataGridView1.TabIndex = 1;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(0, 29);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(849, 518);
            dataGridView2.TabIndex = 1;
            // 
            // dataGridView3
            // 
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Location = new Point(0, 29);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.Size = new Size(849, 511);
            dataGridView3.TabIndex = 1;
            // 
            // dataGridView4
            // 
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Location = new Point(-1, 29);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.Size = new Size(851, 515);
            dataGridView4.TabIndex = 1;
            // 
            // ReportDetailManagementPanel
            // 
            Controls.Add(tabControl);
            Name = "ReportDetailManagementPanel";
            Size = new Size(900, 600);
            tabControl.ResumeLayout(false);
            tabBestSelling.ResumeLayout(false);
            pnlBestSellingTop.ResumeLayout(false);
            tabInventory.ResumeLayout(false);
            pnlInventoryTop.ResumeLayout(false);
            tabCustomers.ResumeLayout(false);
            pnlCustomersTop.ResumeLayout(false);
            tabOrders.ResumeLayout(false);
            pnlOrdersTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            ResumeLayout(false);
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
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private DataGridView dataGridView3;
        private DataGridView dataGridView4;
    }
}
