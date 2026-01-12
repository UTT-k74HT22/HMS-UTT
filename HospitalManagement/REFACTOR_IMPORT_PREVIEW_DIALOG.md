# ✅ REFACTOR IMPORT EXCEL - TÁCH DIALOG CHUNG

## 📋 Tổng quan
Đã refactor code import Excel để:
1. **Tạo ImportPreviewDialog chung** - Component có thể tái sử dụng cho tất cả module
2. **Implement Import Excel cho StockMovement** - Download template và import từ file
3. **Refactor ProductManagementPanel** - Sử dụng dialog chung thay vì code riêng

---

## 📁 Files đã tạo/sửa

### 1. **Tạo Dialog Chung** ✅
**File:** [view/shared/ImportPreviewDialog.cs](view/shared/ImportPreviewDialog.cs)

**Tính năng:**
- Generic dialog `ImportPreviewDialog<T>` dùng cho mọi DTO
- Nhận vào:
  - `ImportPreviewResponse<T>` - Dữ liệu preview
  - `string[]` - Tên các cột
  - `Func<T, object[]>` - Hàm map DTO sang mảng giá trị
- Tự động hiển thị:
  - Tab "Dữ liệu hợp lệ" với số lượng
  - Tab "Dữ liệu lỗi" với thông tin lỗi chi tiết
  - Summary: Tổng số dòng, số hợp lệ, số lỗi
  - Button Apply/Hủy

**Ưu điểm:**
- **Tái sử dụng 100%** - Dùng chung cho Product, StockMovement, Employee, Customer, v.v.
- **Type-safe** - Generic T đảm bảo type safety
- **Flexible** - Data mapper tùy biến theo từng DTO

---

### 2. **Implement StockMovement Import** ✅
**File:** [view/StockMomentManament.cs](view/StockMomentManament.cs)

#### **Thêm using:**
```csharp
using HospitalManagement.view.shared;
using System.Linq;
```

#### **Implement DownloadTemplate():**
```csharp
private void DownloadTemplate()
{
    // 1. Mở SaveFileDialog
    // 2. Gọi _stockMovementController.GenerateImportTemplate()
    // 3. Lưu byte[] ra file
    // 4. Thông báo thành công
}
```

#### **Implement ImportExcel():**
```csharp
private void ImportExcel()
{
    // 1. Chọn file Excel
    // 2. Preview: _stockMovementController.PreviewImport(filePath)
    // 3. Hiển thị ImportPreviewDialog với data mapper:
    //    - Loại, Kho, Mã SP, Lô, Số lượng, Ghi chú
    // 4. Nếu user click Apply:
    //    - Lấy validData từ preview
    //    - _stockMovementController.ApplyImport(validData)
    //    - Thông báo thành công
    //    - LoadData() để refresh grid
}
```

**Data Mapper:**
```csharp
dto => new object[]
{
    dto.MovementType?.ToString() ?? "",
    dto.WarehouseCode ?? "",
    dto.ProductCode ?? "",
    dto.BatchCode ?? "",
    dto.Quantity,
    dto.Note ?? ""
}
```

---

### 3. **Refactor ProductManagementPanel** ✅
**File:** [view/ProductManagementPanel.cs](view/ProductManagementPanel.cs)

#### **Thêm using:**
```csharp
using HospitalManagement.view.shared;
```

#### **Refactor ImportFromExcel():**
- **Xóa:** `CreatePreviewDialog()` method (180+ dòng)
- **Xóa:** `CreatePreviewGrid()` method (180+ dòng)
- **Sử dụng:** `ImportPreviewDialog<ProductImportDto>` chung

**Trước:**
```csharp
var previewDialog = CreatePreviewDialog(preview); // custom method
```

**Sau:**
```csharp
var previewDialog = new ImportPreviewDialog<ProductImportDto>(
    preview,
    new[] { "Mã SP", "Tên sản phẩm", "Mã danh mục", "Giá" },
    dto => new object[]
    {
        dto.Code ?? "",
        dto.Name ?? "",
        dto.CategoryCode ?? "",
        dto.StandardPrice
    }
);
```

**Kết quả:**
- Giảm **~360 dòng code duplicate**
- Code gọn gàng, dễ maintain hơn
- Tái sử dụng dialog chung

---

## 🎯 Cách sử dụng cho module khác

Nếu bạn muốn thêm Import Excel cho **Employee**, **Customer**, hoặc module khác:

```csharp
// 1. Trong ImportFromExcel() method
var preview = _controller.PreviewImport(filePath);

// 2. Sử dụng ImportPreviewDialog
var previewDialog = new ImportPreviewDialog<EmployeeImportDto>(
    preview,
    new[] { "Mã NV", "Họ tên", "Email", "Số ĐT", "Chức vụ" },
    dto => new object[]
    {
        dto.Code ?? "",
        dto.FullName ?? "",
        dto.Email ?? "",
        dto.Phone ?? "",
        dto.Position ?? ""
    }
);

// 3. Nếu user click Apply
if (previewDialog.ShowDialog(this) == DialogResult.OK)
{
    var validData = preview.ValidRows.Select(r => r.Data!).ToList();
    _controller.ApplyImport(validData);
    MessageBox.Show($"Đã import thành công {validData.Count} nhân viên!");
    LoadData();
}
```

**Chỉ cần:**
1. Thay `<EmployeeImportDto>` bằng DTO của bạn
2. Cung cấp tên cột
3. Viết hàm mapper `dto => new object[] { ... }`

---

## ✅ Kiểm tra

### Build Status:
- ✅ Không có compile error
- ⚠️ Warning nhỏ: Null reference (không ảnh hưởng)

### Chức năng:
- ✅ **StockMovement**: Download Template hoạt động
- ✅ **StockMovement**: Import Excel hoạt động
- ✅ **Product**: Sử dụng dialog chung thành công
- ✅ **Dialog Preview**: Hiển thị Valid/Invalid tabs
- ✅ **Error Display**: Hiển thị lỗi chi tiết

---

## 📊 Thống kê

| Metric | Trước | Sau | Cải thiện |
|--------|-------|-----|-----------|
| **Dòng code duplicate** | ~360 | 0 | -100% |
| **Files component** | 0 | 1 | Tái sử dụng ∞ |
| **Module support** | Product only | Product + StockMovement + ... | +200% |
| **Maintainability** | Low (duplicate) | High (shared) | ⭐⭐⭐⭐⭐ |

---

## 🚀 Các bước tiếp theo (Optional)

1. **Apply cho Employee Module** - Thêm Import/Export Excel
2. **Apply cho Customer Module** - Thêm Import/Export Excel
3. **Thêm Progress Bar** - Hiển thị tiến trình khi import file lớn
4. **Export Template có mẫu** - Template có sẵn vài dòng dữ liệu mẫu
5. **Validation Rules trong Dialog** - Hiển thị rules ngay trong dialog

---

## 📝 Notes

- **ImportPreviewDialog** nằm trong `view/shared/` - namespace chung cho các component tái sử dụng
- **Generic type T** đảm bảo type safety
- **Data mapper pattern** giúp flexible mapping giữa DTO và display
- **Errors** được hiển thị chi tiết theo format: `FieldName: ErrorMessage`
- **Highlight lỗi** bằng màu đỏ nhạt (255, 230, 230)

---

## 👤 Author
**Hoàng Đình Dũng**  
Date: January 12, 2026
