using HospitalManagement.Controller;
using HospitalManagement.entity;
using HospitalManagement.service;
using HospitalManagement.service.impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using HospitalManagement.configuration;

namespace HospitalManagement.view
{
    public partial class InvoiceManagementForm : UserControl
    {
        public readonly InvoiceController _invoiceController;
        public InvoiceManagementForm()
        {
            InitializeComponent();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString =
                config.GetConnectionString("DefaultConnection");

            var dbConfig = new DBConfig(config);
            var service = new InvoiceServiceImpl(dbConfig);
            _invoiceController = new InvoiceController(service);


            InitGrid();
            LoadData();
        }
        private void InitGrid()
        {
            dgvInvoice.AutoGenerateColumns = false;
            dgvInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInvoice.MultiSelect = false;
            dgvInvoice.AllowUserToAddRows = false;
            dgvInvoice.ReadOnly = true;

            dgvInvoice.Columns.Clear();

            dgvInvoice.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "STT",
                Name = "STT",
                FillWeight = 10
            });

            dgvInvoice.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                Name = "ID",
                FillWeight = 10
            });

            dgvInvoice.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Invoice Number",
                Name = "InvoiceNumber",
                FillWeight = 20
            });

            dgvInvoice.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Order ID",
                Name = "OrderID",
                FillWeight = 10
            });

            dgvInvoice.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Issue Date",
                Name = "IssueDate",
                FillWeight = 15
            });

            dgvInvoice.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Due Date",
                Name = "DueDate",
                FillWeight = 15
            });

            dgvInvoice.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Total Amount",
                Name = "TotalAmount",
                FillWeight = 10
            });

            dgvInvoice.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Paid Amount",
                Name = "PaidAmount",
                FillWeight = 10
            });

            dgvInvoice.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Status",
                Name = "Status",
                FillWeight = 10
            });

            dgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadData()
        {
            try
            {
                dgvInvoice.Rows.Clear();

                List<Invoice> list = _invoiceController.GetAll();
                int stt = 1;

                foreach (var i in list)
                {
                    dgvInvoice.Rows.Add(
                        stt++,
                        i.Id,
                        i.InvoiceNumber,
                        i.OrderId,
                        i.IssueDate.ToString("yyyy-MM-dd"),
                        i.DueDate?.ToString("yyyy-MM-dd"),
                        i.TotalAmount.ToString("N2"),
                        i.PaidAmount.ToString("N2"),
                        i.Status
                    );
                }

                lblTotal.Text = $"Tổng: {list.Count}";
                dgvInvoice.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi load dữ liệu Invoice:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        
private void btnAdd_Click(object sender, EventArgs e)
{
    using (Form dialog = new Form())
    {
        dialog.Text = "Thêm Invoice";
        dialog.Size = new Size(400, 200);
        dialog.StartPosition = FormStartPosition.CenterParent;

        // Label + ComboBox chọn Order
        Label lblOrder = new Label { Text = "Order ID:", Location = new Point(20, 20), AutoSize = true };
        ComboBox cbOrderId = new ComboBox
        {
            Location = new Point(120, 18),
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        var availableOrders = _invoiceController.GetAvailableOrderIds();
        if (availableOrders.Count == 0)
        {
            MessageBox.Show("Không có Order khả dụng để tạo Invoice!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        cbOrderId.DataSource = availableOrders;

        // Label + TextBox InvoiceNumber
        Label lblInvoiceNumber = new Label { Text = "Invoice Number:", Location = new Point(20, 60), AutoSize = true };
        TextBox txtInvoiceNumber = new TextBox
        {
            Location = new Point(120, 58),
            Width = 200,
            Text = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss") // tạo mặc định
        };

        // Buttons Lưu / Hủy
        Button btnSave = new Button { Text = "Lưu", Location = new Point(120, 110), Width = 80 };
        Button btnCancel = new Button { Text = "Hủy", Location = new Point(240, 110), Width = 80 };

        btnCancel.Click += (s, ev) => dialog.Close();

        btnSave.Click += (s, ev) =>
        {
            try
            {
                int orderId = (int)cbOrderId.SelectedItem;
                string invoiceNumber = txtInvoiceNumber.Text.Trim();

                if (string.IsNullOrEmpty(invoiceNumber))
                {
                    MessageBox.Show("Invoice Number không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi Controller tạo Invoice
                long newId = _invoiceController.CreateInvoice(orderId, invoiceNumber);

                if (newId > 0)
                {
                    MessageBox.Show("Tạo Invoice thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    dialog.Close();
                }
                else
                {
                    MessageBox.Show("Tạo Invoice thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo Invoice:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        dialog.Controls.Add(lblOrder);
        dialog.Controls.Add(cbOrderId);
        dialog.Controls.Add(lblInvoiceNumber);
        dialog.Controls.Add(txtInvoiceNumber);
        dialog.Controls.Add(btnSave);
        dialog.Controls.Add(btnCancel);

        dialog.ShowDialog(this);
    }
}

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
