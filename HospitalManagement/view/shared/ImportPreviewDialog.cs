using HospitalManagement.utils.importer.core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HospitalManagement.view.shared
{
    /// <summary>
    /// Generic dialog để preview dữ liệu import trước khi apply.
    /// Dùng chung cho tất cả các module: Product, StockMovement, Employee, etc.
    /// </summary>
    /// <typeparam name="T">Kiểu DTO import (ProductImportDto, StockMovementImportDto, ...)</typeparam>
    public class ImportPreviewDialog<T> : Form where T : class
    {
        private readonly ImportPreviewResponse<T> _preview;
        private readonly Func<T, object[]> _dataMapper;
        private readonly string[] _columnHeaders;

        public ImportPreviewDialog(
            ImportPreviewResponse<T> preview,
            string[] columnHeaders,
            Func<T, object[]> dataMapper)
        {
            _preview = preview;
            _columnHeaders = columnHeaders;
            _dataMapper = dataMapper;

            InitializeDialog();
        }

        private void InitializeDialog()
        {
            Text = "Preview Import - Kiểm tra dữ liệu";
            Size = new Size(1000, 600);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;

            // Summary panel
            var summaryPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10)
            };

            var lblSummary = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = $"Tổng số dòng: {_preview.TotalRows}\n" +
                       $"✓ Hợp lệ: {_preview.ValidCount}    ✗ Lỗi: {_preview.InvalidCount}"
            };
            summaryPanel.Controls.Add(lblSummary);

            // Tab control for Valid/Invalid rows
            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // Valid rows tab
            var validTab = new TabPage("Dữ liệu hợp lệ (" + _preview.ValidCount + ")");
            var dgvValid = CreatePreviewGrid(_preview.ValidRows, false);
            validTab.Controls.Add(dgvValid);
            tabControl.TabPages.Add(validTab);

            // Invalid rows tab
            var invalidTab = new TabPage("Dữ liệu lỗi (" + _preview.InvalidCount + ")");
            var dgvInvalid = CreatePreviewGrid(_preview.InvalidRows, true);
            invalidTab.Controls.Add(dgvInvalid);
            tabControl.TabPages.Add(invalidTab);

            // Button panel
            var btnPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(10)
            };

            var btnApply = new Button
            {
                Text = "Apply - Lưu dữ liệu hợp lệ",
                Size = new Size(180, 35),
                Location = new Point(800, 12),
                Enabled = _preview.ValidCount > 0,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnApply.Click += (s, e) => DialogResult = DialogResult.OK;

            var btnCancel = new Button
            {
                Text = "Hủy",
                Size = new Size(100, 35),
                Location = new Point(680, 12),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            btnPanel.Controls.Add(btnApply);
            btnPanel.Controls.Add(btnCancel);

            Controls.Add(tabControl);
            Controls.Add(summaryPanel);
            Controls.Add(btnPanel);
        }

        private DataGridView CreatePreviewGrid(List<ImportRowData<T>> rows, bool showErrors)
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };

            // Row Index column
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RowIndex",
                HeaderText = "Dòng",
                DataPropertyName = "RowIndex",
                Width = 60
            });

            // Data columns
            foreach (var header in _columnHeaders)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = header,
                    HeaderText = header,
                    Width = 100
                });
            }

            // Error column (only for invalid rows)
            if (showErrors)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Errors",
                    HeaderText = "Lỗi",
                    Width = 350
                });
            }

            dgv.DataSource = rows;

            dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= rows.Count)
                    return;

                var rowData = rows[e.RowIndex];
                var data = rowData.Data;

                // Map data to columns
                if (data != null && e.ColumnIndex > 0 && e.ColumnIndex <= _columnHeaders.Length)
                {
                    var values = _dataMapper(data);
                    if (e.ColumnIndex - 1 < values.Length)
                        e.Value = values[e.ColumnIndex - 1];
                }

                // Show errors
                if (showErrors && dgv.Columns[e.ColumnIndex].Name == "Errors")
                {
                    e.Value = string.Join("; ", rowData.Errors.Select(err => err.ErrorMessage));
                }

                // Highlight invalid rows
                if (!rowData.IsValid)
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 230, 230);
                }
            };

            return dgv;
        }
    }
}
