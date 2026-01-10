namespace HospitalManagement.view
{
    partial class StockMovementManagementPanel
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
            lblMovementType = new Label();
            cboMovementType = new ComboBox();
            pnlActions = new FlowLayoutPanel();
            btnImport = new Button();
            btnExport = new Button();
            btnAdjust = new Button();
            btnTransfer = new Button();
            btnRefresh = new Button();
            dgvStockMovement = new DataGridView();
            pnlFooter = new Panel();
            lblTotal = new Label();
            pnlRoot.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStockMovement).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRoot
            // 
            pnlRoot.ColumnCount = 1;
            pnlRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlRoot.Controls.Add(pnlToolbar, 0, 0);
            pnlRoot.Controls.Add(dgvStockMovement, 0, 1);
            pnlRoot.Controls.Add(pnlFooter, 0, 2);
            pnlRoot.Dock = DockStyle.Fill;
            pnlRoot.Location = new Point(0, 0);
            pnlRoot.Name = "pnlRoot";
            pnlRoot.Padding = new Padding(16);
            pnlRoot.RowCount = 3;
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            pnlRoot.Size = new Size(1400, 650);
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
            pnlToolbar.Size = new Size(1368, 80);
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
            pnlFilters.Controls.Add(lblMovementType);
            pnlFilters.Controls.Add(cboMovementType);
            pnlFilters.Dock = DockStyle.Fill;
            pnlFilters.Location = new Point(0, 0);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(0, 6, 0, 0);
            pnlFilters.Size = new Size(1368, 40);
            pnlFilters.TabIndex = 0;
            pnlFilters.WrapContents = false;
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new Point(0, 18);
            lblKeyword.Margin = new Padding(0, 10, 8, 0);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(57, 15);
            lblKeyword.TabIndex = 0;
            lblKeyword.Text = "Từ khóa:";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(65, 14);
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
            btnSearch.Location = new Point(315, 14);
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
            lblWarehouse.Location = new Point(407, 18);
            lblWarehouse.Margin = new Padding(0, 10, 8, 0);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(31, 15);
            lblWarehouse.TabIndex = 3;
            lblWarehouse.Text = "Kho:";
            // 
            // cboWarehouse
            // 
            cboWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cboWarehouse.Location = new Point(446, 14);
            cboWarehouse.Margin = new Padding(0, 6, 12, 0);
            cboWarehouse.Name = "cboWarehouse";
            cboWarehouse.Size = new Size(220, 23);
            cboWarehouse.TabIndex = 4;
            // 
            // lblMovementType
            // 
            lblMovementType.AutoSize = true;
            lblMovementType.Location = new Point(678, 18);
            lblMovementType.Margin = new Padding(0, 10, 8, 0);
            lblMovementType.Name = "lblMovementType";
            lblMovementType.Size = new Size(31, 15);
            lblMovementType.TabIndex = 5;
            lblMovementType.Text = "Loại:";
            // 
            // cboMovementType
            // 
            cboMovementType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMovementType.Items.AddRange(new object[] {
                "TẤT CẢ LOẠI",
                "Nhập kho",
                "Xuất kho",
                "Điều chỉnh",
                "Chuyển kho"
            });
            cboMovementType.Location = new Point(717, 14);
            cboMovementType.Margin = new Padding(0, 6, 0, 0);
            cboMovementType.Name = "cboMovementType";
            cboMovementType.SelectedIndex = 0;
            cboMovementType.Size = new Size(200, 23);
            cboMovementType.TabIndex = 6;
            // 
            // pnlActions
            // 
            pnlActions.AutoSize = true;
            pnlActions.Controls.Add(btnImport);
            pnlActions.Controls.Add(btnExport);
            pnlActions.Controls.Add(btnAdjust);
            pnlActions.Controls.Add(btnTransfer);
            pnlActions.Controls.Add(btnRefresh);
            pnlActions.Dock = DockStyle.Fill;
            pnlActions.FlowDirection = FlowDirection.LeftToRight;
            pnlActions.Location = new Point(0, 40);
            pnlActions.Margin = new Padding(0);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new Padding(0, 6, 0, 0);
            pnlActions.Size = new Size(1368, 40);
            pnlActions.TabIndex = 1;
            pnlActions.WrapContents = false;
            // 
            // btnImport
            // 
            btnImport.BackColor = Color.FromArgb(40, 167, 69);
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.ForeColor = Color.White;
            btnImport.Location = new Point(0, 12);
            btnImport.Margin = new Padding(0, 6, 8, 0);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(100, 26);
            btnImport.TabIndex = 0;
            btnImport.Text = "Nhập kho";
            btnImport.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(220, 53, 69);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(108, 12);
            btnExport.Margin = new Padding(0, 6, 8, 0);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(100, 26);
            btnExport.TabIndex = 1;
            btnExport.Text = "Xuất kho";
            btnExport.UseVisualStyleBackColor = false;
            // 
            // btnAdjust
            // 
            btnAdjust.BackColor = Color.FromArgb(255, 193, 7);
            btnAdjust.FlatStyle = FlatStyle.Flat;
            btnAdjust.ForeColor = Color.Black;
            btnAdjust.Location = new Point(216, 12);
            btnAdjust.Margin = new Padding(0, 6, 8, 0);
            btnAdjust.Name = "btnAdjust";
            btnAdjust.Size = new Size(100, 26);
            btnAdjust.TabIndex = 2;
            btnAdjust.Text = "Điều chỉnh";
            btnAdjust.UseVisualStyleBackColor = false;
            // 
            // btnTransfer
            // 
            btnTransfer.BackColor = Color.FromArgb(23, 162, 184);
            btnTransfer.FlatStyle = FlatStyle.Flat;
            btnTransfer.ForeColor = Color.White;
            btnTransfer.Location = new Point(324, 12);
            btnTransfer.Margin = new Padding(0, 6, 8, 0);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(100, 26);
            btnTransfer.TabIndex = 3;
            btnTransfer.Text = "Chuyển kho";
            btnTransfer.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(432, 12);
            btnRefresh.Margin = new Padding(0, 6, 0, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(90, 26);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // dgvStockMovement
            // 
            dgvStockMovement.BackgroundColor = Color.White;
            dgvStockMovement.BorderStyle = BorderStyle.None;
            dgvStockMovement.Dock = DockStyle.Fill;
            dgvStockMovement.Location = new Point(16, 120);
            dgvStockMovement.Margin = new Padding(0);
            dgvStockMovement.Name = "dgvStockMovement";
            dgvStockMovement.Size = new Size(1368, 474);
            dgvStockMovement.TabIndex = 1;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(lblTotal);
            pnlFooter.Dock = DockStyle.Fill;
            pnlFooter.Location = new Point(16, 606);
            pnlFooter.Margin = new Padding(0, 12, 0, 0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1368, 28);
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
            lblTotal.Size = new Size(120, 25);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số giao dịch: 0";
            // 
            // StockMovementManagementPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            Controls.Add(pnlRoot);
            Name = "StockMovementManagementPanel";
            Size = new Size(1400, 650);
            pnlRoot.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStockMovement).EndInit();
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
        private Label lblMovementType;
        private ComboBox cboMovementType;

        private FlowLayoutPanel pnlActions;
        private Button btnImport;
        private Button btnExport;
        private Button btnAdjust;
        private Button btnTransfer;
        private Button btnRefresh;

        private DataGridView dgvStockMovement;

        private Panel pnlFooter;
        private Label lblTotal;
    }
}
