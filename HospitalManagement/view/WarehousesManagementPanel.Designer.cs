namespace HospitalManagement.view
{
    partial class WarehousesManagementPanel
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblSearch;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.panelTop = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();

            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();

            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblSearch.Text = "Từ khóa:";
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(10, 11);
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 9);
            

            // ==================== panelTop ====================
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Height = 40;
            this.panelTop.Controls.Add(this.lblSearch);


            this.panelTop.Controls.Add(this.txtSearch);
            this.panelTop.Controls.Add(this.btnSearch);
            this.panelTop.Controls.Add(this.btnAdd);
            this.panelTop.Controls.Add(this.btnEdit);
            this.panelTop.Controls.Add(this.btnDelete);
            this.panelTop.Controls.Add(this.btnReload);

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(70, 8);
            this.txtSearch.Width = 200;

            // btnSearch
            this.btnSearch.Text = "Tìm";
            this.btnSearch.Location = new System.Drawing.Point(280, 5);
            this.btnSearch.Width = 60;

            // btnAdd
            this.btnAdd.Text = "Thêm";
            this.btnAdd.Location = new System.Drawing.Point(350, 5);
            this.btnAdd.Width = 60;

            // btnEdit
            this.btnEdit.Text = "Sửa";
            this.btnEdit.Location = new System.Drawing.Point(420, 5);
            this.btnEdit.Width = 60;

            // btnDelete
            this.btnDelete.Text = "Xóa";
            this.btnDelete.Location = new System.Drawing.Point(490, 5);
            this.btnDelete.Width = 60;

            // btnReload
            this.btnReload.Text = "Reload";
            this.btnReload.Location = new System.Drawing.Point(560, 5);
            this.btnReload.Width = 60;

            // ==================== dataGridView ====================
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.ReadOnly = true;

            this.dataGridView.Columns.Add("STT", "STT");
            this.dataGridView.Columns.Add("Code", "Code");
            this.dataGridView.Columns.Add("Name", "Name");
            this.dataGridView.Columns.Add("Address", "Address");
            this.dataGridView.Columns.Add("Phone", "Phone");
            this.dataGridView.Columns.Add("Manager", "Manager");
            this.dataGridView.Columns.Add("Status", "Status");

            // ==================== panelBottom ====================
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Height = 30;

            this.panelBottom.Controls.Add(this.lblTotal);

            // lblTotal
            this.lblTotal.Text = "Tổng số kho: 0";
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(5, 5);
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);

            // ==================== WarehouseManagementPanel ====================
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Size = new System.Drawing.Size(800, 600);
        }

        #endregion
    }
}
