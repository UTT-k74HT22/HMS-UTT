# Excel Import/Export Module - C# Migration Guide

## 📋 Tổng quan

Module này cung cấp chức năng xuất/nhập dữ liệu Excel cho hệ thống Hospital Management, được migrate từ Java version sử dụng Apache POI sang C# sử dụng **ClosedXML** hoặc **EPPlus**.

## 🎯 Các module chính

### 1. **Excel Export Module**
- **Mục đích**: Xuất dữ liệu từ hệ thống ra file Excel (.xlsx)
- **Thư mục**: `utils/excel/`
- **Thư viện**: EPPlus (hoặc ClosedXML)

### 2. **Excel Import Module**  
- **Mục đích**: Nhập dữ liệu từ file Excel vào hệ thống
- **Thư mục**: `utils/importer/`
- **Thư viện**: EPPlus (hoặc ClosedXML)

## 📁 Cấu trúc thư mục

```
HospitalManagement/
├── utils/
│   ├── excel/                          # Excel Export
│   │   ├── core/                       # Core classes
│   │   │   ├── IExcelSheetWriter.cs   # Interface định nghĩa writer
│   │   │   ├── AbstractExcelWriter.cs  # Base class cho writers
│   │   │   ├── ExcelExporter.cs        # Utility xuất file
│   │   │   ├── ExcelStyles.cs          # Quản lý styles
│   │   │   └── StyleKey.cs             # Enum các style keys
│   │   ├── writers/                    # Implementations
│   │   │   ├── AccountExcelWriter.cs
│   │   │   ├── EmployeeExcelWriter.cs
│   │   │   ├── InventoryExcelWriter.cs
│   │   │   └── StockExcelWriter.cs
│   │   └── ...
│   │
│   └── importer/                       # Excel Import
│       ├── core/                       # Core classes
│       │   ├── IImportMapper.cs       # Interface mapper
│       │   ├── IImportValidator.cs    # Interface validator
│       │   ├── AbstractImportService.cs # Base import service
│       │   ├── ImportError.cs         # Error model
│       │   └── ImportPreviewResponse.cs # Preview response
│       ├── mappers/                   # Mappers
│       │   ├── AccountImportMapper.cs
│       │   ├── EmployeeImportMapper.cs
│       │   └── ...
│       ├── validators/                # Validators
│       │   ├── AccountImportValidator.cs
│       │   ├── EmployeeImportValidator.cs
│       │   └── ...
│       └── services/                  # Services
│           ├── AccountImportService.cs
│           ├── EmployeeImportService.cs
│           └── ...
```

## 🔧 Setup & Dependencies

### 1. Cài đặt NuGet Package

**Option A: EPPlus (Recommended)**
```bash
Install-Package EPPlus
```

**Option B: ClosedXML**
```bash
Install-Package ClosedXML
```

### 2. License Configuration (EPPlus)

Thêm vào `Program.cs` hoặc `Startup.cs`:
```csharp
// Set EPPlus license context
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
```

## 📝 Hướng dẫn sử dụng

### A. EXCEL EXPORT

#### 1. Tạo Writer mới

Mỗi entity cần 1 writer class kế thừa từ `AbstractExcelWriter<T>`:

```csharp
public class AccountExcelWriter : AbstractExcelWriter<Account>
{
    public override string SheetName => "Accounts";
    public override string Title => "DANH SÁCH TÀI KHOẢN";
    
    public override string[] Headers => new[]
    {
        "STT", "ID", "Username", "Role", "Trạng thái"
    };
    
    public override void Create(ExcelWorksheet sheet, ExcelStyles styles, List<Account> data)
    {
        // Implementation
    }
}
```

#### 2. Xuất file Excel

```csharp
// Lấy dữ liệu
var accounts = _accountController.GetAccounts();

// Tạo writer
var writer = new AccountExcelWriter();

// Xuất với dialog
ExcelExporter.ExportWithDialog(accounts, writer, this);

// Hoặc xuất trực tiếp ra file
string filePath = @"C:\Exports\accounts.xlsx";
ExcelExporter.ExportToFile(accounts, writer, filePath);
```

#### 3. Custom Styles

Sử dụng `ExcelStyles` để định nghĩa các style:
- `TITLE` - Tiêu đề sheet
- `HEADER` - Header cột
- `DATA` - Dữ liệu thường
- `DATA_CENTER` - Dữ liệu căn giữa
- `BADGE_ACTIVE` - Badge active (xanh)
- `BADGE_INACTIVE` - Badge inactive (đỏ)

### B. EXCEL IMPORT

#### 1. Tạo Mapper

```csharp
public class AccountImportMapper : IImportMapper<AccountImportDto>
{
    public string[] RequiredHeaders => new[]
    {
        "Username", "Password", "Role", "Is Active"
    };
    
    public AccountImportDto MapRow(ExcelRow row, int rowIndex)
    {
        return new AccountImportDto
        {
            Username = GetCellValue(row, 0),
            Password = GetCellValue(row, 1),
            Role = GetCellValue(row, 2),
            IsActive = GetCellValue(row, 3) == "Yes"
        };
    }
}
```

#### 2. Tạo Validator

```csharp
public class AccountImportValidator : IImportValidator<AccountImportDto>
{
    public List<ImportError> Validate(AccountImportDto data, int rowIndex)
    {
        var errors = new List<ImportError>();
        
        if (string.IsNullOrWhiteSpace(data.Username))
            errors.Add(new ImportError(rowIndex, "Username", "Username không được để trống"));
            
        if (data.Username?.Length < 3)
            errors.Add(new ImportError(rowIndex, "Username", "Username phải >= 3 ký tự"));
            
        return errors;
    }
}
```

#### 3. Tạo Import Service

```csharp
public class AccountImportService : AbstractImportService<AccountImportDto>
{
    private readonly AccountController _controller;
    
    protected override IImportMapper<AccountImportDto> GetMapper()
        => new AccountImportMapper();
        
    protected override IImportValidator<AccountImportDto> GetValidator()
        => new AccountImportValidator();
        
    protected override void SaveData(List<AccountImportDto> validData)
    {
        foreach (var dto in validData)
        {
            _controller.CreateAccount(dto);
        }
    }
}
```

#### 4. Sử dụng Import

```csharp
// Preview trước khi import
var service = new AccountImportService(_controller);
var preview = await service.PreviewFromFileAsync(filePath);

// Hiển thị preview cho user
ShowPreviewDialog(preview);

// Nếu user confirm, thực hiện import
if (userConfirmed)
{
    var result = await service.ImportFromFileAsync(filePath);
    MessageBox.Show($"Imported {result.SuccessCount} records!");
}
```

## 🎨 Styling Guide

### Color Scheme (giống Java version)

```csharp
// Title: Dark Blue background, White text
// Header: Royal Blue background, White text, Bold
// Data: White background, Black text
// Badge Active: Green background, White text
// Badge Inactive: Red background, White text
```

### Row Heights

```csharp
TitleRowHeight = 25;   // ~600 twips
HeaderRowHeight = 18;  // ~420 twips
DataRowHeight = 16;    // ~380 twips
```

## 🔄 Migration Notes - Java to C#

### Java Apache POI → C# EPPlus

| Java (POI) | C# (EPPlus) |
|------------|-------------|
| `Workbook` | `ExcelPackage` |
| `Sheet` | `ExcelWorksheet` |
| `Row` | `ExcelRow` |
| `Cell` | `ExcelRange` |
| `CellStyle` | `ExcelStyle` |
| `Font` | `ExcelFont` |
| `IndexedColors` | `Color` (System.Drawing) |

### Key Differences

1. **File I/O**
   - Java: `FileInputStream`, `FileOutputStream`
   - C#: `FileInfo`, `Stream`

2. **Date Formatting**
   - Java: `LocalDateTime`, `DateTimeFormatter`
   - C#: `DateTime`, `ToString("dd/MM/yyyy")`

3. **Exceptions**
   - Java: Checked exceptions
   - C#: Unchecked exceptions (try-catch patterns)

4. **Nullability**
   - Java: `@Nullable`, Optional
   - C#: Nullable types (`DateTime?`, `string?`)

## 📊 Implementation Steps

### Phase 1: Core Infrastructure (Làm trước)
1. ✅ Create folder structure
2. ✅ Install EPPlus package
3. ✅ Create core interfaces
4. ✅ Create base classes
5. ✅ Create ExcelStyles
6. ✅ Create ExcelExporter utility

### Phase 2: Export Module (Account & Employee)
1. ✅ AccountExcelWriter
2. ✅ EmployeeExcelWriter
3. ✅ Test export functionality

### Phase 3: Import Module (Account & Employee)
1. ✅ AccountImportMapper
2. ✅ AccountImportValidator
3. ✅ AccountImportService
4. ✅ EmployeeImportMapper
5. ✅ EmployeeImportValidator
6. ✅ EmployeeImportService

### Phase 4: Advanced Modules (Inventory & Stock)
1. ⏳ InventoryExcelWriter & Import
2. ⏳ StockExcelWriter & Import

### Phase 5: UI Integration
1. ⏳ Add Export buttons to management panels
2. ⏳ Add Import buttons with preview dialog
3. ⏳ Add template download functionality

## 🧪 Testing Checklist

- [ ] Export empty list (should create file with headers only)
- [ ] Export large dataset (>1000 rows)
- [ ] Export with null values
- [ ] Export with special characters
- [ ] Import valid file
- [ ] Import file with errors (should show preview with errors)
- [ ] Import file with wrong headers
- [ ] Import empty file
- [ ] Import file with duplicate data

## 📚 References

- **EPPlus Documentation**: https://github.com/EPPlusSoftware/EPPlus
- **ClosedXML Documentation**: https://github.com/ClosedXML/ClosedXML
- **Java Source Code**: `BAITAPUTT/src/main/java/org/example/utils/`

## 💡 Tips

1. **Performance**: For large datasets (>10,000 rows), consider batch processing
2. **Memory**: Dispose `ExcelPackage` properly using `using` statement
3. **Validation**: Always validate data before saving to database
4. **Error Handling**: Provide clear error messages for users
5. **Templates**: Generate downloadable templates for import

## 🐛 Common Issues

### Issue 1: "License context must be set"
**Solution**: Add `ExcelPackage.LicenseContext = LicenseContext.NonCommercial;`

### Issue 2: Memory leak
**Solution**: Always use `using` statement with `ExcelPackage`

### Issue 3: Wrong date format
**Solution**: Use consistent format: `DateTime.ToString("dd/MM/yyyy")`

---

**Author**: Hospital Management Team  
**Last Updated**: January 9, 2026  
**Version**: 1.0.0
