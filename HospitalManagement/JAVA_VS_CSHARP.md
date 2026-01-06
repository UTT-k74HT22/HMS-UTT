# 🔄 So sánh Java (BAITAPUTT) vs C# (HMS-UTT)

## 📊 Mapping các khái niệm

| Java (Swing) | C# (WinForms) | Mô tả |
|--------------|---------------|-------|
| `JPanel` | `Panel` | Container cơ bản |
| `JTable` | `DataGridView` | Bảng hiển thị dữ liệu |
| `DefaultTableModel` | `BindingSource` | Data source cho table |
| `TableRowSorter` | `BindingSource.Filter` | Lọc và sắp xếp |
| `JTextField` | `TextBox` | Input text |
| `JButton` | `Button` | Nút bấm |
| `JLabel` | `Label` | Nhãn văn bản |
| `JComboBox` | `ComboBox` | Dropdown list |
| `BoxLayout` | `FlowLayoutPanel` | Layout theo dòng |
| `BorderLayout` | `TableLayoutPanel` hoặc `DockStyle` | Layout phân vùng |
| `GridBagLayout` | `TableLayoutPanel` | Layout lưới phức tạp |

## 🎨 Base UI Components

### Java: BaseManagementPanel.java

```java
public abstract class BaseManagementPanel<T> extends JPanel {
    protected JTable table;
    protected DefaultTableModel model;
    protected TableRowSorter<DefaultTableModel> sorter;
    protected JLabel totalLabel;

    protected abstract String titleTotal();
    protected abstract String[] columns();
    protected abstract int[] columnWidths();
    protected abstract List<T> fetchData();
    protected abstract Object[] mapRow(T item, int stt);

    protected JPanel buildFilters() { return new JPanel(); }
    protected JPanel buildActions() { return new JPanel(); }
    protected void afterTableCreated() {}
    protected void applyFilters() {}
}
```

### C#: BaseManagementPanel.cs

```csharp
public abstract class BaseManagementPanel<T> : Panel where T : class
{
    protected DataGridView Table { get; private set; } = null!;
    protected BindingSource BindingSource { get; private set; } = null!;
    protected Label TotalLabel { get; private set; } = null!;

    protected abstract string TitleTotal();
    protected abstract (string PropertyName, string HeaderText, int Width)[] GetColumns();
    protected abstract List<T> FetchData();

    protected virtual Panel? BuildFilters() { return null; }
    protected virtual Panel? BuildActions() { return null; }
    protected virtual void AfterTableCreated() {}
    protected virtual void ApplyFilters() {}
}
```

### Khác biệt chính:

✅ **C# không cần `mapRow()`**: C# dùng reflection tự động map properties  
✅ **C# dùng Tuple**: `GetColumns()` return tuple thay vì 2 arrays riêng  
✅ **C# dùng BindingSource**: Thay vì DefaultTableModel, dễ dùng hơn  
✅ **C# Properties**: Dùng properties thay vì fields  

## 🏭 UI Factory Pattern

### Java: UiFactory.java

```java
public static JPanel cardPanel() {
    JPanel p = new JPanel(new BorderLayout());
    p.setBackground(Color.WHITE);
    p.setBorder(BorderFactory.createCompoundBorder(
        BorderFactory.createLineBorder(UiTheme.BORDER),
        new EmptyBorder(12, 12, 12, 12)
    ));
    return p;
}

public static JButton button(String text, Color bg, ActionListener al) {
    JButton b = new JButton(text);
    b.setBackground(bg);
    b.setForeground(Color.WHITE);
    b.addActionListener(al);
    return b;
}
```

### C#: UiFactory.cs

```csharp
public static Panel CreateCardPanel()
{
    var panel = new Panel
    {
        BackColor = Color.White,
        Padding = new Padding(12),
        BorderStyle = BorderStyle.FixedSingle
    };
    
    panel.Paint += (sender, e) =>
    {
        ControlPaint.DrawBorder(e.Graphics, panel.ClientRectangle,
            UiTheme.BORDER, 1, ButtonBorderStyle.Solid,
            // ... other sides
        );
    };
    
    return panel;
}

public static Button CreateButton(string text, Color bgColor, EventHandler? clickHandler = null)
{
    var button = new Button
    {
        Text = text,
        BackColor = bgColor,
        ForeColor = Color.White,
        FlatStyle = FlatStyle.Flat
    };
    
    if (clickHandler != null)
        button.Click += clickHandler;
    
    return button;
}
```

### Khác biệt:

✅ **C# naming**: PascalCase cho methods (CreateButton vs button)  
✅ **C# object initializer**: `new Button { Text = "..." }` rõ ràng hơn  
✅ **C# nullable**: `EventHandler?` cho optional parameters  
✅ **C# lambda**: Dùng lambda expression cho event handlers  

## 📝 Concrete Implementation

### Java: EmployeeManagementPanel.java

```java
public class EmployeeManagementPanel extends JPanel {
    private final EmployeeController controller;
    private JTable employeeTable;
    private DefaultTableModel tableModel;
    private JTextField keywordField;

    public EmployeeManagementPanel() {
        this(new EmployeeController(new EmployeeServiceImpl()));
    }

    private void loadEmployees() {
        List<EmployeeProfileResponse> list = controller.getAllEmployeeProfiles();
        tableModel.setRowCount(0);
        int stt = 1;
        for (EmployeeProfileResponse emp : list) {
            tableModel.addRow(new Object[]{
                stt++,
                emp.getAccountUsername(),
                emp.getProfileId(),
                emp.getCode(),
                emp.getFullName(),
                emp.getPhone(),
                emp.getPosition(),
                emp.getStatus()
            });
        }
        totalLabel.setText("Tổng số nhân viên: " + list.size());
    }
}
```

### C#: EmployeeManagementPanel.cs (kế thừa BaseManagementPanel)

```csharp
public class EmployeeManagementPanel : BaseManagementPanel<EmployeeProfile>
{
    private readonly EmployeeController _controller;
    private TextBox _searchBox = null!;
    
    public EmployeeManagementPanel(EmployeeController controller)
    {
        _controller = controller;
        Reload(); // Tự động load
    }

    protected override List<EmployeeProfile> FetchData()
    {
        return _controller.GetAllEmployees();
    }

    // BaseManagementPanel tự động:
    // - Map properties vào columns
    // - Update total label
    // - Handle selection, sorting, filtering
}
```

### Khác biệt:

✅ **C# tự động hóa nhiều hơn**: Không cần manual mapping  
✅ **C# ít code hơn**: BaseManagementPanel xử lý nhiều logic  
✅ **C# type-safe**: Generic `<EmployeeProfile>` đảm bảo type safety  

## 🎯 Styles & Theming

### Java: UiTheme.java

```java
public static final Color PRIMARY = new Color(113, 99, 248);
public static final Color SUCCESS = new Color(39, 174, 96);
public static final Font FONT_BASE = new Font("Segoe UI", Font.PLAIN, 13);
```

### C#: UiTheme.cs

```csharp
public static readonly Color PRIMARY = Color.FromArgb(113, 99, 248);
public static readonly Color SUCCESS = Color.FromArgb(39, 174, 96);
public static readonly Font FONT_BASE = new Font("Segoe UI", 10F, FontStyle.Regular);
```

### Khác biệt:

✅ **Java `final`** = **C# `readonly`**  
✅ **Java `Font.PLAIN`** = **C# `FontStyle.Regular`**  
✅ **Java `new Color(r,g,b)`** = **C# `Color.FromArgb(r,g,b)`**  

## 🔄 Data Binding

### Java (Manual)

```java
tableModel.setRowCount(0);
for (Employee emp : employees) {
    tableModel.addRow(new Object[]{
        emp.getId(),
        emp.getName(),
        emp.getPosition()
    });
}
```

### C# (Automatic)

```csharp
BindingSource.DataSource = employees;
// Auto-map properties to columns!
```

✅ **C# tự động**: BindingSource tự map properties  
✅ **C# ít lỗi**: Không cần manual mapping cho mỗi field  

## 🔍 Filtering

### Java (TableRowSorter)

```java
sorter = new TableRowSorter<>(model);
table.setRowSorter(sorter);

// Apply filter
sorter.setRowFilter(RowFilter.regexFilter("(?i)" + keyword));
```

### C# (BindingSource.Filter)

```csharp
// Apply filter
BindingSource.Filter = $"Name LIKE '%{keyword}%'";

// Clear filter
BindingSource.Filter = null;
```

✅ **C# SQL-like**: Dùng syntax giống SQL WHERE clause  
✅ **C# đơn giản hơn**: Không cần setup sorter riêng  

## 📊 Table Styling

### Java (Custom Renderer)

```java
public static DefaultTableCellRenderer zebraCenterRenderer() {
    return new DefaultTableCellRenderer() {
        @Override
        public Component getTableCellRendererComponent(JTable t, Object v,
                boolean sel, boolean focus, int r, int c) {
            super.getTableCellRendererComponent(t, v, sel, focus, r, c);
            setHorizontalAlignment(SwingConstants.CENTER);
            setBackground(sel ? UiTheme.SELECT : 
                (r % 2 == 0 ? Color.WHITE : UiTheme.ROW_ALT));
            return this;
        }
    };
}
```

### C# (Built-in)

```csharp
public static void ApplyZebraStripes(DataGridView table)
{
    table.RowsDefaultCellStyle.BackColor = Color.White;
    table.AlternatingRowsDefaultCellStyle.BackColor = UiTheme.ROW_ALT;
    table.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
}
```

✅ **C# built-in support**: DataGridView có sẵn AlternatingRowsDefaultCellStyle  
✅ **C# đơn giản hơn**: Không cần custom renderer  

## 🎭 Event Handling

### Java

```java
button.addActionListener(e -> {
    var selected = getSelectedModelRow();
    if (selected < 0) {
        JOptionPane.showMessageDialog(this, "Vui lòng chọn!");
        return;
    }
    // handle...
});
```

### C#

```csharp
button.Click += (s, e) =>
{
    var selected = GetSelectedItem();
    if (selected == null)
    {
        MessageBox.Show("Vui lòng chọn!");
        return;
    }
    // handle...
};
```

✅ **Similar**: Cả 2 đều dùng lambda  
✅ **C# type-safe**: GetSelectedItem() return `T?` thay vì int index  

## 📦 Summary

| Aspect | Java (Swing) | C# (WinForms) |
|--------|-------------|---------------|
| **Code lượng** | Nhiều hơn | Ít hơn (tự động hóa) |
| **Data binding** | Manual mapping | Auto mapping |
| **Type safety** | Object[] arrays | Generic `<T>` |
| **Filtering** | TableRowSorter | BindingSource.Filter |
| **Styling** | Custom renderers | Built-in properties |
| **Learning curve** | Cao hơn | Thấp hơn |

## ✅ Ưu điểm C# Framework này

1. **Ít code hơn**: BaseManagementPanel tự động hóa nhiều
2. **Type-safe**: Generic `<T>` đảm bảo compile-time safety
3. **Dễ maintain**: Rõ ràng, structured
4. **Consistent**: Tất cả panel có cùng UX
5. **Flexible**: Dễ override và custom

---

**💡 Kết luận**: C# implementation ngắn gọn và dễ dùng hơn Java nhờ:
- Auto data binding
- Built-in WinForms features
- Modern C# syntax (properties, object initializers, tuples)
