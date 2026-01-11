namespace HospitalManagement.view
{
    partial class OrderDetailForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvItems = new DataGridView();
            lblOrderCode = new Label();
            btnClose = new Button();
            lblCustomerName = new Label();
            lblOrderDate = new Label();
            lblStatus = new Label();
            lblTotal = new Label();
            lblCustomerPhone = new Label();
            lblCustomerEmail = new Label();
            lblShippingAddress = new Label();
            lblCreatorName = new Label();
            lblCreatorPhone = new Label();
            lblCreatorEmail = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();
            // 
            // dgvItems
            // 
            dgvItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Location = new Point(12, 197);
            dgvItems.Name = "dgvItems";
            dgvItems.Size = new Size(776, 203);
            dgvItems.TabIndex = 0;
            // 
            // lblOrderCode
            // 
            lblOrderCode.AutoSize = true;
            lblOrderCode.Location = new Point(34, 18);
            lblOrderCode.Name = "lblOrderCode";
            lblOrderCode.Size = new Size(51, 15);
            lblOrderCode.TabIndex = 1;
            lblOrderCode.Text = "Mã đơn:";
            // 
            // btnClose
            // 
            btnClose.Location = new Point(674, 415);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 2;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // lblCustomerName
            // 
            lblCustomerName.AutoSize = true;
            lblCustomerName.Location = new Point(34, 61);
            lblCustomerName.Name = "lblCustomerName";
            lblCustomerName.Size = new Size(73, 15);
            lblCustomerName.TabIndex = 3;
            lblCustomerName.Text = "Khách hàng:";
            // 
            // lblOrderDate
            // 
            lblOrderDate.AutoSize = true;
            lblOrderDate.Location = new Point(219, 18);
            lblOrderDate.Name = "lblOrderDate";
            lblOrderDate.Size = new Size(58, 15);
            lblOrderDate.TabIndex = 4;
            lblOrderDate.Text = "Ngày tạo:";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(363, 18);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(63, 15);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Trạng thái:";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(570, 18);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(61, 15);
            lblTotal.TabIndex = 6;
            lblTotal.Text = "Tổng tiền:";
            // 
            // lblCustomerPhone
            // 
            lblCustomerPhone.AutoSize = true;
            lblCustomerPhone.Location = new Point(219, 61);
            lblCustomerPhone.Name = "lblCustomerPhone";
            lblCustomerPhone.Size = new Size(31, 15);
            lblCustomerPhone.TabIndex = 7;
            lblCustomerPhone.Text = "SĐT:";
            // 
            // lblCustomerEmail
            // 
            lblCustomerEmail.AutoSize = true;
            lblCustomerEmail.Location = new Point(363, 61);
            lblCustomerEmail.Name = "lblCustomerEmail";
            lblCustomerEmail.Size = new Size(39, 15);
            lblCustomerEmail.TabIndex = 8;
            lblCustomerEmail.Text = "Email:";
            // 
            // lblShippingAddress
            // 
            lblShippingAddress.AutoSize = true;
            lblShippingAddress.Location = new Point(34, 109);
            lblShippingAddress.Name = "lblShippingAddress";
            lblShippingAddress.Size = new Size(102, 15);
            lblShippingAddress.TabIndex = 9;
            lblShippingAddress.Text = "Địa chỉ giao hàng:";
            // 
            // lblCreatorName
            // 
            lblCreatorName.AutoSize = true;
            lblCreatorName.Location = new Point(34, 148);
            lblCreatorName.Name = "lblCreatorName";
            lblCreatorName.Size = new Size(63, 15);
            lblCreatorName.TabIndex = 10;
            lblCreatorName.Text = "Người tạo:";
            // 
            // label10
            // 
            lblCreatorEmail.AutoSize = true;
            lblCreatorEmail.Location = new Point(219, 148);
            lblCreatorEmail.Name = "label10";
            lblCreatorEmail.Size = new Size(39, 15);
            lblCreatorEmail.TabIndex = 11;
            lblCreatorEmail.Text = "Email:";
            // 
            // label11
            // 
            lblCreatorPhone.AutoSize = true;
            lblCreatorPhone.Location = new Point(460, 148);
            lblCreatorPhone.Name = "label11";
            lblCreatorPhone.Size = new Size(85, 15);
            lblCreatorPhone.TabIndex = 12;
            lblCreatorPhone.Text = "SĐT người tạo:";
            // 
            // OrderDetailForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblCreatorPhone);
            Controls.Add(lblCreatorEmail);
            Controls.Add(lblCreatorName);
            Controls.Add(lblShippingAddress);
            Controls.Add(lblCustomerEmail);
            Controls.Add(lblCustomerPhone);
            Controls.Add(lblTotal);
            Controls.Add(lblStatus);
            Controls.Add(lblOrderDate);
            Controls.Add(lblCustomerName);
            Controls.Add(btnClose);
            Controls.Add(lblOrderCode);
            Controls.Add(dgvItems);
            Name = "OrderDetailForm";
            Text = "OrderDetailForm";
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvItems;
        private Label lblOrderCode;
        private Button btnClose;
        private Label lblCustomerName;
        private Label lblOrderDate;
        private Label lblStatus;
        private Label lblTotal;
        private Label lblCustomerPhone;
        private Label lblCustomerEmail;
        private Label lblShippingAddress;
        private Label lblCreatorName;
        private Label lblCreatorEmail;
        private Label lblCreatorPhone;
    }
}