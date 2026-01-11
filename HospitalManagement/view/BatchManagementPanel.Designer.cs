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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
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
            lblKeyword.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblKeyword.Location = new Point(136, 33);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(55, 15);
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
            btnSearch.BackColor = Color.FromArgb(128, 128, 255);
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.Location = new Point(413, 29);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 7;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnAddBatch
            // 
            btnAddBatch.BackColor = Color.Green;
            btnAddBatch.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddBatch.ForeColor = SystemColors.Control;
            btnAddBatch.Location = new Point(115, 74);
            btnAddBatch.Name = "btnAddBatch";
            btnAddBatch.Size = new Size(85, 32);
            btnAddBatch.TabIndex = 6;
            btnAddBatch.Text = "Thêm lô";
            btnAddBatch.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.DarkKhaki;
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEdit.ForeColor = SystemColors.ButtonFace;
            btnEdit.Location = new Point(229, 74);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(89, 32);
            btnEdit.TabIndex = 5;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnDisable
            // 
            btnDisable.BackColor = Color.Red;
            btnDisable.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDisable.ForeColor = SystemColors.ControlLightLight;
            btnDisable.Location = new Point(347, 74);
            btnDisable.Name = "btnDisable";
            btnDisable.Size = new Size(110, 32);
            btnDisable.TabIndex = 4;
            btnDisable.Text = "Ngưng sử dụng";
            btnDisable.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = SystemColors.AppWorkspace;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = SystemColors.Control;
            btnRefresh.Location = new Point(466, 74);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(85, 32);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnExpiring
            // 
            btnExpiring.BackColor = Color.FromArgb(255, 128, 0);
            btnExpiring.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExpiring.ForeColor = SystemColors.ControlLight;
            btnExpiring.Location = new Point(570, 74);
            btnExpiring.Name = "btnExpiring";
            btnExpiring.Size = new Size(130, 32);
            btnExpiring.TabIndex = 2;
            btnExpiring.Text = "Lô sắp hết hạn";
            btnExpiring.UseVisualStyleBackColor = false;
            // 
            // dgvBatches
            // 
            dgvBatches.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvBatches.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvBatches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvBatches.DefaultCellStyle = dataGridViewCellStyle2;
            dgvBatches.Location = new Point(52, 128);
            dgvBatches.Name = "dgvBatches";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvBatches.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvBatches.Size = new Size(1243, 450);
            dgvBatches.TabIndex = 1;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
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

        private System.Windows.Forms.Label lblKeyword;
        private TextBox txtKeyword;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAddBatch;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExpiring;
        private DataGridView dgvBatches;
        private System.Windows.Forms.Label lblTotal;
    }
}
