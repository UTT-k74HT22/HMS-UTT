# BACKEND BUSINESS LOGIC REVIEW - CHUẨN CHO JUNIOR DEVELOPER

> **Mục đích**: Tài liệu này giúp junior developer hiểu rõ flow nghiệp vụ chuẩn của Backend trong hệ thống HMS-UTT

---

## 📚 MỤC LỤC

1. [KIẾN TRÚC TỔNG QUAN](#1-kiến-trúc-tổng-quan)
2. [QUẢN LÝ TÀI KHOẢN (Account)](#2-quản-lý-tài-khoản-account)
3. [QUẢN LÝ NHÂN VIÊN (Employee)](#3-quản-lý-nhân-viên-employee)
4. [QUẢN LÝ TỒN KHO (Inventory)](#4-quản-lý-tồn-kho-inventory)
5. [QUẢN LÝ XUẤT NHẬP KHO (Stock Movement)](#5-quản-lý-xuất-nhập-kho-stock-movement)
6. [TÍNH NĂNG EXPORT EXCEL](#6-tính-năng-export-excel)
7. [TÍNH NĂNG IMPORT EXCEL](#7-tính-năng-import-excel)
8. [DESIGN PATTERNS ĐÃ ÁP DỤNG](#8-design-patterns-đã-áp-dụng)
9. [BEST PRACTICES](#9-best-practices)

---

## 1. KIẾN TRÚC TỔNG QUAN

### 1.1. Kiến trúc 3 lớp (Layered Architecture)

```
┌─────────────────────────────────────────┐
│         VIEW LAYER (WinForms)           │
│    - AccountManagementPanel.cs         │
│    - EmployeeManagementPanel.cs        │
│    - InventoryManagement.cs            │
│    - StockMovementManagement.cs        │
└────────────────┬────────────────────────┘
                 │ gọi
┌────────────────▼────────────────────────┐
│      CONTROLLER LAYER                   │
│    - AccountController.cs               │
│    - EmployeeController.cs              │
│    - InventoryController.cs             │
│    - StockMovementController.cs         │
└────────────────┬────────────────────────┘
                 │ gọi
┌────────────────▼────────────────────────┐
│      SERVICE LAYER (Business Logic)     │
│    - AccountServiceImpl.cs              │
│    - EmployeeServiceImpl.cs             │
│    - InventoryServiceImpl.cs            │
│    - StockMovementServiceImpl.cs        │
└────────────────┬────────────────────────┘
                 │ gọi
┌────────────────▼────────────────────────┐
│      REPOSITORY LAYER (Data Access)     │
│    - IAccountRepository                 │
│    - IEmployeeProfileRepository         │
│    - IInventoryRepository               │
│    - IStockMovementRepository           │
└────────────────┬────────────────────────┘
                 │ SQL Query
┌────────────────▼────────────────────────┐
│           DATABASE (SQL Server)         │
└─────────────────────────────────────────┘
```

### 1.2. Trách nhiệm của từng layer

| Layer | Trách nhiệm | Ví dụ |
|-------|-------------|-------|
| **View** | Hiển thị UI, nhận input từ user | Button click, DataGridView |
| **Controller** | Điều phối request, gọi Service, xử lý exception | Try-catch, gọi service method |
| **Service** | Business logic, validation, transaction | Validate, check duplicate, transaction |
| **Repository** | Thao tác với database (CRUD) | INSERT, UPDATE, DELETE, SELECT |

### 1.3. DTO Pattern

**Request DTO**: Dữ liệu từ View → Controller → Service
```csharp
public class CreateAccountRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public RoleType Role { get; set; }
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool Active { get; set; }
}
```

**Response DTO**: Dữ liệu từ Service → Controller → View
```csharp
public class AccountResponse
{
    public long Id { get; set; }
    public string Username { get; set; }
    public RoleType Role { get; set; }
    public bool Active { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
```

---

## 2. QUẢN LÝ TÀI KHOẢN (Account)

### 2.1. Flow tạo tài khoản mới (CreateAccount)

#### 📊 Sequence Diagram
```
User → Controller → Service → Repository → Database
  │         │           │           │           │
  │ Create  │           │           │           │
  ├────────►│           │           │           │
  │         │ Validate  │           │           │
  │         ├──────────►│           │           │
  │         │           │ Check     │           │
  │         │           │ Duplicate │           │
  │         │           ├──────────►│           │
  │         │           │◄──────────┤           │
  │         │           │ Hash      │           │
  │         │           │ Password  │           │
  │         │           │           │           │
  │         │           │ BEGIN     │           │
  │         │           │ TRAN      │           │
  │         │           ├──────────────────────►│
  │         │           │           │ INSERT    │
  │         │           │           │ Account   │
  │         │           │           ├──────────►│
  │         │           │           │ INSERT    │
  │         │           │           │ UserProfile│
  │         │           │           ├──────────►│
  │         │           │           │ INSERT    │
  │         │           │           │ Employee/ │
  │         │           │           │ Customer  │
  │         │           │           ├──────────►│
  │         │           │ COMMIT    │           │
  │         │           ├──────────────────────►│
  │         │◄──────────┤           │           │
  │◄────────┤           │           │           │
```

#### 🔥 Flow chi tiết (QUAN TRỌNG!)

**BƯỚC 1: VALIDATE REQUEST** 
```csharp
private void ValidateCreateRequest(CreateAccountRequest request)
{
    // 1.1 Check null/empty
    if (string.IsNullOrWhiteSpace(request.Username))
        throw new ArgumentException("Username không được để trống");
    
    // 1.2 Check password match
    if (request.Password != request.ConfirmPassword)
        throw new ArgumentException("Password và Confirm Password không khớp");
    
    // 1.3 Check password length
    if (request.Password.Length < 6)
        throw new ArgumentException("Password phải có ít nhất 6 ký tự");
    
    // 1.4 Check full name
    if (string.IsNullOrWhiteSpace(request.FullName))
        throw new ArgumentException("Họ tên không được để trống");
    
    // 1.5 Business rule - không tạo ADMIN qua form
    if (request.Role == RoleType.ADMIN)
        throw new ArgumentException("Không thể tạo tài khoản ADMIN qua form này");
}
```

**BƯỚC 2: CHECK DUPLICATE (QUAN TRỌNG!)**
```csharp
// 2.1 Check duplicate username
if (_accountRepository.ExistsByUsername(request.Username))
    throw new Exception($"Username [{request.Username}] đã tồn tại");

// 2.2 Check duplicate email (nếu có)
if (!string.IsNullOrEmpty(request.Email) && 
    _userProfileRepository.ExistsByEmail(request.Email))
    throw new Exception($"Email [{request.Email}] đã tồn tại");

// 2.3 Check duplicate phone (nếu có)
if (!string.IsNullOrEmpty(request.Phone) && 
    _userProfileRepository.ExistsByPhone(request.Phone))
    throw new Exception($"SĐT [{request.Phone}] đã tồn tại");
```

**BƯỚC 3: HASH PASSWORD**
```csharp
// TODO: Trong production phải dùng BCrypt hoặc PBKDF2
string hashedPassword = HashPassword(request.Password);
```

**BƯỚC 4-7: DATABASE TRANSACTION (CASCADE CREATE)**
```csharp
using var connection = new SqlConnection(_connectionString);
connection.Open();
using var transaction = connection.BeginTransaction();

try
{
    // BƯỚC 4: Tạo Account
    var account = new Account
    {
        Username = request.Username,
        Password = hashedPassword,
        Role = request.Role,
        IsActive = request.Active
    };
    long accountId = _accountRepository.Insert(connection, transaction, account);
    
    // BƯỚC 5: Tạo UserProfile
    string code = _userProfileRepository.GenerateCode(
        request.Role == RoleType.EMPLOYEE ? "EMP" : "CUS"
    );
    
    var userProfile = new UserProfile
    {
        AccountId = (int)accountId,
        Code = code,
        FullName = request.FullName,
        Email = request.Email,
        Phone = request.Phone,
        Address = request.Address,
        Status = ProfileStatus.ACTIVE.ToString()
    };
    long profileId = _userProfileRepository.Insert(connection, transaction, userProfile);
    
    // BƯỚC 6: Tạo EmployeeProfile HOẶC CustomerProfile
    if (request.Role == RoleType.EMPLOYEE)
    {
        _employeeProfileRepository.Insert(
            connection, transaction, profileId,
            position: "Nhân viên",
            department: "Chưa phân bộ phận",
            hiredDate: DateTime.Now,
            baseSalary: 0
        );
    }
    else if (request.Role == RoleType.CUSTOMER)
    {
        _customerProfileRepository.Insert(connection, transaction, new CustomerProfile
        {
            ProfileId = (int)profileId,
            CustomerType = "RETAIL",
            TaxCode = null
        });
    }
    
    // BƯỚC 7: COMMIT
    transaction.Commit();
}
catch (Exception ex)
{
    transaction.Rollback();
    throw new Exception($"Lỗi khi tạo tài khoản: {ex.Message}", ex);
}
```

#### ⚠️ Business Rules

1. **Username phải unique** trong toàn hệ thống
2. **Email và Phone phải unique** (nếu có nhập)
3. **Password tối thiểu 6 ký tự**
4. **Không cho tạo tài khoản ADMIN** qua form thông thường
5. **Cascade create**: Account → UserProfile → EmployeeProfile/CustomerProfile
6. **Default values**:
   - Employee: Position = "Nhân viên", Department = "Chưa phân bộ phận", Salary = 0
   - Customer: CustomerType = "RETAIL"

### 2.2. Flow cập nhật tài khoản (UpdateAccount)

```csharp
public void Update(long accountId, RoleType role, bool active)
{
    // 1. Tìm account
    var account = _accountRepository.FindById(accountId);
    if (account == null) throw new Exception("Account not found");
    
    // 2. Business Rule: Không deactivate ADMIN cuối cùng
    if (account.Role == RoleType.ADMIN && account.IsActive && !active)
    {
        var otherActiveAdmins = _accountRepository.FindAll()
            .Where(a => a.Role == RoleType.ADMIN && a.IsActive && a.Id != accountId)
            .ToList();
        
        if (otherActiveAdmins.Count == 0)
            throw new Exception("Không thể vô hiệu hóa tài khoản ADMIN cuối cùng");
    }
    
    // 3. Business Rule: Không đổi role của ADMIN cuối cùng
    if (account.Role == RoleType.ADMIN && role != RoleType.ADMIN)
    {
        var otherAdmins = _accountRepository.FindAll()
            .Where(a => a.Role == RoleType.ADMIN && a.Id != accountId)
            .ToList();
        
        if (otherAdmins.Count == 0)
            throw new Exception("Không thể thay đổi vai trò của ADMIN cuối cùng");
    }
    
    // 4. Update
    _accountRepository.UpdateRoleAndStatus(accountId, role, active);
}
```

#### ⚠️ Business Rules

1. **Không được deactivate ADMIN cuối cùng** trong hệ thống
2. **Không được đổi role của ADMIN cuối cùng** sang role khác
3. Đảm bảo luôn có ít nhất 1 ADMIN active trong hệ thống

---

## 3. QUẢN LÝ NHÂN VIÊN (Employee)

### 3.1. Cấu trúc dữ liệu

```
Account (1) ──────► (1) UserProfile (1) ──────► (1) EmployeeProfile
   │                       │                            │
   ├─ username             ├─ code (EMP-001)           ├─ position
   ├─ password             ├─ fullName                 ├─ department
   ├─ role                 ├─ email                    ├─ hiredDate
   └─ isActive             ├─ phone                    └─ baseSalary
                           ├─ address
                           └─ status
```

### 3.2. Flow cập nhật chi tiết nhân viên (UpdateEmployeeDetail)

**BƯỚC 1: VALIDATE REQUEST**
```csharp
private void ValidateUpdateDetailRequest(UpdateEmployeeProfileDetailRequest request)
{
    // 1.1 Required fields
    if (string.IsNullOrWhiteSpace(request.FullName))
        throw new ArgumentException("Họ tên không được để trống");
    
    if (string.IsNullOrWhiteSpace(request.Position))
        throw new ArgumentException("Chức vụ không được để trống");
    
    if (string.IsNullOrWhiteSpace(request.Department))
        throw new ArgumentException("Phòng ban không được để trống");
    
    // 1.2 Business rules
    if (request.HiredDate == null)
        throw new ArgumentException("Ngày vào làm không được để trống");
    
    if (request.HiredDate > DateTime.Now)
        throw new ArgumentException("Ngày vào làm không thể trong tương lai");
    
    if (request.Salary == null || request.Salary <= 0)
        throw new ArgumentException("Lương cơ bản phải lớn hơn 0");
}
```

**BƯỚC 2: CHECK EMPLOYEE TỒN TẠI**
```csharp
var employee = GetEmployeeDetailByCode(code);
if (employee == null)
    throw new Exception($"Không tìm thấy nhân viên với mã: {code}");
```

**BƯỚC 3: UPDATE CẢ 2 BẢNG**
```csharp
// Update cả UserProfile và EmployeeProfile
_employeeProfileRepository.UpdateDetailByProfileId(employee.ProfileId.Value, request);
```

#### ⚠️ Business Rules

1. **Ngày vào làm không được trong tương lai**
2. **Lương cơ bản > 0**
3. **Họ tên, Chức vụ, Phòng ban không được trống**

### 3.3. Soft Delete (Vô hiệu hóa nhân viên)

```csharp
public void Delete(string code, ProfileStatus status)
{
    // 1. Check employee tồn tại
    var employee = GetEmployeeDetailByCode(code);
    
    // 2. Soft delete = update status
    _employeeProfileRepository.UpdateStatus(code, status);
}
```

**Lưu ý**: Không xóa vật lý (DELETE), chỉ cập nhật `status = INACTIVE`

---

## 4. QUẢN LÝ TỒN KHO (Inventory)

### 4.1. Cấu trúc dữ liệu

```
InventoryItem
├─ id
├─ productId ──────► Product
├─ batchId ──────► Batch (lô hàng)
├─ warehouseId ──────► Warehouse
├─ quantityOnHand (tồn kho thực tế)
├─ quantityReserved (đặt trước)
├─ quantityAvailable = quantityOnHand - quantityReserved
├─ minThreshold (ngưỡng tối thiểu)
├─ maxThreshold (ngưỡng tối đa)
└─ lastUpdated
```

### 4.2. Business Logic

#### 4.2.1. Kiểm tra tồn kho thấp (Low Stock)

```csharp
public List<InventoryResponse> GetLowStockItems()
{
    // Lấy các item có: quantityOnHand < minThreshold
    return _inventoryRepository.GetLowStockItems();
}
```

#### 4.2.2. Kiểm tra sắp hết hạn (Near Expiry)

```csharp
public List<InventoryResponse> GetNearExpiryItems()
{
    // Lấy các item có: expiryDate < now + 30 days
    return _inventoryRepository.GetNearExpiryItems();
}
```

#### 4.2.3. Cập nhật ngưỡng min/max

```csharp
public void UpdateThresholds(long inventoryItemId, UpdateInventoryThresholdRequest request)
{
    // Validate
    if (request.MinThreshold.HasValue && request.MinThreshold.Value < 0)
        throw new ArgumentException("Ngưỡng tối thiểu không thể âm");
    
    if (request.MaxThreshold.HasValue && request.MaxThreshold.Value < 0)
        throw new ArgumentException("Ngưỡng tối đa không thể âm");
    
    if (request.MinThreshold.HasValue && request.MaxThreshold.HasValue 
        && request.MinThreshold.Value > request.MaxThreshold.Value)
        throw new ArgumentException("Ngưỡng tối thiểu không thể lớn hơn ngưỡng tối đa");
    
    // Update
    _inventoryRepository.UpdateThresholds(inventoryItemId, request);
}
```

#### ⚠️ Business Rules

1. **Min/Max Threshold >= 0**
2. **MinThreshold <= MaxThreshold**
3. **Low Stock**: quantityOnHand < minThreshold
4. **Near Expiry**: expiryDate < (now + 30 days)
5. **Over Stock**: quantityOnHand > maxThreshold

---

## 5. QUẢN LÝ XUẤT NHẬP KHO (Stock Movement)

### 5.1. Các loại giao dịch (StockMovementType)

| Loại | Mô tả | Ảnh hưởng |
|------|-------|----------|
| **IMPORT** | Nhập kho | `quantity AFTER = quantity BEFORE + số lượng nhập` |
| **EXPORT** | Xuất kho | `quantity AFTER = quantity BEFORE - số lượng xuất` |
| **ADJUST** | Điều chỉnh | `quantity AFTER = số lượng mới (set trực tiếp)` |
| **TRANSFER** | Chuyển kho | `kho nguồn: -X, kho đích: +X` |

### 5.2. Flow Xuất Nhập Kho (QUAN TRỌNG!)

#### 📊 Flow IMPORT/EXPORT/ADJUST

```csharp
public void CreateMovement(CreateStockMovementRequest request)
{
    // BƯỚC 1: VALIDATE
    ValidateCreateMovementRequest(request);
    
    using var connection = new SqlConnection(_connectionString);
    connection.Open();
    using var transaction = connection.BeginTransaction();
    
    try
    {
        // BƯỚC 2: Lấy số lượng tồn kho HIỆN TẠI
        var inventoryInfo = _inventoryRepository.GetOrCreateInventoryItem(
            request.ProductId,
            request.BatchId ?? 0,
            request.WarehouseId
        );
        int quantityBefore = inventoryInfo.CurrentQuantity;
        
        // BƯỚC 3: Tính toán số lượng MỚI
        int quantityAfter;
        switch (request.MovementType)
        {
            case StockMovementType.IMPORT:
                quantityAfter = quantityBefore + request.Quantity;
                break;
            
            case StockMovementType.EXPORT:
                // BUSINESS RULE: Kiểm tra đủ hàng để xuất
                if (request.Quantity > quantityBefore)
                    throw new Exception(
                        $"Không đủ hàng để xuất. " +
                        $"Tồn kho: {quantityBefore}, yêu cầu: {request.Quantity}"
                    );
                quantityAfter = quantityBefore - request.Quantity;
                break;
            
            case StockMovementType.ADJUST:
                // ADJUST = set giá trị tuyệt đối
                quantityAfter = quantityBefore + request.Quantity;
                break;
        }
        
        // BƯỚC 4: Cập nhật tồn kho
        _inventoryRepository.UpdateQuantity(
            inventoryInfo.InventoryItemId,
            quantityAfter
        );
        
        // BƯỚC 5: Ghi log giao dịch (theo dõi before/after)
        request.QuantityBefore = quantityBefore;
        request.QuantityAfter = quantityAfter;
        _stockMovementRepository.InsertWithQuantityTracking(request);
        
        // BƯỚC 6: COMMIT
        transaction.Commit();
    }
    catch (Exception)
    {
        transaction.Rollback();
        throw;
    }
}
```

#### 📊 Flow TRANSFER (Chuyển kho)

```csharp
case StockMovementType.TRANSFER:
    // Validate kho đích
    if (!request.DestinationWarehouseId.HasValue || request.DestinationWarehouseId <= 0)
        throw new Exception("Phải chỉ định kho đích để chuyển kho");
    
    if (request.WarehouseId == request.DestinationWarehouseId)
        throw new Exception("Kho nguồn và kho đích không được trùng nhau");
    
    // BƯỚC 1: Kiểm tra kho nguồn đủ hàng
    if (quantityBefore < request.Quantity)
        throw new Exception(
            $"Kho nguồn không đủ hàng. " +
            $"Tồn kho: {quantityBefore}, yêu cầu: {request.Quantity}"
        );
    
    // BƯỚC 2: Giảm tồn kho nguồn
    int sourceAfter = quantityBefore - request.Quantity;
    _inventoryRepository.UpdateQuantity(inventoryInfo.InventoryItemId, sourceAfter);
    
    // BƯỚC 3: Lấy tồn kho đích
    var destInventoryInfo = _inventoryRepository.GetOrCreateInventoryItem(
        request.ProductId,
        request.BatchId ?? 0,
        request.DestinationWarehouseId.Value
    );
    int destBefore = destInventoryInfo.CurrentQuantity;
    int destAfter = destBefore + request.Quantity;
    
    // BƯỚC 4: Tăng tồn kho đích
    _inventoryRepository.UpdateQuantity(destInventoryInfo.InventoryItemId, destAfter);
    
    // BƯỚC 5: Ghi log giao dịch KHO NGUỒN (EXPORT)
    request.QuantityBefore = quantityBefore;
    request.QuantityAfter = sourceAfter;
    _stockMovementRepository.InsertWithQuantityTracking(request);
    
    // BƯỚC 6: Ghi log giao dịch KHO ĐÍCH (IMPORT)
    var destRequest = new CreateStockMovementRequest
    {
        MovementType = StockMovementType.IMPORT,
        ProductId = request.ProductId,
        BatchId = request.BatchId,
        WarehouseId = request.DestinationWarehouseId.Value,
        Quantity = request.Quantity,
        ReferenceType = "TRANSFER",
        ReferenceId = request.ReferenceId,
        PerformedByUserId = request.PerformedByUserId,
        Note = $"Nhận chuyển kho từ WH-{request.WarehouseId}. {request.Note}",
        QuantityBefore = destBefore,
        QuantityAfter = destAfter
    };
    _stockMovementRepository.InsertWithQuantityTracking(destRequest);
    
    transaction.Commit();
    return; // Kết thúc sớm cho TRANSFER
```

#### ⚠️ Business Rules

1. **EXPORT**: Phải kiểm tra đủ hàng trước khi xuất (`quantity <= quantityBefore`)
2. **TRANSFER**: 
   - Kho nguồn ≠ Kho đích
   - Kho nguồn phải đủ hàng
   - Tạo 2 giao dịch: 1 EXPORT (nguồn) + 1 IMPORT (đích)
3. **Quantity Tracking**: Luôn ghi lại `quantityBefore` và `quantityAfter` để audit trail
4. **Transaction**: Tất cả thao tác phải trong 1 transaction
5. **PerformedByUserId**: Bắt buộc phải có (lấy từ session hiện tại)

### 5.3. GetOrCreateInventoryItem (Helper quan trọng)

```csharp
// Nếu chưa có InventoryItem cho Product + Batch + Warehouse
// → Tự động tạo mới với quantity = 0
var inventoryInfo = _inventoryRepository.GetOrCreateInventoryItem(
    productId,
    batchId,
    warehouseId
);
```

**Lợi ích**: Tránh lỗi khi nhập lần đầu tiên vào kho mới

---

## 6. TÍNH NĂNG EXPORT EXCEL

### 6.1. Kiến trúc Export Excel

```
User click Export
      │
      ▼
ExcelExporter.ExportWithDialog<T>()
      │
      ├─ Hiển thị SaveFileDialog
      │
      ├─ Gọi ExportToFile()
      │     │
      │     ├─ Tạo XLWorkbook
      │     │
      │     ├─ Gọi IExcelSheetWriter<T>.Create()
      │     │     │
      │     │     ├─ WriteTitle() (row 1)
      │     │     ├─ WriteHeader() (row 2)
      │     │     └─ WriteDataRows() (row 3+)
      │     │
      │     ├─ Freeze rows (2 rows đầu)
      │     ├─ Auto-fit columns
      │     └─ SaveAs()
      │
      └─ Mở file (optional)
```

### 6.2. Design Pattern: Template Method Pattern

**Abstract Class**
```csharp
public abstract class AbstractExcelWriter<T> : IExcelSheetWriter<T>
{
    // Template properties
    public abstract string SheetName { get; }
    public abstract string Title { get; }
    public abstract string[] Headers { get; }
    
    // Template method - định nghĩa khung sườn
    public abstract void Create(IXLWorksheet worksheet, List<T> data);
    
    // Helper methods - dùng chung
    protected void ApplyTitleStyle(IXLCell cell) { ... }
    protected void ApplyHeaderStyle(IXLCell cell) { ... }
    protected void ApplyDataStyle(IXLCell cell) { ... }
}
```

**Concrete Class (Ví dụ: AccountExcelWriter)**
```csharp
public class AccountExcelWriter : AbstractExcelWriter<AccountResponse>
{
    public override string SheetName => "Danh sách tài khoản";
    public override string Title => "DANH SÁCH TÀI KHOẢN";
    public override string[] Headers => new[]
    {
        "STT", "ID", "Username", "Role", "Active", "Last Login"
    };
    
    public override void Create(IXLWorksheet worksheet, List<AccountResponse> data)
    {
        // Row 1: Title
        worksheet.Cell(1, 1).Value = Title;
        worksheet.Range(1, 1, 1, Headers.Length).Merge();
        ApplyTitleStyle(worksheet.Cell(1, 1));
        
        // Row 2: Headers
        for (int i = 0; i < Headers.Length; i++)
        {
            var cell = worksheet.Cell(2, i + 1);
            cell.Value = Headers[i];
            ApplyHeaderStyle(cell);
        }
        
        // Row 3+: Data
        int row = 3;
        foreach (var account in data)
        {
            worksheet.Cell(row, 1).Value = row - 2; // STT
            worksheet.Cell(row, 2).Value = account.Id;
            worksheet.Cell(row, 3).Value = account.Username;
            worksheet.Cell(row, 4).Value = account.Role.ToString();
            worksheet.Cell(row, 5).Value = account.Active ? "Có" : "Không";
            worksheet.Cell(row, 6).Value = account.LastLoginAt?.ToString("yyyy-MM-dd HH:mm") ?? "Chưa đăng nhập";
            
            // Apply style cho tất cả cells
            for (int col = 1; col <= Headers.Length; col++)
            {
                ApplyDataStyle(worksheet.Cell(row, col));
            }
            
            row++;
        }
    }
}
```

### 6.3. Cách sử dụng trong View

```csharp
private void ExportToExcel()
{
    var filteredData = _bs.List.Cast<AccountResponse>().ToList();
    ExcelExporter.ExportWithDialog<AccountResponse>(
        filteredData,
        new AccountExcelWriter(),
        this.FindForm()
    );
}
```

### 6.4. Best Practices

1. **Separation of Concerns**: Mỗi entity có 1 writer riêng
2. **Template Method Pattern**: Code reuse cao, dễ maintain
3. **Style consistency**: Dùng chung style methods
4. **Auto-fit columns**: Tự động điều chỉnh độ rộng
5. **Freeze panes**: Giữ cố định title + header khi scroll
6. **Error handling**: Try-catch và hiển thị lỗi rõ ràng

---

## 7. TÍNH NĂNG IMPORT EXCEL

### 7.1. Kiến trúc Import Excel

```
User chọn file Excel
      │
      ▼
AbstractImportService.PreviewFromFile()
      │
      ├─ Validate Headers
      │
      ├─ For each row:
      │     ├─ IImportMapper.MapRow() → DTO
      │     ├─ IImportValidator.Validate() → List<ImportError>
      │     └─ Phân loại Valid/Invalid
      │
      └─ Return ImportPreviewResponse
            │
            ▼
ImportPreviewDialog (hiển thị preview)
      │
      ├─ Hiển thị Valid rows (màu xanh)
      ├─ Hiển thị Invalid rows (màu đỏ + lý do lỗi)
      │
      └─ User click "Apply"
            │
            ▼
AbstractImportService.ApplyImport()
      │
      └─ SaveData(validRows) → Database
```

### 7.2. Design Pattern: Template Method Pattern

**Abstract Service**
```csharp
public abstract class AbstractImportService<T> where T : class
{
    // TEMPLATE METHODS - các class con phải implement
    protected abstract IImportMapper<T> GetMapper();
    protected abstract IImportValidator<T> GetValidator();
    protected abstract void SaveData(List<T> validData);
    
    // CONCRETE METHOD - logic chung
    public ImportPreviewResponse<T> PreviewFromFile(string filePath)
    {
        var validRows = new List<ImportRowData<T>>();
        var invalidRows = new List<ImportRowData<T>>();
        
        using var package = new ExcelPackage(new FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        
        var mapper = GetMapper();
        var validator = GetValidator();
        
        // 1. Validate headers
        if (!ValidateHeaders(worksheet, mapper.RequiredHeaders))
            throw new ArgumentException("File không đúng định dạng template");
        
        // 2. Process data rows
        for (int i = 2; i <= worksheet.Dimension.Rows; i++)
        {
            var row = worksheet.Cells[i, 1, i, worksheet.Dimension.Columns];
            
            if (IsEmptyRow(row)) continue;
            
            try
            {
                // Map row → DTO
                T data = mapper.MapRow(row, i);
                
                // Validate DTO
                List<ImportError> errors = validator.Validate(data, i);
                
                var rowData = new ImportRowData<T>
                {
                    RowIndex = i,
                    Data = data,
                    Errors = errors,
                    IsValid = errors.Count == 0
                };
                
                if (errors.Count == 0)
                    validRows.Add(rowData);
                else
                    invalidRows.Add(rowData);
            }
            catch (Exception ex)
            {
                // Parse error → invalid row
                invalidRows.Add(new ImportRowData<T>
                {
                    RowIndex = i,
                    Data = null,
                    IsValid = false,
                    Errors = new List<ImportError>
                    {
                        new ImportError(i, "Parse Error", ex.Message)
                    }
                });
            }
        }
        
        return new ImportPreviewResponse<T>
        {
            ValidRows = validRows,
            InvalidRows = invalidRows,
            TotalRows = validRows.Count + invalidRows.Count,
            HasErrors = invalidRows.Any()
        };
    }
    
    public int ApplyImport(List<T> validData)
    {
        SaveData(validData);
        return validData.Count;
    }
}
```

### 7.3. Ví dụ: StockMovementImportService

**Mapper**
```csharp
public class StockMovementImportMapper : IImportMapper<StockMovementImportDto>
{
    public string[] RequiredHeaders => new[]
    {
        "Loại giao dịch", "Mã kho", "Mã sản phẩm", 
        "Mã lô", "Số lượng", "Ghi chú"
    };
    
    public StockMovementImportDto MapRow(ExcelRange row, int rowIndex)
    {
        return new StockMovementImportDto
        {
            MovementType = ParseMovementType(GetCellValue(row, 1)),
            WarehouseCode = GetCellValue(row, 2),
            ProductCode = GetCellValue(row, 3),
            BatchCode = GetCellValue(row, 4),
            Quantity = int.Parse(GetCellValue(row, 5)),
            Note = GetCellValue(row, 6)
        };
    }
}
```

**Validator**
```csharp
public class StockMovementImportValidator : IImportValidator<StockMovementImportDto>
{
    public List<ImportError> Validate(StockMovementImportDto dto, int rowIndex)
    {
        var errors = new List<ImportError>();
        
        // 1. Required fields
        if (dto.MovementType == null)
            errors.Add(new ImportError(rowIndex, "MovementType", "Loại giao dịch không hợp lệ"));
        
        if (string.IsNullOrEmpty(dto.WarehouseCode))
            errors.Add(new ImportError(rowIndex, "WarehouseCode", "Mã kho không được trống"));
        
        if (string.IsNullOrEmpty(dto.ProductCode))
            errors.Add(new ImportError(rowIndex, "ProductCode", "Mã sản phẩm không được trống"));
        
        // 2. Business rules
        if (dto.Quantity <= 0)
            errors.Add(new ImportError(rowIndex, "Quantity", "Số lượng phải > 0"));
        
        // 3. Check foreign keys
        if (!_warehouseRepo.ExistsByCode(dto.WarehouseCode))
            errors.Add(new ImportError(rowIndex, "WarehouseCode", $"Kho [{dto.WarehouseCode}] không tồn tại"));
        
        if (!_productRepo.ExistsByCode(dto.ProductCode))
            errors.Add(new ImportError(rowIndex, "ProductCode", $"Sản phẩm [{dto.ProductCode}] không tồn tại"));
        
        return errors;
    }
}
```

**Service Implementation**
```csharp
public class StockMovementImportService : AbstractImportService<StockMovementImportDto>
{
    protected override IImportMapper<StockMovementImportDto> GetMapper()
        => new StockMovementImportMapper();
    
    protected override IImportValidator<StockMovementImportDto> GetValidator()
        => new StockMovementImportValidator(_warehouseRepo, _productRepo, _batchRepo);
    
    protected override void SaveData(List<StockMovementImportDto> validData)
    {
        foreach (var dto in validData)
        {
            // Convert DTO → CreateStockMovementRequest
            var request = new CreateStockMovementRequest
            {
                MovementType = dto.MovementType.Value,
                WarehouseId = _warehouseRepo.FindByCode(dto.WarehouseCode).Id,
                ProductId = _productRepo.FindByCode(dto.ProductCode).Id,
                BatchId = _batchRepo.FindByCode(dto.BatchCode).Id,
                Quantity = dto.Quantity,
                Note = dto.Note,
                PerformedByUserId = AuthContextManager.UserProfileId.Value
            };
            
            // Gọi service để tạo giao dịch (tái sử dụng logic existing)
            _stockMovementService.CreateMovement(request);
        }
    }
}
```

### 7.4. Template Generator

```csharp
public class StockMovementTemplateGenerator
{
    public byte[] Generate()
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("StockMovement");
        
        // Headers
        worksheet.Cells[1, 1].Value = "Loại giao dịch";
        worksheet.Cells[1, 2].Value = "Mã kho";
        worksheet.Cells[1, 3].Value = "Mã sản phẩm";
        worksheet.Cells[1, 4].Value = "Mã lô";
        worksheet.Cells[1, 5].Value = "Số lượng";
        worksheet.Cells[1, 6].Value = "Ghi chú";
        
        // Sample data
        worksheet.Cells[2, 1].Value = "IMPORT";
        worksheet.Cells[2, 2].Value = "WH-001";
        worksheet.Cells[2, 3].Value = "PRD-001";
        worksheet.Cells[2, 4].Value = "BATCH-001";
        worksheet.Cells[2, 5].Value = 100;
        worksheet.Cells[2, 6].Value = "Nhập kho mẫu";
        
        // Style
        worksheet.Cells[1, 1, 1, 6].Style.Font.Bold = true;
        worksheet.Cells[1, 1, 1, 6].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        
        worksheet.Cells.AutoFitColumns();
        
        return package.GetAsByteArray();
    }
}
```

### 7.5. Best Practices

1. **Template Method Pattern**: Tái sử dụng logic chung
2. **Strategy Pattern**: Mapper và Validator có thể swap
3. **Validate headers**: Đảm bảo file đúng format
4. **Preview trước khi Apply**: Cho user kiểm tra
5. **Hiển thị lỗi rõ ràng**: Row number + field name + error message
6. **Transaction**: Apply import trong 1 transaction
7. **Reuse existing logic**: SaveData gọi lại service methods đã có

---

## 8. DESIGN PATTERNS ĐÃ ÁP DỤNG

### 8.1. Layered Architecture (Kiến trúc 3 lớp)

**Mục đích**: Tách biệt các concerns, dễ maintain và test

```
View → Controller → Service → Repository → Database
```

### 8.2. Repository Pattern

**Mục đích**: Trừu tượng hóa data access layer

```csharp
public interface IAccountRepository
{
    Account FindById(long id);
    List<Account> FindAll();
    long Insert(SqlConnection conn, SqlTransaction trans, Account account);
    void UpdateRoleAndStatus(long id, RoleType role, bool active);
    void DeleteById(long id);
    bool ExistsByUsername(string username);
}
```

### 8.3. DTO Pattern (Data Transfer Object)

**Mục đích**: Truyền dữ liệu giữa các layer mà không expose entity

```csharp
// Request DTO - từ View vào
public class CreateAccountRequest { ... }

// Response DTO - từ Service ra
public class AccountResponse { ... }
```

### 8.4. Template Method Pattern

**Mục đích**: Định nghĩa khung sườn thuật toán, các bước cụ thể do subclass implement

**Ví dụ 1: AbstractExcelWriter**
```csharp
public abstract class AbstractExcelWriter<T>
{
    // Template properties
    public abstract string SheetName { get; }
    public abstract string Title { get; }
    public abstract string[] Headers { get; }
    
    // Template method
    public abstract void Create(IXLWorksheet worksheet, List<T> data);
    
    // Helper methods (dùng chung)
    protected void ApplyTitleStyle(IXLCell cell) { ... }
    protected void ApplyHeaderStyle(IXLCell cell) { ... }
}
```

**Ví dụ 2: AbstractImportService**
```csharp
public abstract class AbstractImportService<T>
{
    // Template methods
    protected abstract IImportMapper<T> GetMapper();
    protected abstract IImportValidator<T> GetValidator();
    protected abstract void SaveData(List<T> validData);
    
    // Concrete method (logic chung)
    public ImportPreviewResponse<T> PreviewFromFile(string filePath) { ... }
}
```

### 8.5. Strategy Pattern

**Mục đích**: Cho phép swap algorithms tại runtime

```csharp
// Strategies
public interface IExcelSheetWriter<T>
{
    void Create(IXLWorksheet worksheet, List<T> data);
}

// Context
public static class ExcelExporter
{
    public static void ExportWithDialog<T>(
        List<T> data,
        IExcelSheetWriter<T> writer, // Strategy injection
        Form? parent = null
    ) { ... }
}
```

### 8.6. Dependency Injection

**Mục đích**: Giảm coupling, dễ test, dễ swap implementation

```csharp
public class AccountServiceImpl : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IEmployeeProfileRepository _employeeProfileRepository;
    
    // Constructor injection
    public AccountServiceImpl(
        IAccountRepository accountRepository,
        IUserProfileRepository userProfileRepository,
        IEmployeeProfileRepository employeeProfileRepository)
    {
        _accountRepository = accountRepository;
        _userProfileRepository = userProfileRepository;
        _employeeProfileRepository = employeeProfileRepository;
    }
}
```

### 8.7. Transaction Script Pattern

**Mục đích**: Xử lý business logic phức tạp với transaction

```csharp
using var connection = new SqlConnection(_connectionString);
connection.Open();
using var transaction = connection.BeginTransaction();

try
{
    // Bước 1: Insert Account
    long accountId = _accountRepository.Insert(connection, transaction, account);
    
    // Bước 2: Insert UserProfile
    long profileId = _userProfileRepository.Insert(connection, transaction, userProfile);
    
    // Bước 3: Insert EmployeeProfile
    _employeeProfileRepository.Insert(connection, transaction, employeeProfile);
    
    // Commit nếu thành công
    transaction.Commit();
}
catch
{
    // Rollback nếu có lỗi
    transaction.Rollback();
    throw;
}
```

---

## 9. BEST PRACTICES

### 9.1. Error Handling

#### ✅ ĐÚNG
```csharp
public void CreateAccount(CreateAccountRequest request)
{
    try
    {
        Console.WriteLine($"[Service] CreateAccount: Starting for user={request.Username}");
        
        // 1. Validate
        ValidateCreateRequest(request);
        
        // 2. Check duplicate
        if (_accountRepository.ExistsByUsername(request.Username))
            throw new Exception($"Username [{request.Username}] đã tồn tại");
        
        // 3. Business logic...
        
        Console.WriteLine($"[Service] CreateAccount: Success!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Service] CreateAccount: ERROR - {ex}");
        throw new Exception($"Lỗi khi tạo tài khoản: {ex.Message}", ex);
    }
}
```

#### ❌ SAI
```csharp
public void CreateAccount(CreateAccountRequest request)
{
    // Không validate
    // Không log
    // Không throw exception rõ ràng
    _accountRepository.Insert(account); // ← Lỗi sẽ khó debug
}
```

### 9.2. Validation

#### ✅ ĐÚNG - Validate sớm (Fail Fast)
```csharp
private void ValidateCreateRequest(CreateAccountRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Username))
        throw new ArgumentException("Username không được để trống");
    
    if (request.Password.Length < 6)
        throw new ArgumentException("Password phải có ít nhất 6 ký tự");
    
    // ... validate tất cả rules
}

public void CreateAccount(CreateAccountRequest request)
{
    // Validate NGAY ĐẦU
    ValidateCreateRequest(request);
    
    // Sau đó mới thực hiện business logic
    // ...
}
```

### 9.3. Logging

```csharp
public void CreateAccount(CreateAccountRequest request)
{
    Console.WriteLine($"[Service] CreateAccount: Starting for user={request.Username}");
    Console.WriteLine("[Service] Step 1: Validating request...");
    
    ValidateCreateRequest(request);
    
    Console.WriteLine("[Service] Step 1: Validation passed");
    Console.WriteLine("[Service] Step 2: Checking duplicates...");
    
    // ...
    
    Console.WriteLine($"[Service] ✓ Tạo tài khoản thành công: {request.Username}");
}
```

### 9.4. Transaction Management

#### ✅ ĐÚNG
```csharp
using var connection = new SqlConnection(_connectionString);
connection.Open();
using var transaction = connection.BeginTransaction();

try
{
    // Tất cả operations trong 1 transaction
    long accountId = _accountRepository.Insert(connection, transaction, account);
    long profileId = _userProfileRepository.Insert(connection, transaction, userProfile);
    _employeeProfileRepository.Insert(connection, transaction, employeeProfile);
    
    transaction.Commit(); // Commit cuối cùng
}
catch (Exception ex)
{
    transaction.Rollback(); // Rollback nếu có lỗi
    throw;
}
```

### 9.5. Business Rules Enforcement

```csharp
// RULE: Không deactivate ADMIN cuối cùng
if (account.Role == RoleType.ADMIN && account.IsActive && !active)
{
    var otherActiveAdmins = _accountRepository.FindAll()
        .Where(a => a.Role == RoleType.ADMIN && a.IsActive && a.Id != accountId)
        .ToList();
    
    if (otherActiveAdmins.Count == 0)
        throw new Exception("Không thể vô hiệu hóa tài khoản ADMIN cuối cùng");
}
```

### 9.6. Separation of Concerns

```
Controller:
- Nhận request từ View
- Gọi Service
- Xử lý exception
- Trả kết quả cho View

Service:
- Validate business rules
- Orchestrate repositories
- Handle transactions
- Business logic

Repository:
- CRUD operations
- SQL queries
- Data mapping
```

### 9.7. Naming Conventions

| Loại | Convention | Ví dụ |
|------|------------|-------|
| **Class** | PascalCase | `AccountServiceImpl` |
| **Interface** | IPascalCase | `IAccountService` |
| **Method** | PascalCase (verb) | `CreateAccount()`, `GetAllEmployees()` |
| **Variable** | camelCase | `accountId`, `userName` |
| **Constant** | UPPER_SNAKE_CASE | `MIN_PASSWORD_LENGTH` |
| **Private field** | _camelCase | `_accountRepository` |

---

## 📝 CHECKLIST CHO JUNIOR DEVELOPER

Khi implement một tính năng mới, hãy check:

- [ ] **Validate input** ở Service layer
- [ ] **Check business rules** trước khi thực hiện
- [ ] **Sử dụng transaction** cho operations phức tạp
- [ ] **Log các bước quan trọng** để dễ debug
- [ ] **Throw exception có message rõ ràng**
- [ ] **Try-catch ở Controller** để handle exception
- [ ] **DTO cho request/response** (không expose entity)
- [ ] **Kiểm tra null** trước khi sử dụng
- [ ] **Comment cho logic phức tạp**
- [ ] **Test với nhiều scenarios**: happy path + edge cases

---

## 🎯 KẾT LUẬN

Backend của HMS-UTT tuân theo:

1. **Layered Architecture**: Tách biệt rõ ràng View - Controller - Service - Repository
2. **Design Patterns**: Template Method, Strategy, Repository, DTO, DI
3. **Transaction Management**: Đảm bảo data consistency
4. **Business Rules Enforcement**: Validate và enforce rules nghiêm ngặt
5. **Error Handling**: Logging và exception handling đầy đủ
6. **Code Reusability**: Abstract classes, interfaces, helper methods

**Nguyên tắc vàng**: 
- Validate sớm, fail fast
- Luôn dùng transaction cho multi-step operations
- Log đầy đủ để debug
- Throw exception rõ ràng
- Business logic ở Service, data access ở Repository
