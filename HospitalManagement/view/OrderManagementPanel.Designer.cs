namespace HospitalManagement.view
{
    partial class OrderManagementPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            lblTotal = new System.Windows.Forms.Label();
            dgvOrders = new System.Windows.Forms.DataGridView();
            btnCreate = new System.Windows.Forms.Button();
            btnView = new System.Windows.Forms.Button();
            btnConfirm = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            btnSearch = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            txtSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            SuspendLayout();
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new System.Drawing.Point(39, 574);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(110, 15);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số đơn hàng: ";
            // 
            // dgvOrders
            // 
            dgvOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            dgvOrders.BackgroundColor = System.Drawing.SystemColors.Control;
            dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Location = new System.Drawing.Point(3, 85);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.Size = new System.Drawing.Size(3737, 472);
            dgvOrders.TabIndex = 1;
            // 
            // btnCreate
            // 
            btnCreate.BackColor = System.Drawing.Color.Silver;
            btnCreate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnCreate.Location = new System.Drawing.Point(246, 33);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new System.Drawing.Size(75, 34);
            btnCreate.TabIndex = 2;
            btnCreate.Text = "Tạo đơn ";
            btnCreate.UseVisualStyleBackColor = false;
            // 
            // btnView
            // 
            btnView.BackColor = System.Drawing.SystemColors.ActiveBorder;
            btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnView.Location = new System.Drawing.Point(336, 32);
            btnView.Name = "btnView";
            btnView.Size = new System.Drawing.Size(86, 35);
            btnView.TabIndex = 3;
            btnView.Text = "Xem chi tiết ";
            btnView.UseVisualStyleBackColor = false;
            // 
            // btnConfirm
            // 
            btnConfirm.BackColor = System.Drawing.SystemColors.ActiveBorder;
            btnConfirm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnConfirm.Location = new System.Drawing.Point(445, 33);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new System.Drawing.Size(75, 34);
            btnConfirm.TabIndex = 4;
            btnConfirm.Text = "Xác nhận";
            btnConfirm.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = System.Drawing.Color.IndianRed;
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnCancel.Location = new System.Drawing.Point(539, 33);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 34);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Hủy đơn";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnSearch.Location = new System.Drawing.Point(843, 33);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(70, 34);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.SystemColors.ActiveBorder;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnRefresh.Location = new System.Drawing.Point(948, 29);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(75, 31);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.SystemColors.InactiveCaption;
            txtSearch.Location = new System.Drawing.Point(635, 37);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(202, 23);
            txtSearch.TabIndex = 8;
            // 
            // OrderManagementPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(txtSearch);
            Controls.Add(btnRefresh);
            Controls.Add(btnSearch);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(btnView);
            Controls.Add(btnCreate);
            Controls.Add(dgvOrders);
            Controls.Add(lblTotal);
            Size = new System.Drawing.Size(1357, 613);
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTotal;
        private DataGridView dgvOrders;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtSearch;
    }
}
