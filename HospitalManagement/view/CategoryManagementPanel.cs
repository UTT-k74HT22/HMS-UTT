using HospitalManagement.controller;
using HospitalManagement.dto.response.Category;
using HospitalManagement.entity;
using HospitalManagement.service.impl;
using HospitalManagement.repository.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HospitalManagement.view
{
    // ComboBox hiển thị Name nhưng lưu Id
    public class ComboBoxItem
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }

    public partial class CategoryManagementPanel : UserControl
    {
        private readonly CategoryController controller;
        private List<CategoryResponse> currentCategories;

        public CategoryManagementPanel()
        {
            InitializeComponent();

            controller = new CategoryController(
                new CategoryServiceImpl(
                    new CategoryRepositoryImpl(
                        "Server=localhost;Database=hms;User Id=sa;Password=123456789;TrustServerCertificate=True;")
                )
            );

            // Event
            btnSearch.Click += (s, e) => SearchCategories();
            btnRefresh.Click += (s, e) =>
            {
                txtKeyword.Clear();
                LoadCategories();
            };
            btnAdd.Click += (s, e) => OpenCategoryDialog(null);
            btnEdit.Click += (s, e) => EditSelectedCategory();
            btnDelete.Click += (s, e) => DeleteSelectedCategory();

            LoadCategories();
        }

        private void LoadCategories()
        {
            dgvCategory.Columns.Clear();
            dgvCategory.Rows.Clear();

            dgvCategory.Columns.AddRange(new[]
            {
                new DataGridViewTextBoxColumn { Name = "STT", HeaderText = "STT" },
                new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Code" },
                new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name" },
                new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "Description" },
                new DataGridViewTextBoxColumn { Name = "ParentId", HeaderText = "Parent ID" },
                new DataGridViewTextBoxColumn { Name = "ParentName", HeaderText = "Parent Name" },
                new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status" },
                new DataGridViewTextBoxColumn { Name = "DisplayOrder", HeaderText = "Display Order" }
            });

            currentCategories = controller.GetAllCategories();

            int stt = 1;
            foreach (var c in currentCategories)
            {
                dgvCategory.Rows.Add(
                    stt++, c.Code, c.Name, c.Description,
                    c.ParentId?.ToString() ?? "", c.ParentName ?? "",
                    c.Active ? "ACTIVE" : "INACTIVE",
                    c.DisplayOrder ?? 1
                );
            }

            lblTotal.Text = $"Tổng số category: {currentCategories.Count(c => c.Active)}";
        }

        private void SearchCategories()
        {
            string kw = txtKeyword.Text.Trim();
            var filtered = string.IsNullOrWhiteSpace(kw) ? currentCategories : controller.SearchCategories(kw);

            dgvCategory.Rows.Clear();
            int stt = 1;
            foreach (var c in filtered)
            {
                dgvCategory.Rows.Add(
                    stt++, c.Code, c.Name, c.Description,
                    c.ParentId?.ToString() ?? "", c.ParentName ?? "",
                    c.Active ? "ACTIVE" : "INACTIVE",
                    c.DisplayOrder ?? 1
                );
            }

            lblTotal.Text = $"Tổng số category: {filtered.Count}";
        }

        private void DeleteSelectedCategory()
        {
            if (dgvCategory.CurrentRow == null) return;
            string code = dgvCategory.CurrentRow.Cells["Code"].Value.ToString();

            if (MessageBox.Show($"Bạn có chắc chắn muốn xoá category [{code}]?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                controller.DeleteCategory(code);
                LoadCategories();
                MessageBox.Show("Xoá category thành công", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditSelectedCategory()
        {
            if (dgvCategory.CurrentRow == null) return;
            string code = dgvCategory.CurrentRow.Cells["Code"].Value.ToString();
            Category cat = controller.GetCategoryByCode(code);
            OpenCategoryDialog(cat);
        }

        private void OpenCategoryDialog(Category category)
        {
            var panel = new Panel { Size = new System.Drawing.Size(360, 250) };

            // TextBox và các control
            var txtCode = new TextBox { Location = new System.Drawing.Point(120, 18), Width = 200 };
            var txtName = new TextBox { Location = new System.Drawing.Point(120, 58), Width = 200 };
            var txtDesc = new TextBox { Location = new System.Drawing.Point(120, 98), Width = 200 };
            var cmbParent = new ComboBox
            {
                Location = new System.Drawing.Point(120, 138),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            var chkActive = new CheckBox
                { Text = "Active", Location = new System.Drawing.Point(120, 180), AutoSize = true };

            // Label
            panel.Controls.AddRange(new Control[]
            {
                new Label { Text = "Code:", Location = new System.Drawing.Point(10, 20), AutoSize = true },
                txtCode,
                new Label { Text = "Name:", Location = new System.Drawing.Point(10, 60), AutoSize = true },
                txtName,
                new Label { Text = "Description:", Location = new System.Drawing.Point(10, 100), AutoSize = true },
                txtDesc,
                new Label { Text = "Parent:", Location = new System.Drawing.Point(10, 140), AutoSize = true },
                cmbParent,
                chkActive
            });

            // Populate Parent ComboBox
            cmbParent.Items.Add(new ComboBoxItem { Id = null, Name = "None" });
            foreach (var c in currentCategories.Where(c => category == null || c.Id != category.Id))
                cmbParent.Items.Add(new ComboBoxItem { Id = c.Id, Name = c.Name });

            // Nếu là edit
            if (category != null)
            {
                txtCode.Text = category.Code;
                txtCode.Enabled = false;
                txtName.Text = category.Name;
                txtDesc.Text = category.Description;
                chkActive.Checked = category.IsActive;

                // Chọn parent hiện tại
                cmbParent.SelectedIndex = category.ParentId.HasValue
                    ? cmbParent.Items.Cast<ComboBoxItem>().ToList().FindIndex(x => x.Id == category.ParentId)
                    : 0;
            }
            else
            {
                cmbParent.SelectedIndex = 0;
                chkActive.Checked = true;
            }

            // Tạo Form
            var form = new Form
            {
                Text = category == null ? "Tạo Category" : "Sửa Category",
                ClientSize = new System.Drawing.Size(panel.Width, panel.Height + 50), // tăng 50px cho nút
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };
            
            

            form.Controls.Add(panel);

            // Buttons
            var btnOK = new Button
            {
                Text = "OK", DialogResult = DialogResult.OK, Location = new System.Drawing.Point(80, panel.Bottom +10), Width = 80
            };
            var btnCancel = new Button
            {
                Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new System.Drawing.Point(180, panel.Bottom +10),
                Width = 80
            };
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);
            form.AcceptButton = btnOK;
            form.CancelButton = btnCancel;

            if (form.ShowDialog() != DialogResult.OK) return;

            // Validation
            if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Code và Name không được để trống", "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy parentId an toàn
                long? parentIdLong = (cmbParent.SelectedItem as ComboBoxItem)?.Id;
                int? parentId = parentIdLong.HasValue ? (int)parentIdLong.Value : (int?)null;
                long parentKey = parentIdLong ?? 0;

                // Tính displayOrder
                int displayOrder;
                if (category == null) // Thêm mới
                {
                    var siblings = currentCategories.Where(c => (c.ParentId ?? 0) == parentKey);
                    displayOrder = siblings.Any() ? siblings.Max(c => c.DisplayOrder ?? 0) + 1 : 1;

                    controller.CreateCategory(new Category
                    {
                        Code = txtCode.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Description = txtDesc.Text.Trim(),
                        ParentId = parentId,
                        IsActive = chkActive.Checked,
                        DisplayOrder = displayOrder
                    });

                    MessageBox.Show("Tạo category thành công!", "Success", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else // Sửa
                {
                    long oldParentKey = category.ParentId ?? 0;
                    if (oldParentKey != parentKey) // parent đổi
                    {
                        var siblings =
                            currentCategories.Where(c => (c.ParentId ?? 0) == parentKey && c.Id != category.Id);
                        displayOrder = siblings.Any() ? siblings.Max(c => c.DisplayOrder ?? 0) + 1 : 1;
                    }
                    else // parent không đổi
                    {
                        displayOrder = category.DisplayOrder;
                    }

                    // Cập nhật category
                    category.Name = txtName.Text.Trim();
                    category.Description = txtDesc.Text.Trim();
                    category.ParentId = parentId;
                    category.IsActive = chkActive.Checked;
                    category.DisplayOrder = displayOrder;

                    controller.UpdateCategory(category);
                    MessageBox.Show("Cập nhật category thành công!", "Success", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

