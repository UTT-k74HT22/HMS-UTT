namespace HospitalManagement.view
{
    partial class CategoryManagementPanel
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvCategory = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvCategory)).BeginInit();
            this.SuspendLayout();

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Text = "Từ khóa:";

            // txtKeyword
            this.txtKeyword.Location = new System.Drawing.Point(90, 16);
            this.txtKeyword.Size = new System.Drawing.Size(260, 23);

            // btnSearch
            this.btnSearch.Location = new System.Drawing.Point(360, 15);
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
            this.btnSearch.Text = "Tìm";

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(20, 60);
            this.btnAdd.Size = new System.Drawing.Size(75, 28);
            this.btnAdd.Text = "Thêm";

            // btnEdit
            this.btnEdit.Location = new System.Drawing.Point(110, 60);
            this.btnEdit.Size = new System.Drawing.Size(75, 28);
            this.btnEdit.Text = "Sửa";

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(200, 60);
            this.btnDelete.Size = new System.Drawing.Size(75, 28);
            this.btnDelete.Text = "Xóa";

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(290, 60);
            this.btnRefresh.Size = new System.Drawing.Size(75, 28);
            this.btnRefresh.Text = "Refresh";

            // dgvCategory
            this.dgvCategory.Location = new System.Drawing.Point(20, 110);
            this.dgvCategory.Size = new System.Drawing.Size(900, 450);
            this.dgvCategory.ReadOnly = true;
            this.dgvCategory.AllowUserToAddRows = false;
            this.dgvCategory.AllowUserToDeleteRows = false;
            this.dgvCategory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCategory.MultiSelect = false;
            this.dgvCategory.RowHeadersVisible = false;
            this.dgvCategory.AutoGenerateColumns = false;

            // lblTotal
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(20, 580);
            this.lblTotal.Text = "Tổng số category: 0";

            // Add controls
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvCategory);
            this.Controls.Add(this.lblTotal);

            this.Size = new System.Drawing.Size(950, 620);
            this.Name = "CategoryManagementPanel";

            ((System.ComponentModel.ISupportInitialize)(this.dgvCategory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        // Declare controls
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvCategory;
        private System.Windows.Forms.Label lblTotal;
    }
}
