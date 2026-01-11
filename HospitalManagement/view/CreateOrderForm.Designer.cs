namespace HospitalManagement.view
{
    partial class CreateOrderForm
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            lblTotal = new Label();
            txtCustomerId = new TextBox();
            txtShipping = new TextBox();
            txtDiscount = new TextBox();
            dgvProducts = new DataGridView();
            label11 = new Label();
            cbCategory = new ComboBox();
            dgvOrderItems = new DataGridView();
            cbWarehouse = new ComboBox();
            cbBatch = new ComboBox();
            nudQuantity = new NumericUpDown();
            btnAddItem = new Button();
            btnDeleteAll = new Button();
            btnDeleteItem = new Button();
            btnCancel = new Button();
            btnCreateOrder = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudQuantity).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(53, 16);
            label1.Name = "label1";
            label1.Size = new Size(116, 15);
            label1.TabIndex = 0;
            label1.Text = "Thông tin đơn hàng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(316, 42);
            label2.Name = "label2";
            label2.Size = new Size(103, 15);
            label2.TabIndex = 1;
            label2.Text = "Địa chỉ giao hàng:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(682, 42);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 2;
            label3.Text = "Giảm giá:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(53, 42);
            label4.Name = "label4";
            label4.Size = new Size(80, 15);
            label4.TabIndex = 3;
            label4.Text = "Customer ID:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(520, 128);
            label5.Name = "label5";
            label5.Size = new Size(124, 15);
            label5.TabIndex = 4;
            label5.Text = "Sản phẩm trong đơn ";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(49, 409);
            label6.Name = "label6";
            label6.Size = new Size(118, 15);
            label6.TabIndex = 5;
            label6.Text = "Chọn kho và lô hàng";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(53, 458);
            label7.Name = "label7";
            label7.Size = new Size(32, 15);
            label7.TabIndex = 6;
            label7.Text = "Kho:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(247, 458);
            label8.Name = "label8";
            label8.Size = new Size(52, 15);
            label8.TabIndex = 7;
            label8.Text = "Lô Hàng";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(507, 458);
            label9.Name = "label9";
            label9.Size = new Size(60, 15);
            label9.TabIndex = 8;
            label9.Text = "Số Lượng";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotal.Location = new Point(518, 409);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(63, 15);
            lblTotal.TabIndex = 9;
            lblTotal.Text = "Tổng tiền:";
            // 
            // txtCustomerId
            // 
            txtCustomerId.BackColor = SystemColors.ScrollBar;
            txtCustomerId.Location = new Point(53, 60);
            txtCustomerId.Name = "txtCustomerId";
            txtCustomerId.Size = new Size(143, 23);
            txtCustomerId.TabIndex = 10;
            // 
            // txtShipping
            // 
            txtShipping.BackColor = SystemColors.ScrollBar;
            txtShipping.Location = new Point(316, 60);
            txtShipping.Name = "txtShipping";
            txtShipping.Size = new Size(263, 23);
            txtShipping.TabIndex = 11;
            // 
            // txtDiscount
            // 
            txtDiscount.BackColor = SystemColors.ScrollBar;
            txtDiscount.Location = new Point(682, 60);
            txtDiscount.Name = "txtDiscount";
            txtDiscount.Size = new Size(143, 23);
            txtDiscount.TabIndex = 12;
            // 
            // dgvProducts
            // 
            dgvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvProducts.BackgroundColor = SystemColors.ButtonFace;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Location = new Point(53, 166);
            dgvProducts.Name = "dgvProducts";
            dgvProducts.Size = new Size(431, 220);
            dgvProducts.TabIndex = 13;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.Location = new Point(53, 110);
            label11.Name = "label11";
            label11.Size = new Size(134, 15);
            label11.TabIndex = 14;
            label11.Text = "Danh mục và sản phẩm";
            // 
            // cbCategory
            // 
            cbCategory.BackColor = SystemColors.InactiveCaption;
            cbCategory.ForeColor = SystemColors.WindowText;
            cbCategory.FormattingEnabled = true;
            cbCategory.Location = new Point(53, 128);
            cbCategory.Name = "cbCategory";
            cbCategory.Size = new Size(431, 23);
            cbCategory.TabIndex = 15;
            // 
            // dgvOrderItems
            // 
            dgvOrderItems.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrderItems.BackgroundColor = SystemColors.ButtonFace;
            dgvOrderItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrderItems.Location = new Point(520, 166);
            dgvOrderItems.Name = "dgvOrderItems";
            dgvOrderItems.Size = new Size(500, 220);
            dgvOrderItems.TabIndex = 16;
            // 
            // cbWarehouse
            // 
            cbWarehouse.FormattingEnabled = true;
            cbWarehouse.Location = new Point(90, 455);
            cbWarehouse.Name = "cbWarehouse";
            cbWarehouse.Size = new Size(151, 23);
            cbWarehouse.TabIndex = 17;
            // 
            // cbBatch
            // 
            cbBatch.FormattingEnabled = true;
            cbBatch.Location = new Point(305, 455);
            cbBatch.Name = "cbBatch";
            cbBatch.Size = new Size(166, 23);
            cbBatch.TabIndex = 18;
            // 
            // nudQuantity
            // 
            nudQuantity.Location = new Point(587, 456);
            nudQuantity.Name = "nudQuantity";
            nudQuantity.Size = new Size(69, 23);
            nudQuantity.TabIndex = 19;
            // 
            // btnAddItem
            // 
            btnAddItem.BackColor = Color.Gray;
            btnAddItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddItem.ForeColor = SystemColors.ButtonFace;
            btnAddItem.Location = new Point(507, 497);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(149, 23);
            btnAddItem.TabIndex = 20;
            btnAddItem.Text = "Thêm vào đơn hàng";
            btnAddItem.UseVisualStyleBackColor = false;
            // 
            // btnDeleteAll
            // 
            btnDeleteAll.BackColor = Color.Coral;
            btnDeleteAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteAll.Location = new Point(531, 555);
            btnDeleteAll.Name = "btnDeleteAll";
            btnDeleteAll.Size = new Size(75, 23);
            btnDeleteAll.TabIndex = 21;
            btnDeleteAll.Text = "Xóa tất cả";
            btnDeleteAll.UseVisualStyleBackColor = false;
            // 
            // btnDeleteItem
            // 
            btnDeleteItem.BackColor = Color.FromArgb(255, 128, 128);
            btnDeleteItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteItem.Location = new Point(624, 555);
            btnDeleteItem.Name = "btnDeleteItem";
            btnDeleteItem.Size = new Size(97, 23);
            btnDeleteItem.TabIndex = 22;
            btnDeleteItem.Text = "Xóa một dòng";
            btnDeleteItem.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.DarkCyan;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(732, 555);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 23;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnCreateOrder
            // 
            btnCreateOrder.BackColor = Color.Green;
            btnCreateOrder.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCreateOrder.Location = new Point(825, 555);
            btnCreateOrder.Name = "btnCreateOrder";
            btnCreateOrder.Size = new Size(108, 23);
            btnCreateOrder.TabIndex = 24;
            btnCreateOrder.Text = "Tạo đơn hàng ";
            btnCreateOrder.UseVisualStyleBackColor = false;
            // 
            // CreateOrderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1025, 590);
            Controls.Add(btnCreateOrder);
            Controls.Add(btnCancel);
            Controls.Add(btnDeleteItem);
            Controls.Add(btnDeleteAll);
            Controls.Add(btnAddItem);
            Controls.Add(nudQuantity);
            Controls.Add(cbBatch);
            Controls.Add(cbWarehouse);
            Controls.Add(dgvOrderItems);
            Controls.Add(cbCategory);
            Controls.Add(label11);
            Controls.Add(dgvProducts);
            Controls.Add(txtDiscount);
            Controls.Add(txtShipping);
            Controls.Add(txtCustomerId);
            Controls.Add(lblTotal);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "CreateOrderForm";
            Text = "CreateOrderForm";
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudQuantity).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lblTotal;
        private TextBox txtCustomerId;
        private TextBox txtShipping;
        private TextBox txtDiscount;
        private DataGridView dgvProducts;
        private Label label11;
        private ComboBox cbCategory;
        private DataGridView dgvOrderItems;
        private ComboBox cbWarehouse;
        private ComboBox cbBatch;
        private NumericUpDown nudQuantity;
        private Button btnAddItem;
        private Button btnDeleteAll;
        private Button btnDeleteItem;
        private Button btnCancel;
        private Button btnCreateOrder;
    }
}