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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelTop = new System.Windows.Forms.Panel();
            lblSearch = new System.Windows.Forms.Label();
            txtSearch = new System.Windows.Forms.TextBox();
            btnSearch = new System.Windows.Forms.Button();
            btnAdd = new System.Windows.Forms.Button();
            btnEdit = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnReload = new System.Windows.Forms.Button();
            panelBottom = new System.Windows.Forms.Panel();
            lblTotal = new System.Windows.Forms.Label();
            dataGridView = new System.Windows.Forms.DataGridView();
            dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            panelTop.SuspendLayout();
            panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(lblSearch);
            panelTop.Controls.Add(txtSearch);
            panelTop.Controls.Add(btnSearch);
            panelTop.Controls.Add(btnAdd);
            panelTop.Controls.Add(btnEdit);
            panelTop.Controls.Add(btnDelete);
            panelTop.Controls.Add(btnReload);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(800, 40);
            panelTop.TabIndex = 2;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblSearch.Location = new System.Drawing.Point(10, 11);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new System.Drawing.Size(53, 15);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Từ khóa:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new System.Drawing.Point(70, 8);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(200, 23);
            txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new System.Drawing.Point(280, 5);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(60, 23);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Tìm";
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(350, 5);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(60, 23);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Thêm";
            // 
            // btnEdit
            // 
            btnEdit.Location = new System.Drawing.Point(420, 5);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(60, 23);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Sửa";
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(490, 5);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(60, 23);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Xóa";
            // 
            // btnReload
            // 
            btnReload.Location = new System.Drawing.Point(560, 5);
            btnReload.Name = "btnReload";
            btnReload.Size = new System.Drawing.Size(60, 23);
            btnReload.TabIndex = 6;
            btnReload.Text = "Reload";
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(lblTotal);
            panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelBottom.Location = new System.Drawing.Point(0, 570);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new System.Drawing.Size(800, 30);
            panelBottom.TabIndex = 1;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblTotal.Location = new System.Drawing.Point(5, 5);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(107, 19);
            lblTotal.TabIndex = 0;
            lblTotal.Text = "Tổng số kho: 0";
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7 });
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(0, 40);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new System.Drawing.Size(800, 530);
            dataGridView.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "STT";
            dataGridViewTextBoxColumn1.Name = "STT";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Code";
            dataGridViewTextBoxColumn2.Name = "Code";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Name";
            dataGridViewTextBoxColumn3.Name = "Name";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Address";
            dataGridViewTextBoxColumn4.Name = "Address";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Phone";
            dataGridViewTextBoxColumn5.Name = "Phone";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Manager";
            dataGridViewTextBoxColumn6.Name = "Manager";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Status";
            dataGridViewTextBoxColumn7.Name = "Status";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // WarehousesManagementPanel
            // 
            Controls.Add(dataGridView);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Size = new System.Drawing.Size(800, 600);
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

        #endregion
    }
}
