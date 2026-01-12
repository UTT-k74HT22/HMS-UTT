# ✅ FIX STOCK MOVEMENT IMPORT - CHO PHÉP NHẬP TÊN KHO

## 🐛 Vấn đề
1. **Không import được Stock Movement** - Lỗi khi preview/import
2. **Kho hàng chỉ nhận mã** - User muốn nhập tên kho thay vì phải nhớ mã kho
3. **Không có logging** - Khó debug khi có lỗi
4. **Thông báo lỗi chưa rõ ràng** - User không biết sai ở đâu

## ✅ Giải pháp đã implement

### 1. **Cho phép nhập TÊN KHO hoặc MÃ KHO** ✅

#### File: [utils/importer/validator/StockMovementImportValidator.cs](utils/importer/validator/StockMovementImportValidator.cs)

**Trước:**
```csharp
// Chỉ tìm theo Code
var warehouse = _warehouseRepository.GetByCode(dto.WarehouseCode!);
if (warehouse == null)
{
    errors.Add(new ImportError(rowIndex, "Kho hàng", $"Kho hàng không tồn tại: {dto.WarehouseCode}"));
}
```

**Sau:**
```csharp
// Tìm theo Code trước
var warehouse = _warehouseRepository.GetByCode(dto.WarehouseCode!);

// Nếu không tìm thấy theo Code, thử tìm theo Name
if (warehouse == null)
{
    var allWarehouses = _warehouseRepository.GetAll();
    warehouse = allWarehouses?.FirstOrDefault(w => 
        w.Name?.Equals(dto.WarehouseCode, StringComparison.OrdinalIgnoreCase) == true);
}

if (warehouse == null)
{
    errors.Add(new ImportError(rowIndex, "Kho hàng", 
        $"Kho hàng không tồn tại: '{dto.WarehouseCode}'. Vui lòng nhập đúng tên hoặc mã kho."));
}
```

**Ưu điểm:**
- ✅ User có thể nhập "Kho chính" thay vì "WH001"
- ✅ Linh hoạt hơn, dễ sử dụng hơn
- ✅ Case-insensitive (không phân biệt hoa thường)

---

### 2. **Fix SaveData để resolve Warehouse đúng** ✅

#### File: [utils/importer/service/StockMovementImportService.cs](utils/importer/service/StockMovementImportService.cs)

**Trước:**
```csharp
// Chỉ tìm theo Code
var warehouse = _warehouseRepository.GetByCode(dto.WarehouseCode!);
if (warehouse == null)
{
    throw new Exception($"Warehouse not found: {dto.WarehouseCode}");
}
int warehouseId = (int)warehouse.Id;
```

**Sau:**
```csharp
// Tìm theo Code or Name
var warehouse = _warehouseRepository.GetByCode(dto.WarehouseCode!);

// Nếu không tìm thấy theo Code, thử tìm theo Name
if (warehouse == null)
{
    var allWarehouses = _warehouseRepository.GetAll();
    warehouse = allWarehouses?.FirstOrDefault(w => 
        w.Name?.Equals(dto.WarehouseCode, StringComparison.OrdinalIgnoreCase) == true);
}

if (warehouse == null)
{
    throw new Exception($"Warehouse not found: {dto.WarehouseCode}");
}

Console.WriteLine($"[IMPORT] Resolved warehouse: {warehouse.Name} (ID: {warehouse.Id})");
int warehouseId = (int)warehouse.Id;
```

---

### 3. **Thêm Logging chi tiết** ✅

#### File: [utils/importer/core/AbstractImportService.cs](utils/importer/core/AbstractImportService.cs)

**Logging được thêm vào:**
```csharp
Console.WriteLine($"[IMPORT] Bắt đầu đọc file: {filePath}");
Console.WriteLine($"[IMPORT] Tổng số dòng: {rowCount - 1} (không tính header)");

for (int i = 2; i <= rowCount; i++)
{
    // Log từng dòng
    Console.WriteLine($"[IMPORT] Dòng {i}: Đã map dữ liệu -> {data?.GetType().Name}");
    Console.WriteLine($"[IMPORT] Dòng {i}: Validation -> {(errors.Count == 0 ? "OK" : $"{errors.Count} lỗi")}");
    
    // Log chi tiết lỗi
    if (errors.Count > 0)
    {
        foreach (var err in errors)
        {
            Console.WriteLine($"  ❌ [{err.FieldName}]: {err.ErrorMessage}");
        }
    }
}
```

**Output ví dụ:**
```
[IMPORT] Bắt đầu đọc file: C:\Users\...\StockMovement.xlsx
[IMPORT] Tổng số dòng: 3 (không tính header)
[IMPORT] Dòng 2: Đã map dữ liệu -> StockMovementImportDto
[IMPORT] Dòng 2: Validation -> OK
[IMPORT] Dòng 3: Đã map dữ liệu -> StockMovementImportDto
[IMPORT] Dòng 3: Validation -> 1 lỗi
  ❌ [Kho hàng]: Kho hàng không tồn tại: 'Kho ABC'. Vui lòng nhập đúng tên hoặc mã kho.
```

---

### 4. **Cải thiện Template Excel** ✅

#### File: [utils/importer/template/StockMovementTemplateGenerator.cs](utils/importer/template/StockMovementTemplateGenerator.cs)

**Thay đổi:**
1. **Ví dụ rõ ràng hơn:**
   - Trước: `{ "IMPORT", "1", "PRD001", ... }` ❌
   - Sau: `{ "IMPORT", "Kho chính", "PRD001", ... }` ✅

2. **Thêm sheet "Hướng dẫn":**
   - Giải thích chi tiết từng cột
   - Nhấn mạnh: **"Kho hàng: Bạn có thể nhập TÊN KHO hoặc MÃ KHO"**
   - Ví dụ minh họa
   - Lưu ý quan trọng

**Nội dung sheet Hướng dẫn:**
```
📋 CÁC CỘT BẮT BUỘC
Loại         | IMPORT (Nhập kho) | EXPORT (Xuất kho) | ADJUST (Điều chỉnh)
Kho hàng     | Nhập TÊN KHO hoặc MÃ KHO (vd: 'Kho chính', 'WH001')
Mã sản phẩm  | Mã sản phẩm phải tồn tại trong hệ thống
Số lượng     | Số nguyên dương (> 0)

📝 CÁC CỘT TÙY CHỌN
Mã lô        | Để trống nếu không quản lý theo lô
Ghi chú      | Thông tin bổ sung về giao dịch

⚠️ LƯU Ý QUAN TRỌNG
1. Kho hàng: Bạn có thể nhập TÊN KHO (vd: 'Kho chính') hoặc MÃ KHO (vd: 'WH001')
2. File phải có header ở dòng 1 (không được xóa)
3. Dữ liệu bắt đầu từ dòng 2 trở đi
4. Các dòng trống sẽ bị bỏ qua
5. Hệ thống sẽ kiểm tra dữ liệu trước khi import

✅ VÍ DỤ
IMPORT | Kho chính | PRD001 | BATCH001 | 100 | Nhập từ NCC
EXPORT | Kho phụ   | PRD002 |          | 50  | Xuất bán
ADJUST | Kho chính | PRD003 | BATCH003 | 95  | Kiểm kê
```

---

## 📊 So sánh Trước/Sau

| Tính năng | Trước ❌ | Sau ✅ |
|-----------|---------|--------|
| **Nhập kho hàng** | Chỉ nhận mã (WH001) | Nhận cả tên ("Kho chính") và mã |
| **Thông báo lỗi** | "Kho hàng không tồn tại: WH001" | "Kho hàng không tồn tại: 'Kho ABC'. Vui lòng nhập đúng tên hoặc mã kho." |
| **Logging** | Không có | Chi tiết từng dòng, từng lỗi |
| **Template** | Ví dụ dùng mã số | Ví dụ dùng tên kho + sheet hướng dẫn |
| **Debug** | Khó | Dễ dàng với console log |

---

## 🧪 Test Case

### Test 1: Nhập bằng Tên Kho ✅
**Input:**
```
Loại   | Kho hàng   | Mã SP  | Mã lô     | Số lượng | Ghi chú
IMPORT | Kho chính  | PRD001 | BATCH001  | 100      | Test
```
**Kết quả:** ✅ Import thành công

### Test 2: Nhập bằng Mã Kho ✅
**Input:**
```
Loại   | Kho hàng | Mã SP  | Mã lô     | Số lượng | Ghi chú
IMPORT | WH001    | PRD001 | BATCH001  | 100      | Test
```
**Kết quả:** ✅ Import thành công

### Test 3: Tên kho không tồn tại ❌
**Input:**
```
Loại   | Kho hàng  | Mã SP  | Mã lô     | Số lượng | Ghi chú
IMPORT | Kho ABC   | PRD001 | BATCH001  | 100      | Test
```
**Kết quả:** ❌ Lỗi rõ ràng
```
❌ [Kho hàng]: Kho hàng không tồn tại: 'Kho ABC'. Vui lòng nhập đúng tên hoặc mã kho.
```

### Test 4: Case-insensitive ✅
**Input:**
```
Loại   | Kho hàng   | Mã SP  | Mã lô     | Số lượng | Ghi chú
IMPORT | KHO CHÍNH  | PRD001 | BATCH001  | 100      | Test (uppercase)
IMPORT | kho chính  | PRD002 | BATCH002  | 50       | Test (lowercase)
```
**Kết quả:** ✅ Cả 2 đều import thành công

---

## 🎯 Lợi ích

1. **User-friendly:**
   - Không cần nhớ mã kho
   - Nhập tên kho tự nhiên hơn

2. **Debugging dễ dàng:**
   - Console log chi tiết
   - Biết chính xác dòng nào lỗi, lỗi gì

3. **Thông báo lỗi rõ ràng:**
   - User biết chính xác sai ở đâu
   - Gợi ý cách sửa

4. **Template rõ ràng:**
   - Ví dụ thực tế
   - Hướng dẫn chi tiết trong file Excel

---

## 📝 Notes

- **StringComparison.OrdinalIgnoreCase** - Không phân biệt hoa thường
- **FirstOrDefault** - Tìm warehouse đầu tiên match với tên
- **Console.WriteLine** - Logging ra console để debug
- Nếu cần logging vào file, có thể dùng log4net hoặc NLog

---

## 👤 Author
**Hoàng Đình Dũng**  
Date: January 12, 2026  
Fix: Stock Movement Import - Warehouse Name Support + Logging
