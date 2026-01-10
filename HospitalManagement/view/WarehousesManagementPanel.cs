using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.service.impl;
using HospitalManagement.repository.impl;

namespace HospitalManagement.view
{
    public partial class WarehousesManagementPanel : UserControl
    {
        private readonly WarehousesController controller;
        private List<WarehouseResponse> currentWarehouses;

        public WarehousesManagementPanel()
        {
            InitializeComponent();

            // Khởi tạo controller mặc định với service + repository
            controller = new WarehousesController(
                new WarehousesServiceImpl(
                    new WarehousesRepositoryImpl(
                        "Server=localhost;Database=hms;User Id=sa;Password=123456789;TrustServerCertificate=True;"
                    )
                )
            );

            // Event gán
            btnSearch.Click += (s, e) => SearchWarehouses();
            btnAdd.Click += (s, e) => OpenWarehouseForm(null);
            btnEdit.Click += (s, e) => EditSelectedWarehouse();
            btnDelete.Click += (s, e) => DeleteSelectedWarehouse();
            btnReload.Click += (s, e) => LoadWarehouses();

            LoadWarehouses();
        }

        // ================== LOAD & SEARCH ==================
        private void LoadWarehouses()
        {
            dataGridView.Rows.Clear();
            currentWarehouses = controller.GetAllWarehouses();

            int stt = 1;
            foreach (var w in currentWarehouses)
            {
                dataGridView.Rows.Add(
                    stt++,
                    w.Code,
                    w.Name,
                    w.Address,
                    w.Phone,
                    w.ManagerName,
                    w.IsActive ? "ACTIVE" : "INACTIVE"
                );
            }

            lblTotal.Text = $"Tổng số kho: {currentWarehouses.Count(c => c.IsActive)}";
        }

        private void SearchWarehouses()
        {
            string key = txtSearch.Text.Trim().ToLower();
            var filtered = string.IsNullOrWhiteSpace(key)
                ? currentWarehouses
                : currentWarehouses.Where(w =>
                        w.Code.ToLower().Contains(key) ||
                        w.Name.ToLower().Contains(key))
                    .ToList();

            dataGridView.Rows.Clear();
            int stt = 1;
            foreach (var w in filtered)
            {
                dataGridView.Rows.Add(
                    stt++,
                    w.Code,
                    w.Name,
                    w.Address,
                    w.Phone,
                    w.ManagerName,
                    w.IsActive ? "ACTIVE" : "INACTIVE"
                );
            }

            lblTotal.Text = $"Tổng số kho: {filtered.Count}";
        }

        // ================== CRUD ==================
        private WarehouseResponse? GetSelectedWarehouse()
        {
            if (dataGridView.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn 1 kho.");
                return null;
            }

            string code = dataGridView.CurrentRow.Cells["Code"].Value?.ToString() ?? "";
            return controller.GetWarehouseByCode(code);
        }

        private void OpenWarehouseForm(WarehouseResponse? warehouse)
        {
            bool isCreate = warehouse == null;

            var panel = new Panel { Size = new System.Drawing.Size(360, 260) };

            // Controls
            var txtCode = new TextBox { Location = new System.Drawing.Point(120, 20), Width = 200, Text = warehouse?.Code ?? "" };
            var txtName = new TextBox { Location = new System.Drawing.Point(120, 60), Width = 200, Text = warehouse?.Name ?? "" };
            var txtAddress = new TextBox { Location = new System.Drawing.Point(120, 100), Width = 200, Text = warehouse?.Address ?? "" };
            var txtPhone = new TextBox { Location = new System.Drawing.Point(120, 140), Width = 200, Text = warehouse?.Phone ?? "" };
            var txtManager = new TextBox { Location = new System.Drawing.Point(120, 180), Width = 200, Text = warehouse?.ManagerName ?? "" };
            var chkActive = new CheckBox { Text = "Active", Location = new System.Drawing.Point(120, 210), Checked = warehouse?.IsActive ?? true };

            panel.Controls.AddRange(new Control[]
            {
                new Label { Text = "Code:", Location = new System.Drawing.Point(20, 20), AutoSize = true }, txtCode,
                new Label { Text = "Name:", Location = new System.Drawing.Point(20, 60), AutoSize = true }, txtName,
                new Label { Text = "Address:", Location = new System.Drawing.Point(20, 100), AutoSize = true }, txtAddress,
                new Label { Text = "Phone:", Location = new System.Drawing.Point(20, 140), AutoSize = true }, txtPhone,
                new Label { Text = "Manager:", Location = new System.Drawing.Point(20, 180), AutoSize = true }, txtManager,
                chkActive
            });

            if (!isCreate) txtCode.Enabled = false;

            var form = new Form
            {
                Text = isCreate ? "Thêm kho" : "Sửa kho",
                ClientSize = new System.Drawing.Size(panel.Width, panel.Height + 50),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            form.Controls.Add(panel);

            var btnOK = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new System.Drawing.Point(80, panel.Bottom + 10), Width = 80 };
            var btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new System.Drawing.Point(180, panel.Bottom + 10), Width = 80 };

            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.AcceptButton = btnOK;
            form.CancelButton = btnCancel;

            if (form.ShowDialog() != DialogResult.OK) return;

            // Validation
            if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Code và Name không được để trống.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var req = new WarehouseRequest
                {
                    Code = txtCode.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    ManagerName = txtManager.Text.Trim(),
                    IsActive = chkActive.Checked
                };

                if (isCreate)
                    controller.CreateWarehouse(req);
                else
                    controller.UpdateWarehouse(txtCode.Text.Trim(), req);

                MessageBox.Show(isCreate ? "Tạo kho thành công!" : "Cập nhật kho thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadWarehouses();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditSelectedWarehouse()
        {
            var w = GetSelectedWarehouse();
            if (w != null) OpenWarehouseForm(w);
        }

        private void DeleteSelectedWarehouse()
        {
            var w = GetSelectedWarehouse();
            if (w == null) return;

            if (!w.IsActive)
            {
                MessageBox.Show("Kho này đã bị vô hiệu hóa rồi.");
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn vô hiệu hóa kho [{w.Name}]?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                controller.DeleteWarehouse(w.Code);
                LoadWarehouses();
            }
        }
    }
}
