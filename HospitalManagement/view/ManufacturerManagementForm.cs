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
    public partial class ManufacturerManagementForm : UserControl
    {
        private readonly ManufacturerController _controller;

        public ManufacturerManagementForm()
        {
            InitializeComponent();

            // Load appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var dbConfig = new DBConfig(config);

            var service = new ManufacturerServiceImpl(dbConfig);
            _controller = new ManufacturerController(service);

            InitGrid();
            LoadData();
        }

        private void InitGrid()
        {
            dgvManufacturer.AutoGenerateColumns = false;
            dgvManufacturer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvManufacturer.MultiSelect = false;
            dgvManufacturer.AllowUserToAddRows = false;
            dgvManufacturer.ReadOnly = true;

            dgvManufacturer.Columns.Clear();

            dgvManufacturer.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "STT",
                Width = 50
            });

            dgvManufacturer.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                Width = 60
            });

            dgvManufacturer.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Code",
                Width = 120
            });

            dgvManufacturer.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 200
            });

            dgvManufacturer.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Country",
                Width = 120
            });

            dgvManufacturer.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Phone",
                Width = 120
            });

            dgvManufacturer.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Email",
                Width = 200
            });
        }

        // ================= LOAD DATA =================
        private void LoadData()
        {
            try
            {
                dgvManufacturer.Rows.Clear();

                List<Manufacturer> list = _controller.GetAll();
                if (list == null) list = new List<Manufacturer>();

                int stt = 1;
                foreach (var m in list)
                {
                    dgvManufacturer.Rows.Add(
                        stt++,            
                        m.Id,           
                        m.Code,         
                        m.Name,             
                        m.Country,         
                        m.Phone,            
                        m.Email
                    );
                }

                lblTotal.Text = $"Tổng: {list.Count}";
                dgvManufacturer.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi load dữ liệu:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
