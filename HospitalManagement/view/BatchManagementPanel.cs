using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.dto.request.Batch;
using HospitalManagement.dto.response;

namespace HospitalManagement.view
{
    public partial class BatchManagementPanel : UserControl
    {
        private const int WARNING_DAYS = 30;

        private readonly BatchController _controller;
        private List<BatchResponse> _allBatches = new();

        public BatchManagementPanel(BatchController controller)
        {
            InitializeComponent();
            _controller = controller;

            InitGrid();
            InitEvents();
            LoadData();
        }

        // ================= INIT =================

        private void InitGrid()
        {
            dgvBatches.AutoGenerateColumns = false;
            dgvBatches.AllowUserToAddRows = false;
            dgvBatches.ReadOnly = true;
            dgvBatches.AllowUserToDeleteRows = false;
            dgvBatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBatches.RowHeadersVisible = false;
            dgvBatches.MultiSelect = false;
            dgvBatches.BackgroundColor = Color.White;

            dgvBatches.Columns.Clear();
            dgvBatches.Columns.Add("stt", "STT");
            dgvBatches.Columns.Add("code", "Mã lô");
            dgvBatches.Columns.Add("product", "Sản phẩm");
            dgvBatches.Columns.Add("price", "Giá nhập");
            dgvBatches.Columns.Add("mfg", "Ngày SX");
            dgvBatches.Columns.Add("exp", "Hạn dùng");
            dgvBatches.Columns.Add("supplier", "Nhà cung cấp");
            dgvBatches.Columns.Add("status", "Trạng thái");
            dgvBatches.Columns.Add("warn", "Cảnh báo");

            dgvBatches.CellFormatting += DgvBatches_CellFormatting;
        }
        private void BatchManagementPanel_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void InitEvents()
        {
            btnSearch.Click += (_, _) => ApplyFilter();
            btnRefresh.Click += (_, _) => LoadData();
            btnAddBatch.Click += (_, _) => OpenAddEditDialog(null);
            btnEdit.Click += (_, _) => OpenAddEditDialog(GetSelected());
            btnDisable.Click += (_, _) => DisableBatch();
            btnExpiring.Click += (_, _) => ShowExpiringOnly();
        }

        // ================= LOAD =================

        private void LoadData()
        {
            _allBatches = _controller.GetAll();
            BindGrid(_allBatches);
        }

        private void BindGrid(List<BatchResponse> list)
        {
            dgvBatches.Rows.Clear();
            int stt = 1;

            foreach (var b in list)
            {
                dgvBatches.Rows.Add(
                    stt++,
                    b.BatchCode,
                    b.ProductName,
                    b.ImportPrice,
                    b.ManufactureDate?.ToShortDateString(),
                    b.ExpiryDate?.ToShortDateString(),
                    b.SupplierName,
                    b.Status,
                    ResolveWarning(b.ExpiryDate ?? DateTime.MaxValue)
                );
            }

            lblTotal.Text = $"Tổng số lô hàng: {list.Count}";
        }

        // ================= FILTER =================

        private void ApplyFilter()
        {
            string kw = txtKeyword.Text.Trim();
            if (string.IsNullOrEmpty(kw))
            {
                BindGrid(_controller.GetAll());
                return;
            }

            BindGrid(_controller.FindByBatchCode(kw));
        }

        private void ShowExpiringOnly()
        {
            var limit = DateTime.Today.AddDays(WARNING_DAYS);
            var list = _allBatches.Where(b => b.ExpiryDate <= limit).ToList();
            BindGrid(list);
            lblTotal.Text = $"Lô sắp / đã hết hạn: {list.Count}";
        }

        // ================= WARNING =================

        private string ResolveWarning(DateTime expiry)
        {
            var today = DateTime.Today;
            if (expiry < today) return "HẾT HẠN";
            if (expiry <= today.AddDays(WARNING_DAYS)) return "SẮP HẾT HẠN";
            return "BÌNH THƯỜNG";
        }

        private void DgvBatches_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 8 && e.Value != null)
            {
                var v = e.Value.ToString();
                if (v == "HẾT HẠN")
                    dgvBatches.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                else if (v == "SẮP HẾT HẠN")
                    dgvBatches.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Khaki;
            }
        }

        // ================= CRUD =================

        private BatchResponse? GetSelected()
        {
            if (dgvBatches.CurrentRow == null) return null;
            int idx = dgvBatches.CurrentRow.Index;
            return idx >= 0 && idx < _allBatches.Count ? _allBatches[idx] : null;
        }

        private void DisableBatch()
        {
            var b = GetSelected();
            if (b == null) return;

            if (MessageBox.Show("Ngưng sử dụng lô này?", "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _controller.Disable(b.Id!.Value);
                LoadData();
            }
        }

        // ================= ADD / EDIT =================

        private void OpenAddEditDialog(BatchResponse? edit)
        {
            bool isEdit = edit != null;

            var dlg = new Form
            {
                Text = isEdit ? "Cập nhật lô hàng" : "Thêm lô hàng",
                Size = new Size(420, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(12),
                GrowStyle = TableLayoutPanelGrowStyle.AddRows
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

            void AddRow(string label, Control c)
            {
                int row = panel.RowCount++;
                panel.RowStyles.Add(new RowStyle(SizeType.Absolute ,42));

                panel.Controls.Add(new Label
                {
                    Text = label,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight
                }, 0, row);

                c.Dock = DockStyle.Fill;
                panel.Controls.Add(c, 1, row);
            }

            var txtCode = new TextBox();
            var cbProduct = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            var txtPrice = new TextBox();
            var txtSupplier = new TextBox();
            var dtMfg = new DateTimePicker();
            var dtExp = new DateTimePicker();
            var cbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };

            cbStatus.Items.AddRange(new[] { "ACTIVE", "EXPIRED", "BLOCKED", "DEPLETED" });
            cbStatus.SelectedIndex = 0;

            cbProduct.DataSource = _controller.GetAllProducts();
            cbProduct.DisplayMember = "Name";
            cbProduct.ValueMember = "Id";

            if (!isEdit) AddRow("Mã lô *", txtCode);
            AddRow("Sản phẩm *", cbProduct);
            AddRow("Giá nhập *", txtPrice);
            AddRow("Nhà cung cấp", txtSupplier);
            AddRow("Ngày SX", dtMfg);
            AddRow("Hạn dùng *", dtExp);
            AddRow("Trạng thái", cbStatus);

            if (isEdit)
            {
                txtCode.Text = edit!.BatchCode;
                txtCode.Enabled = false;

                cbProduct.SelectedValue = edit.ProductId;
                cbProduct.Enabled = false;

                txtPrice.Text = edit.ImportPrice.ToString();
                txtPrice.Enabled = false;

                txtSupplier.Text = edit.SupplierName;
                txtSupplier.Enabled = false;

                dtMfg.Value = edit.ManufactureDate ?? DateTime.Today;
                dtExp.Value = edit.ExpiryDate ?? DateTime.Today;
                cbStatus.SelectedItem = edit.Status;
            }

            var btnSave = new Button { Text = "Lưu", Width = 90 };
            var btnCancel = new Button { Text = "Hủy", Width = 90 };

            btnSave.Click += (_, _) =>
            {
                if (!decimal.TryParse(txtPrice.Text, out var price))
                {
                    MessageBox.Show("Giá nhập không hợp lệ");
                    return;
                }

                if (isEdit)
                {
                    _controller.Update(edit!.Id!.Value, new UpdateBatchRequest
                    {
                        ExpiryDate = dtExp.Value,
                        Status = cbStatus.SelectedItem!.ToString()!
                    });
                }
                else
                {
                    _controller.Create(new CreateBatchRequest
                    {
                        BatchCode = txtCode.Text.Trim(),
                        ProductId = (long)cbProduct.SelectedValue,
                        ImportPrice = price,
                        SupplierName = txtSupplier.Text.Trim(),
                        ManufactureDate = dtMfg.Value,
                        ExpiryDate = dtExp.Value,
                        Status = cbStatus.SelectedItem!.ToString()!
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
                Height = 45
            };
            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnSave);

            dlg.Controls.Add(panel);
            dlg.Controls.Add(btnPanel);
            dlg.ShowDialog(this);
        }
    }
}
