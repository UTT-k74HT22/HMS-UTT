using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HospitalManagement.view.@base
{
    /// <summary>
    /// Base class cho tất cả các Management Panel trong ứng dụng
    /// Cung cấp cấu trúc chuẩn với: Toolbar (Filters + Actions), Table, Footer
    /// 
    /// Generic Type T: Entity type sẽ hiển thị trong table
    /// 
    /// Cách sử dụng:
    /// 1. Kế thừa class này với entity type cụ thể
    /// 2. Implement các abstract methods (TitleTotal, GetColumns, etc.)
    /// 3. Override các optional hooks nếu cần (BuildFilters, BuildActions, etc.)
    /// </summary>
    public abstract class BaseManagementPanel<T> : Panel where T : class
    {
        // ========== UI Components ==========
        protected DataGridView Table { get; private set; } = null!;
        protected BindingSource BindingSource { get; private set; } = null!;
        protected Label TotalLabel { get; private set; } = null!;

        // ========== Abstract Contract (Bắt buộc implement) ==========
        
        /// <summary>
        /// Tiêu đề hiển thị tổng số record (VD: "Tổng số nhân viên", "Tổng số khách hàng")
        /// </summary>
        protected abstract string TitleTotal();

        /// <summary>
        /// Định nghĩa các cột của table
        /// Return: Array of tuples (ColumnName, HeaderText, Width)
        /// VD: new[] { ("Id", "ID", 80), ("Name", "Họ tên", 200) }
        /// </summary>
        protected abstract (string PropertyName, string HeaderText, int Width)[] GetColumns();

        /// <summary>
        /// Fetch data từ database/service
        /// </summary>
        protected abstract List<T> FetchData();

        // ========== Optional Hooks (Override nếu cần) ==========

        /// <summary>
        /// Build phần filters (search, combo box, date picker, etc.)
        /// Return null nếu không cần filters
        /// </summary>
        protected virtual Panel? BuildFilters()
        {
            return null;
        }

        /// <summary>
        /// Build phần action buttons (Thêm, Sửa, Xóa, Export, etc.)
        /// Return null nếu không cần action buttons
        /// </summary>
        protected virtual Panel? BuildActions()
        {
            return null;
        }

        /// <summary>
        /// Hook được gọi sau khi table được tạo
        /// Dùng để custom thêm cho table (VD: set column colors, etc.)
        /// </summary>
        protected virtual void AfterTableCreated()
        {
        }

        /// <summary>
        /// Hook được gọi khi cần apply filters/search
        /// Override để implement logic filter
        /// </summary>
        protected virtual void ApplyFilters()
        {
        }

        /// <summary>
        /// Format cell value trước khi hiển thị
        /// Override nếu cần format đặc biệt (VD: currency, date, enum)
        /// </summary>
        protected virtual object FormatCellValue(T item, string propertyName)
        {
            // Default: dùng reflection để lấy giá trị property
            var prop = typeof(T).GetProperty(propertyName);
            if (prop == null) return "";
            
            var value = prop.GetValue(item);
            return value ?? "";
        }

        // ========== Designer Mode Detection ==========
        private bool IsDesignerMode()
        {
            return System.ComponentModel.LicenseManager.UsageMode == 
                   System.ComponentModel.LicenseUsageMode.Designtime;
        }

        // ========== Constructor ==========
        public BaseManagementPanel()
        {
            InitializePanel();
            
            // Chỉ build UI khi không phải designer mode
            if (!IsDesignerMode())
            {
                BuildUI();
            }
        }

        private void InitializePanel()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.Transparent;
            Padding = new Padding(6);
        }

        protected void BuildUI()
        {
            // Main layout: TableLayoutPanel với 3 rows
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // Toolbar
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Table
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));  // Footer

            // Build các phần
            var toolbar = BuildToolbar();
            var tablePanel = BuildTablePanel();
            var footer = BuildFooter();

            layout.Controls.Add(toolbar, 0, 0);
            layout.Controls.Add(tablePanel, 0, 1);
            layout.Controls.Add(footer, 0, 2);

            Controls.Add(layout);
        }

        // ========== Build Toolbar (Filters + Actions) ==========

        private Panel BuildToolbar()
        {
            var toolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            toolbar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            toolbar.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var filters = BuildFilters();
            if (filters != null)
            {
                filters.Dock = DockStyle.Top;
                filters.Margin = new Padding(0, 0, 0, 4);
                filters.Padding = new Padding(0);
                toolbar.Controls.Add(filters, 0, 0);
            }

            var actions = BuildActions();
            if (actions != null)
            {
                actions.Dock = DockStyle.Top;
                actions.Margin = new Padding(0, 0, 0, 10);
                actions.Padding = new Padding(0);
                toolbar.Controls.Add(actions, 0, 1);
            }

            // bọc lại bằng Panel để Dock Top ổn định
            return new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0),
                Controls = { toolbar }
            };
        }

        // ========== Build Table Panel ==========
        private Panel BuildTablePanel()
        {
            var cardPanel = UiFactory.CreateCardPanel();
            cardPanel.Dock = DockStyle.Fill;
            cardPanel.Padding = new Padding(0);
            cardPanel.Margin = new Padding(0, 2, 0, 8);

            // Create DataGridView
            Table = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false
            };

            UiFactory.StyleTable(Table);
            UiFactory.StyleHeader(Table);
            UiFactory.ApplyZebraStripes(Table);

            // Create columns based on GetColumns()
            var columns = GetColumns();
            foreach (var (propertyName, headerText, width) in columns)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Name = propertyName,
                    HeaderText = headerText,
                    DataPropertyName = propertyName,
                    Width = width,
                    SortMode = DataGridViewColumnSortMode.Automatic,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                };
                Table.Columns.Add(column);
            }
            
            if (Table.Columns.Count > 0)
            {
                var lastCol = Table.Columns[Table.Columns.Count - 1];
                lastCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                lastCol.MinimumWidth = 140; // tránh bị bóp quá nhỏ
            }
            
            // BindingSource for data
            BindingSource = new BindingSource();
            Table.DataSource = BindingSource;

            // Hook
            AfterTableCreated();

            cardPanel.Controls.Add(Table);
            return cardPanel;
        }

        // ========== Build Footer ==========
        private Panel BuildFooter()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                Height = 45,
                Padding = new Padding(12, 10, 12, 10)
            };

            // Vẽ border top cho footer
            panel.Paint += (sender, e) =>
            {
                var pen = new Pen(Color.FromArgb(235, 237, 242), 1);
                e.Graphics.DrawLine(pen, 0, 0, panel.Width, 0);
            };

            TotalLabel = new Label
            {
                Text = TitleTotal() + ": 0",
                Font = UiTheme.FONT_BOLD,
                ForeColor = UiTheme.TEXT,
                AutoSize = true,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };

            panel.Controls.Add(TotalLabel);
            return panel;
        }

        // ========== Public API ==========

        /// <summary>
        /// Reload dữ liệu từ database và refresh table
        /// </summary>
        public virtual void Reload()
        {
            try
            {
                var data = FetchData() ?? new List<T>();
                
                BindingSource.DataSource = data;
                TotalLabel.Text = $"{TitleTotal()}: {data.Count}";
                
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Lấy item đang được chọn trong table
        /// </summary>
        protected T? GetSelectedItem()
        {
            if (Table.CurrentRow == null) return null;
            return Table.CurrentRow.DataBoundItem as T;
        }

        /// <summary>
        /// Lấy index của row đang được chọn
        /// </summary>
        protected int GetSelectedRowIndex()
        {
            return Table.CurrentRow?.Index ?? -1;
        }

        /// <summary>
        /// Lấy giá trị của cell tại row và column name
        /// </summary>
        protected object? GetCellValue(int rowIndex, string columnName)
        {
            if (rowIndex < 0 || rowIndex >= Table.Rows.Count) return null;
            
            var row = Table.Rows[rowIndex];
            return row.Cells[columnName]?.Value;
        }

        /// <summary>
        /// Apply filter text trên BindingSource
        /// </summary>
        protected void ApplyTextFilter(string filterExpression)
        {
            try
            {
                BindingSource.Filter = filterExpression;
                TotalLabel.Text = $"{TitleTotal()}: {BindingSource.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Clear filter
        /// </summary>
        protected void ClearFilter()
        {
            BindingSource.Filter = null;
            var totalCount = BindingSource.Count;
            TotalLabel.Text = $"{TitleTotal()}: {totalCount}";
        }
    }
}
