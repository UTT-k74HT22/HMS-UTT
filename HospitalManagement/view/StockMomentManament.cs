using HospitalManagement.controller;
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.router;
using HospitalManagement.utils.excel.core;
using HospitalManagement.utils.excel.writers;

namespace HospitalManagement.view
{
    public partial class StockMovementManagementPanel : UserControl
    {
        private readonly StockMovementController _stockMovementController;
        private readonly WarehousesController _warehouseController;
        private readonly ProductController _productController;
        private readonly BatchController _batchController;

        private readonly BindingSource _bs = new();
        private List<StockMovementResponse> _all = new();

        public StockMovementManagementPanel(
            StockMovementController stockMovementController,
            WarehousesController warehouseController,
            ProductController productController,
            BatchController batchController)
        {
            _stockMovementController = stockMovementController;
            _warehouseController = warehouseController;
            _productController = productController;
            _batchController = batchController;
            
            InitializeComponent();

            dgvStockMovement.DataSource = _bs;

            InitGrid();
            InitEvents();
            LoadWarehouses();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                _all = _stockMovementController.GetAllMovements();
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
            dgvStockMovement.AutoGenerateColumns = false;
            dgvStockMovement.AllowUserToResizeColumns = false;
            dgvStockMovement.AllowUserToAddRows = false;
            dgvStockMovement.AllowUserToDeleteRows = false;
            dgvStockMovement.RowHeadersVisible = false;
            dgvStockMovement.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStockMovement.MultiSelect = false;
            dgvStockMovement.ReadOnly = true;
            dgvStockMovement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvStockMovement.Columns.Clear();

            // STT
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                Width = 50,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            // Movement Type
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MovementType",
                HeaderText = "Loại",
                FillWeight = 10
            });

            // Movement Date
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(StockMovementResponse.MovementDate),
                DataPropertyName = nameof(StockMovementResponse.MovementDate),
                HeaderText = "Ngày giờ",
                FillWeight = 15
            });

            // Warehouse Name
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(StockMovementResponse.WarehouseName),
                DataPropertyName = nameof(StockMovementResponse.WarehouseName),
                HeaderText = "Kho",
                FillWeight = 12
            });

            // Product Code
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(StockMovementResponse.ProductCode),
                DataPropertyName = nameof(StockMovementResponse.ProductCode),
                HeaderText = "Mã SP",
                FillWeight = 10
            });

            // Product Name
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(StockMovementResponse.ProductName),
                DataPropertyName = nameof(StockMovementResponse.ProductName),
                HeaderText = "Tên sản phẩm",
                FillWeight = 18
            });

            // Batch Code
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(StockMovementResponse.BatchCode),
                DataPropertyName = nameof(StockMovementResponse.BatchCode),
                HeaderText = "Mã lô",
                FillWeight = 10
            });

            // Quantity
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "QuantityDisplay",
                HeaderText = "SL",
                FillWeight = 8
            });

            // Quantity Before
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(StockMovementResponse.QuantityBefore),
                DataPropertyName = nameof(StockMovementResponse.QuantityBefore),
                HeaderText = "Trước",
                FillWeight = 8
            });

            // Quantity After
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(StockMovementResponse.QuantityAfter),
                DataPropertyName = nameof(StockMovementResponse.QuantityAfter),
                HeaderText = "Sau",
                FillWeight = 8
            });

            // Performed By
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(StockMovementResponse.PerformedByFullName),
                DataPropertyName = nameof(StockMovementResponse.PerformedByFullName),
                HeaderText = "Người thực hiện",
                FillWeight = 15
            });

            // Note
            dgvStockMovement.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(StockMovementResponse.Note),
                DataPropertyName = nameof(StockMovementResponse.Note),
                HeaderText = "Ghi chú",
                FillWeight = 20
            });

            dgvStockMovement.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0) return;

                var item = dgvStockMovement.Rows[e.RowIndex].DataBoundItem as StockMovementResponse;
                if (item == null) return;

                // STT
                if (dgvStockMovement.Columns[e.ColumnIndex].Name == "STT")
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }

                // Movement Type Display
                if (dgvStockMovement.Columns[e.ColumnIndex].Name == "MovementType")
                {
                    e.Value = item.MovementType switch
                    {
                        StockMovementType.IMPORT => "Nhập",
                        StockMovementType.EXPORT => "Xuất",
                        StockMovementType.ADJUST => "Điều chỉnh",
                        StockMovementType.TRANSFER => "Chuyển",
                        _ => "-"
                    };
                    e.FormattingApplied = true;
                }

                // Quantity Display with +/- sign
                if (dgvStockMovement.Columns[e.ColumnIndex].Name == "QuantityDisplay")
                {
                    string quantityDisplay;
                    if (item.MovementType == StockMovementType.ADJUST)
                    {
                        int before = item.QuantityBefore ?? 0;
                        int after = item.QuantityAfter ?? 0;
                        int delta = after - before;
                        quantityDisplay = $"{delta:+#;-#;0}";
                    }
                    else if (item.MovementType == StockMovementType.EXPORT || 
                             item.MovementType == StockMovementType.TRANSFER)
                    {
                        quantityDisplay = $"-{item.Quantity}";
                    }
                    else
                    {
                        quantityDisplay = $"+{item.Quantity}";
                    }
                    e.Value = quantityDisplay;
                    e.FormattingApplied = true;
                }

                // Date formatting
                if (dgvStockMovement.Columns[e.ColumnIndex].Name == nameof(StockMovementResponse.MovementDate))
                {
                    if (item.MovementDate != null)
                    {
                        e.Value = item.MovementDate.Value.ToString("dd/MM/yyyy HH:mm");
                        e.FormattingApplied = true;
                    }
                }

                // Handle null values
                if (e.Value == null)
                {
                    e.Value = "-";
                    e.FormattingApplied = true;
                }
            };
        }

        private void InitEvents()
        {
            btnSearch.Click += (_, _) => ApplyFilters();
            btnRefresh.Click += (_, _) => { txtKeyword.Clear(); cboWarehouse.SelectedIndex = 0; cboMovementType.SelectedIndex = 0; LoadData(); };

            btnImport.Click += (_, _) => OpenMovementDialog(StockMovementType.IMPORT);
            btnExport.Click += (_, _) => OpenMovementDialog(StockMovementType.EXPORT);
            btnAdjust.Click += (_, _) => OpenMovementDialog(StockMovementType.ADJUST);
            btnTransfer.Click += (_, _) => OpenMovementDialog(StockMovementType.TRANSFER);
            btnExportExcel.Click += (_, _) => ExportExcel();
            btnDowloadTemplate.Click += (_, _) => DownloadTemplate();
            btnImportExcel.Click += (sender, args) => ImportExcel();

            txtKeyword.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    ApplyFilters();
                }
            };

            cboWarehouse.SelectedIndexChanged += (_, _) => ApplyFilters();
            cboMovementType.SelectedIndexChanged += (_, _) => ApplyFilters();
        }

        private void ImportExcel()
        {
            throw new NotImplementedException();
        }

        private void DownloadTemplate()
        {
            throw new NotImplementedException();
        }

        private void ExportExcel()
        {
            Console.WriteLine("Export excel running...");
            
            var filteredDate = _bs.List.Cast<StockMovementResponse>().ToList();
            ExcelExporter.ExportWithDialog<StockMovementResponse>(filteredDate, new StockMovementExcelWriter(), this.FindForm());
        }

        private void ApplyFilters()
        {
            var kw = (txtKeyword.Text ?? "").Trim().ToLower();
            var selectedWarehouse = cboWarehouse.SelectedItem as WarehouseItem;
            var movementTypeFilter = cboMovementType.SelectedItem?.ToString() ?? "TẤT CẢ LOẠI";

            var filtered = _all.Where(x =>
            {
                // Keyword filter
                bool matchKeyword = string.IsNullOrEmpty(kw) ||
                    (x.WarehouseName?.ToLower().Contains(kw) ?? false) ||
                    (x.ProductCode?.ToLower().Contains(kw) ?? false) ||
                    (x.ProductName?.ToLower().Contains(kw) ?? false) ||
                    (x.BatchCode?.ToLower().Contains(kw) ?? false) ||
                    (x.PerformedByFullName?.ToLower().Contains(kw) ?? false) ||
                    (x.Note?.ToLower().Contains(kw) ?? false);

                if (!matchKeyword) return false;

                // Warehouse filter
                if (selectedWarehouse?.Id != null && x.WarehouseName != selectedWarehouse.Name)
                    return false;

                // Movement Type filter
                if (movementTypeFilter != "TẤT CẢ LOẠI")
                {
                    string itemType = x.MovementType switch
                    {
                        StockMovementType.IMPORT => "Nhập kho",
                        StockMovementType.EXPORT => "Xuất kho",
                        StockMovementType.ADJUST => "Điều chỉnh",
                        StockMovementType.TRANSFER => "Chuyển kho",
                        _ => ""
                    };

                    if (itemType != movementTypeFilter) return false;
                }

                return true;
            }).ToList();

            _bs.DataSource = filtered;
            lblTotal.Text = $"Tổng số giao dịch: {filtered.Count}";
        }

        private void OpenMovementDialog(StockMovementType movementType)
        {
            var dialog = new StockMovementDialog(movementType, _warehouseController, _productController, _batchController);

            if (dialog.ShowDialog() == DialogResult.OK && dialog.Result != null)
            {
                try
                {
                    _stockMovementController.CreateStockMovement(dialog.Result);

                    string successMsg = movementType switch
                    {
                        StockMovementType.IMPORT => "Nhập kho thành công!",
                        StockMovementType.EXPORT => "Xuất kho thành công!",
                        StockMovementType.ADJUST => "Điều chỉnh tồn kho thành công!",
                        StockMovementType.TRANSFER => "Chuyển kho thành công!",
                        _ => "Thành công!"
                    };

                    MessageBox.Show(successMsg, "Success", 
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

        private StockMovementResponse? GetSelected()
            => dgvStockMovement.CurrentRow?.DataBoundItem as StockMovementResponse;

        private class WarehouseItem
        {
            public long? Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }

    // ===== STOCK MOVEMENT DIALOG =====
    public class StockMovementDialog : Form
    {
        public CreateStockMovementRequest? Result { get; private set; }

        private readonly StockMovementType _movementType;
        private readonly WarehousesController _warehouseController;
        private readonly ProductController _productController;
        private readonly BatchController _batchController;

        private ComboBox cboWarehouse = null!;
        private ComboBox? cboDestinationWarehouse; // For TRANSFER only
        private ComboBox cboProduct = null!;
        private ComboBox cboBatch = null!;
        private TextBox txtQuantity = null!;
        private TextBox txtNote = null!;

        public StockMovementDialog(StockMovementType movementType, WarehousesController warehouseController, ProductController productController, BatchController batchController)
        {
            _movementType = movementType;
            _warehouseController = warehouseController;
            _productController = productController;
            _batchController = batchController;

            string title = movementType switch
            {
                StockMovementType.IMPORT => "Tạo phiếu nhập kho",
                StockMovementType.EXPORT => "Tạo phiếu xuất kho",
                StockMovementType.ADJUST => "Điều chỉnh tồn kho",
                StockMovementType.TRANSFER => "Chuyển kho",
                _ => "Giao dịch kho"
            };

            Text = title;
            Size = new Size(500, 420);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            InitializeDialog();
        }

        private void InitializeDialog()
        {
            var pnlMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = _movementType == StockMovementType.TRANSFER ? 8 : 7,
                Padding = new Padding(16)
            };

            pnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            pnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int rowsCount = _movementType == StockMovementType.TRANSFER ? 7 : 6;
            for (int i = 0; i < rowsCount; i++)
                pnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            pnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

            int row = 0;

            // Type Label
            pnlMain.Controls.Add(new Label { Text = "Loại:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            string typeLabel = _movementType switch
            {
                StockMovementType.IMPORT => "Nhập kho",
                StockMovementType.EXPORT => "Xuất kho",
                StockMovementType.ADJUST => "Điều chỉnh",
                StockMovementType.TRANSFER => "Chuyển kho",
                _ => "-"
            };
            pnlMain.Controls.Add(new Label { Text = typeLabel, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font(Font, FontStyle.Bold) }, 1, row++);

            // Warehouse (Source for TRANSFER)
            string warehouseLabel = _movementType == StockMovementType.TRANSFER ? "Kho nguồn *:" : "Kho *:";
            pnlMain.Controls.Add(new Label { Text = warehouseLabel, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            cboWarehouse = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            LoadWarehouses();
            pnlMain.Controls.Add(cboWarehouse, 1, row++);

            // Destination Warehouse (only for TRANSFER)
            if (_movementType == StockMovementType.TRANSFER)
            {
                pnlMain.Controls.Add(new Label { Text = "Kho đích *:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
                cboDestinationWarehouse = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
                LoadDestinationWarehouses();
                pnlMain.Controls.Add(cboDestinationWarehouse, 1, row++);
            }

            // Product
            pnlMain.Controls.Add(new Label { Text = "Sản phẩm *:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            cboProduct = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            LoadProducts();
            cboProduct.SelectedIndexChanged += (s, e) => LoadBatchesForProduct();
            pnlMain.Controls.Add(cboProduct, 1, row++);

            // Batch
            pnlMain.Controls.Add(new Label { Text = "Lô hàng *:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            cboBatch = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
            pnlMain.Controls.Add(cboBatch, 1, row++);

            // Quantity
            string qtyLabel = _movementType == StockMovementType.ADJUST ? "Số lượng mới *:" : "Số lượng *:";
            pnlMain.Controls.Add(new Label { Text = qtyLabel, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            txtQuantity = new TextBox { Dock = DockStyle.Fill };
            pnlMain.Controls.Add(txtQuantity, 1, row++);

            if (_movementType == StockMovementType.ADJUST)
            {
                pnlMain.Controls.Add(new Label { Text = "", Dock = DockStyle.Fill }, 0, row);
                pnlMain.Controls.Add(new Label 
                { 
                    Text = "(Điều chỉnh = set giá trị tuyệt đối)", 
                    Dock = DockStyle.Fill, 
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleLeft 
                }, 1, row++);
            }
            else
            {
                row++; // Skip this row
            }

            // Note
            pnlMain.Controls.Add(new Label { Text = "Ghi chú:", Dock = DockStyle.Fill }, 0, row);
            txtNote = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 80, ScrollBars = ScrollBars.Vertical };
            pnlMain.Controls.Add(txtNote, 1, row++);

            // Buttons
            var pnlButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 50,
                Padding = new Padding(16, 10, 16, 10)
            };

            var btnCancel = new Button { Text = "Hủy", Width = 80, Height = 30 };
            btnCancel.Click += (_, _) => DialogResult = DialogResult.Cancel;

            var btnSave = new Button { Text = "Lưu", Width = 80, Height = 30 };
            btnSave.Click += (_, _) => SaveMovement();

            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnSave);

            Controls.Add(pnlMain);
            Controls.Add(pnlButtons);
        }

        private void LoadWarehouses()
        {
            try
            {
                var warehouses = _warehouseController.GetAllWarehouses();
                cboWarehouse.DisplayMember = "Name";
                cboWarehouse.ValueMember = "Id";
                cboWarehouse.DataSource = warehouses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách kho: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDestinationWarehouses()
        {
            if (cboDestinationWarehouse == null) return;
            
            try
            {
                var warehouses = _warehouseController.GetAllWarehouses();
                cboDestinationWarehouse.DisplayMember = "Name";
                cboDestinationWarehouse.ValueMember = "Id";
                cboDestinationWarehouse.DataSource = warehouses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách kho đích: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                var products = _productController.GetAll();
                cboProduct.DisplayMember = "Name";
                cboProduct.ValueMember = "Id";
                cboProduct.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sản phẩm: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBatchesForProduct()
        {
            cboBatch.DataSource = null;
            cboBatch.Items.Clear();
            cboBatch.Enabled = false;

            if (cboProduct.SelectedValue != null)
            {
                long productId = Convert.ToInt64(cboProduct.SelectedValue);
                try
                {
                    var batches = _batchController.GetByProduct(productId)
                        // .Where(b => b.Status == BatchStatus.ACTIVE)
                        .ToList();

                    if (batches.Any())
                    {
                        cboBatch.DisplayMember = "BatchCode";
                        cboBatch.ValueMember = "Id";
                        cboBatch.DataSource = batches;
                        cboBatch.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải danh sách lô hàng: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveMovement()
        {
            try
            {
                if (cboWarehouse.SelectedValue == null || cboProduct.SelectedValue == null)
                {
                    throw new Exception("Vui lòng chọn kho và sản phẩm");
                }

                // Validate destination warehouse for TRANSFER
                if (_movementType == StockMovementType.TRANSFER)
                {
                    if (cboDestinationWarehouse == null || cboDestinationWarehouse.SelectedValue == null)
                    {
                        throw new Exception("Vui lòng chọn kho đích");
                    }

                    if (cboWarehouse.SelectedValue.Equals(cboDestinationWarehouse.SelectedValue))
                    {
                        throw new Exception("Kho nguồn và kho đích không được trùng nhau");
                    }
                }

                if (cboBatch.SelectedValue == null)
                {
                    throw new Exception("Vui lòng chọn lô hàng");
                }

                if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity) || quantity < 0)
                {
                    throw new Exception("Số lượng phải là số nguyên >= 0");
                }

                if (_movementType != StockMovementType.ADJUST && quantity == 0)
                {
                    throw new Exception("Số lượng phải > 0");
                }

                Result = new CreateStockMovementRequest
                {
                    MovementType = _movementType,
                    WarehouseId = Convert.ToInt64(cboWarehouse.SelectedValue),
                    DestinationWarehouseId = _movementType == StockMovementType.TRANSFER && cboDestinationWarehouse != null
                        ? Convert.ToInt64(cboDestinationWarehouse.SelectedValue) 
                        : null,
                    ProductId   = Convert.ToInt64(cboProduct.SelectedValue),
                    BatchId     = Convert.ToInt64(cboBatch.SelectedValue),
                    Quantity = quantity,
                    Note = txtNote.Text.Trim(),
                    PerformedByUserId = AuthContextManager.UserProfileId ?? throw new Exception("Không lấy được user profile ID")
                };

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
