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
            lblTotal = new Label();
            dgvOrders = new DataGridView();
            btnCreate = new Button();
            btnView = new Button();
            btnConfirm = new Button();
            btnCancel = new Button();
            btnSearch = new Button();
            btnRefresh = new Button();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            SuspendLayout();
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(39, 574);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(110, 15);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số đơn hàng: ";
            // 
            // dgvOrders
            // 
            dgvOrders.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrders.BackgroundColor = SystemColors.Control;
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Location = new Point(39, 73);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.Size = new Size(1255, 472);
            dgvOrders.TabIndex = 1;
            // 
            // btnCreate
            // 
            btnCreate.BackColor = Color.Silver;
            btnCreate.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCreate.Location = new Point(246, 33);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(75, 34);
            btnCreate.TabIndex = 2;
            btnCreate.Text = "Tạo đơn ";
            btnCreate.UseVisualStyleBackColor = false;
            // 
            // btnView
            // 
            btnView.BackColor = SystemColors.ActiveBorder;
            btnView.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnView.Location = new Point(336, 32);
            btnView.Name = "btnView";
            btnView.Size = new Size(86, 35);
            btnView.TabIndex = 3;
            btnView.Text = "Xem chi tiết ";
            btnView.UseVisualStyleBackColor = false;
            // 
            // btnConfirm
            // 
            btnConfirm.BackColor = SystemColors.ActiveBorder;
            btnConfirm.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConfirm.Location = new Point(445, 32);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(75, 34);
            btnConfirm.TabIndex = 4;
            btnConfirm.Text = "Xác nhận";
            btnConfirm.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.IndianRed;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(539, 32);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 34);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Hủy đơn";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = SystemColors.ActiveCaption;
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.Location = new Point(843, 30);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(70, 34);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = SystemColors.ActiveBorder;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.Location = new Point(928, 32);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 31);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = SystemColors.InactiveCaption;
            txtSearch.Location = new Point(635, 37);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(202, 23);
            txtSearch.TabIndex = 8;
            // 
            // OrderManagementPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtSearch);
            Controls.Add(btnRefresh);
            Controls.Add(btnSearch);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(btnView);
            Controls.Add(btnCreate);
            Controls.Add(dgvOrders);
            Controls.Add(lblTotal);
            Name = "OrderManagementPanel";
            Size = new Size(1357, 613);
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
