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
        dialog.Size = new Size(400, 200);
        dialog.StartPosition = FormStartPosition.CenterParent;

        Label lblInvoice = new Label { Text = "Mã hóa đơn:", Location = new Point(20, 20), AutoSize = true };
        ComboBox cbInvoiceId = new ComboBox
        {
            Location = new Point(120, 18),
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        var invoiceIds = _controller.GetAvailableInvoiceIds();
        if (invoiceIds.Count == 0)
        {
            MessageBox.Show("Không có hóa đơn khả dụng để thanh toán!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        cbInvoiceId.DataSource = invoiceIds;

        Label lblMethod = new Label { Text = "Phương thức:", Location = new Point(20, 60), AutoSize = true };
        ComboBox cbMethod = new ComboBox
        {
            Location = new Point(120, 58),
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cbMethod.Items.AddRange(new string[] { "CASH", "BANK_TRANSFER", "CREDIT_CARD", "E_WALLET" });
        cbMethod.SelectedIndex = 0;

        Button btnSave = new Button { Text = "Lưu", Location = new Point(120, 110), Width = 80 };
        Button btnCancel = new Button { Text = "Hủy", Location = new Point(240, 110), Width = 80 };

        btnCancel.Click += (s, ev) => dialog.Close();
        btnSave.Click += (s, ev) =>
        {
            int invoiceId = (int)cbInvoiceId.SelectedItem;
            string method = cbMethod.SelectedItem.ToString();
            string paymentNumber = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                _controller.CreatePaymentByInvoice(invoiceId, paymentNumber, method);
                MessageBox.Show("Tạo Payment thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                dialog.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm Payment:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        dialog.Controls.Add(lblInvoice);
        dialog.Controls.Add(cbInvoiceId);
        dialog.Controls.Add(lblMethod);
        dialog.Controls.Add(cbMethod);
        dialog.Controls.Add(btnSave);
        dialog.Controls.Add(btnCancel);

        dialog.ShowDialog(this);
    }
}


private void btnEdit_Click(object sender, EventArgs e)
{
    if (dgvPayment.SelectedRows.Count == 0)
    {
        MessageBox.Show("Vui lòng chọn một Payment để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    int paymentId = Convert.ToInt32(dgvPayment.SelectedRows[0].Cells["ID"].Value);
    Payment payment = _controller.GetById(paymentId);

    if (payment == null)
    {
        MessageBox.Show("Không tìm thấy Payment!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    using (Form dialog = new Form())
    {
        dialog.Text = "Sửa Payment";
        dialog.Size = new Size(400, 200);
        dialog.StartPosition = FormStartPosition.CenterParent;

        Label lblMethod = new Label { Text = "Phương thức:", Location = new Point(20, 20), AutoSize = true };
        ComboBox cbMethod = new ComboBox { Location = new Point(120, 18), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cbMethod.Items.AddRange(new string[] { "CASH", "BANK_TRANSFER", "CREDIT_CARD", "E_WALLET" });
        cbMethod.SelectedItem = payment.Method;

        Label lblStatus = new Label { Text = "Trạng thái:", Location = new Point(20, 60), AutoSize = true };
        ComboBox cbStatus = new ComboBox { Location = new Point(120, 58), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cbStatus.Items.AddRange(new string[] { "SUCCESS", "FAILED", "PENDING", "CANCELED" });
        cbStatus.SelectedItem = payment.Status;
        
        Button btnSave = new Button { Text = "Lưu", Location = new Point(120, 100), Width = 80 };
        Button btnCancel = new Button { Text = "Hủy", Location = new Point(240, 100), Width = 80 };

        btnCancel.Click += (s, ev) => dialog.Close();
        btnSave.Click += (s, ev) =>
        {
            payment.Method = cbMethod.SelectedItem.ToString();
            payment.Status = cbStatus.SelectedItem.ToString();

            try
            {
                _controller.Update(payment);
                MessageBox.Show("Cập nhật Payment thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                dialog.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật Payment:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        dialog.Controls.Add(lblMethod);
        dialog.Controls.Add(cbMethod);
        dialog.Controls.Add(lblStatus);
        dialog.Controls.Add(cbStatus);
        dialog.Controls.Add(btnSave);
        dialog.Controls.Add(btnCancel);

        dialog.ShowDialog(this);
    }
}

private void btnDelete_Click(object sender, EventArgs e)
{
    if (dgvPayment.SelectedRows.Count == 0)
    {
        MessageBox.Show("Vui lòng chọn một Payment để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    int paymentId = Convert.ToInt32(dgvPayment.SelectedRows[0].Cells["ID"].Value);
    string paymentNumber = dgvPayment.SelectedRows[0].Cells["PaymentNumber"].Value.ToString();

    DialogResult dr = MessageBox.Show(
        $"Bạn có chắc muốn xóa Payment {paymentNumber} (ID: {paymentId}) không?",
        "Xác nhận xóa",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    );

    if (dr == DialogResult.Yes)
    {
        try
        {
            _controller.Cancel(paymentId);
            MessageBox.Show("Xóa Payment thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi xóa Payment:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData(); 
                return;
            }
            try
            {
                List<Payment> list = _controller.SearchByPaymentNumber(keyword);
                dgvPayment.Rows.Clear();
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
                MessageBox.Show("Lỗi tìm kiếm Payment:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
