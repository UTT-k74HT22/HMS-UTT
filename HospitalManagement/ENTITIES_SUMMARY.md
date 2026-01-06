# 📊 ENTITY SUMMARY & DATABASE MAPPING

## ✅ Hoàn thiện tất cả 15 Entities

Dưới đây là danh sách đầy đủ các entities đã được tạo/hoàn thiện để match với database schema:

---

## 📋 DANH SÁCH ENTITIES

| # | Entity | Properties | DB Table | Status |
|---|--------|-----------|----------|--------|
| 1 | **Account** | Id, Username, Password, Role, IsActive, LastLoginAt, CreatedAt, UpdatedAt | `accounts` | ✅ |
| 2 | **UserProfile** | Id, AccountId, Code, FullName, Phone, Email, Address, Status, CreatedAt, UpdatedAt | `user_profiles` | ✅ |
| 3 | **EmployeeProfile** | Id, ProfileId, Position, Department, HiredDate, BaseSalary, CreatedAt, UpdatedAt | `employee_profiles` | ✅ |
| 4 | **CustomerProfile** | Id, ProfileId, CustomerType, TaxCode, CreatedAt, UpdatedAt | `customer_profiles` | ✅ |
| 5 | **Category** | Id, Code, Name, Description, ParentId, IsActive, DisplayOrder, CreatedAt, UpdatedAt | `categories` | ✅ |
| 6 | **Manufacturer** | Id, Code, Name, Country, Address, Phone, Email, ContactPerson, CreatedAt, UpdatedAt | `manufacturers` | ✅ |
| 7 | **Product** | Id, CategoryId, ManufacturerId, Code, Barcode, Name, DosageForm, Unit, Description, ImageUrl, StandardPrice, RequiresPrescription, Status, CreatedAt, UpdatedAt | `products` | ✅ |
| 8 | **Warehouse** | Id, Code, Name, Address, Phone, ManagerName, IsActive, CreatedAt, UpdatedAt | `warehouses` | ✅ |
| 9 | **Batch** | Id, ProductId, BatchCode, ImportPrice, ManufactureDate, ExpiryDate, SupplierName, Status, CreatedAt, UpdatedAt | `batches` | ✅ |
| 10 | **InventoryItem** | Id, ProductId, BatchId, WarehouseId, QuantityOnHand, QuantityReserved, MinThreshold, MaxThreshold, LastStockCheck, CreatedAt, UpdatedAt | `inventory_items` | ✅ |
| 11 | **StockMovement** | Id, MovementType, ProductId, BatchId, WarehouseId, Quantity, QuantityBefore, QuantityAfter, MovementDate, ReferenceType, ReferenceId, PerformedByUserId, Note, CreatedAt | `stock_movements` | ✅ |
| 12 | **Order** | Id, CustomerId, OrderNumber, OrderDate, Status, Subtotal, Discount, Tax, TotalAmount, ShippingAddress, CreatedByUserId, Note, CreatedAt, UpdatedAt | `orders` | ✅ |
| 13 | **OrderItem** | Id, OrderId, ProductId, BatchId, Quantity, UnitPrice, Discount, LineTotal | `order_items` | ✅ |
| 14 | **Invoice** | Id, OrderId, InvoiceNumber, IssueDate, DueDate, TotalAmount, PaidAmount, Status, CreatedAt, UpdatedAt | `invoices` | ✅ |
| 15 | **Payment** | Id, InvoiceId, PaymentNumber, PaymentDate, Amount, Method, Status, CreatedAt | `payments` | ✅ |

---

## 🏗️ ENTITY RELATIONSHIPS (ER DIAGRAM TEXT)

```
Account (1)
    └──> (M) UserProfile
            ├──> (1) EmployeeProfile
            └──> (1) CustomerProfile

Category (1)
    ├──> (M) Product
    └──> (M) Category (ParentId - Self Reference)

Manufacturer (1)
    └──> (M) Product

Product (1)
    ├──> (M) Batch
    ├──> (M) InventoryItem
    ├──> (M) StockMovement
    └──> (M) OrderItem

Batch (1)
    ├──> (M) InventoryItem
    ├──> (M) StockMovement
    └──> (M) OrderItem

Warehouse (1)
    ├──> (M) InventoryItem
    └──> (M) StockMovement

InventoryItem (N,N,N) - Composite Key: ProductId, BatchId, WarehouseId

StockMovement - Audit trail cho inventory

Order (1) [Customer: UserProfile]
    └──> (M) OrderItem
            ├──> Product
            └──> Batch

Invoice (1)
    └──> (M) Payment
    └──> (1) Order

UserProfile (1) [Employee/Creator]
    └──> (M) StockMovement
    └──> (M) Order
```

---

## 📁 FILE LOCATIONS

**Entity Folder**: `HospitalManagement/entity/`

```
entity/
├── BaseEntity.cs                ← Parent class cho tất cả entities
├── Enums.cs                     ← Tất cả enum definitions
├── Account.cs                   ✅
├── UserProfile.cs               ✅
├── EmployeeProfile.cs           ✅
├── CustomerProfile.cs           ✅
├── Category.cs                  ✅
├── Manufacturer.cs              ✅
├── Product.cs                   ✅
├── Warehouse.cs                 ✅
├── Batch.cs                     ✅
├── InventoryItem.cs             ✅
├── StockMovement.cs             ✅
├── Order.cs                     ✅
├── OrderItem.cs                 ✅
├── Invoice.cs                   ✅ [NEW]
└── Payment.cs                   ✅ [NEW]
```

---

## 🎯 ENTITY CHARACTERISTICS

### **1. Account** - Authentication & Authorization
- Stores login credentials
- Role-based access control (ADMIN, EMPLOYEE, CUSTOMER)
- Last login tracking
- Status management

### **2. UserProfile** - Common user data
- Shared by all user types
- Contact information (Phone, Email, Address)
- Unique user code for business logic
- Status tracking

### **3. EmployeeProfile** - Employee-specific info
- One-to-One with UserProfile
- Job position and department
- Hire date and salary info
- Department/team management

### **4. CustomerProfile** - Customer-specific info
- One-to-One with UserProfile
- Customer type (RETAIL/WHOLESALE)
- Tax code for invoicing
- B2B vs B2C distinction

### **5. Category** - Product categorization
- Hierarchical structure (ParentId)
- Display ordering
- Soft status (is_active)
- Self-referencing foreign key

### **6. Manufacturer** - Product manufacturers
- Contact information
- Country of origin
- Supply chain tracking

### **7. Product** - Core product data
- Pharmacy/medicine specifics (DosageForm, Unit)
- Barcode for inventory tracking
- Prescription requirement flag
- Status: ACTIVE/INACTIVE/DISCONTINUED

### **8. Warehouse** - Storage locations
- Multiple warehouses support
- Manager tracking
- Operation status

### **9. Batch** - Product batches/lots
- Expiry date tracking (critical for medicines)
- Import price for cost accounting
- Manufacturing/Expiry dates
- Batch status (ACTIVE/EXPIRED/BLOCKED/DEPLETED)

### **10. InventoryItem** - Stock tracking per location
- Product + Batch + Warehouse combination
- On-hand vs Reserved quantity
- Min/Max thresholds for reordering
- Last stock check date

### **11. StockMovement** - Audit trail
- IMPORT/EXPORT/ADJUST/TRANSFER types
- Before/After quantity tracking
- User who performed movement
- Reference to source document (Order, PO, etc.)

### **12. Order** - Customer orders
- Order status workflow
- Subtotal/Tax/Discount/Total calculations
- Shipping address
- Employee who created the order

### **13. OrderItem** - Line items in order
- Product + Batch selection
- Quantity and unit price at order time
- Line-level discount
- Line total calculation

### **14. Invoice** - Billing documents
- Link to Order
- Invoice-specific number (separate from order)
- Due date for payment tracking
- Paid amount for partial payments

### **15. Payment** - Payment records
- Link to Invoice
- Payment method (CASH, CARD, BANK_TRANSFER)
- Payment status (SUCCESS, FAILED, PENDING, CANCELED)
- Payment date and amount

---

## 🔑 KEY FEATURES IMPLEMENTED

### ✅ XML Documentation
- Mọi entity class có summary comments
- Mọi property có descriptive comments
- Support IntelliSense trong Visual Studio

### ✅ Type Safety
- Use `int?` cho optional foreign keys
- Use `string?` cho nullable strings
- Use `decimal` cho prices/currency
- Use `DateTime` cho dates

### ✅ Enum Usage
- Định nghĩa centralized trong `Enums.cs`
- Strong typing cho status fields
- `.ToString()` khi cần lưu vào DB

### ✅ Default Values
- `""` cho string fields (prevent null reference)
- Enum default values
- Boolean defaults (false)

### ✅ Data Integrity
- Non-nullable foreign keys (1 side của relationships)
- Nullable FK cho optional relationships
- Proper cascade rules (defined in DB)

---

## 📊 ENUM DEFINITIONS (Enums.cs)

```csharp
public enum AccountRole { ADMIN, EMPLOYEE, CUSTOMER }
public enum ProfileStatus { ACTIVE, INACTIVE, SUSPENDED }
public enum CustomerType { RETAIL, WHOLESALE }
public enum CategoryStatus { ACTIVE, INACTIVE, DISCONTINUED }
public enum BatchStatus { ACTIVE, EXPIRED, BLOCKED, DEPLETED }
public enum StockMovementType { IMPORT, EXPORT, ADJUST, TRANSFER }
public enum OrderStatus { NEW, CONFIRMED, PROCESSING, SHIPPED, COMPLETED, CANCELED }
public enum InvoiceStatus { NEW, PAID, PARTIAL, CANCELED }
public enum PaymentStatus { SUCCESS, FAILED, PENDING, CANCELED }
```

---

## 🚀 NEXT STEPS: BASE REPOSITORY IMPLEMENTATION

Đã hoàn thiện tất cả entities. Tiếp theo:

1. ✅ **Entities**: COMPLETED
2. ⏳ **Base Repository**: IN PROGRESS
   - [ ] IBaseRepository<T> interface
   - [ ] BaseRepository<T> implementation
   - [ ] IUnitOfWork interface
   - [ ] UnitOfWork implementation
   - [ ] DI Setup in Program.cs
3. ⏳ **Specific Repositories**
   - [ ] AccountRepositoryImpl
   - [ ] ProductRepositoryImpl
   - [ ] InventoryRepositoryImpl
   - [ ] OrderRepositoryImpl
4. ⏳ **Services Layer** (sử dụng repositories)
5. ⏳ **Controllers** (sử dụng services)

---

## 📝 CONVENTIONS USED (Junior Mindset)

### Naming:
- ✅ PascalCase cho class names
- ✅ camelCase cho properties/methods
- ✅ Meaningful names (GetByIdAsync, not GetById)
- ✅ ENTITY_FIELD_NAMES match Database column names

### Documentation:
- ✅ XML comments (///) cho public members
- ✅ Summary tags cho clarity
- ✅ Param/Returns description

### Coding Style:
- ✅ Async/await throughout
- ✅ Null-safety (string? vs string)
- ✅ LINQ where appropriate
- ✅ DRY (Don't Repeat Yourself)

---

## ✨ QUALITY CHECKLIST

- [x] All entities created with proper properties
- [x] All entities inherit from BaseEntity
- [x] XML documentation on all classes
- [x] XML documentation on all properties
- [x] Correct data types (int, string, decimal, DateTime)
- [x] Proper nullable annotations (?)
- [x] Default values where needed
- [x] Enum references using strong typing
- [x] Foreign key properties named correctly
- [x] Status fields using enum defaults

---

**Created**: January 6, 2026
**Total Entities**: 15
**Status**: ✅ COMPLETE

---

## 📞 QUICK REFERENCE

**To use an entity:**
```csharp
// Create
var newProduct = new Product 
{ 
    Code = "MED-001",
    Name = "Aspirin",
    CategoryId = 1,
    StandardPrice = 5000m,
    Status = CategoryStatus.ACTIVE.ToString()
};

// Access enum
var roleString = AccountRole.ADMIN.ToString(); // "ADMIN"

// Check status
if (product.Status == CategoryStatus.ACTIVE.ToString()) { }
```

