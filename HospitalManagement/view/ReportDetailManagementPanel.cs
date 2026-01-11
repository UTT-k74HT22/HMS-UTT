using System;
using System.ComponentModel;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.dto.response.ReportDetailResponse;
using System.Collections.Generic;

namespace HospitalManagement.view
{
    public partial class ReportDetailManagementPanel : UserControl
    {
        private readonly ReportDetailController _controller;

        // Constructor có controller
        public ReportDetailManagementPanel(ReportDetailController controller)
        {
            InitializeComponent();
            _controller = controller;

            if (!DesignMode)
            {
                AttachEvents();
                LoadAll();
            }
        }

        // Constructor mặc định cho Designer
        public ReportDetailManagementPanel()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                string connectionString = "Server=localhost;Database=hms;User Id=sa;Password=123456789;TrustServerCertificate=True;";
                _controller = new ReportDetailController(connectionString);

                AttachEvents();
                LoadAll();
            }
        }

        private void AttachEvents()
        {
            btnReloadBestSelling.Click += (s, e) => LoadBestSelling();
            btnReloadInventory.Click += (s, e) => LoadInventory();
            btnReloadCustomers.Click += (s, e) => LoadCustomers();
            btnReloadOrders.Click += (s, e) => LoadOrders();
        }

        private void LoadAll()
        {
            LoadBestSelling();
            LoadInventory();
            LoadCustomers();
            LoadOrders();
        }

        private void LoadBestSelling()
        {
            var list = _controller.GetBestSellingProducts(10) ?? new List<BestSellingProductResponse>();

            dgvBestSelling.DataSource = null;
            dgvBestSelling.Rows.Clear();
            dgvBestSelling.Columns.Clear();

            // Cột STT
            dgvBestSelling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 50 });

            dgvBestSelling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Product", DataPropertyName = "Product" });
            dgvBestSelling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Product Code", DataPropertyName = "ProductCode" });
            dgvBestSelling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total Sold", DataPropertyName = "TotalSold" });
            dgvBestSelling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total Revenue", DataPropertyName = "TotalRevenue" });

            for (int i = 0; i < list.Count; i++)
            {
                dgvBestSelling.Rows.Add(i + 1, list[i].Product, list[i].ProductCode, list[i].TotalSold, list[i].TotalRevenue);
            }

            dgvBestSelling.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadInventory()
        {
            var list = _controller.GetInventory(10) ?? new List<InventoryItemResponse>();

            dgvInventory.DataSource = null;
            dgvInventory.Rows.Clear();
            dgvInventory.Columns.Clear();

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 50 });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Warehouse", DataPropertyName = "Warehouse" });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Product", DataPropertyName = "Product" });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Batch", DataPropertyName = "Batch" });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Quantity", DataPropertyName = "Quantity" });
            dgvInventory.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Low Stock", DataPropertyName = "IsLowStock" });

            for (int i = 0; i < list.Count; i++)
            {
                dgvInventory.Rows.Add(i + 1, list[i].Warehouse, list[i].Product, list[i].Batch, list[i].Quantity, list[i].IsLowStock);
            }

            dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadCustomers()
        {
            var list = _controller.GetCustomers()?
                                  .OrderByDescending<CustomerResponse, decimal>(c => c.TotalSpent)
                                  .Take(10)
                                  .ToList() ?? new List<CustomerResponse>();

            dgvCustomers.DataSource = null;
            dgvCustomers.Rows.Clear();
            dgvCustomers.Columns.Clear();

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 50 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Customer", DataPropertyName = "Customer" });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Type", DataPropertyName = "Type" });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total Orders", DataPropertyName = "TotalOrders" });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total Spent", DataPropertyName = "TotalSpent" });

            for (int i = 0; i < list.Count; i++)
            {
                dgvCustomers.Rows.Add(i + 1, list[i].Customer, list[i].Type, list[i].TotalOrders, list[i].TotalSpent);
            }

            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadOrders()
        {
            var list = _controller.GetOrdersByStatus() ?? new List<OrderStatusResponse>();

            dgvOrders.DataSource = null;
            dgvOrders.Rows.Clear();
            dgvOrders.Columns.Clear();

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Width = 50 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Status", DataPropertyName = "Status" });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total Orders", DataPropertyName = "TotalOrders" });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total Value", DataPropertyName = "TotalValue" });

            for (int i = 0; i < list.Count; i++)
            {
                dgvOrders.Rows.Add(i + 1, list[i].Status, list[i].TotalOrders, list[i].TotalValue);
            }

            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
