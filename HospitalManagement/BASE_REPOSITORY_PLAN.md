# 📋 BASE REPOSITORY ARCHITECTURE PLAN

## 🎯 Mục đích
Xây dựng một Generic Repository Pattern cho toàn hệ thống, cung cấp CRUD operations cơ bản và phổ biến, tuân theo mindset junior - rõ ràng, dễ bảo trì, dễ mở rộng.

---

## 📁 FOLDER STRUCTURE (sẽ tạo)
```
HospitalManagement/
├── repository/
│   ├── IBaseRepository.cs          (Interface generic)
│   ├── IUnitOfWork.cs               (Unit of Work Pattern)
│   ├── impl/
│   │   ├── BaseRepository.cs        (Generic implementation)
│   │   ├── UnitOfWork.cs            (Implementation of Unit of Work)
│   │   ├── AccountRepositoryImpl.cs  (Specific repository - đã có)
│   │   ├── ProductRepositoryImpl.cs  (New)
│   │   ├── InventoryRepositoryImpl.cs (New)
│   │   └── ... (các repository khác)
```

---

## 🏗️ ARCHITECTURE COMPONENTS

### 1. **IBaseRepository<T>** (Interface)
```csharp
public interface IBaseRepository<T> where T : BaseEntity
{
    // CREATE
    Task<T> CreateAsync(T entity);
    Task<List<T>> CreateMultipleAsync(List<T> entities);
    
    // READ
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetByPageAsync(int pageNo, int pageSize);
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    
    // UPDATE
    Task<T> UpdateAsync(T entity);
    Task<bool> UpdateMultipleAsync(List<T> entities);
    
    // DELETE
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteAsync(T entity);
    Task<bool> DeleteMultipleAsync(List<int> ids);
    
    // EXISTS
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    
    // SAVE
    Task<int> SaveChangesAsync();
}
```

---

### 2. **BaseRepository<T>** (Generic Implementation)
**Tính năng chính:**
- Kế thừa từ IBaseRepository<T>
- Làm việc với DbContext
- Auto-mapped timestamps (CreatedAt, UpdatedAt)
- Error handling cơ bản
- Logging (optional)

**Key Methods:**
- Sử dụng LINQ to Entities
- Async/await patterns
- Soft delete support (optional)
- Pagination support

---

### 3. **IUnitOfWork** (Interface)
**Mục đích:** Quản lý các repositories và transactions
```csharp
public interface IUnitOfWork : IDisposable
{
    IBaseRepository<Account> Accounts { get; }
    IBaseRepository<UserProfile> UserProfiles { get; }
    IBaseRepository<EmployeeProfile> EmployeeProfiles { get; }
    IBaseRepository<CustomerProfile> CustomerProfiles { get; }
    IBaseRepository<Category> Categories { get; }
    IBaseRepository<Manufacturer> Manufacturers { get; }
    IBaseRepository<Product> Products { get; }
    IBaseRepository<Warehouse> Warehouses { get; }
    IBaseRepository<Batch> Batches { get; }
    IBaseRepository<InventoryItem> InventoryItems { get; }
    IBaseRepository<StockMovement> StockMovements { get; }
    IBaseRepository<Order> Orders { get; }
    IBaseRepository<OrderItem> OrderItems { get; }
    IBaseRepository<Invoice> Invoices { get; }
    IBaseRepository<Payment> Payments { get; }
    
    Task<int> SaveChangesAsync();
    Task<bool> BeginTransactionAsync();
    Task<bool> CommitAsync();
    Task<bool> RollbackAsync();
}
```

---

### 4. **UnitOfWork** (Implementation)
**Trách nhiệm:**
- Khởi tạo tất cả repositories
- Quản lý DbContext lifecycle
- Quản lý transactions

---

## 🔄 DEPENDENCY INJECTION SETUP

**Program.cs:**
```csharp
// Database
services.AddScoped<IDbContext, HmsDbContext>();

// Repositories
services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services sẽ inject IUnitOfWork
services.AddScoped<IAuthService, AuthServiceImpl>();
```

---

## 📊 FLOW DIAGRAM

```
Controller
    ↓
Service (Business Logic)
    ↓
IUnitOfWork
    ├── IBaseRepository<Account>
    ├── IBaseRepository<Product>
    ├── IBaseRepository<InventoryItem>
    └── ... (các repositories khác)
    ↓
DbContext
    ↓
Database
```

---

## 🛠️ IMPLEMENTATION PLAN (Chi tiết)

### **PHASE 1: Base Infrastructure** (Tuần 1)
- [ ] 1.1 Tạo IBaseRepository<T> interface
- [ ] 1.2 Tạo BaseRepository<T> implementation
- [ ] 1.3 Tạo IUnitOfWork interface
- [ ] 1.4 Tạo UnitOfWork implementation

### **PHASE 2: Dependency Injection Setup** (Tuần 1)
- [ ] 2.1 Cấu hình DI trong Program.cs
- [ ] 2.2 Setup DbContext connection

### **PHASE 3: Specific Repositories** (Tuần 2)
- [ ] 3.1 Implement AccountRepositoryImpl (nâng cấp từ hiện tại)
- [ ] 3.2 Implement ProductRepositoryImpl (với filters)
- [ ] 3.3 Implement InventoryRepositoryImpl (complex queries)
- [ ] 3.4 Implement OrderRepositoryImpl (với joins)

### **PHASE 4: Testing & Refinement** (Tuần 2-3)
- [ ] 4.1 Unit tests cho BaseRepository
- [ ] 4.2 Integration tests
- [ ] 4.3 Optimize queries

---

## 💡 BEST PRACTICES (Junior Mindset)

### ✅ DO:
1. **Naming Convention**: Rõ ràng, descriptive
   - `GetByIdAsync()` không `GetById()`
   - `CreateAsync()` không `Add()`

2. **Documentation**: XML Comments cho mọi public method
   ```csharp
   /// <summary>
   /// Lấy entity theo ID
   /// </summary>
   /// <param name="id">Primary key</param>
   /// <returns>Entity hoặc null nếu không tìm thấy</returns>
   Task<T?> GetByIdAsync(int id);
   ```

3. **Async All The Way**: Tất cả database operations phải async
   ```csharp
   Task<T> CreateAsync(T entity);  // ✓
   T Create(T entity);              // ✗
   ```

4. **Error Handling**: Try-catch với meaningful messages
   ```csharp
   try
   {
       return await _context.Set<T>().FindAsync(id);
   }
   catch (Exception ex)
   {
       _logger?.LogError($"Error getting {typeof(T).Name} with id {id}: {ex.Message}");
       throw;
   }
   ```

5. **LINQ Best Practices**:
   - Sử dụng `Where()` filter trước `Select()`
   - Avoid `ToList()` trong queries
   - Dùng `FirstOrDefaultAsync()` thay vì `ToListAsync().FirstOrDefault()`

### ❌ DON'T:
1. ❌ Không hardcode connection strings
2. ❌ Không catch Exception generic mà không re-throw
3. ❌ Không dùng `.Result` hoặc `.Wait()` (deadlock risk)
4. ❌ Không bỏ qua null checks

---

## 🧪 TESTING STRATEGY

### Unit Tests:
```
BaseRepository<T>Tests
├── CreateAsync_WithValidEntity_ReturnsEntity
├── GetByIdAsync_WithValidId_ReturnsEntity
├── UpdateAsync_WithModifiedEntity_UpdatesSuccessfully
├── DeleteAsync_WithValidId_ReturnsTrue
└── FindAllAsync_WithPredicate_ReturnsFilteredResults
```

### Integration Tests:
```
IntegrationTests
├── CreateAndRetrieveProduct_Works
├── UpdateInventory_UpdatesCorrectly
└── DeleteOrder_CascadesCorrectly
```

---

## 📝 COMMON REPOSITORY PATTERNS FOR SPECIFIC ENTITIES

### ProductRepository (ngoài BaseRepository):
```csharp
public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<Product>> GetByCategory(int categoryId);
    Task<List<Product>> GetNeedingPrescription();
    Task<List<Product>> SearchByName(string keyword);
}
```

### InventoryRepository:
```csharp
public interface IInventoryRepository : IBaseRepository<InventoryItem>
{
    Task<List<InventoryItem>> GetLowStockItems();
    Task<InventoryItem?> GetByProductAndWarehouse(int productId, int warehouseId);
    Task<decimal> GetTotalValueByWarehouse(int warehouseId);
}
```

### OrderRepository:
```csharp
public interface IOrderRepository : IBaseRepository<Order>
{
    Task<List<Order>> GetByCustomer(int customerId);
    Task<List<Order>> GetByStatus(string status);
    Task<List<Order>> GetOrdersWithItems(int orderId);
}
```

---

## 🚀 NEXT STEPS

1. **Tạo IBaseRepository.cs** với comprehensive interface
2. **Tạo BaseRepository.cs** implementation
3. **Tạo IUnitOfWork.cs** interface
4. **Tạo UnitOfWork.cs** implementation
5. **Cấu hình DI** trong Program.cs
6. **Refactor AccountRepositoryImpl** để kế thừa từ BaseRepository
7. **Tạo ProductRepositoryImpl**, **InventoryRepositoryImpl**, etc.

---

## 📚 REFERENCE DOCS

- Microsoft EF Core Best Practices
- Generic Repository Pattern (Microsoft Docs)
- Unit of Work Pattern
- Dependency Injection in .NET

---

**Status**: 📋 Planning Complete ✓
**Date**: Jan 6, 2026
**Author**: Junior C# Developer Mindset
