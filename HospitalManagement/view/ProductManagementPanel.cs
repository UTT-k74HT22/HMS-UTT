using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Category;
using HospitalManagement.dto.response.Product;
using HospitalManagement.entity.enums;
using HospitalManagement.Service.Impl;
using Microsoft.Extensions.Configuration;

namespace HospitalManagement.view
{
    public partial class ProductManagementPanel : UserControl
    {
        private readonly ProductController _controller;

        private List<ProductResponse> _allProducts = new();
        private List<CategoryResponse> _categories = new();
        private List<ManufacturerResponse> _manufacturers = new();

        private readonly BindingSource _bs = new();

        // ================= CONSTRUCTOR =================

        public ProductManagementPanel()
        {
            InitializeComponent();

            // ===== Load appsettings.json =====
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString =
                config.GetConnectionString("DefaultConnection");
            // ===== Khởi tạo service + controller =====
            var productService = new ProductServiceImpl(connectionString);
            _controller = new ProductController(productService);

            dgvProducts.DataSource = _bs;

            InitGrid();
            InitStatusCombo();
            InitEvents();
        }

        private void ProductManagementPanel_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // ================= INIT =================

        private void InitGrid()
        {
            dgvProducts.AutoGenerateColumns = true;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.AllowUserToResizeColumns = false;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.BackgroundColor = Color.White;

            if (!dgvProducts.Columns.Contains("STT"))
            {
                dgvProducts.Columns.Insert(0, new DataGridViewTextBoxColumn
                {
                    Name = "STT",
                    HeaderText = "STT",
                    Width = 50,
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                });
            }
        }

        private void InitStatusCombo()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("ALL");
            cbStatus.Items.AddRange(Enum.GetNames(typeof(ProductStatus)));
            cbStatus.SelectedIndex = 0;
        }

        private void InitEvents()
        {
            btnSearch.Click += (_, _) => ApplyFilters();
            btnRefresh.Click += (_, _) => LoadData();
            cbStatus.SelectedIndexChanged += (_, _) => ApplyFilters();

            btnAdd.Click += (_, _) => OpenAddEditDialog(null);
            btnEdit.Click += (_, _) => OpenAddEditDialog(GetSelected());
            btnDelete.Click += (_, _) => DeleteSelected();
            btnDetail.Click += (_, _) => ShowDetail();
        }

        // ================= LOAD DATA =================

        private void LoadData()
        {
            _allProducts = _controller.GetAll();
            _categories = _controller.GetAllCategories();
            _manufacturers = _controller.GetAllManufacturers();

            ApplyFilters();
        }

        private void BindGrid(List<ProductResponse> data)
        {
            _bs.DataSource = data;

            for (int i = 0; i < dgvProducts.Rows.Count; i++)
                dgvProducts.Rows[i].Cells["STT"].Value = i + 1;

            dgvProducts.Columns[nameof(ProductResponse.Id)].Visible = false;

            dgvProducts.Columns[nameof(ProductResponse.Code)].HeaderText = "Mã SP";
            dgvProducts.Columns[nameof(ProductResponse.Name)].HeaderText = "Tên sản phẩm";
            dgvProducts.Columns[nameof(ProductResponse.CategoryName)].HeaderText = "Danh mục";
            dgvProducts.Columns[nameof(ProductResponse.ManufacturerName)].HeaderText = "Nhà sản xuất";
            dgvProducts.Columns[nameof(ProductResponse.StandardPrice)].HeaderText = "Giá";
            dgvProducts.Columns[nameof(ProductResponse.RequiresPrescription)].HeaderText = "Toa thuốc";
            dgvProducts.Columns[nameof(ProductResponse.Status)].HeaderText = "Trạng thái";

            lblTotal.Text = $"Tổng số sản phẩm: {data.Count}";
        }

        // ================= FILTER =================

        private void ApplyFilters()
        {
            string kw = txtKeyword.Text.Trim().ToLower();
            string status = cbStatus.SelectedItem?.ToString() ?? "ALL";

            var filtered = _allProducts.Where(p =>
                (string.IsNullOrEmpty(kw)
                 || p.Code.ToLower().Contains(kw)
                 || p.Name.ToLower().Contains(kw)
                 || p.CategoryName.ToLower().Contains(kw)
                 || p.ManufacturerName.ToLower().Contains(kw))
                &&
                (status == "ALL" || p.Status.ToString() == status)
            ).ToList();

            BindGrid(filtered);
        }

        // ================= CRUD =================

        private ProductResponse? GetSelected()
            => dgvProducts.CurrentRow?.DataBoundItem as ProductResponse;

        private void DeleteSelected()
        {
            var p = GetSelected();
            if (p == null) return;

            if (MessageBox.Show($"Xóa [{p.Code}] ?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _controller.Delete(p.Code);
                LoadData();
            }
        }

        private void ShowDetail()
        {
            var selected = GetSelected();
            if (selected == null) return;

            var detail = _controller.GetDetail(selected.Code);

            MessageBox.Show(
                $"Mã: {detail.Code}\n" +
                $"Tên: {detail.Name}\n" +
                $"Barcode: {detail.Barcode}\n" +
                $"Danh mục: {detail.CategoryName}\n" +
                $"Nhà sản xuất: {detail.ManufacturerName}\n" +
                $"Quốc gia NSX: {detail.ManufacturerCountry}\n" +
                $"Dạng bào chế: {detail.DosageForm}\n" +
                $"Đơn vị: {detail.Unit}\n" +
                $"Giá: {detail.StandardPrice}\n" +
                $"Cần toa thuốc: {(detail.RequiresPrescription ? "Có" : "Không")}\n" +
                $"Trạng thái: {detail.Status}\n\n" +
                $"Mô tả:\n{detail.Description}",
                "Chi tiết sản phẩm",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // ================= ADD / EDIT =================

        private void OpenAddEditDialog(ProductResponse? edit)
        {
            bool isEdit = edit != null;

            var dlg = new Form
            {
                Text = isEdit ? "Sửa sản phẩm" : "Thêm sản phẩm",
                Size = new Size(520, 520),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(16),
                AutoScroll = true
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            void AddRow(string label, Control c)
            {
                panel.Controls.Add(new Label { Text = label, Anchor = AnchorStyles.Right });
                c.Dock = DockStyle.Fill;
                panel.Controls.Add(c);
            }

            var txtCode = new TextBox();
            var txtName = new TextBox();
            var txtPrice = new TextBox();
            var txtBarcode = new TextBox();
            var txtDosage = new TextBox();
            var txtUnit = new TextBox();
            var txtDesc = new TextBox { Multiline = true, Height = 60 };
            var chkRx = new CheckBox { Text = "Cần toa thuốc" };

            var cbCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            var cbManufacturer = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            var cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

            cbCategory.DataSource = _categories;
            cbCategory.DisplayMember = nameof(CategoryResponse.Name);
            cbCategory.ValueMember = nameof(CategoryResponse.Id);

            cbManufacturer.DataSource = _manufacturers;
            cbManufacturer.DisplayMember = nameof(ManufacturerResponse.Name);
            cbManufacturer.ValueMember = nameof(ManufacturerResponse.Id);

            cbStatus.Items.AddRange(Enum.GetNames(typeof(ProductStatus)));

            AddRow("Mã *", txtCode);
            AddRow("Tên *", txtName);
            AddRow("Barcode", txtBarcode);
            AddRow("Danh mục *", cbCategory);
            AddRow("Nhà sản xuất", cbManufacturer);
            AddRow("Dạng bào chế", txtDosage);
            AddRow("Đơn vị", txtUnit);
            AddRow("Giá *", txtPrice);
            AddRow("Trạng thái", cbStatus);
            AddRow("", chkRx);
            AddRow("Mô tả", txtDesc);

            if (isEdit)
            {
                txtCode.Text = edit!.Code;
                txtCode.Enabled = false;

                txtName.Text = edit.Name;
                txtPrice.Text = edit.StandardPrice.ToString();
                chkRx.Checked = edit.RequiresPrescription;
                cbStatus.SelectedItem = edit.Status.ToString();
            }
            else
            {
                cbStatus.SelectedIndex = 0;
            }

            var btnSave = new Button { Text = "Lưu", Width = 90 };
            btnSave.Click += (_, _) =>
            {
                if (!decimal.TryParse(txtPrice.Text, out var price))
                {
                    MessageBox.Show("Giá không hợp lệ");
                    return;
                }

                if (isEdit)
                {
                    _controller.Update(edit!.Code, new UpdateProductRequest
                    {
                        Name = txtName.Text.Trim(),
                        Barcode = txtBarcode.Text.Trim(),
                        DosageForm = txtDosage.Text.Trim(),
                        Unit = txtUnit.Text.Trim(),
                        Description = txtDesc.Text.Trim(),
                        StandardPrice = price,
                        RequiresPrescription = chkRx.Checked,
                        Status = Enum.Parse<ProductStatus>(cbStatus.SelectedItem!.ToString()!),
                        CategoryId = (long)cbCategory.SelectedValue,
                        ManufacturerId = (long?)cbManufacturer.SelectedValue
                    });
                }
                else
                {
                    _controller.Create(new CreateProductRequest
                    {
                        Code = txtCode.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Barcode = txtBarcode.Text.Trim(),
                        DosageForm = txtDosage.Text.Trim(),
                        Unit = txtUnit.Text.Trim(),
                        Description = txtDesc.Text.Trim(),
                        StandardPrice = price,
                        RequiresPrescription = chkRx.Checked,
                        Status = Enum.Parse<ProductStatus>(cbStatus.SelectedItem!.ToString()!),
                        CategoryId = (long)cbCategory.SelectedValue,
                        ManufacturerId = (long?)cbManufacturer.SelectedValue
                    });
                }

                dlg.Close();
                LoadData();
            };

            var btnCancel = new Button { Text = "Hủy", Width = 90 };
            btnCancel.Click += (_, _) => dlg.Close();

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 50
            };
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnSave);

            dlg.Controls.Add(panel);
            dlg.Controls.Add(btnPanel);
            dlg.ShowDialog(this);
        }
    }
}
