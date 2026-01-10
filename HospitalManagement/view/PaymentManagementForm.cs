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
    }
}
