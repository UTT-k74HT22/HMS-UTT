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
                HeaderText = "Address",
                Width = 200
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
            
            dgvManufacturer.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Contact Person",
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
                        m.Address,
                        m.Phone,            
                        m.Email,
                        m.ContactPerson
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
            Form f = new Form
            {
                Text = "Thêm Manufacturer",
                Size = new Size(400, 480),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            // Các trường cần nhập
            string[] labels = { "Code", "Name", "Country", "Address", "Phone", "Email", "Contact Person" };
            TextBox[] inputs = new TextBox[labels.Length];

            int leftLabel = 30;
            int leftInput = 130;
            int topStart = 30;
            int heightStep = 40;

            for (int i = 0; i < labels.Length; i++)
            {
                int top = topStart + i * heightStep;
                var lbl = new Label { Text = labels[i], Left = leftLabel, Top = top + 3, Width = 100 };
                var txt = new TextBox { Left = leftInput, Top = top, Width = 200 };
                inputs[i] = txt;
                f.Controls.Add(lbl);
                f.Controls.Add(txt);
            }
            
            Button btnSave = new Button { Text = "Lưu", Left = leftInput, Top = topStart + labels.Length * heightStep + 10, Width = 80 };
            Button btnCancel = new Button { Text = "Hủy", Left = leftInput + 120, Top = btnSave.Top, Width = 80 };

            btnCancel.Click += (_, __) => f.Close();
            btnSave.Click += (_, __) =>
            {
                try
                {
                    _controller.Create(new Manufacturer
                    {
                        Code = inputs[0].Text.Trim(),
                        Name = inputs[1].Text.Trim(),
                        Country = inputs[2].Text.Trim(),
                        Address = inputs[3].Text.Trim(),
                        Phone = inputs[4].Text.Trim(),
                        Email = inputs[5].Text.Trim(),
                        ContactPerson = inputs[6].Text.Trim()
                    });
                    f.Close();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi");
                }
            };

            f.Controls.AddRange(new Control[] { btnSave, btnCancel });
            f.ShowDialog();
        }
        
private void btnEdit_Click(object sender, EventArgs e)
{
    if (dgvManufacturer.SelectedRows.Count == 0)
    {
        MessageBox.Show("Vui lòng chọn một bản ghi để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }
    
    var selectedRow = dgvManufacturer.SelectedRows[0];
    int id = Convert.ToInt32(selectedRow.Cells[1].Value);
    Manufacturer m = _controller.FindById(id);
    if (m == null)
    {
        MessageBox.Show("Không tìm thấy bản ghi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }
    
    Form f = new Form
    {
        Text = "Sửa Manufacturer",
        Size = new Size(400, 480),
        StartPosition = FormStartPosition.CenterParent,
        FormBorderStyle = FormBorderStyle.FixedDialog
    };

    string[] labels = { "Code", "Name", "Country", "Address", "Phone", "Email", "Contact Person" };
    TextBox[] inputs = new TextBox[labels.Length];

    int leftLabel = 30;
    int leftInput = 130;
    int topStart = 30;
    int heightStep = 40;

    for (int i = 0; i < labels.Length; i++)
    {
        int top = topStart + i * heightStep;
        var lbl = new Label { Text = labels[i], Left = leftLabel, Top = top + 3, Width = 100 };
        var txt = new TextBox { Left = leftInput, Top = top, Width = 200 };
        inputs[i] = txt;
        f.Controls.Add(lbl);
        f.Controls.Add(txt);
    }

    inputs[0].Text = m.Code;
    inputs[1].Text = m.Name;
    inputs[2].Text = m.Country;
    inputs[3].Text = m.Address;
    inputs[4].Text = m.Phone;
    inputs[5].Text = m.Email;
    inputs[6].Text = m.ContactPerson;

    Button btnSave = new Button { Text = "Lưu", Left = leftInput, Top = topStart + labels.Length * heightStep + 10, Width = 80 };
    Button btnCancel = new Button { Text = "Hủy", Left = leftInput + 120, Top = btnSave.Top, Width = 80 };

    btnCancel.Click += (_, __) => f.Close();
    btnSave.Click += (_, __) =>
    {
        try
        {
            m.Code = inputs[0].Text.Trim();
            m.Name = inputs[1].Text.Trim();
            m.Country = inputs[2].Text.Trim();
            m.Address = inputs[3].Text.Trim();
            m.Phone = inputs[4].Text.Trim();
            m.Email = inputs[5].Text.Trim();
            m.ContactPerson = inputs[6].Text.Trim();

            _controller.Update(m); // Cần controller có Update
            f.Close();
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Lỗi");
        }
    };

    f.Controls.AddRange(new Control[] { btnSave, btnCancel });
    f.ShowDialog();
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
