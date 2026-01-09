namespace HospitalManagement.view
{
    partial class BatchManagementPanel
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
            txtKeyword = new TextBox();
            btnSearch = new Button();
            btnAddBatch = new Button();
            btnEdit = new Button();
            btnDisable = new Button();
            btnRefresh = new Button();
            btnExpiring = new Button();
            dgvBatches = new DataGridView();
            lblTotal = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvBatches).BeginInit();
            SuspendLayout();
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new Point(136, 33);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(53, 15);
            lblKeyword.TabIndex = 9;
            lblKeyword.Text = "Từ khóa:";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(195, 29);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new Size(212, 23);
            txtKeyword.TabIndex = 8;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(413, 29);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 7;
            btnSearch.Text = "Tìm kiếm";
            // 
            // btnAddBatch
            // 
            btnAddBatch.Location = new Point(125, 74);
            btnAddBatch.Name = "btnAddBatch";
            btnAddBatch.Size = new Size(75, 23);
            btnAddBatch.TabIndex = 6;
            btnAddBatch.Text = "Thêm lô";
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(229, 74);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(89, 23);
            btnEdit.TabIndex = 5;
            btnEdit.Text = "Sửa";
            // 
            // btnDisable
            // 
            btnDisable.Location = new Point(347, 74);
            btnDisable.Name = "btnDisable";
            btnDisable.Size = new Size(110, 23);
            btnDisable.TabIndex = 4;
            btnDisable.Text = "Ngưng sử dụng";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(466, 74);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(85, 23);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            // 
            // btnExpiring
            // 
            btnExpiring.Location = new Point(570, 74);
            btnExpiring.Name = "btnExpiring";
            btnExpiring.Size = new Size(130, 23);
            btnExpiring.TabIndex = 2;
            btnExpiring.Text = "Lô sắp hết hạn";
            // 
            // dgvBatches
            // 
            dgvBatches.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvBatches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBatches.Location = new Point(54, 127);
            dgvBatches.Name = "dgvBatches";
            dgvBatches.Size = new Size(1232, 450);
            dgvBatches.TabIndex = 1;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(73, 596);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(96, 15);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số lô hàng:";
            // 
            // BatchManagementPanel
            // 
            Controls.Add(lblTotal);
            Controls.Add(dgvBatches);
            Controls.Add(btnExpiring);
            Controls.Add(btnRefresh);
            Controls.Add(btnDisable);
            Controls.Add(btnEdit);
            Controls.Add(btnAddBatch);
            Controls.Add(btnSearch);
            Controls.Add(txtKeyword);
            Controls.Add(lblKeyword);
            Name = "BatchManagementPanel";
            Size = new Size(1400, 673);
            Load += BatchManagementPanel_Load;
            ((System.ComponentModel.ISupportInitialize)dgvBatches).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblKeyword;
        private TextBox txtKeyword;
        private Button btnSearch;
        private Button btnAddBatch;
        private Button btnEdit;
        private Button btnDisable;
        private Button btnRefresh;
        private Button btnExpiring;
        private DataGridView dgvBatches;
        private Label lblTotal;
    }
}
