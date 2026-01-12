using HospitalManagement.controller;
using HospitalManagement.dto.response;
using HospitalManagement.entity.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HospitalManagement.dto.request;

namespace HospitalManagement.view
{
    public partial class InventoryManagementPanel : UserControl
    {
        private readonly InventoryController _inventoryController;
        private readonly WarehousesController _warehouseController;

        private readonly BindingSource _bs = new();
        private List<InventoryResponse> _all = new();

        public InventoryManagementPanel(InventoryController inventoryController, WarehousesController warehouseController)
        {
            _inventoryController = inventoryController;
            _warehouseController = warehouseController;
            
            InitializeComponent();

            dgvInventory.DataSource = _bs;

            InitGrid();
            InitEvents();
            LoadWarehouses();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                _all = _inventoryController.GetAllInventory();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadWarehouses()
        {
            try
            {
                var warehouses = _warehouseController.GetAllWarehouses();
                cboWarehouse.Items.Clear();
                cboWarehouse.Items.Add(new WarehouseItem { Id = null, Name = "TẤT CẢ KHO" });
                
                foreach (var wh in warehouses)
                {
                    cboWarehouse.Items.Add(new WarehouseItem { Id = wh.Id, Name = wh.Name });
                }
                
                cboWarehouse.DisplayMember = "Name";
                cboWarehouse.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách kho: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitGrid()
        {
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.AllowUserToResizeColumns = false;
            dgvInventory.AllowUserToAddRows = false;
            dgvInventory.AllowUserToDeleteRows = false;
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventory.MultiSelect = false;
            dgvInventory.ReadOnly = true;
            dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvInventory.Columns.Clear();

            // STT (unbound)
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                Width = 50,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            // Warehouse
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.WarehouseName),
                DataPropertyName = nameof(InventoryResponse.WarehouseName),
                HeaderText = "Kho",
                FillWeight = 15
            });

            // Product Code
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.ProductCode),
                DataPropertyName = nameof(InventoryResponse.ProductCode),
                HeaderText = "Mã SP",
                FillWeight = 10
            });

            // Product Name
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.ProductName),
                DataPropertyName = nameof(InventoryResponse.ProductName),
                HeaderText = "Tên sản phẩm",
                FillWeight = 20
            });

            // Batch Code
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.BatchCode),
                DataPropertyName = nameof(InventoryResponse.BatchCode),
                HeaderText = "Mã lô",
                FillWeight = 10
            });

            // Expiry Date
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.ExpiryDate),
                DataPropertyName = nameof(InventoryResponse.ExpiryDate),
                HeaderText = "HSD",
                FillWeight = 10
            });

            // Quantity On Hand
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.QuantityOnHand),
                DataPropertyName = nameof(InventoryResponse.QuantityOnHand),
                HeaderText = "Tồn kho",
                FillWeight = 8
            });

            // Quantity Reserved
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.QuantityReserved),
                DataPropertyName = nameof(InventoryResponse.QuantityReserved),
                HeaderText = "Đặt trước",
                FillWeight = 8
            });

            // Quantity Available
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.QuantityAvailable),
                DataPropertyName = nameof(InventoryResponse.QuantityAvailable),
                HeaderText = "Khả dụng",
                FillWeight = 8
            });

            // Min Threshold
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.MinThreshold),
                DataPropertyName = nameof(InventoryResponse.MinThreshold),
                HeaderText = "Min",
                FillWeight = 6
            });

            // Max Threshold
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(InventoryResponse.MaxThreshold),
                DataPropertyName = nameof(InventoryResponse.MaxThreshold),
                HeaderText = "Max",
                FillWeight = 6
            });

            // Status
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Trạng thái",
                FillWeight = 12
            });

            dgvInventory.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0) return;

                var item = dgvInventory.Rows[e.RowIndex].DataBoundItem as InventoryResponse;
                if (item == null) return;

                // STT
                if (dgvInventory.Columns[e.ColumnIndex].Name == "STT")
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }

                // Status
                if (dgvInventory.Columns[e.ColumnIndex].Name == "Status")
                {
                    string status;
                    if (item.IsLowStock == true) status = "Sắp hết";
                    else if (item.IsNearExpiry == true) status = "Sắp hết hạn";
                    else if (item.IsOverStock == true) status = "Dư thừa";
                    else status = "Bình thường";

                    e.Value = status;
                    e.FormattingApplied = true;

                    // Color coding
                    if (status == "Sắp hết")
                        e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69);
                    else if (status == "Sắp hết hạn")
                        e.CellStyle.ForeColor = Color.FromArgb(255, 87, 34);
                    else if (status == "Dư thừa")
                        e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7);
                }

                // Batch Code - show dash if empty
                if (dgvInventory.Columns[e.ColumnIndex].Name == nameof(InventoryResponse.BatchCode))
                {
                    if (string.IsNullOrEmpty(item.BatchCode))
                    {
                        e.Value = "-";
                        e.FormattingApplied = true;
                    }
                }

                // Expiry Date - show dash if null
                if (dgvInventory.Columns[e.ColumnIndex].Name == nameof(InventoryResponse.ExpiryDate))
                {
                    if (item.ExpiryDate == null)
                    {
                        e.Value = "-";
                        e.FormattingApplied = true;
                    }
                }
            };
        }

        private void InitEvents()
        {
            btnSearch.Click += (_, _) => ApplyFilters();
            btnRefresh.Click += (_, _) => { txtKeyword.Clear(); cboWarehouse.SelectedIndex = 0; cboStatus.SelectedIndex = 0; LoadData(); };

            btnUpdateThreshold.Click += (_, _) => UpdateThresholds();
            btnLowStock.Click += (_, _) => { cboStatus.SelectedIndex = 1; ApplyFilters(); };
            btnNearExpiry.Click += (_, _) => { cboStatus.SelectedIndex = 2; ApplyFilters(); };

            txtKeyword.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    ApplyFilters();
                }
            };

            cboWarehouse.SelectedIndexChanged += (_, _) => ApplyFilters();
            cboStatus.SelectedIndexChanged += (_, _) => ApplyFilters();
        }

        private void ApplyFilters()
        {
            var kw = (txtKeyword.Text ?? "").Trim().ToLower();
            var selectedWarehouse = cboWarehouse.SelectedItem as WarehouseItem;
            var statusFilter = cboStatus.SelectedItem?.ToString() ?? "TẤT CẢ";

            var filtered = _all.Where(x =>
            {
                // Keyword filter
                bool matchKeyword = string.IsNullOrEmpty(kw) ||
                    (x.WarehouseName?.ToLower().Contains(kw) ?? false) ||
                    (x.ProductCode?.ToLower().Contains(kw) ?? false) ||
                    (x.ProductName?.ToLower().Contains(kw) ?? false) ||
                    (x.BatchCode?.ToLower().Contains(kw) ?? false);

                if (!matchKeyword) return false;

                // Warehouse filter
                if (selectedWarehouse?.Id != null && x.WarehouseName != selectedWarehouse.Name)
                    return false;

                // Status filter
                if (statusFilter != "TẤT CẢ")
                {
                    string itemStatus;
                    if (x.IsLowStock == true) itemStatus = "SẮP HẾT HÀNG";
                    else if (x.IsNearExpiry == true) itemStatus = "SẮP HẾT HẠN";
                    else if (x.IsOverStock == true) itemStatus = "DƯ THỪA";
                    else itemStatus = "BÌNH THƯỜNG";

                    if (itemStatus != statusFilter) return false;
                }

                return true;
            }).ToList();

            _bs.DataSource = filtered;
            lblTotal.Text = $"Tổng số mục tồn kho: {filtered.Count}";
        }

        private void UpdateThresholds()
        {
            var item = GetSelected();
            if (item == null)
            {
                MessageBox.Show("Vui lòng chọn 1 dòng để cập nhật", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dialog = new InventoryThresholdDialog(item);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _inventoryController.UpdateThresholds((long)item.Id, new UpdateInventoryThresholdRequest
                    {
                        MinThreshold = dialog.MinThreshold,
                        MaxThreshold = dialog.MaxThreshold
                    });
                    MessageBox.Show("Cập nhật ngưỡng thành công!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private InventoryResponse? GetSelected()
            => dgvInventory.CurrentRow?.DataBoundItem as InventoryResponse;

        private class WarehouseItem
        {
            public long? Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }

    // ===== THRESHOLD UPDATE DIALOG =====
    public class InventoryThresholdDialog : Form
    {
        public int MinThreshold { get; private set; }
        public int MaxThreshold { get; private set; }

        private readonly TextBox txtMin;
        private readonly TextBox txtMax;

        public InventoryThresholdDialog(InventoryResponse item)
        {
            Text = "Cập nhật ngưỡng tồn kho";
            Size = new Size(400, 280);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var pnlMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(16)
            };

            pnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            pnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            pnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            pnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            pnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            pnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            pnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            // Labels and values
            pnlMain.Controls.Add(new Label { Text = "Sản phẩm:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            pnlMain.Controls.Add(new Label { Text = item.ProductName, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 1, 0);

            pnlMain.Controls.Add(new Label { Text = "Kho:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
            pnlMain.Controls.Add(new Label { Text = item.WarehouseName, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 1, 1);

            pnlMain.Controls.Add(new Label { Text = "Min Threshold *:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 2);
            txtMin = new TextBox { Text = item.MinThreshold.ToString(), Dock = DockStyle.Fill };
            pnlMain.Controls.Add(txtMin, 1, 2);

            pnlMain.Controls.Add(new Label { Text = "Max Threshold *:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 3);
            txtMax = new TextBox { Text = item.MaxThreshold.ToString(), Dock = DockStyle.Fill };
            pnlMain.Controls.Add(txtMax, 1, 3);

            // Buttons
            var pnlButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 10, 0, 0)
            };

            var btnCancel = new Button { Text = "Hủy", Width = 80, Height = 30 };
            btnCancel.Click += (_, _) => DialogResult = DialogResult.Cancel;

            var btnSave = new Button { Text = "Lưu", Width = 80, Height = 30 };
            btnSave.Click += (_, _) =>
            {
                try
                {
                    MinThreshold = int.Parse(txtMin.Text.Trim());
                    MaxThreshold = int.Parse(txtMax.Text.Trim());

                    if (MinThreshold < 0 || MaxThreshold < 0)
                        throw new Exception("Min/Max phải >= 0");
                    if (MinThreshold > MaxThreshold)
                        throw new Exception("Min không được lớn hơn Max");

                    DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnSave);

            pnlMain.Controls.Add(pnlButtons, 0, 4);
            pnlMain.SetColumnSpan(pnlButtons, 2);

            Controls.Add(pnlMain);
        }
    }
}
