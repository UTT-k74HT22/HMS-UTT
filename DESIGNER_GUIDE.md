# 🎨 Hướng Dẫn Sử Dụng WinForms Designer

## ✅ Vấn Đề Đã Được Khắc Phục

Tất cả các Panel/Form giờ đã hỗ trợ Designer mode và có thể mở trong tab Designer để kéo thả UI.

## 🔧 Cách Fix Designer Issues

### Pattern được áp dụng cho tất cả UI components:

```csharp
public class MyPanel : Panel
{
    // 1. Nullable dependencies
    private readonly SomeController? _controller;
    
    // 2. Parameterless constructor
    public MyPanel() : this(null!)
    {
    }
    
    // 3. Constructor với dependencies
    public MyPanel(SomeController? controller)
    {
        _controller = controller;
        
        // 4. Designer mode detection
        if (!IsDesignerMode())
        {
            BuildUI();  // Chỉ build UI khi runtime
            Reload();   // Chỉ load data khi runtime
        }
    }
    
    // 5. Helper method
    private bool IsDesignerMode()
    {
        return System.ComponentModel.LicenseManager.UsageMode == 
               System.ComponentModel.LicenseUsageMode.Designtime;
    }
}
```

## 📁 Các File Đã Fix

### ✅ BaseManagementPanel.cs (Base class)
- Thêm `IsDesignerMode()` detection
- Chỉ gọi `BuildUI()` khi không phải designer mode
- `BuildUI()` changed from `private` → `protected` để child classes có thể gọi

### ✅ AccountManagementPanel.cs
- Nullable `AccountController?`
- Parameterless constructor
- Gọi `BuildUI()` explicitly trong constructor
- Designer mode check trước khi Reload()

### ✅ EmployeeManagementPanel.cs
- Designer mode check
- Gọi `BuildUI()` và `Reload()` chỉ khi runtime

### ✅ DashboardPanel.cs
- Refactor constructor thành `InitializeComponent()`
- Designer mode → hiển thị label "Designer Mode"
- Runtime mode → build full UI với `BuildDashboardUI()`

### ✅ LoginForm.cs (đã fix trước)
- Parameterless constructor
- Nullable dependencies
- Runtime validation

### ✅ MainFrame.cs (đã fix trước)
- Parameterless constructor với default values
- Nullable `AccountController?`

### ✅ Sidebar.cs & Header.cs (đã fix trước)
- Parameterless constructor với default values

## 🧪 Cách Test Designer

### Trong Visual Studio:
1. Mở Solution trong Visual Studio
2. Trong Solution Explorer, tìm file bạn muốn xem:
   - `view/LoginForm.cs`
   - `view/MainFrame.cs`
   - `view/AccountManagementPanel.cs`
   - `view/EmployeeManagementPanel.cs`
   - `view/DashboardPanel.cs`

3. **Double-click** file → Mặc định sẽ mở Designer view
4. Nếu mở code view, click tab **"Designer"** ở dưới cùng
5. Giờ bạn có thể:
   - ✅ Kéo thả controls từ Toolbox
   - ✅ Xem layout visually
   - ✅ Edit properties trong Properties window
   - ✅ Không còn lỗi!

### Trong JetBrains Rider:
1. Mở Solution
2. Right-click file `.cs` → "View" → "Designer"
3. Hoặc double-click file và chọn tab "Designer"

## 🎯 Workflow Làm Việc Với Designer

### 1. Thiết kế UI trong Designer
```
1. Mở file Panel/Form trong Designer
2. Kéo thả Button, TextBox, Label, etc.
3. Set properties (Name, Text, Font, Color)
4. Lưu file → Designer auto-generate code
```

### 2. Viết code logic
```csharp
// Designer đã tạo controls
private Button btnSave;
private TextBox txtName;

// Bạn viết event handlers
private void btnSave_Click(object sender, EventArgs e)
{
    var name = txtName.Text;
    // Business logic...
}
```

### 3. Best Practices

#### ❌ KHÔNG nên:
```csharp
public MyPanel()
{
    // KHÔNG gọi method abstract/virtual
    Reload();  // ❌ Sẽ lỗi trong designer
    
    // KHÔNG gọi dependencies ngay
    _controller.GetData();  // ❌ Controller null trong designer
}
```

#### ✅ NÊN:
```csharp
public MyPanel()
{
    if (!IsDesignerMode())
    {
        BuildUI();     // ✅ Chỉ khi runtime
        Reload();      // ✅ Chỉ khi runtime
    }
}
```

## 🛠️ Tạo Panel/Form Mới Hỗ Trợ Designer

### Template cho Panel mới:

```csharp
using System;
using System.Windows.Forms;
using HospitalManagement.controller;

namespace HospitalManagement.view
{
    public class MyNewPanel : Panel
    {
        // Dependencies (nullable)
        private readonly MyController? _controller;
        
        // UI Controls
        private Button _btnSave = null!;
        private TextBox _txtName = null!;
        
        // Parameterless constructor cho Designer
        public MyNewPanel() : this(null!)
        {
        }
        
        // Runtime constructor với DI
        public MyNewPanel(MyController? controller)
        {
            _controller = controller;
            
            if (!IsDesignerMode())
            {
                InitializeUI();
                LoadData();
            }
        }
        
        private bool IsDesignerMode()
        {
            return System.ComponentModel.LicenseManager.UsageMode == 
                   System.ComponentModel.LicenseUsageMode.Designtime;
        }
        
        private void InitializeUI()
        {
            // Build UI programmatically
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            
            _btnSave = new Button { Text = "Save" };
            _txtName = new TextBox();
            
            Controls.Add(_btnSave);
            Controls.Add(_txtName);
        }
        
        private void LoadData()
        {
            if (_controller == null) return;
            
            var data = _controller.GetData();
            // Populate UI...
        }
    }
}
```

### Template cho Form mới:

```csharp
public partial class MyNewForm : Form
{
    private readonly MyService? _service;
    
    // Designer constructor
    public MyNewForm()
    {
        InitializeComponent();  // Designer-generated
    }
    
    // Runtime constructor với DI
    public MyNewForm(MyService service) : this()
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        LoadData();
    }
    
    private void LoadData()
    {
        // Safe to call here (after InitializeComponent)
    }
}
```

## 📋 Checklist Khi Thêm UI Component Mới

- [ ] Có parameterless constructor
- [ ] Dependencies là nullable (`?`)
- [ ] Check `IsDesignerMode()` trước khi:
  - [ ] Gọi abstract/virtual methods
  - [ ] Gọi dependencies
  - [ ] Load data từ database
  - [ ] Build complex UI
- [ ] Constructor chaining: `this()` hoặc `: this(null!)`
- [ ] Null check khi dùng dependencies
- [ ] Test mở trong Designer → Không lỗi ✓

## 🐛 Debug Designer Issues

### Nếu vẫn lỗi trong Designer:

1. **Check constructor:**
   ```csharp
   // Có parameterless constructor?
   public MyPanel() { }
   ```

2. **Check dependencies:**
   ```csharp
   // Có nullable?
   private readonly MyService? _service;
   ```

3. **Check initialization:**
   ```csharp
   // Có designer mode check?
   if (!IsDesignerMode()) { ... }
   ```

4. **Rebuild Solution:**
   ```bash
   dotnet clean
   dotnet build
   ```

5. **Restart IDE** (Visual Studio / Rider)

## 📊 Kết Quả

### ✅ Trước khi fix:
```
❌ System.ComponentModel.Win32Exception
❌ Failed to build designer surface
❌ Cannot create instance (abstract class)
❌ This method/object is not implemented by design
```

### ✅ Sau khi fix:
```
✓ Designer mở được tất cả Panel/Form
✓ Có thể kéo thả controls
✓ Có thể edit properties visually
✓ Code và Designer sync hoàn hảo
```

## 🎓 Lưu Ý Quan Trọng

1. **Designer chỉ gọi parameterless constructor** → Luôn phải có
2. **Dependencies null trong designer mode** → Phải check null
3. **Abstract methods không call được trong designer** → Check `IsDesignerMode()`
4. **Virtual members trong constructor = warning** → Acceptable, không critical
5. **InitializeComponent() được designer auto-generate** → Không edit manually

---

**Status:** ✅ Tất cả UI components giờ hỗ trợ Designer mode!

**Ngày cập nhật:** 8/1/2026

