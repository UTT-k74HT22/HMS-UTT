namespace HospitalManagement.view
{
    partial class CustomerProfileManagementPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlRoot = new TableLayoutPanel();
            pnlToolbar = new TableLayoutPanel();
            pnlFilters = new FlowLayoutPanel();
            lblKeyword = new Label();
            txtKeyword = new TextBox();
            btnSearch = new Button();
            btnRefresh = new Button();
            pnlActions = new FlowLayoutPanel();
            btnEdit = new Button();
            btnDelete = new Button();
            btnDetail = new Button();
            dgvCustomers = new DataGridView();
            pnlFooter = new Panel();
            lblTotal = new Label();
            pnlRoot.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRoot
            // 
            pnlRoot.ColumnCount = 1;
            pnlRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlRoot.Controls.Add(pnlToolbar, 0, 0);
            pnlRoot.Controls.Add(dgvCustomers, 0, 1);
            pnlRoot.Controls.Add(pnlFooter, 0, 2);
            pnlRoot.Dock = DockStyle.Fill;
            pnlRoot.Location = new Point(0, 0);
            pnlRoot.Name = "pnlRoot";
            pnlRoot.Padding = new Padding(16);
            pnlRoot.RowCount = 3;
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            pnlRoot.Size = new Size(1137, 600);
            pnlRoot.TabIndex = 0;
            // 
            // pnlToolbar
            // 
            pnlToolbar.ColumnCount = 1;
            pnlToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlToolbar.Controls.Add(pnlFilters, 0, 0);
            pnlToolbar.Controls.Add(pnlActions, 0, 1);
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(16, 16);
            pnlToolbar.Margin = new Padding(0, 0, 0, 12);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.RowCount = 2;
            pnlToolbar.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            pnlToolbar.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            pnlToolbar.Size = new Size(1105, 80);
            pnlToolbar.TabIndex = 0;
            // 
            // pnlFilters
            // 
            pnlFilters.AutoSize = true;
            pnlFilters.Controls.Add(lblKeyword);
            pnlFilters.Controls.Add(txtKeyword);
            pnlFilters.Controls.Add(btnSearch);
            pnlFilters.Controls.Add(btnRefresh);
            pnlFilters.Dock = DockStyle.Fill;
            pnlFilters.Location = new Point(3, 3);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(0, 6, 0, 0);
            pnlFilters.Size = new Size(1099, 34);
            pnlFilters.TabIndex = 0;
            pnlFilters.WrapContents = false;
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new Point(0, 16);
            lblKeyword.Margin = new Padding(0, 10, 8, 0);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new Size(57, 15);
            lblKeyword.TabIndex = 0;
            lblKeyword.Text = "Tìm kiếm";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new Point(65, 12);
            txtKeyword.Margin = new Padding(0, 6, 10, 0);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new Size(220, 23);
            txtKeyword.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(295, 12);
            btnSearch.Margin = new Padding(0, 6, 8, 0);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(86, 26);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Tìm";
            btnSearch.Click += btnSearch_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(389, 12);
            btnRefresh.Margin = new Padding(0, 6, 0, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(86, 26);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Làm mới";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // pnlActions
            // 
            pnlActions.AutoSize = true;
            pnlActions.Controls.Add(btnEdit);
            pnlActions.Controls.Add(btnDelete);
            pnlActions.Controls.Add(btnDetail);
            pnlActions.Dock = DockStyle.Fill;
            pnlActions.Location = new Point(3, 43);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new Padding(0, 6, 0, 0);
            pnlActions.Size = new Size(1099, 34);
            pnlActions.TabIndex = 1;
            pnlActions.WrapContents = false;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.Gold;
            btnEdit.Location = new Point(0, 12);
            btnEdit.Margin = new Padding(0, 6, 8, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(95, 26);
            btnEdit.TabIndex = 0;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.Crimson;
            btnDelete.Location = new Point(103, 12);
            btnDelete.Margin = new Padding(0, 6, 8, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(95, 26);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnDetail
            // 
            btnDetail.Location = new Point(206, 12);
            btnDetail.Margin = new Padding(0, 6, 0, 0);
            btnDetail.Name = "btnDetail";
            btnDetail.Size = new Size(95, 26);
            btnDetail.TabIndex = 2;
            btnDetail.Text = "Chi tiết";
            // 
            // dgvCustomers
            // 
            dgvCustomers.BackgroundColor = SystemColors.Window;
            dgvCustomers.Dock = DockStyle.Fill;
            dgvCustomers.Location = new Point(19, 111);
            dgvCustomers.Name = "dgvCustomers";
            dgvCustomers.Size = new Size(1099, 430);
            dgvCustomers.TabIndex = 1;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(lblTotal);
            pnlFooter.Dock = DockStyle.Fill;
            pnlFooter.Location = new Point(16, 556);
            pnlFooter.Margin = new Padding(0, 12, 0, 0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1105, 28);
            pnlFooter.TabIndex = 2;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = DockStyle.Left;
            lblTotal.Location = new Point(0, 0);
            lblTotal.Name = "lblTotal";
            lblTotal.Padding = new Padding(0, 10, 0, 0);
            lblTotal.Size = new Size(62, 25);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số: 0";
            // 
            // CustomerProfileManagementPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlRoot);
            Name = "CustomerProfileManagementPanel";
            Size = new Size(1137, 600);
            pnlRoot.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlRoot;
        private System.Windows.Forms.TableLayoutPanel pnlToolbar;
        private System.Windows.Forms.FlowLayoutPanel pnlFilters;
        private System.Windows.Forms.Label lblKeyword;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnDetail;

        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblTotal;
    }
}
