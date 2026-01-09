namespace HospitalManagement.view
{
    partial class CategoryManagementPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Designer generated code

        private void InitializeComponent()
        {
            label1 = new Label();
            txtKeyword = new TextBox();
            btnSearch = new Button();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            dgvCategory = new DataGridView();
            lblTotal = new Label();

            ((System.ComponentModel.ISupportInitialize)dgvCategory).BeginInit();
            SuspendLayout();

            // label1
            label1.AutoSize = true;
            label1.Location = new Point(20, 20);
            label1.Text = "Từ khóa:";

            // txtKeyword
            txtKeyword.Location = new Point(90, 16);
            txtKeyword.Size = new Size(260, 23);

            // btnSearch
            btnSearch.Location = new Point(360, 15);
            btnSearch.Size = new Size(75, 25);
            btnSearch.Text = "Tìm";

            // btnAdd
            btnAdd.Location = new Point(20, 60);
            btnAdd.Size = new Size(75, 28);
            btnAdd.Text = "Thêm";

            // btnEdit
            btnEdit.Location = new Point(110, 60);
            btnEdit.Size = new Size(75, 28);
            btnEdit.Text = "Sửa";

            // btnDelete
            btnDelete.Location = new Point(200, 60);
            btnDelete.Size = new Size(75, 28);
            btnDelete.Text = "Xóa";

            // btnRefresh
            btnRefresh.Location = new Point(290, 60);
            btnRefresh.Size = new Size(75, 28);
            btnRefresh.Text = "Refresh";

            // dgvCategory
            dgvCategory.Location = new Point(20, 110);
            dgvCategory.Size = new Size(900, 450);
            dgvCategory.ReadOnly = true;
            dgvCategory.AllowUserToAddRows = false;
            dgvCategory.AllowUserToDeleteRows = false;

            // lblTotal
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(20, 580);
            lblTotal.Text = "Tổng số category: 0";

            // CategoryManagementPanel
            Controls.Add(label1);
            Controls.Add(txtKeyword);
            Controls.Add(btnSearch);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Controls.Add(dgvCategory);
            Controls.Add(lblTotal);

            Size = new Size(950, 620);
            Name = "CategoryManagementPanel";

            ((System.ComponentModel.ISupportInitialize)dgvCategory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtKeyword;
        private Button btnSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private DataGridView dgvCategory;
        private Label lblTotal;
    }
}
