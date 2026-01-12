# FRONTEND UI KNOWLEDGE REVIEW - WINFORMS C# (.NET)

> **Mục đích**: Tài liệu này tổng hợp kiến thức Frontend đã áp dụng trong HMS-UTT dành cho Junior Developer

---

## 📚 MỤC LỤC

1. [TỔNG QUAN WINFORMS](#1-tổng-quan-winforms)
2. [KIẾN TRÚC UI COMPONENTS](#2-kiến-trúc-ui-components)
3. [DATA BINDING](#3-data-binding)
4. [EVENT HANDLING](#4-event-handling)
5. [DIALOG MANAGEMENT](#5-dialog-management)
6. [DATAGRIDVIEW CUSTOMIZATION](#6-datagridview-customization)
7. [FILTER & SEARCH](#7-filter--search)
8. [EXCEL EXPORT/IMPORT UI](#8-excel-exportimport-ui)
9. [UI/UX BEST PRACTICES](#9-uiux-best-practices)
10. [COMMON CONTROLS](#10-common-controls)

---

## 1. TỔNG QUAN WINFORMS

### 1.1. Windows Forms là gì?

**Windows Forms (WinForms)** là framework UI desktop của .NET cho phép xây dựng ứng dụng Windows native với:
- Event-driven programming
- Rich controls (Button, TextBox, DataGridView, ...)
- Drag-and-drop designer trong Visual Studio

### 1.2. Kiến trúc Event-Driven

```
┌─────────────┐
│    User     │
└──────┬──────┘
       │ Click button
       ▼
┌─────────────┐
│   Button    │
└──────┬──────┘
       │ Raise event
       ▼
┌─────────────┐
│ Event       │
│ Handler     │ ← Đây là nơi bạn viết code logic
└──────┬──────┘
       │ Update UI
       ▼
┌─────────────┐
│   View      │
└─────────────┘
```

### 1.3. Control Hierarchy

```
Form (cửa sổ chính)
├─ Panel
│  ├─ Label
│  ├─ TextBox
│  └─ Button
├─ DataGridView
└─ StatusStrip
```

---

## 2. KIẾN TRÚC UI COMPONENTS

### 2.1. UserControl (Panel) Architecture

Mỗi màn hình là một **UserControl** (reusable component):

```csharp
public partial class AccountManagementPanel : UserControl
{
    private readonly AccountController _controller;
    private readonly BindingSource _bs = new();
    private List<AccountResponse> _all = new();

    public AccountManagementPanel(AccountController controller)
    {
        _controller = controller;
        
        InitializeComponent(); // Khởi tạo UI components
        
        dgvAccounts.DataSource = _bs; // Bind DataGridView với BindingSource
        
        InitGrid();   // Cấu hình DataGridView
        InitEvents(); // Đăng ký event handlers
        
        LoadData();   // Load dữ liệu ban đầu
    }
}
```

### 2.2. Layout Structure

**Ví dụ: AccountManagementPanel**

```
┌─────────────────────────────────────────────────┐
│ PANEL: Search & Filter                          │
│ ┌───────────┐ ┌────────┐ ┌─────────┐           │
│ │ TextBox   │ │ Search │ │ Refresh │           │
│ └───────────┘ └────────┘ └─────────┘           │
└─────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────┐
│ PANEL: Action Buttons                           │
│ ┌────┐ ┌────┐ ┌────┐ ┌──────┐ ┌──────┐        │
│ │Add │ │Edit│ │Del │ │Detail│ │Export│        │
│ └────┘ └────┘ └────┘ └──────┘ └──────┘        │
└─────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────┐
│ DataGridView (Main data display)                │
│ ┌────┬──────────┬──────┬────────┬────────┐    │
│ │STT │Username  │Role  │Active  │Actions │    │
│ ├────┼──────────┼──────┼────────┼────────┤    │
│ │ 1  │admin     │ADMIN │ ✓      │        │    │
│ │ 2  │staff1    │STAFF │ ✓      │        │    │
│ └────┴──────────┴──────┴────────┴────────┘    │
└─────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────┐
│ StatusStrip: Tổng số: 2                         │
└─────────────────────────────────────────────────┘
```

### 2.3. Component Initialization Flow

```csharp
Constructor
    │
    ├─ InitializeComponent()  // Designer-generated code
    │
    ├─ Setup data source
    │  └─ dgvAccounts.DataSource = _bs;
    │
    ├─ InitGrid()
    │  ├─ Set column properties
    │  ├─ Set styles
    │  └─ Register formatting events
    │
    ├─ InitEvents()
    │  ├─ Button.Click += handler
    │  └─ TextBox.KeyDown += handler
    │
    └─ LoadData()
       └─ Fetch data from controller
```

---

## 3. DATA BINDING

### 3.1. BindingSource Pattern

**BindingSource** là layer trung gian giữa data và UI controls:

```csharp
// 1. Khai báo BindingSource
private readonly BindingSource _bs = new();

// 2. Bind với DataGridView
dgvAccounts.DataSource = _bs;

// 3. Set data source
_bs.DataSource = accountList; // List<AccountResponse>

// 4. DataGridView tự động cập nhật khi _bs thay đổi
```

### 3.2. Lợi ích của BindingSource

| Tính năng | Mô tả |
|-----------|-------|
| **Two-way binding** | Thay đổi data → UI tự cập nhật |
| **Filtering** | Dễ dàng filter dữ liệu |
| **Sorting** | Hỗ trợ sort tự động |
| **Current row tracking** | Biết user đang chọn row nào |
| **Change notification** | Tự động refresh UI khi data thay đổi |

### 3.3. Load Data Flow

```csharp
private void LoadData()
{
    try
    {
        // 1. Gọi controller để lấy data
        _all = _controller.GetAllAccounts();
        
        // 2. Apply filters (nếu có)
        ApplyFilters();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Error", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void ApplyFilters()
{
    var kw = (txtKeyword.Text ?? "").Trim().ToLower();
    
    // 3. Filter data
    var filtered = _all.Where(x =>
            string.IsNullOrEmpty(kw)
            || x.Username.ToLower().Contains(kw)
            || x.Role.ToString().Contains(kw)
            || x.Id.ToString().Contains(kw)
        )
        .ToList();
    
    // 4. Update BindingSource → DataGridView tự refresh
    _bs.DataSource = filtered;
    
    // 5. Update status label
    lblTotal.Text = $"Tổng số: {filtered.Count}";
}
```

---

## 4. EVENT HANDLING

### 4.1. Đăng ký Event Handlers

```csharp
private void InitEvents()
{
    // Button click events
    btnSearch.Click += (_, _) => ApplyFilters();
    btnRefresh.Click += (_, _) => { txtKeyword.Clear(); LoadData(); };
    btnAdd.Click += (_, _) => CreateAccount();
    btnEdit.Click += (_, _) => UpdateAccount();
    btnDelete.Click += (_, _) => DeleteAccount();
    
    // Keyboard events
    txtKeyword.KeyDown += (_, e) =>
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true; // Ngăn "ding" sound
            ApplyFilters();
        }
    };
    
    // ComboBox events
    cboWarehouse.SelectedIndexChanged += (_, _) => ApplyFilters();
}
```

### 4.2. Lambda Expression vs Method Reference

#### ✅ Lambda (phù hợp cho logic ngắn)
```csharp
btnSearch.Click += (_, _) => ApplyFilters();
```

#### ✅ Method Reference (phù hợp cho logic dài)
```csharp
btnSearch.Click += BtnSearch_Click;

private void BtnSearch_Click(object sender, EventArgs e)
{
    // Complex logic here...
}
```

### 4.3. Event Handler Patterns

#### Pattern 1: CRUD Operations

```csharp
private void CreateAccount()
{
    // 1. Mở dialog
    var dialog = new AccountFormDialog();
    
    // 2. Đợi user nhập và submit
    if (dialog.ShowDialog() == DialogResult.OK && dialog.Result != null)
    {
        try
        {
            // 3. Gọi controller
            _controller.CreateAccount(dialog.Result);
            
            // 4. Hiển thị thông báo thành công
            MessageBox.Show("Tạo tài khoản thành công!", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // 5. Refresh data
            LoadData();
        }
        catch (Exception ex)
        {
            // 6. Hiển thị lỗi
            MessageBox.Show($"Lỗi: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
```

#### Pattern 2: Delete with Confirmation

```csharp
private void DeleteAccount()
{
    // 1. Lấy item được chọn
    var account = GetSelected();
    if (account == null)
    {
        MessageBox.Show("Vui lòng chọn tài khoản cần xóa", "Warning", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }
    
    // 2. Xác nhận trước khi xóa
    if (MessageBox.Show(
        $"Xác nhận xóa tài khoản [{account.Username}]?", 
        "Confirm", 
        MessageBoxButtons.YesNo, 
        MessageBoxIcon.Question) == DialogResult.Yes)
    {
        try
        {
            // 3. Thực hiện xóa
            _controller.DeleteAccount(account.Id);
            
            // 4. Thông báo thành công
            MessageBox.Show("Xóa tài khoản thành công!", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // 5. Refresh data
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
```

---

## 5. DIALOG MANAGEMENT

### 5.1. Custom Dialog Pattern

```csharp
public class AccountFormDialog : Form
{
    // Public property để lấy kết quả
    public CreateAccountRequest? Result { get; private set; }
    
    // UI controls
    private TextBox txtUsername;
    private TextBox txtPassword;
    private ComboBox cboRole;
    private Button btnSave;
    private Button btnCancel;
    
    public AccountFormDialog()
    {
        // Setup form properties
        Text = "Tạo tài khoản mới";
        Size = new Size(400, 500);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        
        // Initialize UI
        InitializeControls();
    }
    
    private void InitializeControls()
    {
        // Layout panel
        var pnlMain = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 6,
            Padding = new Padding(16)
        };
        
        // Add labels and inputs
        pnlMain.Controls.Add(new Label { Text = "Username *:" }, 0, 0);
        txtUsername = new TextBox { Dock = DockStyle.Fill };
        pnlMain.Controls.Add(txtUsername, 1, 0);
        
        // ... more controls
        
        // Buttons
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += (_, _) => DialogResult = DialogResult.Cancel;
        
        Controls.Add(pnlMain);
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
                throw new Exception("Username không được trống");
            
            // Tạo result object
            Result = new CreateAccountRequest
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text,
                Role = (RoleType)cboRole.SelectedItem,
                // ... other fields
            };
            
            // Close dialog với OK result
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
```

### 5.2. Sử dụng Dialog

```csharp
// Mở dialog
var dialog = new AccountFormDialog();

// ShowDialog() = modal (block until closed)
if (dialog.ShowDialog() == DialogResult.OK && dialog.Result != null)
{
    // User clicked Save và có data
    _controller.CreateAccount(dialog.Result);
}
else
{
    // User clicked Cancel hoặc Close
}
```

### 5.3. Dialog Types

| Type | Method | Mô tả |
|------|--------|-------|
| **Modal** | `ShowDialog()` | Block UI, đợi user đóng dialog |
| **Modeless** | `Show()` | Không block, cho phép interact với form khác |

---

## 6. DATAGRIDVIEW CUSTOMIZATION

### 6.1. Cấu hình cơ bản

```csharp
private void InitGrid()
{
    // Disable auto-generation
    dgvAccounts.AutoGenerateColumns = false;
    
    // Read-only grid
    dgvAccounts.AllowUserToAddRows = false;
    dgvAccounts.AllowUserToDeleteRows = false;
    dgvAccounts.ReadOnly = true;
    
    // Selection
    dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    dgvAccounts.MultiSelect = false;
    
    // Appearance
    dgvAccounts.RowHeadersVisible = false;
    dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    
    // Clear default columns
    dgvAccounts.Columns.Clear();
    
    // Add custom columns
    AddColumns();
    
    // Register formatting event
    dgvAccounts.CellFormatting += DgvAccounts_CellFormatting;
}
```

### 6.2. Thêm Columns

```csharp
private void AddColumns()
{
    // STT (unbound column - không bind với data)
    dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
    {
        Name = "STT",
        HeaderText = "STT",
        Width = 60,
        SortMode = DataGridViewColumnSortMode.NotSortable
    });
    
    // ID (bound column - bind với property của DTO)
    dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
    {
        Name = nameof(AccountResponse.Id),
        DataPropertyName = nameof(AccountResponse.Id), // Bind to AccountResponse.Id
        HeaderText = "ID",
        FillWeight = 18 // Tỉ lệ chiều rộng
    });
    
    // Username
    dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
    {
        Name = nameof(AccountResponse.Username),
        DataPropertyName = nameof(AccountResponse.Username),
        HeaderText = "Username",
        FillWeight = 34
    });
    
    // Active (CheckBox column)
    dgvAccounts.Columns.Add(new DataGridViewCheckBoxColumn
    {
        Name = nameof(AccountResponse.Active),
        DataPropertyName = nameof(AccountResponse.Active),
        HeaderText = "Active",
        FillWeight = 16
    });
}
```

### 6.3. Cell Formatting

```csharp
private void DgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
{
    if (e.RowIndex < 0) return; // Skip header row
    
    var item = dgvAccounts.Rows[e.RowIndex].DataBoundItem as AccountResponse;
    if (item == null) return;
    
    // Format STT column
    if (dgvAccounts.Columns[e.ColumnIndex].Name == "STT")
    {
        e.Value = (e.RowIndex + 1).ToString();
        e.FormattingApplied = true;
    }
    
    // Format Role column with color
    if (dgvAccounts.Columns[e.ColumnIndex].Name == nameof(AccountResponse.Role))
    {
        if (item.Role == RoleType.ADMIN)
        {
            e.CellStyle.ForeColor = Color.Red;
            e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
        }
    }
    
    // Format Active column
    if (dgvAccounts.Columns[e.ColumnIndex].Name == nameof(AccountResponse.Active))
    {
        e.Value = item.Active ? "✓" : "✗";
        e.CellStyle.ForeColor = item.Active ? Color.Green : Color.Red;
        e.FormattingApplied = true;
    }
}
```

### 6.4. Advanced Formatting (Inventory Example)

```csharp
dgvInventory.CellFormatting += (_, e) =>
{
    if (e.RowIndex < 0) return;
    var item = dgvInventory.Rows[e.RowIndex].DataBoundItem as InventoryResponse;
    if (item == null) return;
    
    // Dynamic status với color coding
    if (dgvInventory.Columns[e.ColumnIndex].Name == "Status")
    {
        string status;
        if (item.IsLowStock == true) 
            status = "Sắp hết";
        else if (item.IsNearExpiry == true) 
            status = "Sắp hết hạn";
        else if (item.IsOverStock == true) 
            status = "Dư thừa";
        else 
            status = "Bình thường";
        
        e.Value = status;
        
        // Color coding
        if (status == "Sắp hết")
            e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69); // Red
        else if (status == "Sắp hết hạn")
            e.CellStyle.ForeColor = Color.FromArgb(255, 87, 34); // Orange
        else if (status == "Dư thừa")
            e.CellStyle.ForeColor = Color.FromArgb(255, 193, 7); // Yellow
        
        e.FormattingApplied = true;
    }
    
    // Handle null values
    if (e.Value == null)
    {
        e.Value = "-";
        e.FormattingApplied = true;
    }
};
```

### 6.5. Get Selected Row

```csharp
private AccountResponse? GetSelected()
{
    return dgvAccounts.CurrentRow?.DataBoundItem as AccountResponse;
}

// Sử dụng
var account = GetSelected();
if (account == null)
{
    MessageBox.Show("Vui lòng chọn tài khoản", "Warning", 
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
    return;
}
```

---

## 7. FILTER & SEARCH

### 7.1. Real-time Search Pattern

```csharp
private void InitEvents()
{
    // Search as you type
    txtKeyword.TextChanged += (_, _) => ApplyFilters();
    
    // Search on Enter key
    txtKeyword.KeyDown += (_, e) =>
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            ApplyFilters();
        }
    };
    
    // Search button
    btnSearch.Click += (_, _) => ApplyFilters();
}
```

### 7.2. Multi-criteria Filter

```csharp
private void ApplyFilters()
{
    var kw = (txtKeyword.Text ?? "").Trim().ToLower();
    var selectedWarehouse = cboWarehouse.SelectedItem as WarehouseItem;
    var statusFilter = cboStatus.SelectedItem?.ToString() ?? "TẤT CẢ";
    
    var filtered = _all.Where(x =>
    {
        // 1. Keyword filter (search nhiều fields)
        bool matchKeyword = string.IsNullOrEmpty(kw) ||
            (x.ProductCode?.ToLower().Contains(kw) ?? false) ||
            (x.ProductName?.ToLower().Contains(kw) ?? false) ||
            (x.WarehouseName?.ToLower().Contains(kw) ?? false);
        
        if (!matchKeyword) return false;
        
        // 2. Warehouse filter
        if (selectedWarehouse?.Id != null && 
            x.WarehouseId != selectedWarehouse.Id)
            return false;
        
        // 3. Status filter
        if (statusFilter != "TẤT CẢ")
        {
            string itemStatus = x.IsLowStock == true ? "SẮP HẾT HÀNG" : "BÌNH THƯỜNG";
            if (itemStatus != statusFilter) return false;
        }
        
        return true;
    }).ToList();
    
    // Update UI
    _bs.DataSource = filtered;
    lblTotal.Text = $"Tổng số: {filtered.Count}";
}
```

### 7.3. ComboBox Filter Pattern

```csharp
// Class helper cho ComboBox items
private class WarehouseItem
{
    public long? Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

private void LoadWarehouses()
{
    var warehouses = _warehouseController.GetAllWarehouses();
    
    cboWarehouse.Items.Clear();
    
    // Add "All" option
    cboWarehouse.Items.Add(new WarehouseItem 
    { 
        Id = null, 
        Name = "TẤT CẢ KHO" 
    });
    
    // Add warehouse items
    foreach (var wh in warehouses)
    {
        cboWarehouse.Items.Add(new WarehouseItem 
        { 
            Id = wh.Id, 
            Name = wh.Name 
        });
    }
    
    cboWarehouse.DisplayMember = "Name";
    cboWarehouse.SelectedIndex = 0;
}
```

---

## 8. EXCEL EXPORT/IMPORT UI

### 8.1. Export Excel Button

```csharp
private void ExportToExcel()
{
    var filteredData = _bs.List.Cast<AccountResponse>().ToList();
    
    // Sử dụng ExcelExporter utility
    ExcelExporter.ExportWithDialog<AccountResponse>(
        filteredData,
        new AccountExcelWriter(),
        this.FindForm() // Parent form
    );
}
```

**Flow:**
```
User click Export
     │
     ├─ Get filtered data từ BindingSource
     │
     ├─ Gọi ExcelExporter.ExportWithDialog()
     │     │
     │     ├─ Show SaveFileDialog
     │     │
     │     ├─ Generate Excel file
     │     │
     │     ├─ Show success message
     │     │
     │     └─ Offer to open file
     │
     └─ Done
```

### 8.2. Import Excel Button (Stock Movement Example)

```csharp
private void ImportExcel()
{
    // 1. Mở file dialog
    using var ofd = new OpenFileDialog
    {
        Filter = "Excel Files (*.xlsx)|*.xlsx",
        Title = "Chọn file Excel để import Stock Movement"
    };
    
    if (ofd.ShowDialog() != DialogResult.OK) return;
    
    try
    {
        // 2. Preview dữ liệu
        var preview = _stockMovementController.PreviewImport(ofd.FileName);
        
        // 3. Hiển thị preview dialog
        var previewDialog = new ImportPreviewDialog<StockMovementImportDto>(
            preview,
            new[] { "Loại", "Kho", "Mã SP", "Lô", "Số lượng", "Ghi chú" },
            dto => new object[]
            {
                dto.MovementType?.ToString() ?? "",
                dto.WarehouseCode ?? "",
                dto.ProductCode ?? "",
                dto.BatchCode ?? "",
                dto.Quantity,
                dto.Note ?? ""
            }
        );
        
        // 4. User xác nhận Apply
        if (previewDialog.ShowDialog(this) == DialogResult.OK)
        {
            var validData = preview.ValidRows.Select(r => r.Data!).ToList();
            _stockMovementController.ApplyImport(validData);
            
            MessageBox.Show(
                $"Đã import thành công {validData.Count} giao dịch!",
                "Thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            
            LoadData(); // Refresh grid
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show(
            $"Lỗi khi import: {ex.Message}",
            "Lỗi",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
    }
}
```

### 8.3. Download Template Button

```csharp
private void DownloadTemplate()
{
    using var sfd = new SaveFileDialog
    {
        Filter = "Excel Files (*.xlsx)|*.xlsx",
        FileName = "StockMovement_Import_Template.xlsx"
    };
    
    if (sfd.ShowDialog() == DialogResult.OK)
    {
        try
        {
            // Gọi controller để generate template
            var templateData = _stockMovementController.GenerateImportTemplate();
            
            // Lưu file
            System.IO.File.WriteAllBytes(sfd.FileName, templateData);
            
            MessageBox.Show(
                $"Đã tải mẫu về: {sfd.FileName}",
                "Thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Lỗi khi tạo template: {ex.Message}",
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}
```

### 8.4. ImportPreviewDialog

```csharp
public class ImportPreviewDialog<T> : Form where T : class
{
    private DataGridView dgvPreview;
    private Label lblSummary;
    
    public ImportPreviewDialog(
        ImportPreviewResponse<T> preview,
        string[] headers,
        Func<T, object[]> rowMapper)
    {
        Text = "Preview Import Data";
        Size = new Size(800, 600);
        
        // Setup grid
        dgvPreview = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            ReadOnly = true
        };
        
        // Add columns
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Status",
            HeaderText = "Status",
            Width = 80
        });
        
        foreach (var header in headers)
        {
            dgvPreview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = header,
                HeaderText = header
            });
        }
        
        dgvPreview.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Errors",
            HeaderText = "Lỗi",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
        
        // Populate data
        foreach (var row in preview.ValidRows)
        {
            var dgvRow = new DataGridViewRow();
            dgvRow.CreateCells(dgvPreview);
            dgvRow.Cells[0].Value = "✓ OK";
            dgvRow.Cells[0].Style.BackColor = Color.LightGreen;
            
            var values = rowMapper(row.Data!);
            for (int i = 0; i < values.Length; i++)
            {
                dgvRow.Cells[i + 1].Value = values[i];
            }
            
            dgvPreview.Rows.Add(dgvRow);
        }
        
        foreach (var row in preview.InvalidRows)
        {
            var dgvRow = new DataGridViewRow();
            dgvRow.CreateCells(dgvPreview);
            dgvRow.Cells[0].Value = "✗ ERROR";
            dgvRow.Cells[0].Style.BackColor = Color.LightCoral;
            
            if (row.Data != null)
            {
                var values = rowMapper(row.Data);
                for (int i = 0; i < values.Length; i++)
                {
                    dgvRow.Cells[i + 1].Value = values[i];
                }
            }
            
            dgvRow.Cells[dgvPreview.Columns.Count - 1].Value = 
                string.Join(", ", row.Errors.Select(e => e.ErrorMessage));
            
            dgvPreview.Rows.Add(dgvRow);
        }
        
        // Summary label
        lblSummary.Text = $"Valid: {preview.ValidRows.Count} | " +
                         $"Invalid: {preview.InvalidRows.Count} | " +
                         $"Total: {preview.TotalRows}";
        
        // Buttons
        var btnApply = new Button { Text = "Apply Import" };
        var btnCancel = new Button { Text = "Cancel" };
        
        btnApply.Click += (_, _) => 
        {
            if (preview.ValidRows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hợp lệ để import!");
                return;
            }
            DialogResult = DialogResult.OK;
        };
        
        btnCancel.Click += (_, _) => DialogResult = DialogResult.Cancel;
    }
}
```

---

## 9. UI/UX BEST PRACTICES

### 9.1. Error Handling UI

```csharp
try
{
    _controller.CreateAccount(request);
    
    // ✅ Success message
    MessageBox.Show(
        "Tạo tài khoản thành công!",
        "Success",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information
    );
    
    LoadData();
}
catch (Exception ex)
{
    // ✅ Error message chi tiết
    MessageBox.Show(
        $"Lỗi: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error
    );
}
```

### 9.2. Confirmation Dialogs

```csharp
// ✅ Confirm trước khi delete
if (MessageBox.Show(
    $"Xác nhận xóa tài khoản [{account.Username}]?",
    "Confirm",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question) == DialogResult.Yes)
{
    // Proceed with delete
}
```

### 9.3. Loading States

```csharp
private void LoadData()
{
    try
    {
        // ✅ Disable controls khi đang load
        btnRefresh.Enabled = false;
        Cursor = Cursors.WaitCursor;
        
        _all = _controller.GetAllAccounts();
        ApplyFilters();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi: {ex.Message}", "Error");
    }
    finally
    {
        // ✅ Re-enable controls
        btnRefresh.Enabled = true;
        Cursor = Cursors.Default;
    }
}
```

### 9.4. Input Validation UI

```csharp
private void BtnSave_Click(object sender, EventArgs e)
{
    try
    {
        // ✅ Clear previous error indicators
        errorProvider.Clear();
        
        // ✅ Validate từng field
        if (string.IsNullOrWhiteSpace(txtUsername.Text))
        {
            errorProvider.SetError(txtUsername, "Username không được trống");
            txtUsername.Focus();
            return;
        }
        
        if (txtPassword.Text.Length < 6)
        {
            errorProvider.SetError(txtPassword, "Password phải có ít nhất 6 ký tự");
            txtPassword.Focus();
            return;
        }
        
        // Save logic...
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message, "Error");
    }
}
```

### 9.5. Accessibility

```csharp
// ✅ TabIndex ordering
txtUsername.TabIndex = 1;
txtPassword.TabIndex = 2;
cboRole.TabIndex = 3;
btnSave.TabIndex = 4;

// ✅ Keyboard shortcuts
btnSave.Text = "&Save"; // Alt+S
btnCancel.Text = "&Cancel"; // Alt+C

// ✅ Default button
AcceptButton = btnSave; // Enter key
CancelButton = btnCancel; // Escape key
```

---

## 10. COMMON CONTROLS

### 10.1. TextBox

```csharp
var txtUsername = new TextBox
{
    Dock = DockStyle.Fill,
    MaxLength = 50,
    PlaceholderText = "Nhập username..." // .NET 6+
};

// Events
txtUsername.TextChanged += (s, e) => { /* Real-time validation */ };
txtUsername.KeyDown += (s, e) => 
{
    if (e.KeyCode == Keys.Enter)
    {
        // Submit on Enter
    }
};
```

### 10.2. ComboBox

```csharp
var cboRole = new ComboBox
{
    Dock = DockStyle.Fill,
    DropDownStyle = ComboBoxStyle.DropDownList // Không cho nhập text
};

// Populate
cboRole.DataSource = Enum.GetValues(typeof(RoleType));

// Or with custom items
cboRole.Items.Add(new { Id = 1, Name = "Admin" });
cboRole.DisplayMember = "Name";
cboRole.ValueMember = "Id";

// Get selected
var selected = (RoleType)cboRole.SelectedItem;
```

### 10.3. DateTimePicker

```csharp
var dtpHiredDate = new DateTimePicker
{
    Dock = DockStyle.Fill,
    Format = DateTimePickerFormat.Short,
    Value = DateTime.Now
};

// Get value
DateTime hiredDate = dtpHiredDate.Value;
```

### 10.4. NumericUpDown

```csharp
var nudQuantity = new NumericUpDown
{
    Dock = DockStyle.Fill,
    Minimum = 0,
    Maximum = 999999,
    DecimalPlaces = 0
};

// Get value
int quantity = (int)nudQuantity.Value;
```

### 10.5. CheckBox

```csharp
var chkActive = new CheckBox
{
    Text = "Kích hoạt",
    Checked = true
};

// Get value
bool isActive = chkActive.Checked;
```

### 10.6. TableLayoutPanel (Layout Manager)

```csharp
var pnlMain = new TableLayoutPanel
{
    Dock = DockStyle.Fill,
    ColumnCount = 2,
    RowCount = 5,
    Padding = new Padding(16)
};

// Define column styles
pnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); // Label column
pnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));  // Input column

// Add controls
pnlMain.Controls.Add(new Label { Text = "Username:" }, 0, 0); // col=0, row=0
pnlMain.Controls.Add(txtUsername, 1, 0);                      // col=1, row=0
```

### 10.7. FlowLayoutPanel (Linear Layout)

```csharp
var pnlButtons = new FlowLayoutPanel
{
    Dock = DockStyle.Bottom,
    FlowDirection = FlowDirection.RightToLeft, // Right-align buttons
    Height = 50,
    Padding = new Padding(10)
};

pnlButtons.Controls.Add(btnCancel);
pnlButtons.Controls.Add(btnSave);
```

---

## 📝 CHECKLIST CHO JUNIOR DEVELOPER

Khi implement một UI screen mới:

### Setup
- [ ] **UserControl** cho màn hình chính
- [ ] **BindingSource** cho data binding
- [ ] **Controller** dependency injection

### Grid
- [ ] **AutoGenerateColumns = false**
- [ ] **ReadOnly = true** (nếu không cho edit)
- [ ] **FullRowSelect**
- [ ] **Custom columns** với DataPropertyName
- [ ] **CellFormatting** event cho styling

### Events
- [ ] **Button click** handlers
- [ ] **Enter key** submit
- [ ] **ComboBox SelectedIndexChanged**
- [ ] **TextBox TextChanged** cho real-time search

### CRUD
- [ ] **Create**: Dialog → Validate → Controller → Refresh
- [ ] **Update**: Get selected → Dialog → Controller → Refresh
- [ ] **Delete**: Get selected → Confirm → Controller → Refresh
- [ ] **View**: Get selected → Show details

### UX
- [ ] **Error messages** rõ ràng
- [ ] **Confirmation** trước khi delete
- [ ] **Success messages** sau khi save
- [ ] **Loading states** khi fetch data
- [ ] **Null checks** trước khi access

---

## 🎯 KẾT LUẬN

Frontend WinForms của HMS-UTT áp dụng:

1. **UserControl Architecture**: Mỗi màn hình là 1 reusable component
2. **Data Binding**: BindingSource pattern cho two-way binding
3. **Event-Driven**: Lambda expressions và method handlers
4. **Custom Dialogs**: Modal forms cho CRUD operations
5. **DataGridView Customization**: Manual columns + CellFormatting
6. **Filter & Search**: Real-time filtering với LINQ
7. **Excel Integration**: Export/Import với preview
8. **UX Best Practices**: Error handling, confirmations, loading states

**Nguyên tắc vàng**:
- Luôn validate input trước khi submit
- Hiển thị error messages rõ ràng
- Confirm trước khi delete
- Refresh data sau khi CRUD
- Disable controls khi đang loading
- Null checks everywhere!
