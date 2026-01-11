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
            lblKeyword = new System.Windows.Forms.Label();
            btnAdd = new System.Windows.Forms.Button();
            btnEdit = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            txtKeyword = new System.Windows.Forms.TextBox();
            btnDetail = new System.Windows.Forms.Button();
            btnSearch = new System.Windows.Forms.Button();
            lblStatus = new System.Windows.Forms.Label();
            cbStatus = new System.Windows.Forms.ComboBox();
            btnRefresh = new System.Windows.Forms.Button();
            lblTotal = new System.Windows.Forms.Label();
            dgvProducts = new System.Windows.Forms.DataGridView();
            btnDowloadExcel = new System.Windows.Forms.Button();
            btnImportExcel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            lblKeyword.Location = new System.Drawing.Point(147, 26);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new System.Drawing.Size(55, 15);
            lblKeyword.TabIndex = 11;
            lblKeyword.Text = "Từ khóa:";
            // 
            // btnAdd
            // 
            btnAdd.BackColor = System.Drawing.Color.ForestGreen;
            btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnAdd.ForeColor = System.Drawing.Color.Transparent;
            btnAdd.Location = new System.Drawing.Point(136, 54);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(102, 23);
            btnAdd.TabIndex = 10;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = System.Drawing.Color.DarkOrange;
            btnEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnEdit.ForeColor = System.Drawing.Color.Transparent;
            btnEdit.Location = new System.Drawing.Point(244, 54);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(92, 23);
            btnEdit.TabIndex = 9;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)((byte)192)), ((int)((byte)64)), ((int)((byte)0)));
            btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnDelete.ForeColor = System.Drawing.Color.Snow;
            btnDelete.Location = new System.Drawing.Point(342, 54);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(89, 23);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new System.Drawing.Point(206, 22);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new System.Drawing.Size(230, 23);
            txtKeyword.TabIndex = 7;
            // 
            // btnDetail
            // 
            btnDetail.BackColor = System.Drawing.SystemColors.ControlDark;
            btnDetail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnDetail.ForeColor = System.Drawing.Color.Snow;
            btnDetail.Location = new System.Drawing.Point(437, 54);
            btnDetail.Name = "btnDetail";
            btnDetail.Size = new System.Drawing.Size(102, 23);
            btnDetail.TabIndex = 6;
            btnDetail.Text = "Chi tiết";
            btnDetail.UseVisualStyleBackColor = false;
            btnDetail.Click += btnDetail_Click;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = System.Drawing.SystemColors.HotTrack;
            btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnSearch.ForeColor = System.Drawing.SystemColors.Control;
            btnSearch.Location = new System.Drawing.Point(442, 21);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(48, 23);
            btnSearch.TabIndex = 5;
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            lblStatus.Location = new System.Drawing.Point(578, 25);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(65, 15);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Trạng thái:";
            // 
            // cbStatus
            // 
            cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new System.Drawing.Point(647, 22);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new System.Drawing.Size(121, 23);
            cbStatus.TabIndex = 3;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.SystemColors.ControlDark;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnRefresh.ForeColor = System.Drawing.SystemColors.Control;
            btnRefresh.Location = new System.Drawing.Point(555, 54);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(102, 23);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            lblTotal.Location = new System.Drawing.Point(61, 518);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(108, 15);
            lblTotal.TabIndex = 1;
            lblTotal.Text = "Tổng số sản phẩm:";
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new System.Drawing.Point(61, 103);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.Size = new System.Drawing.Size(2177, 814);
            dgvProducts.TabIndex = 0;
            // 
            // btnDowloadExcel
            // 
            btnDowloadExcel.BackColor = System.Drawing.Color.DarkCyan;
            btnDowloadExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnDowloadExcel.ForeColor = System.Drawing.SystemColors.Control;
            btnDowloadExcel.Location = new System.Drawing.Point(687, 54);
            btnDowloadExcel.Name = "btnDowloadExcel";
            btnDowloadExcel.Size = new System.Drawing.Size(121, 23);
            btnDowloadExcel.TabIndex = 12;
            btnDowloadExcel.Text = "DowloadTemplate";
            btnDowloadExcel.UseVisualStyleBackColor = false;
            btnDowloadExcel.Click += btnDowload_Click;
            // 
            // btnImportExcel
            // 
            btnImportExcel.BackColor = System.Drawing.Color.FromArgb(((int)((byte)64)), ((int)((byte)64)), ((int)((byte)0)));
            btnImportExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnImportExcel.ForeColor = System.Drawing.SystemColors.Control;
            btnImportExcel.Location = new System.Drawing.Point(814, 54);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new System.Drawing.Size(102, 23);
            btnImportExcel.TabIndex = 13;
            btnImportExcel.Text = "Import";
            btnImportExcel.UseVisualStyleBackColor = false;
            btnImportExcel.Click += btnImportExcel_Click;
            // 
            // ProductManagementPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            Size = new System.Drawing.Size(1210, 570);
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
