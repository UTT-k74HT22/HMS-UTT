using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.controller;
using HospitalManagement.dto.response;
using HospitalManagement.entity.enums;
using Microsoft.Extensions.Configuration;
using HospitalManagement.configuration;
using HospitalManagement.Controller;
using HospitalManagement.entity;
using HospitalManagement.repository.impl;
using HospitalManagement.service.impl;

namespace HospitalManagement.view
{
    public partial class CustomerProfileManagementPanel : UserControl
    {
        private readonly CustomerProfileController _controller;

        public CustomerProfileManagementPanel()
        {
            InitializeComponent();
            
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            
            string connectionString = config.GetConnectionString("DefaultConnection");
            
            var repository = new CustomerProfileRepositoryImpl(connectionString);
            
            var service = new CustomerProfileServiceImpl(repository);
            
            _controller = new CustomerProfileController(service);
            
            InitGrid();
            
            LoadData();
        }

        private void InitGrid()
        {
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.ReadOnly = true;

            dgvCustomers.Columns.Clear();

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STT", Name = "STT", FillWeight = 10 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", Name = "ProfileId", FillWeight = 15 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã KH", Name = "Code", FillWeight = 15 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Họ Tên", Name = "FullName", FillWeight = 20 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SĐT", Name = "Phone", FillWeight = 15 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email", Name = "Email", FillWeight = 20 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Địa chỉ", Name = "Address", FillWeight = 25 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Loại KH", Name = "CustomerType", FillWeight = 15 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", Name = "Status", FillWeight = 15 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã thuế", Name = "TaxCode", FillWeight = 15 });

            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadData(string keyword = "")
        {
            try
            {
                dgvCustomers.Rows.Clear();
                List<CustomerProfileResponse> list = _controller.GetAll();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = keyword.Trim().ToLower();
                    list = list.Where(c =>
                        (c.FullName?.ToLower().Contains(keyword) ?? false) ||
                        (c.Code?.ToLower().Contains(keyword) ?? false) ||
                        (c.Phone?.ToLower().Contains(keyword) ?? false) ||
                        (c.Email?.ToLower().Contains(keyword) ?? false)
                    ).ToList();
                }

                int stt = 1;
                foreach (var c in list)
                {
                    dgvCustomers.Rows.Add(
                        stt++,
                        c.ProfileId,
                        c.Code,
                        c.FullName,
                        c.Phone,
                        c.Email,
                        c.Address,
                        c.CustomerType.ToString(),
                        c.Status.ToString(),
                        c.TaxCode
                    );
                }

                lblTotal.Text = $"Tổng: {list.Count}";
                dgvCustomers.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int profileId = (int)dgvCustomers.SelectedRows[0].Cells["ProfileId"].Value;
            var customer = _controller.GetByProfileId(profileId);
            if (customer == null)
            {
                MessageBox.Show("Không tìm thấy khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Form dialog = new Form())
            {
                dialog.Text = "Sửa Khách hàng";
                dialog.Size = new Size(400, 350);
                dialog.StartPosition = FormStartPosition.CenterParent;

                Label lblFullName = new Label { Text = "Họ Tên:", Location = new Point(20, 20), AutoSize = true };
                TextBox txtFullName = new TextBox { Location = new Point(120, 18), Width = 220, Text = customer.FullName };

                Label lblPhone = new Label { Text = "SĐT:", Location = new Point(20, 60), AutoSize = true };
                TextBox txtPhone = new TextBox { Location = new Point(120, 58), Width = 220, Text = customer.Phone };

                Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 100), AutoSize = true };
                TextBox txtEmail = new TextBox { Location = new Point(120, 98), Width = 220, Text = customer.Email };

                Label lblAddress = new Label { Text = "Địa chỉ:", Location = new Point(20, 140), AutoSize = true };
                TextBox txtAddress = new TextBox { Location = new Point(120, 138), Width = 220, Text = customer.Address };

                Label lblCustomerType = new Label { Text = "Loại KH:", Location = new Point(20, 180), AutoSize = true };
                ComboBox cbCustomerType = new ComboBox
                {
                    Location = new Point(120, 178),
                    Width = 220,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cbCustomerType.Items.AddRange(Enum.GetNames(typeof(CustomerType)));
                cbCustomerType.SelectedItem = customer.CustomerType.ToString();

                Label lblStatus = new Label { Text = "Trạng thái:", Location = new Point(20, 220), AutoSize = true };
                ComboBox cbStatus = new ComboBox
                {
                    Location = new Point(120, 218),
                    Width = 220,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cbStatus.Items.AddRange(Enum.GetNames(typeof(ProfileStatus)));
                cbStatus.SelectedItem = customer.Status.ToString();

                Button btnSave = new Button { Text = "Lưu", Location = new Point(120, 260), Width = 80 };
                Button btnCancel = new Button { Text = "Hủy", Location = new Point(220, 260), Width = 80 };

                btnCancel.Click += (s, ev) => dialog.Close();
                btnSave.Click += (s, ev) =>
                {
                    try
                    {
                        customer.FullName = txtFullName.Text.Trim();
                        customer.Phone = txtPhone.Text.Trim();
                        customer.Email = txtEmail.Text.Trim();
                        customer.Address = txtAddress.Text.Trim();
                        customer.CustomerType = (CustomerType)Enum.Parse(typeof(CustomerType), cbCustomerType.SelectedItem.ToString());
                        customer.Status = (ProfileStatus)Enum.Parse(typeof(ProfileStatus), cbStatus.SelectedItem.ToString());

                        _controller.Update(customer);
                        MessageBox.Show("Cập nhật khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        dialog.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi cập nhật khách hàng:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                dialog.Controls.Add(lblFullName);
                dialog.Controls.Add(txtFullName);
                dialog.Controls.Add(lblPhone);
                dialog.Controls.Add(txtPhone);
                dialog.Controls.Add(lblEmail);
                dialog.Controls.Add(txtEmail);
                dialog.Controls.Add(lblAddress);
                dialog.Controls.Add(txtAddress);
                dialog.Controls.Add(lblCustomerType);
                dialog.Controls.Add(cbCustomerType);
                dialog.Controls.Add(lblStatus);
                dialog.Controls.Add(cbStatus);
                dialog.Controls.Add(btnSave);
                dialog.Controls.Add(btnCancel);

                dialog.ShowDialog(this);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int profileId = (int)dgvCustomers.SelectedRows[0].Cells["ProfileId"].Value;
            string code = dgvCustomers.SelectedRows[0].Cells["Code"].Value.ToString();

            DialogResult dr = MessageBox.Show(
                $"Bạn có chắc muốn xóa khách hàng {code} không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr == DialogResult.Yes)
            {
                try
                {
                    _controller.SoftDelete(profileId);
                    MessageBox.Show("Xóa khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa khách hàng:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtKeyword.Text = "";
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(txtKeyword.Text);
        }
    }
}
