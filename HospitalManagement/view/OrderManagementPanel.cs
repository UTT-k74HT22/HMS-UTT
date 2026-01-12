using HospitalManagement.controller;
using HospitalManagement.dto.response.Order;
using HospitalManagement.entity.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HospitalManagement.view
{
    public partial class OrderManagementPanel : UserControl
    {
        // ===== Controllers (DI) =====
        private readonly OrderController _orderController;
        private readonly InventoryController _inventoryController;
        private readonly ProductController _productController;
        private readonly StockMovementController _stockMovementController;
        private readonly CategoryController _categoryController;


        // ===== Context =====
        private readonly long _userId;
        private List<OrderResponse> _allOrders = new();

        // ===== Constructor =====
        public OrderManagementPanel(
            long userId,
            OrderController orderController,
            InventoryController inventoryController,
            ProductController productController,
            CategoryController categoryController,
            StockMovementController stockMovementController)
        {
            InitializeComponent();

            _userId = userId;
            _orderController = orderController;
            _inventoryController = inventoryController;
            _productController = productController;
            _stockMovementController = stockMovementController;
            _categoryController = categoryController;

            InitGrid();
            InitEvents();
            LoadData();
        }

        // ================= INIT =================

        private void InitGrid()
        {
            dgvOrders.AutoGenerateColumns = false;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.MultiSelect = false;
            dgvOrders.RowHeadersVisible = false;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvOrders.Columns.Clear();

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "STT",
                Width = 60
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderResponse.Id),
                Visible = false
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderResponse.OrderNumber),
                HeaderText = "Mã đơn"
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderResponse.OrderDate),
                HeaderText = "Ngày tạo"
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderResponse.Status),
                HeaderText = "Trạng thái"
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(OrderResponse.TotalAmount),
                HeaderText = "Tổng tiền",
                
                DefaultCellStyle = new DataGridViewCellStyle
                {
                Format = "N0" 
            }
            });
        }

        private void InitEvents()
        {
            btnRefresh.Click += (_, _) => LoadData();
            btnSearch.Click += (_, _) => ApplySearch();
            btnCreate.Click += (_, _) => OnCreate();
            btnView.Click += (_, _) => OnViewDetail();
            btnConfirm.Click += (_, _) => OnConfirm();
            btnCancel.Click += (_, _) => OnCancel();
            
            dgvOrders.RowPostPaint += dgvOrders_RowPostPaint;
        }

        // ================= DATA =================
        private void dgvOrders_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;

            string stt = (e.RowIndex + 1).ToString();

            // Gán giá trị cho cột STT
            grid.Rows[e.RowIndex].Cells[0].Value = stt;
        }

        private void LoadData()
        {
            _allOrders = _orderController.GetAll();
            BindGrid(_allOrders);
        }

        private void BindGrid(List<OrderResponse> data)
        {
            dgvOrders.DataSource = null;
            dgvOrders.DataSource = data;
            lblTotal.Text = $"Tổng số đơn hàng: {data.Count}";
        }

        // ================= ACTIONS =================

        private void ApplySearch()
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            var filtered = _allOrders
                .Where(o => o.OrderNumber != null &&
                            o.OrderNumber.ToLower().Contains(keyword))
                .ToList();

            BindGrid(filtered);
        }

        private OrderResponse? GetSelected()
        {
            return dgvOrders.CurrentRow?.DataBoundItem as OrderResponse;
        }

        private void OnCreate()
        {
            var dlg = new CreateOrderForm(
                _userId,
                _orderController,
                _productController,
                _inventoryController,
                _categoryController
            );

            dlg.ShowDialog();
            LoadData();
        }
        private void OnViewDetail()
        {
            var selected = GetSelected();
            if (selected == null)
            {
                MessageBox.Show("Chưa chọn đơn hàng!");
                return;
            }

            var dlg = new OrderDetailForm(
                selected.Id!.Value,
                _orderController
            );
            dlg.ShowDialog();
        }

        private void OnConfirm()
        {
            var order = GetSelected();
            if (order == null)
            {
                MessageBox.Show("Chưa chọn đơn!");
                return;
            }

            if (order.Status != OrderStatus.NEW.ToString())
            {
                MessageBox.Show("Chỉ xác nhận được đơn ở trạng thái NEW!");
                return;
            }

            // ===== Lấy order items =====
            var items = _orderController.GetItems(order.Id!.Value);

            foreach (var item in items)
            {
                if (item.WarehouseId == null)
                {
                    MessageBox.Show(
                        $"Order item '{item.ProductName}' chưa có kho!",
                        "Lỗi dữ liệu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                int available = _inventoryController.GetCurrentQuantity(
                    item.ProductId.Value,
                    item.BatchId.Value,
                    item.WarehouseId.Value
                );

                if (available < item.Quantity)
                {
                    MessageBox.Show(
                        $"Sản phẩm {item.ProductName} không đủ tồn kho!",
                        "Lỗi tồn kho",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                int before = available;
                int after = before - item.Quantity;

                _inventoryController.UpdateStock(
                    item.ProductId.Value,
                    item.BatchId.Value,
                    item.WarehouseId.Value,
                    after
                );

                _inventoryController.InsertStockMovement(
                    item.ProductId.Value,
                    item.BatchId.Value,
                    item.WarehouseId.Value,
                    item.Quantity,
                    before,
                    after,
                    $"Trừ kho từ Order {order.OrderNumber}",
                    "EXPORT"
                );
            }

            _orderController.Confirm(order.Id.Value);
            LoadData();
        }

        private void OnCancel()
        {
            var order = GetSelected();
            if (order == null) return;

            if (order.Status != OrderStatus.NEW.ToString()) return;

            _orderController.Cancel(order.Id!.Value);
            LoadData();
        }
    }
}
