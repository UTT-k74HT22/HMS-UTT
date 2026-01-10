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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            lblKeyword = new System.Windows.Forms.Label();
            txtKeyword = new System.Windows.Forms.TextBox();
            btnSearch = new System.Windows.Forms.Button();
            btnAddBatch = new System.Windows.Forms.Button();
            btnEdit = new System.Windows.Forms.Button();
            btnDisable = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnExpiring = new System.Windows.Forms.Button();
            dgvBatches = new System.Windows.Forms.DataGridView();
            lblTotal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvBatches).BeginInit();
            SuspendLayout();
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            lblKeyword.Location = new System.Drawing.Point(136, 33);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new System.Drawing.Size(55, 15);
            lblKeyword.TabIndex = 9;
            lblKeyword.Text = "Từ khóa:";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new System.Drawing.Point(195, 29);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new System.Drawing.Size(212, 23);
            txtKeyword.TabIndex = 8;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)((byte)128)), ((int)((byte)128)), ((int)((byte)255)));
            btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnSearch.Location = new System.Drawing.Point(413, 29);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(75, 23);
            btnSearch.TabIndex = 7;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnAddBatch
            // 
            btnAddBatch.BackColor = System.Drawing.Color.Green;
            btnAddBatch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnAddBatch.ForeColor = System.Drawing.SystemColors.Control;
            btnAddBatch.Location = new System.Drawing.Point(115, 74);
            btnAddBatch.Name = "btnAddBatch";
            btnAddBatch.Size = new System.Drawing.Size(85, 32);
            btnAddBatch.TabIndex = 6;
            btnAddBatch.Text = "Thêm lô";
            btnAddBatch.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = System.Drawing.Color.DarkKhaki;
            btnEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnEdit.ForeColor = System.Drawing.SystemColors.ButtonFace;
            btnEdit.Location = new System.Drawing.Point(229, 74);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(89, 32);
            btnEdit.TabIndex = 5;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnDisable
            // 
            btnDisable.BackColor = System.Drawing.Color.Red;
            btnDisable.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnDisable.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btnDisable.Location = new System.Drawing.Point(347, 74);
            btnDisable.Name = "btnDisable";
            btnDisable.Size = new System.Drawing.Size(110, 32);
            btnDisable.TabIndex = 4;
            btnDisable.Text = "Ngưng sử dụng";
            btnDisable.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.SystemColors.AppWorkspace;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnRefresh.ForeColor = System.Drawing.SystemColors.Control;
            btnRefresh.Location = new System.Drawing.Point(466, 74);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(85, 32);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnExpiring
            // 
            btnExpiring.BackColor = System.Drawing.Color.FromArgb(((int)((byte)255)), ((int)((byte)128)), ((int)((byte)0)));
            btnExpiring.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnExpiring.ForeColor = System.Drawing.SystemColors.ControlLight;
            btnExpiring.Location = new System.Drawing.Point(570, 74);
            btnExpiring.Name = "btnExpiring";
            btnExpiring.Size = new System.Drawing.Size(130, 32);
            btnExpiring.TabIndex = 2;
            btnExpiring.Text = "Lô sắp hết hạn";
            btnExpiring.UseVisualStyleBackColor = false;
            // 
            // dgvBatches
            // 
            dgvBatches.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvBatches.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvBatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvBatches.DefaultCellStyle = dataGridViewCellStyle2;
            dgvBatches.Location = new System.Drawing.Point(54, 127);
            dgvBatches.Name = "dgvBatches";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvBatches.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvBatches.Size = new System.Drawing.Size(2482, 450);
            dgvBatches.TabIndex = 1;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            lblTotal.Location = new System.Drawing.Point(73, 596);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(96, 15);
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
            Size = new System.Drawing.Size(1400, 673);
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
