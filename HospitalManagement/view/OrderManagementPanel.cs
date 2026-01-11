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
        private readonly OrderController _orderController;
        private readonly InventoryController _inventoryController;
        private readonly ProductController _productController;

        private readonly long _userId;
        private List<OrderResponse> _allOrders = new();
        
        

        public OrderManagementPanel(long userId)
        {
            InitializeComponent();
            _userId = userId;

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

            dgvOrders.Columns.Clear();
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 60 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", Visible = false });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OrderNumber", HeaderText = "Mã đơn" });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OrderDate", HeaderText = "Ngày tạo" });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Trạng thái" });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TotalAmount", HeaderText = "Tổng tiền" });
        }

        private void InitEvents()
        {
            btnRefresh.Click += (_, _) => LoadData();
            btnSearch.Click += (_, _) => ApplySearch();
            btnCreate.Click += (_, _) => OnCreate();
            btnView.Click += (_, _) => OnViewDetail();
            btnConfirm.Click += (_, _) => OnConfirm();
            btnCancel.Click += (_, _) => OnCancel();
        }

        // ================= DATA =================

        private void LoadData()
        {
            _allOrders = _orderController.GetAll();
            BindGrid(_allOrders);
        }

        private void BindGrid(List<OrderResponse> data)
        {
            dgvOrders.DataSource = null;
            dgvOrders.DataSource = data;

            for (int i = 0; i < dgvOrders.Rows.Count; i++)
                dgvOrders.Rows[i].Cells[0].Value = i + 1;

            lblTotal.Text = $"Tổng số đơn hàng: {data.Count}";
        }

        // ================= ACTIONS =================

        private void ApplySearch()
        {
            string kw = txtSearch.Text.Trim().ToLower();
            var filtered = _allOrders.Where(o =>
                o.OrderNumber.ToLower().Contains(kw)
            ).ToList();

            BindGrid(filtered);
        }

        private OrderResponse? GetSelected()
        {
            return dgvOrders.CurrentRow?.DataBoundItem as OrderResponse;
        }

        private void OnCreate()
        {
            var dlg = new CreateOrderForm(_userId);
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

            var dlg = new OrderDetailForm(selected.Id.Value);
            dlg.ShowDialog();
        }

        private void OnConfirm()
        {
            var order = GetSelected();
            if (order == null) return;

            if (order.Status != OrderStatus.NEW.ToString())
            {
                MessageBox.Show("Chỉ confirm được đơn NEW!");
                return;
            }

            _orderController.Confirm(order.Id.Value);
            LoadData();
        }

        private void OnCancel()
        {
            var order = GetSelected();
            if (order == null) return;

            if (order.Status != OrderStatus.NEW.ToString())
                return;

            _orderController.Cancel(order.Id.Value);
            LoadData();
        }
    }
}
