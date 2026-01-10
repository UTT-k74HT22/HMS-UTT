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
    public partial class PaymentManagementForm : UserControl
    {
        private readonly PaymentController _controller;

        public PaymentManagementForm()
        {
            InitializeComponent();

            // Load appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var dbConfig = new DBConfig(config);

            var service = new PaymentServiceImpl(dbConfig);
            _controller = new PaymentController(service);

            InitGrid();
            LoadData();
        }

        private void InitGrid()
        {
            dgvPayment.AutoGenerateColumns = false;
            dgvPayment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayment.MultiSelect = false;
            dgvPayment.AllowUserToAddRows = false;
            dgvPayment.ReadOnly = true;

            dgvPayment.Columns.Clear();

            dgvPayment.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Name = "STT", FillWeight = 10 });
            dgvPayment.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", Name = "ID", FillWeight = 15 });
            dgvPayment.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Invoice ID", Name = "InvoiceID", FillWeight = 15 });
            dgvPayment.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Payment Number", Name = "PaymentNumber", FillWeight = 15 });
            dgvPayment.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Payment Date", Name = "PaymentDate", FillWeight = 15 });
            dgvPayment.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Amount", Name = "Amount", FillWeight = 10 });
            dgvPayment.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Method", Name = "Method", FillWeight = 10 });
            dgvPayment.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Status", Name = "Status", FillWeight = 10 });

            dgvPayment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadData()
        {
            try
            {
                dgvPayment.Rows.Clear();
                List<Payment> list = _controller.GetAll();
                int stt = 1;
                foreach (var p in list)
                {
                    dgvPayment.Rows.Add(
                        stt++,
                        p.Id,
                        p.InvoiceId,
                        p.PaymentNumber,
                        p.PaymentDate.ToString("yyyy-MM-dd HH:mm"),
                        p.Amount,
                        p.Method,
                        p.Status
                    );
                }
                lblTotal.Text = $"Tổng: {list.Count}";
                dgvPayment.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
private void btnAdd_Click(object sender, EventArgs e)
{
    using (Form dialog = new Form())
    {
        dialog.Text = "Thêm Payment";
        dialog.Size = new Size(400, 180);
        dialog.StartPosition = FormStartPosition.CenterParent;

        // Invoice ID
        Label lblInvoice = new Label { Text = "Mã hóa đơn:", Location = new Point(20, 20), AutoSize = true };
        TextBox txtInvoiceId = new TextBox { Location = new Point(120, 18), Width = 200 };

        // Payment method
        Label lblMethod = new Label { Text = "Phương thức:", Location = new Point(20, 60), AutoSize = true };
        ComboBox cbMethod = new ComboBox { Location = new Point(120, 58), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cbMethod.Items.AddRange(new string[] { "CASH", "BANK_TRANSFER", "CREDIT_CARD", "E_WALLET" });
        cbMethod.SelectedIndex = 0;

        // Buttons
        Button btnSave = new Button { Text = "Lưu", Location = new Point(120, 100), Width = 80 };
        Button btnCancel = new Button { Text = "Hủy", Location = new Point(240, 100), Width = 80 };

        btnCancel.Click += (s, ev) => dialog.Close();
        btnSave.Click += (s, ev) =>
        {
            if (!int.TryParse(txtInvoiceId.Text.Trim(), out int invoiceId))
            {
                MessageBox.Show("Mã hóa đơn không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string method = cbMethod.SelectedItem.ToString();
            string paymentNumber = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                int newId = _controller.CreatePaymentByInvoice(invoiceId, paymentNumber, method);

                MessageBox.Show($"Tạo Payment thành công!\nPayment ID: {newId}\nPayment Number: {paymentNumber}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData();
                dialog.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm Payment:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        dialog.Controls.Add(lblInvoice);
        dialog.Controls.Add(txtInvoiceId);
        dialog.Controls.Add(lblMethod);
        dialog.Controls.Add(cbMethod);
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

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
