namespace HospitalManagement.view
{
    partial class ProductManagementPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblKeyword = new Label();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            txtKeyword = new TextBox();
            btnDetail = new Button();
            btnSearch = new Button();
            lblStatus = new Label();
            cbStatus = new ComboBox();
            btnRefresh = new Button();
            lblTotal = new Label();
            dgvProducts = new DataGridView();
            btnDowloadExcel = new Button();
            btnImportExcel = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblKeyword.Location = new Point(147, 26);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(55, 15);
            lblKeyword.TabIndex = 11;
            lblKeyword.Text = "Từ khóa:";
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.ForestGreen;
            btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.ForeColor = Color.Transparent;
            btnAdd.Location = new Point(136, 54);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(102, 23);
            btnAdd.TabIndex = 10;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.DarkOrange;
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEdit.ForeColor = Color.Transparent;
            btnEdit.Location = new Point(244, 54);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(92, 23);
            btnEdit.TabIndex = 9;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(192, 64, 0);
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = Color.Snow;
            btnDelete.Location = new Point(342, 54);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(89, 23);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(206, 22);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new Size(230, 23);
            txtKeyword.TabIndex = 7;
            // 
            // btnDetail
            // 
            btnDetail.BackColor = SystemColors.ControlDark;
            btnDetail.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDetail.ForeColor = Color.Snow;
            btnDetail.Location = new Point(437, 54);
            btnDetail.Name = "btnDetail";
            btnDetail.Size = new Size(102, 23);
            btnDetail.TabIndex = 6;
            btnDetail.Text = "Chi tiết";
            btnDetail.UseVisualStyleBackColor = false;
            btnDetail.Click += btnDetail_Click;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = SystemColors.HotTrack;
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.ForeColor = SystemColors.Control;
            btnSearch.Location = new Point(442, 21);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(48, 23);
            btnSearch.TabIndex = 5;
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.Location = new Point(578, 25);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(65, 15);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Trạng thái:";
            // 
            // cbStatus
            // 
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(647, 22);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(121, 23);
            cbStatus.TabIndex = 3;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = SystemColors.ControlDark;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = SystemColors.Control;
            btnRefresh.Location = new Point(555, 54);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(102, 23);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotal.Location = new Point(61, 518);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(108, 15);
            lblTotal.TabIndex = 1;
            lblTotal.Text = "Tổng số sản phẩm:";
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(61, 100);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.Size = new Size(1124, 399);
            dgvProducts.TabIndex = 0;
            // 
            // btnDowloadExcel
            // 
            btnDowloadExcel.BackColor = Color.DarkCyan;
            btnDowloadExcel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDowloadExcel.ForeColor = SystemColors.Control;
            btnDowloadExcel.Location = new Point(687, 54);
            btnDowloadExcel.Name = "btnDowloadExcel";
            btnDowloadExcel.Size = new Size(121, 23);
            btnDowloadExcel.TabIndex = 12;
            btnDowloadExcel.Text = "DowloadTemplate";
            btnDowloadExcel.UseVisualStyleBackColor = false;
            btnDowloadExcel.Click += btnDowload_Click;
            // 
            // btnImportExcel
            // 
            btnImportExcel.BackColor = Color.FromArgb(64, 64, 0);
            btnImportExcel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnImportExcel.ForeColor = SystemColors.Control;
            btnImportExcel.Location = new Point(814, 54);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(102, 23);
            btnImportExcel.TabIndex = 13;
            btnImportExcel.Text = "Import";
            btnImportExcel.UseVisualStyleBackColor = false;
            btnImportExcel.Click += btnImportExcel_Click;
            // 
            // ProductManagementPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnImportExcel);
            Controls.Add(btnDowloadExcel);
            Controls.Add(dgvProducts);
            Controls.Add(lblTotal);
            Controls.Add(btnRefresh);
            Controls.Add(cbStatus);
            Controls.Add(lblStatus);
            Controls.Add(btnSearch);
            Controls.Add(btnDetail);
            Controls.Add(txtKeyword);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(lblKeyword);
            Name = "ProductManagementPanel";
            Size = new Size(1210, 570);
            Load += ProductManagementPanel_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Button btnDowloadExcel;
        private System.Windows.Forms.Button btnImportExcel;

        #endregion

        private System.Windows.Forms.Label lblKeyword;
        private TextBox txtKeyword;
        private System.Windows.Forms.Button btnSearch;

        private System.Windows.Forms.Label lblStatus;
        private ComboBox cbStatus;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Button btnRefresh;

        private Label lblTotal;
        private DataGridView dgvProducts;
    }
}
