using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Spreadsheet;
using HospitalManagement.controller;
using HospitalManagement.dto.response.Order;

namespace HospitalManagement.view
{
    public partial class OrderDetailForm : Form
    {
        private readonly OrderController _controller;
        private readonly long _orderId;

        public OrderDetailForm(long orderId, OrderController controller)
        {
            InitializeComponent();

            _orderId = orderId;
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));

            InitGrid();
            LoadOrderInfo();
            LoadOrderItems();

            btnClose.Click += (_, _) => Close();
        }

        // ================= INIT GRID =================
        private void InitGrid()
        {
            dgvItems.ReadOnly = true;
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.RowHeadersVisible = false;

            dgvItems.Columns.Clear();
            dgvItems.Columns.Add("STT", "STT");
            dgvItems.Columns.Add("Product", "Tên thuốc");
            dgvItems.Columns.Add("Qty", "Số lượng");
            dgvItems.Columns.Add("UnitPrice", "Đơn giá");
            dgvItems.Columns.Add("LineTotal", "Thành tiền");
            dgvItems.Columns.Add("Note", "Ghi chú");
        }

        // ================= LOAD INFO =================
        private void LoadOrderInfo()
        {
            OrderResponse o = _controller.GetById(_orderId);

            lblOrderCode.Text = $"Mã đơn: {o.OrderNumber}";
            lblOrderDate.Text = $"Ngày: {o.OrderDate}";
            lblStatus.Text = $"Trạng thái: {o.Status}";
            lblTotal.Text = $"Tổng tiền: {FormatMoney(o.TotalAmount)}";

            lblCustomerName.Text = $"Khách hàng: {Safe(o.CustomerName)}";
            lblCustomerPhone.Text = $"SĐT: {Safe(o.CustomerPhone)}";
            lblCustomerEmail.Text = $"Email: {Safe(o.CustomerEmail)}";

            lblShippingAddress.Text = $"Địa chỉ giao hàng: {Safe(o.ShippingAddress)}";

            lblCreatorName.Text = $"Người tạo: {Safe(o.CreatorName)}";
            lblCreatorEmail.Text = $"Email: {Safe(o.CreatorEmail)}";
            lblCreatorPhone.Text = $"SĐT người tạo: {Safe(o.CreatorPhone)}";
        }

        // ================= LOAD ITEMS =================
        private void LoadOrderItems()
        {
            dgvItems.Rows.Clear();
            List<OrderItemResponse> items = _controller.GetItems(_orderId);

            int stt = 1;
            foreach (var i in items)
            {
                dgvItems.Rows.Add(
                    stt++,
                    i.ProductName,
                    i.Quantity,
                    FormatMoney(i.UnitPrice),
                    FormatMoney(i.LineTotal),
                    Safe(i.Note)
                );
            }
        }

        // ================= UTIL =================
        private string Safe(string? s)
            => string.IsNullOrWhiteSpace(s) ? "-" : s;

        private string FormatMoney(decimal money)
            => money.ToString("N0");
    }
}
