# Excel Import System - Design Patterns & Usage Guide

## 📐 Design Patterns Used

### 1. **Template Method Pattern** (AbstractImportService)
```
AbstractImportService<T>
├── PreviewFromFile() - Template method (định nghĩa khung thuật toán)
│   ├── ValidateHeaders()
│   ├── GetMapper() ← Abstract (để subclass implement)
│   ├── GetValidator() ← Abstract (để subclass implement)
│   └── MapRow() + Validate()
└── ApplyImport()
    └── SaveData() ← Abstract (để subclass implement)
```

**Ý tưởng:** 
- Class cha định nghĩa khung sườn thuật toán import (đọc file → validate header → map data → validate data)
- Class con chỉ cần implement các bước cụ thể (mapper, validator, save logic)
- Tránh code trùng lặp, dễ mở rộng cho nhiều loại entity khác nhau

### 2. **Strategy Pattern** (IImportMapper & IImportValidator)
```
IImportMapper<T>
├── ProductImportMapper
├── EmployeeImportMapper
└── AccountImportMapper

IImportValidator<T>
├── ProductImportValidator
├── EmployeeImportValidator
└── AccountImportValidator
```

**Ý tưởng:**
- Tách biệt logic map và validate thành các strategy riêng
- Có thể swap/thay đổi cách map và validate mà không ảnh hưởng code khác
- Dễ test từng strategy riêng lẻ

### 3. **Dependency Injection**
```csharp
public class ProductImportService : AbstractImportService<ProductImportDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    public ProductImportService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }
}
```

**Ý tưởng:**
- Inject dependencies qua constructor
- Loose coupling, dễ test, dễ mock
- Tuân thủ SOLID principles

---

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────────┐
│                 UI Layer (View)                      │
│  ┌──────────────────────────────────────────┐       │
│  │  Import Button → File Dialog             │       │
│  │  ↓                                        │       │
│  │  Preview Dialog (ValidRows/InvalidRows)  │       │
│  │  ↓                                        │       │
│  │  Apply Button → Save to DB               │       │
│  └──────────────────────────────────────────┘       │
└───────────────────┬─────────────────────────────────┘
                    │
┌───────────────────▼─────────────────────────────────┐
│            Service Layer (Import Service)            │
│  ┌──────────────────────────────────────────┐       │
│  │  AbstractImportService<T>                │       │
│  │  ├─ PreviewFromFile()                    │       │
│  │  └─ ApplyImport()                        │       │
│  └──────────────────────────────────────────┘       │
│              ▲                                       │
│              │                                       │
│  ┌───────────┴──────────────────────────────┐       │
│  │  ProductImportService                     │       │
│  │  ├─ GetMapper() → ProductImportMapper    │       │
│  │  ├─ GetValidator() → ProductValidator    │       │
│  │  └─ SaveData() → ProductRepository       │       │
│  └──────────────────────────────────────────┘       │
└───────────────────┬─────────────────────────────────┘
                    │
┌───────────────────▼─────────────────────────────────┐
│         Data Access Layer (Repository)               │
│  ┌──────────────────────────────────────────┐       │
│  │  ProductRepository.Insert()              │       │
│  │  CategoryRepository.FindByCode()         │       │
│  │  ManufacturerRepository.ExistsByCode()   │       │
│  └──────────────────────────────────────────┘       │
└──────────────────────────────────────────────────────┘
```

---

## 📁 File Structure

```
HospitalManagement/
├── utils/importer/
│   ├── core/                           # Core classes (Template Method)
│   │   ├── AbstractImportService.cs    # Template method pattern
│   │   ├── IImportMapper.cs            # Strategy interface
│   │   ├── IImportValidator.cs         # Strategy interface
│   │   ├── ImportError.cs              # Error model
│   │   └── ImportModels.cs             # Response models
│   │
│   ├── dto/                            # Import DTOs
│   │   ├── ProductImportDto.cs
│   │   ├── EmployeeImportDto.cs
│   │   └── AccountImportDto.cs
│   │
│   ├── mappers/                        # Mapper implementations (Strategy)
│   │   ├── ProductImportMapper.cs
│   │   ├── EmployeeImportMapper.cs
│   │   └── AccountImportMapper.cs
│   │
│   ├── validators/                     # Validator implementations (Strategy)
│   │   ├── ProductImportValidator.cs
│   │   ├── EmployeeImportValidator.cs
│   │   └── AccountImportValidator.cs
│   │
│   ├── services/                       # Import services (Template Method subclasses)
│   │   ├── ProductImportService.cs
│   │   ├── EmployeeImportService.cs
│   │   └── AccountImportService.cs
│   │
│   └── template/                       # Excel template generators
│       ├── ProductTemplateGenerator.cs
│       └── EmployeeTemplateGenerator.cs
```

---

## 🚀 How to Use - Product Import Example

### Step 1: Create DTO
```csharp
// dto/request/ProductImportDto.cs
public class ProductImportDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string CategoryCode { get; set; }
    public decimal? StandardPrice { get; set; }
    // ... other fields
}
```

### Step 2: Create Mapper
```csharp
// mappers/ProductImportMapper.cs
public class ProductImportMapper : IImportMapper<ProductImportDto>
{
    public string[] RequiredHeaders => new[]
    {
        "Code", "Name", "Category Code", "Standard Price"
    };

    public ProductImportDto MapRow(ExcelRange row, int rowIndex)
    {
        return new ProductImportDto
        {
            Code = GetCellValue(row, 1),
            Name = GetCellValue(row, 2),
            CategoryCode = GetCellValue(row, 3),
            StandardPrice = ParseDecimal(GetCellValue(row, 4))
        };
    }
}
```

### Step 3: Create Validator
```csharp
// validators/ProductImportValidator.cs
public class ProductImportValidator : IImportValidator<ProductImportDto>
{
    private readonly IProductRepository _productRepository;
    
    public List<ImportError> Validate(ProductImportDto data, int rowIndex)
    {
        var errors = new List<ImportError>();
        
        if (string.IsNullOrWhiteSpace(data.Code))
            errors.Add(new ImportError(rowIndex, "Code", "Code is required"));
            
        if (_productRepository.ExistsByCode(data.Code))
            errors.Add(new ImportError(rowIndex, "Code", "Code already exists"));
            
        return errors;
    }
}
```

### Step 4: Create Import Service
```csharp
// services/ProductImportService.cs
public class ProductImportService : AbstractImportService<ProductImportDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    protected override IImportMapper<ProductImportDto> GetMapper()
    {
        return new ProductImportMapper();
    }
    
    protected override IImportValidator<ProductImportDto> GetValidator()
    {
        return new ProductImportValidator(_productRepository, _categoryRepository);
    }
    
    protected override void SaveData(List<ProductImportDto> validData)
    {
        foreach (var dto in validData)
        {
            var product = new CreateProductRequest
            {
                Code = dto.Code,
                Name = dto.Name,
                CategoryId = GetCategoryId(dto.CategoryCode)
            };
            _productRepository.Insert(product);
        }
    }
}
```

### Step 5: Use in UI
```csharp
// In your View (ProductManagementPanel.cs)
private void btnImport_Click(object sender, EventArgs e)
{
    var openFileDialog = new OpenFileDialog
    {
        Filter = "Excel Files|*.xlsx;*.xls"
    };
    
    if (openFileDialog.ShowDialog() == DialogResult.OK)
    {
        try
        {
            // 1. Preview data
            var preview = _productImportService.PreviewFromFile(openFileDialog.FileName);
            
            // 2. Show preview dialog
            var dialog = new ImportPreviewDialog<ProductImportDto>(
                preview,
                new[] { "Code", "Name", "Category", "Price" }
            );
            
            // 3. If user clicks Apply
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var validData = preview.ValidRows.Select(r => r.Data).ToList();
                int count = _productImportService.ApplyImport(validData);
                
                MessageBox.Show($"Imported {count} products successfully!");
                LoadProducts(); // Refresh grid
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Import failed: {ex.Message}");
        }
    }
}
```

---

## 📋 Excel Template Generation

```csharp
// template/ProductTemplateGenerator.cs
public class ProductTemplateGenerator
{
    public static void GenerateTemplate(string outputPath)
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Products");
        
        // Create headers
        string[] headers = { "Code", "Name", "Category Code", "Price" };
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[1, i + 1].Value = headers[i];
            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
        }
        
        // Add example row
        worksheet.Cells[2, 1].Value = "PRD001";
        worksheet.Cells[2, 2].Value = "Paracetamol 500mg";
        worksheet.Cells[2, 3].Value = "CAT001";
        worksheet.Cells[2, 4].Value = 15000;
        
        package.SaveAs(new FileInfo(outputPath));
    }
}
```

---

## 🔄 Workflow

```
┌─────────────┐
│ User clicks │
│   Import    │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│ Select File │
└──────┬──────┘
       │
       ▼
┌──────────────────────────┐
│  PreviewFromFile()        │
│  ├─ Read Excel           │
│  ├─ Validate headers     │
│  ├─ Map each row         │
│  └─ Validate each row    │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│  Show Preview Dialog      │
│  ├─ Valid rows (green)   │
│  └─ Invalid rows (red)   │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│  User clicks "Apply"     │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│  ApplyImport()           │
│  └─ SaveData()           │
│     └─ Repository.Insert │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│  Refresh UI Grid         │
└──────────────────────────┘
```

---

## 🎯 Benefits

1. **Tái sử dụng code**: Template method giúp tránh lặp lại logic đọc file, validate header
2. **Dễ mở rộng**: Thêm import cho entity mới chỉ cần 4 files (DTO, Mapper, Validator, Service)
3. **Dễ test**: Mỗi component (mapper, validator) có thể test riêng
4. **Separation of Concerns**: Logic map, validate, save được tách biệt rõ ràng
5. **Type-safe**: Generic type `<T>` đảm bảo type safety
6. **Maintainable**: Thay đổi logic ở một nơi không ảnh hưởng nơi khác

---

## 🆚 Comparison with Java Implementation

| Aspect | Java | C# |
|--------|------|-----|
| Abstract class | `AbstractImportService<T>` | `AbstractImportService<T> where T : class` |
| Mapper interface | `ImportMapper<T>` | `IImportMapper<T>` |
| Validator interface | `ImportValidator<T>` | `IImportValidator<T>` |
| Excel library | Apache POI | EPPlus |
| Naming convention | camelCase methods | PascalCase methods |
| Access modifiers | protected/private | protected/private |
| Row representation | `Row` (Apache POI) | `ExcelRange` (EPPlus) |

**Core logic is identical - same design patterns applied!**

---

## 📝 Notes

- Always validate headers first to ensure file format matches template
- Use try-catch in map step to handle parsing errors gracefully
- Validators should check both format and business rules (e.g., foreign key existence)
- Keep DTOs simple - no business logic, just data containers
- Use repositories for all database operations (don't use DbContext directly)

---

Created: January 2026  
Pattern: Template Method + Strategy + Dependency Injection  
Based on: Java implementation in BAITAPUTT project
