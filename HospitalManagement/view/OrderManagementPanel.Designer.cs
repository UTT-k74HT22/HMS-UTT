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
            dgvOrders.Location = new Point(3, 85);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.Size = new Size(1323, 472);
            dgvOrders.TabIndex = 1;
            // 
            // btnCreate
            // 
            btnCreate.Location = new Point(246, 33);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(75, 34);
            btnCreate.TabIndex = 2;
            btnCreate.Text = "Tạo đơn ";
            btnCreate.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            btnView.Location = new Point(336, 32);
            btnView.Name = "btnView";
            btnView.Size = new Size(86, 35);
            btnView.TabIndex = 3;
            btnView.Text = "Xem chi tiết ";
            btnView.UseVisualStyleBackColor = true;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(445, 33);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(75, 34);
            btnConfirm.TabIndex = 4;
            btnConfirm.Text = "Xác nhận";
            btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(539, 33);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 34);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Hủy đơn";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(855, 33);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 34);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(945, 32);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 31);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(647, 40);
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
        private Button btnCreate;
        private Button btnView;
        private Button btnConfirm;
        private Button btnCancel;
        private Button btnSearch;
        private Button btnRefresh;
        private TextBox txtSearch;
    }
}
