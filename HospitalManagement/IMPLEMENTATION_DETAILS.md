# 🔧 IMPLEMENTATION ROADMAP - BASE REPOSITORY

## 📋 QUÁ TRÌNH IMPLEMENTATION TỪNG BƯỚC

---

## **BƯỚC 1: Tạo IBaseRepository<T> Interface**

**File**: `repository/IBaseRepository.cs`

```csharp
using HospitalManagement.entity;
using System.Linq.Expressions;

namespace HospitalManagement.repository
{
    /// <summary>
    /// Generic Base Repository Interface cho tất cả entities
    /// Định nghĩa các phương thức CRUD và querying cơ bản
    /// </summary>
    /// <typeparam name="T">Entity type, phải inherit từ BaseEntity</typeparam>
    public interface IBaseRepository<T> where T : BaseEntity
    {
        #region CREATE Operations
        
        /// <summary>
        /// Tạo mới một entity
        /// </summary>
        /// <param name="entity">Entity cần tạo</param>
        /// <returns>Entity đã được tạo (có Id)</returns>
        Task<T> CreateAsync(T entity);

        /// <summary>
        /// Tạo nhiều entities cùng lúc (bulk insert)
        /// </summary>
        /// <param name="entities">Danh sách entities</param>
        /// <returns>Danh sách các entities đã tạo</returns>
        Task<List<T>> CreateMultipleAsync(List<T> entities);

        #endregion

        #region READ Operations

        /// <summary>
        /// Lấy entity theo Id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Entity hoặc null nếu không tìm thấy</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Lấy tất cả entities (CẨN THẬN với bảng lớn!)
        /// </summary>
        /// <returns>Danh sách tất cả entities</returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Lấy entities theo trang (pagination)
        /// </summary>
        /// <param name="pageNo">Số trang (bắt đầu từ 1)</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <returns>Danh sách entities của trang đó</returns>
        Task<List<T>> GetByPageAsync(int pageNo, int pageSize);

        /// <summary>
        /// Tìm entity đầu tiên thỏa điều kiện
        /// </summary>
        /// <param name="predicate">Lambda expression filter</param>
        /// <returns>Entity hoặc null nếu không tìm thấy</returns>
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Tìm tất cả entities thỏa điều kiện
        /// </summary>
        /// <param name="predicate">Lambda expression filter</param>
        /// <returns>Danh sách entities tìm thấy</returns>
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Đếm số lượng entities
        /// </summary>
        /// <param name="predicate">Lambda expression filter (optional)</param>
        /// <returns>Số lượng entities</returns>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        #endregion

        #region UPDATE Operations

        /// <summary>
        /// Cập nhật một entity
        /// </summary>
        /// <param name="entity">Entity với dữ liệu cập nhật (phải có Id)</param>
        /// <returns>Entity đã cập nhật</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Cập nhật nhiều entities cùng lúc
        /// </summary>
        /// <param name="entities">Danh sách entities cập nhật</param>
        /// <returns>True nếu thành công</returns>
        Task<bool> UpdateMultipleAsync(List<T> entities);

        #endregion

        #region DELETE Operations

        /// <summary>
        /// Xóa entity theo Id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Xóa một entity cụ thể
        /// </summary>
        /// <param name="entity">Entity cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Xóa nhiều entities theo danh sách Id
        /// </summary>
        /// <param name="ids">Danh sách Ids cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteMultipleAsync(List<int> ids);

        #endregion

        #region EXISTS/UTILS

        /// <summary>
        /// Kiểm tra entity có tồn tại hay không
        /// </summary>
        /// <param name="predicate">Lambda expression filter</param>
        /// <returns>True nếu tồn tại</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        #endregion

        #region TRANSACTION/SAVE

        /// <summary>
        /// Lưu tất cả thay đổi vào database
        /// Thường được gọi từ UnitOfWork
        /// </summary>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        Task<int> SaveChangesAsync();

        #endregion
    }
}
```

---

## **BƯỚC 2: Tạo BaseRepository<T> Implementation**

**File**: `repository/impl/BaseRepository.cs`

```csharp
using HospitalManagement.configuration;
using HospitalManagement.entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HospitalManagement.repository.impl
{
    /// <summary>
    /// Generic base repository implementation
    /// Cung cấp CRUD operations cho tất cả entities
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        #region CREATE

        public async Task<T> CreateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            _dbSet.Add(entity);
            await SaveChangesAsync();

            return entity;
        }

        public async Task<List<T>> CreateMultipleAsync(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
                throw new ArgumentNullException(nameof(entities));

            var now = DateTime.UtcNow;
            foreach (var entity in entities)
            {
                entity.CreatedAt = now;
                entity.UpdatedAt = now;
            }

            _dbSet.AddRange(entities);
            await SaveChangesAsync();

            return entities;
        }

        #endregion

        #region READ

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetByPageAsync(int pageNo, int pageSize)
        {
            if (pageNo < 1) pageNo = 1;
            if (pageSize < 1) pageSize = 10;

            var skip = (pageNo - 1) * pageSize;

            return await _dbSet
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();

            return await _dbSet.CountAsync(predicate);
        }

        #endregion

        #region UPDATE

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.UpdatedAt = DateTime.UtcNow;

            _dbSet.Update(entity);
            await SaveChangesAsync();

            return entity;
        }

        public async Task<bool> UpdateMultipleAsync(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
                throw new ArgumentNullException(nameof(entities));

            var now = DateTime.UtcNow;
            foreach (var entity in entities)
            {
                entity.UpdatedAt = now;
            }

            _dbSet.UpdateRange(entities);
            var result = await SaveChangesAsync();

            return result > 0;
        }

        #endregion

        #region DELETE

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            var result = await SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
            var result = await SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteMultipleAsync(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                throw new ArgumentNullException(nameof(ids));

            var entities = await _dbSet
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();

            if (entities.Count == 0)
                return false;

            _dbSet.RemoveRange(entities);
            var result = await SaveChangesAsync();

            return result > 0;
        }

        #endregion

        #region EXISTS/UTILS

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return await _dbSet.AnyAsync(predicate);
        }

        #endregion

        #region TRANSACTION/SAVE

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion
    }
}
```

---

## **BƯỚC 3: Tạo IUnitOfWork Interface**

**File**: `repository/IUnitOfWork.cs`

```csharp
using HospitalManagement.entity;

namespace HospitalManagement.repository
{
    /// <summary>
    /// Unit of Work Pattern Interface
    /// Quản lý tất cả repositories và transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Repository Properties

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

        #endregion

        #region Transaction Methods

        /// <summary>
        /// Lưu tất cả thay đổi
        /// </summary>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Bắt đầu transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit transaction
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rollback transaction
        /// </summary>
        Task RollbackAsync();

        #endregion
    }
}
```

---

## **BƯỚC 4: Tạo UnitOfWork Implementation**

**File**: `repository/impl/UnitOfWork.cs`

```csharp
using HospitalManagement.configuration;
using HospitalManagement.entity;
using Microsoft.EntityFrameworkCore.Storage;

namespace HospitalManagement.repository.impl
{
    /// <summary>
    /// Unit of Work Implementation
    /// Khởi tạo và quản lý tất cả repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private IDbContextTransaction? _transaction;

        // Repositories
        private IBaseRepository<Account>? _accounts;
        private IBaseRepository<UserProfile>? _userProfiles;
        private IBaseRepository<EmployeeProfile>? _employeeProfiles;
        private IBaseRepository<CustomerProfile>? _customerProfiles;
        private IBaseRepository<Category>? _categories;
        private IBaseRepository<Manufacturer>? _manufacturers;
        private IBaseRepository<Product>? _products;
        private IBaseRepository<Warehouse>? _warehouses;
        private IBaseRepository<Batch>? _batches;
        private IBaseRepository<InventoryItem>? _inventoryItems;
        private IBaseRepository<StockMovement>? _stockMovements;
        private IBaseRepository<Order>? _orders;
        private IBaseRepository<OrderItem>? _orderItems;
        private IBaseRepository<Invoice>? _invoices;
        private IBaseRepository<Payment>? _payments;

        public UnitOfWork(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Repository Properties

        public IBaseRepository<Account> Accounts
            => _accounts ??= new BaseRepository<Account>(_context);

        public IBaseRepository<UserProfile> UserProfiles
            => _userProfiles ??= new BaseRepository<UserProfile>(_context);

        public IBaseRepository<EmployeeProfile> EmployeeProfiles
            => _employeeProfiles ??= new BaseRepository<EmployeeProfile>(_context);

        public IBaseRepository<CustomerProfile> CustomerProfiles
            => _customerProfiles ??= new BaseRepository<CustomerProfile>(_context);

        public IBaseRepository<Category> Categories
            => _categories ??= new BaseRepository<Category>(_context);

        public IBaseRepository<Manufacturer> Manufacturers
            => _manufacturers ??= new BaseRepository<Manufacturer>(_context);

        public IBaseRepository<Product> Products
            => _products ??= new BaseRepository<Product>(_context);

        public IBaseRepository<Warehouse> Warehouses
            => _warehouses ??= new BaseRepository<Warehouse>(_context);

        public IBaseRepository<Batch> Batches
            => _batches ??= new BaseRepository<Batch>(_context);

        public IBaseRepository<InventoryItem> InventoryItems
            => _inventoryItems ??= new BaseRepository<InventoryItem>(_context);

        public IBaseRepository<StockMovement> StockMovements
            => _stockMovements ??= new BaseRepository<StockMovement>(_context);

        public IBaseRepository<Order> Orders
            => _orders ??= new BaseRepository<Order>(_context);

        public IBaseRepository<OrderItem> OrderItems
            => _orderItems ??= new BaseRepository<OrderItem>(_context);

        public IBaseRepository<Invoice> Invoices
            => _invoices ??= new BaseRepository<Invoice>(_context);

        public IBaseRepository<Payment> Payments
            => _payments ??= new BaseRepository<Payment>(_context);

        #endregion

        #region Transaction Methods

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _context.SaveChangesAsync();
                    await _transaction.CommitAsync();
                }
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        #endregion

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
```

---

## **BƯỚC 5: Cấu hình Dependency Injection**

**File**: `Program.cs` (Thêm vào)

```csharp
// Database Context
services.AddScoped<DbContext>(provider =>
    provider.GetRequiredService<HmsDbContext>());

// Generic Repository
services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Unit of Work
services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
services.AddScoped<IAuthService, AuthServiceImpl>();
// ... các services khác
```

---

## **BƯỚC 6: Cách sử dụng**

### **Trong Service:**
```csharp
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Tạo product
    public async Task<Product> CreateProductAsync(Product product)
    {
        return await _unitOfWork.Products.CreateAsync(product);
    }

    // Lấy products theo category
    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _unitOfWork.Products.FindAllAsync(
            p => p.CategoryId == categoryId && p.Status == "ACTIVE"
        );
    }

    // Update product
    public async Task<Product> UpdateProductAsync(Product product)
    {
        return await _unitOfWork.Products.UpdateAsync(product);
    }

    // Delete product
    public async Task<bool> DeleteProductAsync(int productId)
    {
        return await _unitOfWork.Products.DeleteAsync(productId);
    }
}
```

---

## **CHECKLIST - Những gì cần làm**

- [ ] Tạo file `repository/IBaseRepository.cs`
- [ ] Tạo file `repository/impl/BaseRepository.cs`
- [ ] Tạo file `repository/IUnitOfWork.cs`
- [ ] Tạo file `repository/impl/UnitOfWork.cs`
- [ ] Update `Program.cs` thêm DI configuration
- [ ] Test với một simple service
- [ ] Tạo AccountRepositoryImpl (nâng cấp từ cũ)
- [ ] Tạo ProductRepositoryImpl (với custom methods)
- [ ] Unit tests

---

**Status**: ✅ Ready to Implement
