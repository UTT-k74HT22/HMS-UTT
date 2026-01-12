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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlRoot = new System.Windows.Forms.TableLayoutPanel();
            pnlToolbar = new System.Windows.Forms.TableLayoutPanel();
            pnlFilters = new System.Windows.Forms.FlowLayoutPanel();
            lblKeyword = new System.Windows.Forms.Label();
            txtKeyword = new System.Windows.Forms.TextBox();
            btnSearch = new System.Windows.Forms.Button();
            lblWarehouse = new System.Windows.Forms.Label();
            cboWarehouse = new System.Windows.Forms.ComboBox();
            lblMovementType = new System.Windows.Forms.Label();
            cboMovementType = new System.Windows.Forms.ComboBox();
            pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            btnImport = new System.Windows.Forms.Button();
            btnExport = new System.Windows.Forms.Button();
            btnAdjust = new System.Windows.Forms.Button();
            btnTransfer = new System.Windows.Forms.Button();
            btnImportExcel = new System.Windows.Forms.Button();
            btnDowloadTemplate = new System.Windows.Forms.Button();
            btnExportExcel = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            dgvStockMovement = new System.Windows.Forms.DataGridView();
            pnlFooter = new System.Windows.Forms.Panel();
            lblTotal = new System.Windows.Forms.Label();
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
            pnlRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlRoot.Controls.Add(pnlToolbar, 0, 0);
            pnlRoot.Controls.Add(dgvStockMovement, 0, 1);
            pnlRoot.Controls.Add(pnlFooter, 0, 2);
            pnlRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRoot.Location = new System.Drawing.Point(0, 0);
            pnlRoot.Name = "pnlRoot";
            pnlRoot.Padding = new System.Windows.Forms.Padding(16);
            pnlRoot.RowCount = 3;
            pnlRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            pnlRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            pnlRoot.Size = new System.Drawing.Size(1400, 650);
            pnlRoot.TabIndex = 0;
            // 
            // pnlToolbar
            // 
            pnlToolbar.ColumnCount = 1;
            pnlToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlToolbar.Controls.Add(pnlFilters, 0, 0);
            pnlToolbar.Controls.Add(pnlActions, 0, 1);
            pnlToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlToolbar.Location = new System.Drawing.Point(16, 16);
            pnlToolbar.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.RowCount = 2;
            pnlToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            pnlToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            pnlToolbar.Size = new System.Drawing.Size(1368, 80);
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
            pnlFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlFilters.Location = new System.Drawing.Point(0, 0);
            pnlFilters.Margin = new System.Windows.Forms.Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            pnlFilters.Size = new System.Drawing.Size(1368, 40);
            pnlFilters.TabIndex = 0;
            pnlFilters.WrapContents = false;
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new System.Drawing.Point(0, 16);
            lblKeyword.Margin = new System.Windows.Forms.Padding(0, 10, 8, 0);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new System.Drawing.Size(53, 15);
            lblKeyword.TabIndex = 0;
            lblKeyword.Text = "Từ khóa:";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new System.Drawing.Point(61, 12);
            txtKeyword.Margin = new System.Windows.Forms.Padding(0, 6, 10, 0);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new System.Drawing.Size(240, 23);
            txtKeyword.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)((byte)0)), ((int)((byte)123)), ((int)((byte)255)));
            btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSearch.ForeColor = System.Drawing.Color.White;
            btnSearch.Location = new System.Drawing.Point(311, 12);
            btnSearch.Margin = new System.Windows.Forms.Padding(0, 6, 12, 0);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(80, 26);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new System.Drawing.Point(403, 16);
            lblWarehouse.Margin = new System.Windows.Forms.Padding(0, 10, 8, 0);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new System.Drawing.Size(31, 15);
            lblWarehouse.TabIndex = 3;
            lblWarehouse.Text = "Kho:";
            // 
            // cboWarehouse
            // 
            cboWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboWarehouse.Location = new System.Drawing.Point(442, 12);
            cboWarehouse.Margin = new System.Windows.Forms.Padding(0, 6, 12, 0);
            cboWarehouse.Name = "cboWarehouse";
            cboWarehouse.Size = new System.Drawing.Size(220, 23);
            cboWarehouse.TabIndex = 4;
            // 
            // lblMovementType
            // 
            lblMovementType.AutoSize = true;
            lblMovementType.Location = new System.Drawing.Point(674, 16);
            lblMovementType.Margin = new System.Windows.Forms.Padding(0, 10, 8, 0);
            lblMovementType.Name = "lblMovementType";
            lblMovementType.Size = new System.Drawing.Size(32, 15);
            lblMovementType.TabIndex = 5;
            lblMovementType.Text = "Loại:";
            // 
            // cboMovementType
            // 
            cboMovementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMovementType.Items.AddRange(new object[] { "TẤT CẢ LOẠI", "Nhập kho", "Xuất kho", "Điều chỉnh", "Chuyển kho" });
            cboMovementType.Location = new System.Drawing.Point(714, 12);
            cboMovementType.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            cboMovementType.Name = "cboMovementType";
            cboMovementType.Size = new System.Drawing.Size(200, 23);
            cboMovementType.TabIndex = 6;
            // 
            // pnlActions
            // 
            pnlActions.AutoSize = true;
            pnlActions.Controls.Add(btnImport);
            pnlActions.Controls.Add(btnExport);
            pnlActions.Controls.Add(btnAdjust);
            pnlActions.Controls.Add(btnTransfer);
            pnlActions.Controls.Add(btnImportExcel);
            pnlActions.Controls.Add(btnDowloadTemplate);
            pnlActions.Controls.Add(btnExportExcel);
            pnlActions.Controls.Add(btnRefresh);
            pnlActions.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlActions.Location = new System.Drawing.Point(0, 40);
            pnlActions.Margin = new System.Windows.Forms.Padding(0);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            pnlActions.Size = new System.Drawing.Size(1368, 40);
            pnlActions.TabIndex = 1;
            pnlActions.WrapContents = false;
            // 
            // btnImport
            // 
            btnImport.BackColor = System.Drawing.Color.FromArgb(((int)((byte)40)), ((int)((byte)167)), ((int)((byte)69)));
            btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnImport.ForeColor = System.Drawing.Color.White;
            btnImport.Location = new System.Drawing.Point(0, 12);
            btnImport.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnImport.Name = "btnImport";
            btnImport.Size = new System.Drawing.Size(100, 26);
            btnImport.TabIndex = 0;
            btnImport.Text = "Nhập kho";
            btnImport.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            btnExport.BackColor = System.Drawing.Color.FromArgb(((int)((byte)220)), ((int)((byte)53)), ((int)((byte)69)));
            btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExport.ForeColor = System.Drawing.Color.White;
            btnExport.Location = new System.Drawing.Point(108, 12);
            btnExport.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnExport.Name = "btnExport";
            btnExport.Size = new System.Drawing.Size(100, 26);
            btnExport.TabIndex = 1;
            btnExport.Text = "Xuất kho";
            btnExport.UseVisualStyleBackColor = false;
            // 
            // btnAdjust
            // 
            btnAdjust.BackColor = System.Drawing.Color.FromArgb(((int)((byte)255)), ((int)((byte)193)), ((int)((byte)7)));
            btnAdjust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdjust.ForeColor = System.Drawing.Color.Black;
            btnAdjust.Location = new System.Drawing.Point(216, 12);
            btnAdjust.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnAdjust.Name = "btnAdjust";
            btnAdjust.Size = new System.Drawing.Size(100, 26);
            btnAdjust.TabIndex = 2;
            btnAdjust.Text = "Điều chỉnh";
            btnAdjust.UseVisualStyleBackColor = false;
            // 
            // btnTransfer
            // 
            btnTransfer.BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)162)), ((int)((byte)184)));
            btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTransfer.ForeColor = System.Drawing.Color.White;
            btnTransfer.Location = new System.Drawing.Point(324, 12);
            btnTransfer.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new System.Drawing.Size(100, 26);
            btnTransfer.TabIndex = 3;
            btnTransfer.Text = "Chuyển kho";
            btnTransfer.UseVisualStyleBackColor = false;
            // 
            // btnImportExcel
            // 
            btnImportExcel.BackColor = System.Drawing.Color.LimeGreen;
            btnImportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnImportExcel.ForeColor = System.Drawing.Color.White;
            btnImportExcel.Location = new System.Drawing.Point(432, 12);
            btnImportExcel.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new System.Drawing.Size(90, 26);
            btnImportExcel.TabIndex = 7;
            btnImportExcel.Text = "Import Excel";
            btnImportExcel.UseVisualStyleBackColor = false;
            // 
            // btnDowloadTemplate
            // 
            btnDowloadTemplate.BackColor = System.Drawing.Color.SeaGreen;
            btnDowloadTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDowloadTemplate.ForeColor = System.Drawing.Color.White;
            btnDowloadTemplate.Location = new System.Drawing.Point(522, 12);
            btnDowloadTemplate.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnDowloadTemplate.Name = "btnDowloadTemplate";
            btnDowloadTemplate.Size = new System.Drawing.Size(90, 26);
            btnDowloadTemplate.TabIndex = 6;
            btnDowloadTemplate.Text = "Dowload Template Excel";
            btnDowloadTemplate.UseVisualStyleBackColor = false;
            // 
            // btnExportExcel
            // 
            btnExportExcel.BackColor = System.Drawing.Color.Green;
            btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExportExcel.ForeColor = System.Drawing.Color.White;
            btnExportExcel.Location = new System.Drawing.Point(612, 12);
            btnExportExcel.Margin = new System.Windows.Forms.Padding(0, 6, 16, 0);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new System.Drawing.Size(90, 26);
            btnExportExcel.TabIndex = 5;
            btnExportExcel.Text = "Export Excel";
            btnExportExcel.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)((byte)108)), ((int)((byte)117)), ((int)((byte)125)));
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.Location = new System.Drawing.Point(702, 12);
            btnRefresh.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(90, 26);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // dgvStockMovement
            // 
            dgvStockMovement.BackgroundColor = System.Drawing.Color.White;
            dgvStockMovement.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvStockMovement.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvStockMovement.Location = new System.Drawing.Point(16, 108);
            dgvStockMovement.Margin = new System.Windows.Forms.Padding(0);
            dgvStockMovement.Name = "dgvStockMovement";
            dgvStockMovement.Size = new System.Drawing.Size(1368, 486);
            dgvStockMovement.TabIndex = 1;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(lblTotal);
            pnlFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlFooter.Location = new System.Drawing.Point(16, 606);
            pnlFooter.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new System.Drawing.Size(1368, 28);
            pnlFooter.TabIndex = 2;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = System.Windows.Forms.DockStyle.Left;
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblTotal.Location = new System.Drawing.Point(0, 0);
            lblTotal.Margin = new System.Windows.Forms.Padding(0);
            lblTotal.Name = "lblTotal";
            lblTotal.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            lblTotal.Size = new System.Drawing.Size(115, 25);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số giao dịch: 0";
            // 
            // StockMovementManagementPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.WhiteSmoke;
            Controls.Add(pnlRoot);
            Size = new System.Drawing.Size(1400, 650);
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
        private System.Windows.Forms.FlowLayoutPanel pnlFilters;
        private Label lblKeyword;
        private TextBox txtKeyword;
        private Button btnSearch;
        private Label lblWarehouse;
        private System.Windows.Forms.ComboBox cboWarehouse;
        private System.Windows.Forms.Label lblMovementType;
        private System.Windows.Forms.ComboBox cboMovementType;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnAdjust;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnImportExcel;
        private System.Windows.Forms.Button btnDowloadTemplate;
        private System.Windows.Forms.Button btnExportExcel;
        
        private DataGridView dgvStockMovement;

        private Panel pnlFooter;
        private System.Windows.Forms.Label lblTotal;
    }
}
