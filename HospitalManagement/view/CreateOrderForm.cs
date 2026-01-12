using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.dto.request.Order;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Category;

namespace HospitalManagement.view
{
    public partial class CreateOrderForm : Form
    {
        // ===== Context =====
        private readonly long _employeeId;

        // ===== Controllers (DI) =====
        private readonly OrderController _orderController;
        private readonly ProductController _productController;
        private readonly InventoryController _inventoryController;
        private readonly CategoryController _categoryController;

        // ===== State =====
        private readonly List<OrderItemRequest> _orderItems = new();

        // ===== Constructor =====
        public CreateOrderForm(
            long employeeId,
            OrderController orderController,
            ProductController productController,
            InventoryController inventoryController,
            CategoryController categoryController)
        {
            InitializeComponent();

            _employeeId = employeeId;
            _orderController = orderController;
            _productController = productController;
            _inventoryController = inventoryController;
            _categoryController = categoryController;

            InitGrid();
            RegisterEvents();
            LoadCategories();
        }

        // ================= INIT =================

        private void InitGrid()
        {
            // Products grid
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.Columns.Clear();
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            dgvProducts.Columns.Add("Id", "ID");
            dgvProducts.Columns.Add("Name", "Tên SP");
            dgvProducts.Columns.Add("Price", "Giá");

            dgvProducts.Columns["Id"].Visible = false;

            // Order items grid
            dgvOrderItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrderItems.MultiSelect = false;
            dgvOrderItems.AutoGenerateColumns = false;
            dgvOrderItems.Columns.Clear();
            dgvOrderItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvOrderItems.Columns.Add("Product", "Sản phẩm");
            dgvOrderItems.Columns.Add("Warehouse", "Kho");
            dgvOrderItems.Columns.Add("Batch", "Lô");
            dgvOrderItems.Columns.Add("Quantity", "SL");
            dgvOrderItems.Columns.Add("Price", "Đơn giá");
            dgvOrderItems.Columns.Add("Total", "Thành tiền");
        }

        private void RegisterEvents()
        {
            cbCategory.SelectedIndexChanged += cbCategory_SelectedIndexChanged;
            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;

            btnAddItem.Click += btnAddItem_Click;
            btnDeleteItem.Click += btnDeleteItem_Click;
            btnDeleteAll.Click += btnDeleteAll_Click;
            btnCancel.Click += (_, _) => Close();
            btnCreateOrder.Click += btnCreateOrder_Click;
        }

        // ================= LOAD DATA =================

        private void LoadCategories()
        {
            cbCategory.Items.Clear();

            var categories = _categoryController.GetAllCategories();
            foreach (var c in categories)
            {
                cbCategory.Items.Add(c);
            }
        }


        private void cbCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbCategory.SelectedItem is not CategoryResponse c) return;

            dgvProducts.Rows.Clear();

            foreach (var p in _productController.GetByCategory(c.Id))
            {
                dgvProducts.Rows.Add(p.Id, p.Name, p.StandardPrice);
            }
        }

        private void dgvProducts_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
                return;

            var cellValue = dgvProducts.CurrentRow.Cells[0].Value;

            if (cellValue == null)
                return;

            if (!long.TryParse(cellValue.ToString(), out var productId))
                return;

            cbWarehouse.Items.Clear();
            cbBatch.Items.Clear();

            var inventories = _inventoryController.GetInventoryByProduct(productId);
            foreach (var inv in inventories)
                cbWarehouse.Items.Add(inv);

            var batches = _productController.GetBatchesByProduct(productId);
            foreach (var batch in batches)
                cbBatch.Items.Add(batch);

            if (cbWarehouse.Items.Count > 0)
                cbWarehouse.SelectedIndex = 0;

            if (cbBatch.Items.Count > 0)
                cbBatch.SelectedIndex = 0;
        }


        // ================= ADD / REMOVE ITEM =================

        private void btnAddItem_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Chưa chọn sản phẩm!");
                return;
            }

            if (cbWarehouse.SelectedItem is not InventoryResponse wh)
            {
                MessageBox.Show("Chưa chọn kho!");
                return;
            }

            if (cbBatch.SelectedItem is not BatchResponse batch)
            {
                MessageBox.Show("Chưa chọn lô!");
                return;
            }

            int qty = (int)nudQuantity.Value;
            if (qty <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            decimal price = Convert.ToDecimal(dgvProducts.CurrentRow.Cells[2].Value);

            var item = new OrderItemRequest
            {
                ProductId = (long)dgvProducts.CurrentRow.Cells[0].Value,
                WarehouseId = wh.WarehouseId,
                BatchId = batch.Id,
                Quantity = qty,
                UnitPrice = price
            };

            _orderItems.Add(item);

            dgvOrderItems.Rows.Add(
                dgvProducts.CurrentRow.Cells[1].Value,
                wh.WarehouseName,
                batch.BatchCode,
                qty,
                price,
                qty * price
            );

            UpdateTotal();
        }

        private void btnDeleteItem_Click(object? sender, EventArgs e)
        {
            if (dgvOrderItems.CurrentRow == null) return;

            int index = dgvOrderItems.CurrentRow.Index;
            _orderItems.RemoveAt(index);
            dgvOrderItems.Rows.RemoveAt(index);

            UpdateTotal();
        }

        private void btnDeleteAll_Click(object? sender, EventArgs e)
        {
            if (_orderItems.Count == 0) return;

            if (MessageBox.Show("Xóa tất cả sản phẩm trong đơn?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            _orderItems.Clear();
            dgvOrderItems.Rows.Clear();
            UpdateTotal();
        }

        // ================= TOTAL =================

        private void UpdateTotal()
        {
            decimal total = _orderItems.Sum(i => i.UnitPrice * i.Quantity);

            if (decimal.TryParse(txtDiscount.Text, out var discount))
                total -= discount;

            lblTotal.Text = total.ToString("N0");
        }

        // ================= SUBMIT =================

        private void btnCreateOrder_Click(object? sender, EventArgs e)
        {
            if (_orderItems.Count == 0)
            {
                MessageBox.Show("Đơn hàng chưa có sản phẩm!");
                return;
            }

            if (!long.TryParse(txtCustomerId.Text, out var customerId))
            {
                MessageBox.Show("Customer ID không hợp lệ!");
                return;
            }

            if (!decimal.TryParse(txtDiscount.Text, out var discount))
                discount = 0;

            var req = new CreateOrderWithItemsRequest
            {
                CustomerId = customerId,
                ShippingAddress = txtShipping.Text,
                Discount = discount,
                Items = _orderItems
            };

            _orderController.CreateOrder(req, _employeeId);

            MessageBox.Show("Tạo đơn hàng thành công!", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }
    }
}
