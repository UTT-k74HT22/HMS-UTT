# 🔍 COMPREHENSIVE PROJECT REVIEW - HMS INVENTORY & ORDER MANAGEMENT SYSTEM

**Reviewer Role**: Senior Software Architect & Business Analyst  
**Date**: January 13, 2026  
**Project**: Hospital Management System - Inventory & Order Module  
**Technology Stack**: C# .NET, WinForms, SQL Server

---

## 📋 EXECUTIVE SUMMARY

Dự án HMS là một hệ thống quản lý kho và đơn hàng cho ngành dược phẩm/bệnh viện, được xây dựng trên nền tảng C# .NET với kiến trúc 3-layer (Controller-Service-Repository). Sau khi review toàn bộ source code, database schema và business logic, tôi đã xác định được **19 vấn đề nghiêm trọng** và **15 điểm cần cải thiện** trong hệ thống.

### ⚠️ Mức độ nghiêm trọng:
- 🔴 **CRITICAL (5)**: Ảnh hưởng trực tiếp đến tính toàn vẹn dữ liệu và bảo mật
- 🟠 **HIGH (7)**: Có thể gây mất dữ liệu hoặc sai business logic
- 🟡 **MEDIUM (7)**: Ảnh hưởng đến hiệu năng và user experience
- 🔵 **LOW (15)**: Code quality và maintainability

---

## 🚨 CRITICAL ISSUES (Ưu tiên cao nhất)

### 🔴 1. **Race Condition trong Inventory Update** 
**File**: `StockMovementServiceImpl.cs`, `InventoryRepositoryImpl.cs`

**Vấn đề**: 
Khi có nhiều transaction đồng thời cập nhật inventory (ví dụ: 2 nhân viên cùng bán hàng từ cùng 1 batch), có thể xảy ra race condition:

```csharp
// StockMovementServiceImpl.cs - Line 48
var inventoryInfo = _inventoryRepository.GetOrCreateInventoryItem(...);  // READ
int currentQuantityBefore = inventoryInfo.CurrentQuantity;

// ... Xử lý logic ...

_inventoryRepository.UpdateQuantity(inventoryInfo.InventoryItemId, quantityAfter); // WRITE
```

**Kịch bản lỗi**:
1. Transaction A đọc quantity = 100
2. Transaction B đọc quantity = 100 (vẫn chưa commit)
3. Transaction A trừ 50 → Update quantity = 50
4. Transaction B trừ 30 → Update quantity = 70 (WRONG! Should be 20)

**Tác động**: 
- ❌ Mất dữ liệu tồn kho
- ❌ Bán quá số lượng thực tế
- ❌ Số liệu báo cáo sai

**Giải pháp**:
```sql
-- Sử dụng Row-Level Locking với UPDLOCK, ROWLOCK
UPDATE inventory_items WITH (UPDLOCK, ROWLOCK)
SET quantity_on_hand = quantity_on_hand - @quantity,
    updated_at = SYSDATETIME()
WHERE id = @inventoryItemId 
  AND quantity_on_hand >= @quantity  -- Optimistic check
OUTPUT INSERTED.quantity_on_hand;

-- Nếu affected rows = 0 → Throw exception insufficient stock
```

---

### 🔴 2. **Không Có Transaction Trong Order Creation**
**File**: `OrderServiceImpl.cs` - Line 19-43

**Vấn đề**:
```csharp
public long CreateOrder(CreateOrderWithItemsRequest req, long employeeId)
{
    var orderId = _repo.InsertOrder(...);  // Operation 1
    
    foreach (var item in items)
    {
        _repo.InsertItem(orderId, item);   // Operation 2, 3, 4...
    }
    
    _repo.UpdateOrderTotal(orderId);       // Final operation
    return orderId;
}
```

**Kịch bản lỗi**:
1. Order được tạo thành công (ID=1001)
2. 5 items đầu được insert thành công
3. Item thứ 6 bị lỗi (FK violation, network issue...)
4. ❌ Kết quả: Order 1001 chỉ có 5/10 items → Dữ liệu inconsistent

**Tác động**:
- ❌ Đơn hàng bị thiếu sản phẩm
- ❌ Tổng tiền sai
- ❌ Customer complaint

**Giải pháp**:
```csharp
public long CreateOrder(CreateOrderWithItemsRequest req, long employeeId)
{
    using var connection = new SqlConnection(_connectionString);
    connection.Open();
    using var transaction = connection.BeginTransaction();
    
    try 
    {
        var orderId = _repo.InsertOrder(connection, transaction, ...);
        
        foreach (var item in req.Items)
        {
            _repo.InsertItem(connection, transaction, orderId, item);
        }
        
        _repo.UpdateOrderTotal(connection, transaction, orderId);
        
        transaction.Commit();
        return orderId;
    }
    catch 
    {
        transaction.Rollback();
        throw;
    }
}
```

---

### 🔴 3. **Password Được Lưu Plain Text**
**File**: `db.sql` - Line 23, `AccountServiceImpl.cs`

**Vấn đề**:
```sql
CREATE TABLE dbo.accounts (
    username VARCHAR(50) NOT NULL,
    [password] VARCHAR(255) NOT NULL,  -- Plain text!
    ...
)

INSERT INTO accounts (username, [password], role)
VALUES ('admin', '123456789', 'ADMIN');  -- Plain text password!
```

**Tác động**:
- 🚨 **GDPR Violation**
- 🚨 **Security Breach** - Nếu DB bị leak, toàn bộ password bị lộ
- 🚨 **Compliance Issue** - Không đạt chuẩn ISO 27001, PCI-DSS

**Giải pháp**:
```csharp
// Sử dụng BCrypt
using BCrypt.Net;

// Khi tạo account
string hashedPassword = BCrypt.HashPassword(request.Password);
account.Password = hashedPassword;

// Khi login
bool isValid = BCrypt.Verify(inputPassword, storedHashedPassword);
```

**Action Items**:
1. Cài đặt BCrypt.Net-Next NuGet package
2. Hash tất cả password hiện có trong DB
3. Update login logic

---

### 🔴 4. **Không Validate Inventory Trước Khi Confirm Order**
**File**: `OrderServiceImpl.cs` - Line 49

**Vấn đề**:
```csharp
public void ConfirmOrder(long orderId)
{
    _repo.UpdateStatus(orderId, OrderStatus.CONFIRMED.ToString());
    // ❌ KHÔNG kiểm tra stock availability!
}
```

**Kịch bản lỗi**:
1. Customer đặt hàng 1000 viên thuốc X
2. Employee tạo order thành công (status=NEW)
3. Kho chỉ còn 500 viên
4. Employee confirm order → Status=CONFIRMED
5. ❌ Không thể fulfill order → Customer angry

**Giải pháp**:
```csharp
public void ConfirmOrder(long orderId)
{
    // 1. Get order items
    var orderItems = _repo.GetItems(orderId);
    
    // 2. Check stock for each item
    foreach (var item in orderItems)
    {
        int availableQty = _inventoryService.GetAvailableQuantity(
            item.ProductId, 
            item.WarehouseId
        );
        
        if (availableQty < item.Quantity)
        {
            throw new InsufficientStockException(
                $"Sản phẩm {item.ProductName}: Yêu cầu {item.Quantity}, " +
                $"tồn kho {availableQty}"
            );
        }
    }
    
    // 3. Reserve stock (quantity_reserved)
    foreach (var item in orderItems)
    {
        _inventoryService.ReserveStock(
            item.ProductId, 
            item.WarehouseId, 
            item.Quantity
        );
    }
    
    // 4. Update status
    _repo.UpdateStatus(orderId, OrderStatus.CONFIRMED.ToString());
}
```

---

### 🔴 5. **Thiếu Soft Delete Cho Các Entity Quan Trọng**
**File**: `db.sql` - Tables products, batches, inventory_items

**Vấn đề**:
```sql
-- InventoryRepositoryImpl.cs - Line 85
WHERE p.deleted_at IS NULL  -- ❌ Column này không tồn tại!
```

Database schema không có cột `deleted_at` nhưng code có reference đến nó.

**Tác động**:
- ❌ Runtime error khi query
- ❌ Không thể soft delete products
- ❌ Mất lịch sử khi hard delete

**Giải pháp**:
```sql
-- Thêm soft delete columns
ALTER TABLE products 
    ADD deleted_at DATETIME2 NULL,
        deleted_by INT NULL;

ALTER TABLE batches 
    ADD deleted_at DATETIME2 NULL,
        deleted_by INT NULL;

ALTER TABLE warehouses 
    ADD deleted_at DATETIME2 NULL,
        deleted_by INT NULL;

-- Index cho performance
CREATE INDEX idx_products_deleted_at 
    ON products(deleted_at) WHERE deleted_at IS NULL;
```

---

## 🟠 HIGH PRIORITY ISSUES

### 🟠 6. **Không Có Optimistic Concurrency Control**

**Vấn đề**: Khi nhiều user cùng edit 1 record, last-write-wins → Mất data

**Giải pháp**: Thêm `row_version` (timestamp) column:
```sql
ALTER TABLE inventory_items 
    ADD row_version ROWVERSION;

-- Update with version check
UPDATE inventory_items
SET quantity_on_hand = @newQty,
    updated_at = SYSDATETIME()
WHERE id = @id 
  AND row_version = @expectedVersion;

IF @@ROWCOUNT = 0
    THROW 50001, 'Data has been modified by another user', 1;
```

---

### 🟠 7. **Batch Expiry Check Không Được Enforce Khi Bán Hàng**

**Vấn đề**: Có thể bán thuốc đã hết hạn

**Giải pháp**:
```csharp
// Trong OrderServiceImpl.CreateOrder()
foreach (var item in items)
{
    if (item.BatchId.HasValue)
    {
        var batch = _batchService.GetById(item.BatchId.Value);
        if (batch.ExpiryDate < DateTime.Now)
        {
            throw new ExpiredBatchException(
                $"Batch {batch.BatchCode} đã hết hạn ngày {batch.ExpiryDate:dd/MM/yyyy}"
            );
        }
    }
}
```

---

### 🟠 8. **SQL Injection Risk**

**File**: Một số query string concatenation

**Giải pháp**: Sử dụng parameterized queries EVERYWHERE (đã làm tốt ở hầu hết chỗ, cần audit lại)

---

### 🟠 9. **Không Có Database Connection Pooling Configuration**

**File**: `appsettings.json`

**Hiện tại**:
```json
"Server=localhost;Database=hms;User Id=sa;Password=123456789;TrustServerCertificate=True;"
```

**Nên**:
```json
"Server=localhost;Database=hms;User Id=sa;Password=123456789;TrustServerCertificate=True;Min Pool Size=5;Max Pool Size=100;Pooling=true;"
```

---

### 🟠 10. **Thiếu Audit Trail Cho Các Thay Đổi Quan Trọng**

**Vấn đề**: Không biết ai đã:
- Sửa giá sản phẩm
- Xóa batch
- Điều chỉnh tồn kho

**Giải pháp**: Tạo bảng audit_logs:
```sql
CREATE TABLE audit_logs (
    id INT IDENTITY(1,1) PRIMARY KEY,
    table_name VARCHAR(50) NOT NULL,
    record_id INT NOT NULL,
    action VARCHAR(20) NOT NULL, -- INSERT, UPDATE, DELETE
    old_values NVARCHAR(MAX) NULL, -- JSON
    new_values NVARCHAR(MAX) NULL, -- JSON
    changed_by INT NOT NULL,
    changed_at DATETIME2 DEFAULT SYSDATETIME(),
    ip_address VARCHAR(50) NULL
);
```

---

### 🟠 11. **Không Validate Business Rules Cho Discount**

**File**: `OrderRepositoryImpl.cs`

**Vấn đề**:
```csharp
cmd.Parameters.AddWithValue("@discount", discount);
// ❌ Không check discount > total?
// ❌ Discount có thể âm?
```

**Giải pháp**:
```csharp
if (discount < 0)
    throw new ArgumentException("Discount không thể âm");
    
if (discount > subtotal)
    throw new ArgumentException("Discount không thể lớn hơn subtotal");
```

---

### 🟠 12. **Thiếu Index Cho Performance**

**Missing Indexes**:
```sql
-- Cho stock movements search by date
CREATE INDEX idx_stock_movements_movement_date 
    ON stock_movements(movement_date DESC);

-- Cho order search by customer + date
CREATE INDEX idx_orders_customer_date 
    ON orders(customer_id, order_date DESC);

-- Cho inventory low stock queries
CREATE INDEX idx_inventory_low_stock 
    ON inventory_items(quantity_on_hand) 
    WHERE quantity_on_hand <= min_threshold;
```

---

## 🟡 MEDIUM PRIORITY ISSUES

### 🟡 13. **Batch Code Generator Không Unique**

**Issue**: Dựa vào timestamp có thể trùng nếu tạo nhanh

**Giải pháp**: Sử dụng GUID hoặc sequence:
```csharp
string batchCode = $"BCH-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
```

---

### 🟡 14. **Không Có Retry Logic Cho Database Transient Errors**

**Giải pháp**: Implement Polly retry policy:
```csharp
var retryPolicy = Policy
    .Handle<SqlException>()
    .WaitAndRetry(3, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    );

retryPolicy.Execute(() => {
    // Database operation
});
```

---

### 🟡 15. **Error Messages Không Multilingual**

Hardcoded Vietnamese messages → Khó mở rộng quốc tế

---

### 🟡 16. **Không Có Caching Layer**

Queries như GetAllCategories, GetAllManufacturers nên cache

---

### 🟡 17. **Thiếu Input Validation ở Controller Layer**

Validate data đầu vào trước khi xuống Service layer

---

### 🟡 18. **Connection String Hardcoded Password**

**File**: `appsettings.json`

**Security Risk**: Password visible trong source code

**Giải pháp**: 
- Development: User Secrets
- Production: Azure Key Vault / Environment Variables

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=hms;User Id=sa;Password=${DB_PASSWORD};TrustServerCertificate=True;"
  }
}
```

---

### 🟡 19. **Không Có Health Check Endpoint**

Cần implement health check để monitoring:
- Database connectivity
- Disk space
- Memory usage

---

## 📊 ARCHITECTURE REVIEW

### ✅ Điểm Mạnh:
1. ✅ **Layered Architecture** tốt (Controller → Service → Repository)
2. ✅ **Dependency Injection** được áp dụng đúng
3. ✅ **Transaction handling** ở StockMovementService
4. ✅ **Parameterized queries** tránh SQL injection (hầu hết)
5. ✅ **Database schema** có indexes hợp lý
6. ✅ **Trigger** để auto-update timestamps
7. ✅ **Foreign keys** đảm bảo referential integrity

### ❌ Điểm Yếu:
1. ❌ **Thiếu Unit Tests** hoàn toàn
2. ❌ **Không có Logging framework** (Serilog, NLog)
3. ❌ **Exception handling** chưa centralized
4. ❌ **Validation** scatter khắp nơi, không consistent
5. ❌ **DTOs** thiếu data annotations
6. ❌ **Repository pattern** chưa hoàn chỉnh (còn direct SQL)

---

## 🎯 MAIN FLOW ANALYSIS

### 📦 **Inventory Management Flow**

**Current Flow**:
```
1. Import Stock
   ├─ Tạo Batch → Insert stock_movements (IMPORT)
   ├─ Update inventory_items.quantity_on_hand
   └─ ✅ Có transaction protection

2. Export Stock  
   ├─ Check quantity available
   ├─ Insert stock_movements (EXPORT)
   ├─ Update inventory_items.quantity_on_hand
   └─ ⚠️ RISK: Race condition nếu concurrent exports

3. Transfer Stock
   ├─ Decrease từ warehouse A
   ├─ Increase vào warehouse B
   ├─ Log 2 movements (EXPORT + IMPORT)
   └─ ✅ Transaction đảm bảo atomicity

4. Stock Adjustment
   ├─ Admin điều chỉnh số lượng
   └─ ⚠️ RISK: Không có approval workflow
```

**Critical Issues**:
- 🔴 **No optimistic locking** → Lost updates
- 🔴 **No stock reservation** → Overselling
- 🟠 **No audit trail** → Không truy vết được thay đổi
- 🟠 **No expiry enforcement** → Bán hàng hết hạn

---

### 🛒 **Order/Purchase Flow**

**Current Flow**:
```
1. Create Order (Status: NEW)
   ├─ Insert orders table
   ├─ Insert order_items (multiple)
   ├─ Calculate totals
   └─ ❌ NO TRANSACTION!

2. Confirm Order (NEW → CONFIRMED)
   ├─ Update status only
   └─ ❌ Không check stock availability!

3. Process Order (CONFIRMED → PROCESSING)
   └─ ❌ Không có auto stock deduction!

4. Complete Order (PROCESSING → COMPLETED)
   └─ ❌ Inventory không được update!
```

**Critical Issues**:
- 🔴 **Order creation không atomic** → Partial orders
- 🔴 **Confirm không validate stock** → Overselling
- 🔴 **Không có stock reservation** → Double selling
- 🟠 **Manual inventory update** → Human error
- 🟠 **Không có auto-fulfillment** → Inefficient

**Recommended Flow**:
```
1. Create Order
   ├─ [Transaction Start]
   ├─ Validate customer exists
   ├─ Validate products exist
   ├─ Check stock availability for ALL items
   ├─ Insert order
   ├─ Insert order_items
   ├─ Calculate totals
   └─ [Transaction Commit]

2. Confirm Order
   ├─ [Transaction Start]
   ├─ Validate order status = NEW
   ├─ Re-check stock availability
   ├─ Reserve stock (quantity_reserved++)
   ├─ Update status → CONFIRMED
   └─ [Transaction Commit]

3. Process Order (Auto/Manual)
   ├─ [Transaction Start]
   ├─ For each order_item:
   │   ├─ Decrease inventory.quantity_on_hand
   │   ├─ Decrease inventory.quantity_reserved
   │   └─ Log stock_movement (EXPORT)
   ├─ Update status → PROCESSING
   └─ [Transaction Commit]

4. Complete Order
   ├─ Generate invoice
   ├─ Update status → COMPLETED
   └─ Send confirmation email
```

---

## 🔐 SECURITY ISSUES SUMMARY

| Issue | Severity | Impact | Status |
|-------|----------|--------|--------|
| Plain text passwords | 🔴 CRITICAL | Data breach | ❌ Not fixed |
| SQL Injection (potential) | 🟠 HIGH | Data loss | ⚠️ Mostly safe |
| No authentication timeout | 🟡 MEDIUM | Session hijacking | ❌ Not implemented |
| No role-based access control | 🟡 MEDIUM | Privilege escalation | ⚠️ Partial |
| Sensitive data in logs | 🟡 MEDIUM | Information disclosure | ❌ Not audited |
| No rate limiting | 🔵 LOW | DoS attack | ❌ Not implemented |

---

## ⚡ PERFORMANCE ISSUES

1. **N+1 Query Problem**: `GetAllInventory()` có thể optimize bằng JOIN thay vì multiple queries
2. **Missing Indexes**: Một số foreign keys chưa có index
3. **No Query Pagination**: `GetAll()` methods load toàn bộ data
4. **No Caching**: Static data như categories, manufacturers nên cache
5. **Inefficient Stock Check**: Multiple DB calls, nên làm batch query

---

## 📈 SCALABILITY CONCERNS

1. **Single Database**: Không có read replicas
2. **No Message Queue**: Stock updates nên async
3. **Monolithic**: Tất cả logic trong 1 app
4. **No API Layer**: Khó integrate với mobile/web
5. **File-based Config**: Không dùng config server

---

## 🧪 TESTING GAPS

- ❌ **0% Unit Test Coverage**
- ❌ **No Integration Tests**
- ❌ **No Load Testing**
- ❌ **No Security Testing**
- ❌ **No Regression Tests**

**Recommendation**: 
- Minimum 70% code coverage
- Critical paths: 100% coverage (Order, Inventory)

---

## 📝 CODE QUALITY ISSUES

1. **Magic Strings**: `"ACTIVE"`, `"NEW"`, etc. nên dùng Constants
2. **Long Methods**: Một số methods > 100 lines
3. **Commented Code**: Remove hoặc explain
4. **Inconsistent Naming**: Lẫn lộn Vietnamese/English
5. **Missing XML Documentation**: Nhiều public methods thiếu docs

---

## 💡 RECOMMENDATIONS PRIORITY

### 🔴 **IMMEDIATE (Week 1-2)**
1. ✅ Fix password hashing (BCrypt)
2. ✅ Add transaction to Order creation
3. ✅ Implement stock validation before order confirm
4. ✅ Fix race condition in inventory updates (locking)
5. ✅ Add soft delete columns

### 🟠 **SHORT TERM (Month 1)**
6. ✅ Implement audit trail
7. ✅ Add stock reservation mechanism
8. ✅ Batch expiry enforcement
9. ✅ Optimistic concurrency control
10. ✅ Add missing indexes

### 🟡 **MEDIUM TERM (Month 2-3)**
11. ✅ Implement logging (Serilog)
12. ✅ Add unit tests (critical paths first)
13. ✅ Implement caching (Redis/Memory)
14. ✅ API layer for future integrations
15. ✅ Health checks & monitoring

### 🔵 **LONG TERM (Month 4-6)**
16. ✅ Migrate to microservices architecture
17. ✅ Implement event sourcing for inventory
18. ✅ Add message queue (RabbitMQ/Azure Service Bus)
19. ✅ Multi-language support
20. ✅ Mobile app integration

---

## 📌 CONCLUSION

Dự án có **foundation tốt** với kiến trúc phân lớp rõ ràng, nhưng còn **nhiều lỗ hổng nghiêm trọng** về:
- ✅ Data integrity (race conditions, no transactions)
- ✅ Security (plain text passwords, missing auth)
- ✅ Business logic (no stock validation, no reservation)

**Priority #1**: Fix 5 critical issues trước khi go-live  
**Priority #2**: Implement testing & monitoring  
**Priority #3**: Prepare for scalability (API, caching, async)

**Overall Assessment**: ⭐⭐⭐ (3/5) - Functional but needs major improvements before production

---

**Next Steps**: See VERSION_2_ROADMAP.md for detailed enhancement plan.
