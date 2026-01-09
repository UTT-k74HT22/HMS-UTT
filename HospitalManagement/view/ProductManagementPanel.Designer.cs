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

            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();

            // lblKeyword
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new Point(147, 26);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(56, 15);
            lblKeyword.Text = "Từ khóa:";

            // txtKeyword
            txtKeyword.Location = new Point(206, 22);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new Size(230, 23);

            // btnSearch
            btnSearch.Location = new Point(437, 22);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(48, 23);
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = true;

            // lblStatus
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(578, 25);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(63, 15);
            lblStatus.Text = "Trạng thái:";

            // cbStatus
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(647, 22);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(121, 23);

            // btnAdd
            btnAdd.Location = new Point(136, 54);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(102, 23);
            btnAdd.Text = "Thêm";

            // btnEdit
            btnEdit.Location = new Point(244, 54);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(92, 23);
            btnEdit.Text = "Sửa";

            // btnDelete
            btnDelete.Location = new Point(342, 54);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(89, 23);
            btnDelete.Text = "Xóa";

            // btnDetail
            btnDetail.Location = new Point(437, 54);
            btnDetail.Name = "btnDetail";
            btnDetail.Size = new Size(102, 23);
            btnDetail.Text = "Chi tiết";

            // btnRefresh
            btnRefresh.Location = new Point(555, 54);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(102, 23);
            btnRefresh.Text = "Refresh";

            // dgvProducts
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(61, 109);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.Size = new Size(1065, 380);

            // lblTotal
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(98, 519);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(108, 15);
            lblTotal.Text = "Tổng số sản phẩm:";

            // ProductManagementPanel
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
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

        #endregion

        private Label lblKeyword;
        private TextBox txtKeyword;
        private Button btnSearch;

        private Label lblStatus;
        private ComboBox cbStatus;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnDetail;
        private Button btnRefresh;

        private Label lblTotal;
        private DataGridView dgvProducts;
    }
}
