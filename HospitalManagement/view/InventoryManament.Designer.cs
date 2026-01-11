namespace HospitalManagement.view
{
    partial class InventoryManagementPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            pnlRoot = new TableLayoutPanel();
            pnlToolbar = new TableLayoutPanel();
            pnlFilters = new FlowLayoutPanel();
            lblKeyword = new Label();
            txtKeyword = new TextBox();
            btnSearch = new Button();
            lblWarehouse = new Label();
            cboWarehouse = new ComboBox();
            lblStatus = new Label();
            cboStatus = new ComboBox();
            pnlActions = new FlowLayoutPanel();
            btnUpdateThreshold = new Button();
            btnLowStock = new Button();
            btnNearExpiry = new Button();
            btnRefresh = new Button();
            dgvInventory = new DataGridView();
            pnlFooter = new Panel();
            lblTotal = new Label();
            pnlRoot.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventory).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRoot
            // 
            pnlRoot.ColumnCount = 1;
            pnlRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlRoot.Controls.Add(pnlToolbar, 0, 0);
            pnlRoot.Controls.Add(dgvInventory, 0, 1);
            pnlRoot.Controls.Add(pnlFooter, 0, 2);
            pnlRoot.Dock = DockStyle.Fill;
            pnlRoot.Location = new Point(0, 0);
            pnlRoot.Name = "pnlRoot";
            pnlRoot.Padding = new Padding(16);
            pnlRoot.RowCount = 3;
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            pnlRoot.Size = new Size(1200, 650);
            pnlRoot.TabIndex = 0;
            // 
            // pnlToolbar
            // 
            pnlToolbar.ColumnCount = 1;
            pnlToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlToolbar.Controls.Add(pnlFilters, 0, 0);
            pnlToolbar.Controls.Add(pnlActions, 0, 1);
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(16, 16);
            pnlToolbar.Margin = new Padding(0, 0, 0, 12);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.RowCount = 2;
            pnlToolbar.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            pnlToolbar.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            pnlToolbar.Size = new Size(1168, 80);
            pnlToolbar.TabIndex = 0;
            // 
            // pnlFilters
            // 
            pnlFilters.AutoSize = true;
            pnlFilters.Controls.Add(lblKeyword);
            pnlFilters.Controls.Add(txtKeyword);
            pnlFilters.Controls.Add(btnSearch);
            pnlFilters.Controls.Add(lblWarehouse);
            pnlFilters.Controls.Add(cboWarehouse);
            pnlFilters.Controls.Add(lblStatus);
            pnlFilters.Controls.Add(cboStatus);
            pnlFilters.Dock = DockStyle.Fill;
            pnlFilters.Location = new Point(0, 0);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(0, 6, 0, 0);
            pnlFilters.Size = new Size(1168, 40);
            pnlFilters.TabIndex = 0;
            pnlFilters.WrapContents = false;
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new Point(0, 16);
            lblKeyword.Margin = new Padding(0, 10, 8, 0);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(53, 15);
            lblKeyword.TabIndex = 0;
            lblKeyword.Text = "Từ khóa:";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(61, 12);
            txtKeyword.Margin = new Padding(0, 6, 10, 0);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new Size(240, 23);
            txtKeyword.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(0, 123, 255);
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(311, 12);
            btnSearch.Margin = new Padding(0, 6, 12, 0);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(80, 26);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new Point(403, 16);
            lblWarehouse.Margin = new Padding(0, 10, 8, 0);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(31, 15);
            lblWarehouse.TabIndex = 3;
            lblWarehouse.Text = "Kho:";
            // 
            // cboWarehouse
            // 
            cboWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cboWarehouse.Location = new Point(442, 12);
            cboWarehouse.Margin = new Padding(0, 6, 12, 0);
            cboWarehouse.Name = "cboWarehouse";
            cboWarehouse.Size = new Size(200, 23);
            cboWarehouse.TabIndex = 4;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(654, 16);
            lblStatus.Margin = new Padding(0, 10, 8, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(63, 15);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Trạng thái:";
            // 
            // cboStatus
            // 
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.Items.AddRange(new object[] { "TẤT CẢ", "SẮP HẾT HÀNG", "SẮP HẾT HẠN", "DƯ THỪA", "BÌNH THƯỜNG" });
            cboStatus.Location = new Point(725, 12);
            cboStatus.Margin = new Padding(0, 6, 0, 0);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(180, 23);
            cboStatus.TabIndex = 6;
            // 
            // pnlActions
            // 
            pnlActions.AutoSize = true;
            pnlActions.Controls.Add(btnUpdateThreshold);
            pnlActions.Controls.Add(btnLowStock);
            pnlActions.Controls.Add(btnNearExpiry);
            pnlActions.Controls.Add(btnRefresh);
            pnlActions.Dock = DockStyle.Fill;
            pnlActions.Location = new Point(0, 40);
            pnlActions.Margin = new Padding(0);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new Padding(0, 6, 0, 0);
            pnlActions.Size = new Size(1168, 40);
            pnlActions.TabIndex = 1;
            pnlActions.WrapContents = false;
            // 
            // btnUpdateThreshold
            // 
            btnUpdateThreshold.BackColor = Color.FromArgb(255, 193, 7);
            btnUpdateThreshold.FlatStyle = FlatStyle.Flat;
            btnUpdateThreshold.ForeColor = Color.Black;
            btnUpdateThreshold.Location = new Point(0, 12);
            btnUpdateThreshold.Margin = new Padding(0, 6, 8, 0);
            btnUpdateThreshold.Name = "btnUpdateThreshold";
            btnUpdateThreshold.Size = new Size(130, 26);
            btnUpdateThreshold.TabIndex = 0;
            btnUpdateThreshold.Text = "Cập nhật ngưỡng";
            btnUpdateThreshold.UseVisualStyleBackColor = false;
            // 
            // btnLowStock
            // 
            btnLowStock.BackColor = Color.FromArgb(220, 53, 69);
            btnLowStock.FlatStyle = FlatStyle.Flat;
            btnLowStock.ForeColor = Color.White;
            btnLowStock.Location = new Point(138, 12);
            btnLowStock.Margin = new Padding(0, 6, 8, 0);
            btnLowStock.Name = "btnLowStock";
            btnLowStock.Size = new Size(120, 26);
            btnLowStock.TabIndex = 1;
            btnLowStock.Text = "Sắp hết hàng";
            btnLowStock.UseVisualStyleBackColor = false;
            // 
            // btnNearExpiry
            // 
            btnNearExpiry.BackColor = Color.FromArgb(255, 87, 34);
            btnNearExpiry.FlatStyle = FlatStyle.Flat;
            btnNearExpiry.ForeColor = Color.White;
            btnNearExpiry.Location = new Point(266, 12);
            btnNearExpiry.Margin = new Padding(0, 6, 8, 0);
            btnNearExpiry.Name = "btnNearExpiry";
            btnNearExpiry.Size = new Size(110, 26);
            btnNearExpiry.TabIndex = 2;
            btnNearExpiry.Text = "Sắp hết hạn";
            btnNearExpiry.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(384, 12);
            btnRefresh.Margin = new Padding(0, 6, 0, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(90, 26);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // dgvInventory
            // 
            dgvInventory.BackgroundColor = Color.White;
            dgvInventory.BorderStyle = BorderStyle.None;
            dgvInventory.Dock = DockStyle.Fill;
            dgvInventory.Location = new Point(16, 108);
            dgvInventory.Margin = new Padding(0);
            dgvInventory.Name = "dgvInventory";
            dgvInventory.Size = new Size(1168, 486);
            dgvInventory.TabIndex = 1;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(lblTotal);
            pnlFooter.Dock = DockStyle.Fill;
            pnlFooter.Location = new Point(16, 606);
            pnlFooter.Margin = new Padding(0, 12, 0, 0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1168, 28);
            pnlFooter.TabIndex = 2;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = DockStyle.Left;
            lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotal.Location = new Point(0, 0);
            lblTotal.Margin = new Padding(0);
            lblTotal.Name = "lblTotal";
            lblTotal.Padding = new Padding(0, 10, 0, 0);
            lblTotal.Size = new Size(136, 25);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số mục tồn kho: 0";
            // 
            // InventoryManagementPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            Controls.Add(pnlRoot);
            Name = "InventoryManagementPanel";
            Size = new Size(1200, 650);
            pnlRoot.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvInventory).EndInit();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel pnlRoot;
        private TableLayoutPanel pnlToolbar;
        private FlowLayoutPanel pnlFilters;
        private Label lblKeyword;
        private TextBox txtKeyword;
        private Button btnSearch;
        private Label lblWarehouse;
        private ComboBox cboWarehouse;
        private Label lblStatus;
        private ComboBox cboStatus;

        private FlowLayoutPanel pnlActions;
        private Button btnUpdateThreshold;
        private Button btnLowStock;
        private Button btnNearExpiry;
        private Button btnRefresh;

        private DataGridView dgvInventory;

        private Panel pnlFooter;
        private Label lblTotal;
    }
}
