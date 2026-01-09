using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response.Product;
using HospitalManagement.entity.enums;

namespace HospitalManagement.view
{
    public partial class ProductManagementPanel : UserControl
    {
        private readonly ProductController _controller;

        private List<ProductResponse> _allProducts = new();
        private readonly BindingSource _bs = new();


        // ===== MOCK DATA (TẠM THỜI) =====
        private readonly List<(long Id, string Name)> _categories = new()
        {
            (1, "Thuốc"),
            (2, "Thực phẩm chức năng"),
            (3, "Vật tư y tế")
        };

        private readonly List<(long Id, string Name)> _manufacturers = new()
        {
            (1, "Dược Hậu Giang"),
            (2, "Traphaco"),
            (3, "Sanofi")
        };
        private void InitGrid()
        {
            dgvProducts.AutoGenerateColumns = true;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.AllowUserToResizeColumns = false;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;

            // ===== CỘT STT (THÊM 1 LẦN DUY NHẤT) =====
            if (!dgvProducts.Columns.Contains("STT"))
            {
                var colStt = new DataGridViewTextBoxColumn
                {
                    Name = "STT",            
                    HeaderText = "STT",
                    Width = 50,
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };

                dgvProducts.Columns.Insert(0, colStt);
            }
        }



        public ProductManagementPanel()
        {
            InitializeComponent();
            _controller = new ProductController();

            dgvProducts.AutoGenerateColumns = true;
            dgvProducts.DataSource = _bs;
            dgvProducts.AllowUserToResizeColumns = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            InitGrid();


            InitStatusCombo();
            InitEvents();
        }

        private void ProductManagementPanel_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // ================= INIT =================

        private void InitStatusCombo()
        {
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
            ApplyFilters();
        }

        private void BindGrid(List<ProductResponse> data)
        {
            _bs.DataSource = data;

            for (int i = 0; i < dgvProducts.Rows.Count; i++)
            {
                dgvProducts.Rows[i].Cells["STT"].Value = i + 1;
            }

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
                (string.IsNullOrEmpty(kw) ||
                 p.Code.ToLower().Contains(kw) ||
                 p.Name.ToLower().Contains(kw) ||
                 p.CategoryName.ToLower().Contains(kw) ||
                 p.ManufacturerName.ToLower().Contains(kw))
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
            var p = GetSelected();
            if (p == null) return;

            MessageBox.Show(
                $"Mã: {p.Code}\nTên: {p.Name}\nDanh mục: {p.CategoryName}\n" +
                $"NSX: {p.ManufacturerName}\nGiá: {p.StandardPrice}\nTrạng thái: {p.Status}",
                "Chi tiết sản phẩm",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // ================= ADD / EDIT DIALOG (INLINE) =================

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

    // ===== Layout =====
    var panel = new TableLayoutPanel
    {
        Dock = DockStyle.Fill,
        ColumnCount = 2,
        Padding = new Padding(16),
        AutoSize = true,
        AutoScroll = true
    };

    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

    void AddRow(string labelText, Control control)
    {
        var label = new Label
        {
            Text = labelText,
            AutoSize = true,
            Anchor = AnchorStyles.Right
        };

        control.Dock = DockStyle.Fill;
        control.Margin = new Padding(3, 6, 3, 6);

        panel.Controls.Add(label);
        panel.Controls.Add(control);
    }

    // ===== Fields =====
    var txtCode = new TextBox();
    var txtName = new TextBox();
    var txtBarcode = new TextBox();
    var txtDosage = new TextBox();
    var txtUnit = new TextBox();
    var txtPrice = new TextBox();

    var cbCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
    var cbManufacturer = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
    var cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

    var chkRx = new CheckBox { Text = "Cần toa thuốc" };

    var txtDesc = new TextBox
    {
        Multiline = true,
        Height = 60
    };

    // ===== Bind data =====
    cbCategory.DataSource = _categories;
    cbCategory.DisplayMember = "Name";
    cbCategory.ValueMember = "Id";

    cbManufacturer.DataSource = _manufacturers;
    cbManufacturer.DisplayMember = "Name";
    cbManufacturer.ValueMember = "Id";

    cbStatus.Items.AddRange(Enum.GetNames(typeof(ProductStatus)));
    cbStatus.SelectedIndex = 0;

    // ===== Add rows =====
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

    // ===== Load edit data =====
    if (isEdit)
    {
        txtCode.Text = edit!.Code;
        txtCode.Enabled = false;

        txtName.Text = edit.Name;
        txtPrice.Text = edit.StandardPrice.ToString();

        chkRx.Checked = edit.RequiresPrescription;
        cbStatus.SelectedItem = edit.Status.ToString();
        
        cbCategory.Text = edit.CategoryName;
        cbManufacturer.Text = edit.ManufacturerName;
    }

    // ===== Buttons =====
    var btnSave = new Button { Text = "Lưu", Width = 90 };
    var btnCancel = new Button { Text = "Hủy", Width = 90 };

    btnSave.Click += (_, _) =>
    {
        if (string.IsNullOrWhiteSpace(txtCode.Text) ||
            string.IsNullOrWhiteSpace(txtName.Text) ||
            string.IsNullOrWhiteSpace(txtPrice.Text))
        {
            MessageBox.Show("Vui lòng nhập đầy đủ các trường bắt buộc (*)");
            return;
        }

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
                CategoryId = (dynamic)cbCategory.SelectedValue,
                ManufacturerId = (dynamic)cbManufacturer.SelectedValue
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
                CategoryId = (dynamic)cbCategory.SelectedValue,
                ManufacturerId = (dynamic)cbManufacturer.SelectedValue
            });
        }

        dlg.Close();
        LoadData();
    };

    btnCancel.Click += (_, _) => dlg.Close();

    var btnPanel = new FlowLayoutPanel
    {
        Dock = DockStyle.Bottom,
        FlowDirection = FlowDirection.RightToLeft,
        Padding = new Padding(8),
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
