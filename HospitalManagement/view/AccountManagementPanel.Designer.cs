namespace HospitalManagement.view
{
    partial class AccountManagementPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlRoot = new System.Windows.Forms.TableLayoutPanel();
            pnlToolbar = new System.Windows.Forms.TableLayoutPanel();
            pnlFilters = new System.Windows.Forms.FlowLayoutPanel();
            lblKeyword = new System.Windows.Forms.Label();
            txtKeyword = new System.Windows.Forms.TextBox();
            btnSearch = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            btnAdd = new System.Windows.Forms.Button();
            btnEdit = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnDetail = new System.Windows.Forms.Button();
            btnExport = new System.Windows.Forms.Button();
            dgvAccounts = new System.Windows.Forms.DataGridView();
            pnlFooter = new System.Windows.Forms.Panel();
            lblTotal = new System.Windows.Forms.Label();
            pnlRoot.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRoot
            // 
            pnlRoot.ColumnCount = 1;
            pnlRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlRoot.Controls.Add(pnlToolbar, 0, 0);
            pnlRoot.Controls.Add(dgvAccounts, 0, 1);
            pnlRoot.Controls.Add(pnlFooter, 0, 2);
            pnlRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRoot.Location = new System.Drawing.Point(0, 0);
            pnlRoot.Name = "pnlRoot";
            pnlRoot.Padding = new System.Windows.Forms.Padding(16);
            pnlRoot.RowCount = 3;
            pnlRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            pnlRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            pnlRoot.Size = new System.Drawing.Size(1137, 600);
            pnlRoot.TabIndex = 0;
            // 
            // pnlToolbar
            // 
            pnlToolbar.ColumnCount = 1;
            pnlToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlToolbar.Controls.Add(pnlFilters, 0, 0);
            pnlToolbar.Controls.Add(pnlActions, 0, 1);
            pnlToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlToolbar.Location = new System.Drawing.Point(16, 16);
            pnlToolbar.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.RowCount = 2;
            pnlToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            pnlToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            pnlToolbar.Size = new System.Drawing.Size(1105, 80);
            pnlToolbar.TabIndex = 0;
            // 
            // pnlFilters
            // 
            pnlFilters.AutoSize = true;
            pnlFilters.Controls.Add(lblKeyword);
            pnlFilters.Controls.Add(txtKeyword);
            pnlFilters.Controls.Add(btnSearch);
            pnlFilters.Controls.Add(btnRefresh);
            pnlFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlFilters.Location = new System.Drawing.Point(0, 0);
            pnlFilters.Margin = new System.Windows.Forms.Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            pnlFilters.Size = new System.Drawing.Size(1105, 40);
            pnlFilters.TabIndex = 0;
            pnlFilters.WrapContents = false;
            // 
            // lblKeyword
            // 
            lblKeyword.AutoSize = true;
            lblKeyword.Location = new System.Drawing.Point(0, 16);
            lblKeyword.Margin = new System.Windows.Forms.Padding(0, 10, 8, 0);
            lblKeyword.Name = "lblKeyword";
            lblKeyword.Size = new System.Drawing.Size(57, 15);
            lblKeyword.TabIndex = 0;
            lblKeyword.Text = "Tìm kiếm";
            // 
            // txtKeyword
            // 
            txtKeyword.Location = new System.Drawing.Point(65, 12);
            txtKeyword.Margin = new System.Windows.Forms.Padding(0, 6, 10, 0);
            txtKeyword.Name = "txtKeyword";
            txtKeyword.Size = new System.Drawing.Size(220, 23);
            txtKeyword.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new System.Drawing.Point(295, 12);
            btnSearch.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(86, 26);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Tìm";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new System.Drawing.Point(389, 12);
            btnRefresh.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(86, 26);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // pnlActions
            // 
            pnlActions.AutoSize = true;
            pnlActions.Controls.Add(btnAdd);
            pnlActions.Controls.Add(btnEdit);
            pnlActions.Controls.Add(btnDelete);
            pnlActions.Controls.Add(btnDetail);
            pnlActions.Controls.Add(btnExport);
            pnlActions.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlActions.Location = new System.Drawing.Point(0, 40);
            pnlActions.Margin = new System.Windows.Forms.Padding(0);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            pnlActions.Size = new System.Drawing.Size(1105, 40);
            pnlActions.TabIndex = 1;
            pnlActions.WrapContents = false;
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(0, 12);
            btnAdd.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(95, 26);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            btnEdit.Location = new System.Drawing.Point(103, 12);
            btnEdit.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(95, 26);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(206, 12);
            btnDelete.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(95, 26);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnDetail
            // 
            btnDetail.Location = new System.Drawing.Point(309, 12);
            btnDetail.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            btnDetail.Name = "btnDetail";
            btnDetail.Size = new System.Drawing.Size(95, 26);
            btnDetail.TabIndex = 3;
            btnDetail.Text = "Chi tiết";
            btnDetail.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            btnExport.Location = new System.Drawing.Point(412, 12);
            btnExport.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            btnExport.Name = "btnExport";
            btnExport.Size = new System.Drawing.Size(95, 26);
            btnExport.TabIndex = 4;
            btnExport.Text = "Export Excel";
            btnExport.UseVisualStyleBackColor = true;
            // 
            // dgvAccounts
            // 
            dgvAccounts.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvAccounts.Location = new System.Drawing.Point(16, 108);
            dgvAccounts.Margin = new System.Windows.Forms.Padding(0);
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.Size = new System.Drawing.Size(1105, 436);
            dgvAccounts.TabIndex = 1;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(lblTotal);
            pnlFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlFooter.Location = new System.Drawing.Point(16, 556);
            pnlFooter.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new System.Drawing.Size(1105, 28);
            pnlFooter.TabIndex = 2;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = System.Windows.Forms.DockStyle.Left;
            lblTotal.Location = new System.Drawing.Point(0, 0);
            lblTotal.Margin = new System.Windows.Forms.Padding(0);
            lblTotal.Name = "lblTotal";
            lblTotal.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            lblTotal.Size = new System.Drawing.Size(62, 25);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số: 0";
            // 
            // AccountManagementPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlRoot);
            Size = new System.Drawing.Size(1137, 600);
            pnlRoot.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlFilters.ResumeLayout(false);
            pnlFilters.PerformLayout();
            pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
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
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Button btnExport;

        private System.Windows.Forms.DataGridView dgvAccounts;

        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblTotal;
    }
}
