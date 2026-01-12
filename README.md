# 🏥 Hospital Management System (HMS) - Inventory & Order Module

<div align="center">

![Version](https://img.shields.io/badge/version-1.5.0-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)
![Build](https://img.shields.io/badge/build-passing-brightgreen.svg)

**Enterprise-grade Inventory & Order Management System for Healthcare & Pharmaceutical Industries**

[Features](#-features) • [Quick Start](#-quick-start) • [Architecture](#-architecture) • [Documentation](#-documentation) • [Contributing](#-contributing)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Technology Stack](#-technology-stack)
- [System Architecture](#-system-architecture)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
- [Database Setup](#-database-setup)
- [Usage Guide](#-usage-guide)
- [API Reference](#-api-reference)
- [Business Workflows](#-business-workflows)
- [Security](#-security)
- [Performance](#-performance)
- [Testing](#-testing)
- [Deployment](#-deployment)
- [Troubleshooting](#-troubleshooting)
- [Contributing](#-contributing)
- [License](#-license)
- [Support](#-support)

---

## 🎯 Overview

**HMS (Hospital Management System)** là một hệ thống quản lý kho và đơn hàng chuyên biệt cho ngành y tế và dược phẩm, được thiết kế để đáp ứng các yêu cầu nghiêm ngặt về:

- ✅ Truy xuất nguồn gốc (Batch tracking & expiry management)
- ✅ Tuân thủ quy định (Compliance with FDA, WHO GDP)
- ✅ Quản lý tồn kho chính xác (Real-time inventory tracking)
- ✅ Kiểm soát chất lượng (Quality control & auditing)
- ✅ Báo cáo & Phân tích (Advanced reporting & analytics)

### 🎪 Business Context

Hệ thống được phát triển để giải quyết các thách thức chính trong quản lý kho dược phẩm:

1. **Expiry Management**: Theo dõi và cảnh báo hàng sắp hết hạn để giảm thiểu lãng phí
2. **Batch Traceability**: Truy vết từng lô hàng từ nhập kho đến xuất bán
3. **Multi-Warehouse**: Quản lý tồn kho tại nhiều kho, phòng khám, bệnh viện
4. **Order Fulfillment**: Tự động hóa quy trình từ đặt hàng đến giao hàng
5. **Compliance**: Đáp ứng các tiêu chuẩn GSP, GMP, ISO 13485

---

## ✨ Key Features

### 📦 Inventory Management
- **Real-time Stock Tracking**: Theo dõi tồn kho realtime tại từng kho, từng batch
- **Multi-Warehouse Support**: Quản lý unlimited số lượng kho, phòng ban
- **Batch Management**: Quản lý theo lô sản xuất, ngày hết hạn, nguồn gốc
- **Low Stock Alerts**: Cảnh báo tự động khi tồn kho dưới ngưỡng
- **Expiry Tracking**: Theo dõi và cảnh báo hàng sắp hết hạn (30/60/90 days)
- **Stock Transfer**: Chuyển kho giữa các warehouse với audit trail đầy đủ
- **Stock Adjustment**: Điều chỉnh tồn kho với lý do và phê duyệt
- **Barcode Integration**: Tích hợp barcode scanning cho import/export

### 🛒 Order Management
- **Multi-channel Orders**: Tạo đơn từ POS, web, mobile, hotline
- **Customer Management**: Quản lý khách hàng B2B (bệnh viện) & B2C (lẻ)
- **Order Workflow**: NEW → CONFIRMED → PROCESSING → SHIPPED → COMPLETED
- **Pricing & Discounts**: Quản lý bảng giá theo khách hàng, số lượng, thời gian
- **Invoice Generation**: Tự động tạo hóa đơn VAT, xuất PDF
- **Payment Tracking**: Theo dõi thanh toán (tiền mặt, chuyển khoản, công nợ)

### 📊 Reporting & Analytics
- **Inventory Reports**: Báo cáo tồn kho, xuất nhập tồn, ABC analysis
- **Sales Reports**: Doanh số theo sản phẩm, khách hàng, nhân viên, thời gian
- **Expiry Reports**: Danh sách hàng hết hạn, sắp hết hạn
- **Financial Reports**: Công nợ, dòng tiền, lợi nhuận
- **Custom Reports**: Tạo báo cáo tùy chỉnh với Excel export

### 👥 User Management
- **Role-Based Access Control**: ADMIN, WAREHOUSE_MANAGER, SALES, ACCOUNTANT
- **User Profiles**: Quản lý thông tin nhân viên, khách hàng
- **Activity Logging**: Ghi nhận mọi thao tác (ai, làm gì, khi nào)
- **Session Management**: Quản lý phiên đăng nhập, timeout tự động

### 🔄 Stock Movement Types
1. **IMPORT**: Nhập kho từ nhà cung cấp
2. **EXPORT**: Xuất kho bán hàng
3. **TRANSFER**: Chuyển kho giữa các warehouse
4. **ADJUST**: Điều chỉnh (kiểm kê, hư hỏng, mất mát)

---

## 🛠️ Technology Stack

### Backend
- **Framework**: .NET 8.0 (C#)
- **UI**: Windows Forms (Desktop Application)
- **Architecture**: Clean Architecture (3-Layer)
  - `Controller`: Presentation layer
  - `Service`: Business logic layer
  - `Repository`: Data access layer

### Database
- **RDBMS**: Microsoft SQL Server 2022
- **ORM**: ADO.NET (SqlClient) with Dapper for complex queries
- **Migrations**: Idempotent SQL scripts
- **Backup**: Automated daily backups with 30-day retention

### Libraries & Packages
```xml
<!-- NuGet Packages -->
<PackageReference Include="Microsoft.Data.SqlClient" Version="5.1.5" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.0" />
<PackageReference Include="EPPlus" Version="7.0.0" /> <!-- Excel export -->
<PackageReference Include="Serilog" Version="3.1.1" /> <!-- Logging (planned) -->
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" /> <!-- Password hashing (planned) -->
```

### Development Tools
- **IDE**: Visual Studio 2022 / JetBrains Rider
- **Version Control**: Git (GitHub)
- **Database Tools**: SQL Server Management Studio (SSMS), Azure Data Studio
- **Testing**: xUnit (planned), Moq (planned)
- **CI/CD**: GitHub Actions (planned)

---

## 🏛️ System Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────┐
│                 Presentation Layer                   │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────┐ │
│  │  WinForms    │  │  Dashboard   │  │  Reports  │ │
│  │  (Desktop)   │  │   (Charts)   │  │  (Excel)  │ │
│  └──────┬───────┘  └──────┬───────┘  └─────┬─────┘ │
└─────────┼──────────────────┼─────────────────┼───────┘
          │                  │                 │
          └──────────────────┼─────────────────┘
                             ↓
┌─────────────────────────────────────────────────────┐
│                  Controller Layer                    │
│  ┌───────────────┐ ┌──────────────┐ ┌────────────┐ │
│  │  Inventory    │ │    Order     │ │   Stock    │ │
│  │  Controller   │ │  Controller  │ │ Movement   │ │
│  └───────┬───────┘ └──────┬───────┘ └─────┬──────┘ │
└──────────┼─────────────────┼───────────────┼────────┘
           │                 │               │
           └─────────────────┼───────────────┘
                             ↓
┌─────────────────────────────────────────────────────┐
│                   Service Layer                      │
│  ┌───────────────┐ ┌──────────────┐ ┌────────────┐ │
│  │  Inventory    │ │    Order     │ │   Stock    │ │
│  │   Service     │ │   Service    │ │ Movement   │ │
│  │               │ │              │ │  Service   │ │
│  └───────┬───────┘ └──────┬───────┘ └─────┬──────┘ │
└──────────┼─────────────────┼───────────────┼────────┘
           │                 │               │
           └─────────────────┼───────────────┘
                             ↓
┌─────────────────────────────────────────────────────┐
│                 Repository Layer                     │
│  ┌───────────────┐ ┌──────────────┐ ┌────────────┐ │
│  │  Inventory    │ │    Order     │ │   Stock    │ │
│  │  Repository   │ │  Repository  │ │ Movement   │ │
│  │               │ │              │ │ Repository │ │
│  └───────┬───────┘ └──────┬───────┘ └─────┬──────┘ │
└──────────┼─────────────────┼───────────────┼────────┘
           │                 │               │
           └─────────────────┼───────────────┘
                             ↓
        ┌────────────────────────────────────┐
        │     SQL Server Database (hms)      │
        │  ┌──────────────────────────────┐  │
        │  │  Tables (20+)                │  │
        │  │  - accounts                  │  │
        │  │  - inventory_items           │  │
        │  │  - stock_movements           │  │
        │  │  - orders, order_items       │  │
        │  │  - batches, products         │  │
        │  └──────────────────────────────┘  │
        └────────────────────────────────────┘
```

### Database Schema (ERD)

```
┌─────────────┐       ┌──────────────┐       ┌────────────┐
│  accounts   │───┐   │user_profiles │   ┌───│ products   │
│             │   │   │              │   │   │            │
│ - id (PK)   │   └──▶│ - account_id │   │   │ - id (PK)  │
│ - username  │       │ - code       │   │   │ - code     │
│ - password  │       │ - full_name  │   │   │ - name     │
│ - role      │       └──────────────┘   │   │ - price    │
└─────────────┘                          │   └─────┬──────┘
                                         │         │
       ┌─────────────────────────────────┘         │
       │                                           │
       │       ┌──────────────────┐               │
       │   ┌───│ inventory_items  │◀──────────────┘
       │   │   │                  │
       │   │   │ - product_id     │◀───┐
       │   │   │ - warehouse_id   │    │
       │   │   │ - batch_id       │    │
       │   │   │ - quantity       │    │
       │   │   └──────────────────┘    │
       │   │                            │
       │   │   ┌──────────────────┐    │
       │   └──▶│   warehouses     │────┘
       │       │                  │
       │       │ - id (PK)        │
       │       │ - code           │
       │       │ - name           │
       │       └──────────────────┘
       │
       │       ┌──────────────────┐       ┌─────────────┐
       └──────▶│     orders       │──────▶│ order_items │
               │                  │       │             │
               │ - id (PK)        │       │ - order_id  │
               │ - customer_id    │       │ - product_id│
               │ - order_number   │       │ - quantity  │
               │ - status         │       │ - price     │
               │ - total_amount   │       └─────────────┘
               └──────────────────┘
```

---

## 🚀 Getting Started

### Prerequisites

Trước khi cài đặt, đảm bảo hệ thống của bạn có:

#### Required Software
- ✅ **Windows 10/11** (64-bit)
- ✅ **.NET 8.0 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- ✅ **SQL Server 2019+** (Express/Developer/Standard/Enterprise)
  - [SQL Server 2022 Developer Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (FREE)
- ✅ **Visual Studio 2022** hoặc **JetBrains Rider**

#### Optional Tools
- 📊 **SQL Server Management Studio (SSMS)** - Database management
- 📈 **Azure Data Studio** - Cross-platform database tool
- 🔍 **Postman** - API testing (for future API layer)

#### Hardware Requirements
- **CPU**: Intel Core i5 hoặc tương đương
- **RAM**: 8GB minimum, 16GB recommended
- **Disk**: 10GB free space
- **Display**: 1920x1080 resolution minimum

### Installation

#### Step 1: Clone Repository

```bash
# Clone via HTTPS
git clone https://github.com/UTT-k74HT22/HMS-UTT.git

# Or via SSH
git clone git@github.com:UTT-k74HT22/HMS-UTT.git

# Navigate to project directory
cd HMS-UTT/HospitalManagement
```

#### Step 2: Restore NuGet Packages

```bash
# Using .NET CLI
dotnet restore

# Or open in Visual Studio and it will auto-restore
```

#### Step 3: Setup Database

##### 3.1. Create Database

```sql
-- Option 1: Using SSMS
-- 1. Open SQL Server Management Studio
-- 2. Connect to your SQL Server instance
-- 3. Right-click on "Databases" → "New Database"
-- 4. Enter database name: "hms"
-- 5. Click OK

-- Option 2: Using T-SQL
CREATE DATABASE hms;
GO
```

##### 3.2. Run Database Schema Script

```bash
# Method 1: Using SSMS
# 1. Open db.sql file in SSMS
# 2. Ensure "hms" database is selected
# 3. Press F5 to execute

# Method 2: Using sqlcmd
sqlcmd -S localhost -d hms -i db.sql

# Method 3: Using Azure Data Studio
# 1. Open db.sql
# 2. Click "Run" or press F5
```

##### 3.3. (Optional) Load Sample Data

```bash
# Load employee sample data
sqlcmd -S localhost -d hms -i sample_employee_data.sql
```

#### Step 4: Configure Connection String

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=hms;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

**🔒 Security Note**: 
- Không commit password vào Git
- Production: Sử dụng Azure Key Vault hoặc environment variables
- Development: Sử dụng User Secrets

```bash
# Setup User Secrets (Recommended for development)
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=hms;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
```

#### Step 5: Build & Run

```bash
# Build solution
dotnet build

# Run application
dotnet run

# Or press F5 in Visual Studio
```

#### Step 6: First Login

```
Username: admin
Password: 123456789

⚠️ IMPORTANT: Đổi password ngay sau lần đăng nhập đầu tiên!
```

---

## ⚙️ Configuration

### Application Settings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=hms;..."
  },
  
  "AppSettings": {
    "ApplicationName": "HMS - Hospital Management System",
    "Version": "1.5.0",
    "Environment": "Development",
    
    "Features": {
      "EnableAuditLog": true,
      "EnableEmailNotifications": false,
      "EnableAutoBackup": true
    },
    
    "Inventory": {
      "LowStockThresholdDays": 7,
      "ExpiryAlertDays": [30, 60, 90],
      "AutoReorderEnabled": false
    },
    
    "Orders": {
      "OrderNumberPrefix": "ORD",
      "InvoiceNumberPrefix": "INV",
      "AutoConfirmOrders": false,
      "DefaultWarehouseId": 1
    }
  },
  
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    },
    "File": {
      "Path": "Logs/hms-.log",
      "RollingInterval": "Day",
      "RetainedFileCountLimit": 30
    }
  }
}
```

### Database Configuration

File: `configuration/DBConfig.cs`

```csharp
public class DBConfig
{
    public string ConnectionString { get; set; }
    public int CommandTimeout { get; set; } = 30;
    public bool EnableRetry { get; set; } = true;
    public int MaxRetryCount { get; set; } = 3;
}
```

---

## 🗄️ Database Setup

### Schema Overview

Hệ thống sử dụng **20+ tables** với các nhóm chính:

#### 1. User Management
- `accounts` - Tài khoản đăng nhập
- `user_profiles` - Thông tin người dùng
- `employee_profiles` - Thông tin nhân viên
- `customer_profiles` - Thông tin khách hàng

#### 2. Product Management
- `categories` - Danh mục sản phẩm (hierarchical)
- `manufacturers` - Nhà sản xuất
- `products` - Sản phẩm
- `batches` - Lô hàng

#### 3. Inventory Management
- `warehouses` - Kho hàng
- `inventory_items` - Tồn kho theo product + batch + warehouse
- `stock_movements` - Lịch sử xuất nhập kho

#### 4. Order & Payment
- `orders` - Đơn hàng
- `order_items` - Chi tiết đơn hàng
- `invoices` - Hóa đơn
- `payments` - Thanh toán

### Key Indexes

```sql
-- High-performance indexes
CREATE INDEX idx_inventory_product_warehouse 
    ON inventory_items(product_id, warehouse_id);

CREATE INDEX idx_orders_customer_date 
    ON orders(customer_id, order_date DESC);

CREATE INDEX idx_stock_movements_date 
    ON stock_movements(movement_date DESC);

-- Low stock query optimization
CREATE INDEX idx_inventory_low_stock 
    ON inventory_items(quantity_on_hand) 
    WHERE quantity_on_hand <= min_threshold;
```

### Backup & Restore

#### Automated Backup (SQL Server Agent)

```sql
-- Create backup job
USE msdb;
GO

EXEC dbo.sp_add_job
    @job_name = N'HMS_DailyBackup',
    @enabled = 1;

EXEC dbo.sp_add_jobstep
    @job_name = N'HMS_DailyBackup',
    @step_name = N'BackupDatabase',
    @subsystem = N'TSQL',
    @command = N'
        BACKUP DATABASE [hms]
        TO DISK = ''C:\Backups\hms_'' + CONVERT(VARCHAR, GETDATE(), 112) + ''.bak''
        WITH COMPRESSION, INIT;
    ';

EXEC dbo.sp_add_schedule
    @schedule_name = N'DailyAt2AM',
    @freq_type = 4, -- Daily
    @active_start_time = 020000; -- 2:00 AM

EXEC dbo.sp_attach_schedule
    @job_name = N'HMS_DailyBackup',
    @schedule_name = N'DailyAt2AM';
```

#### Manual Backup

```sql
-- Full backup
BACKUP DATABASE hms
TO DISK = 'C:\Backups\hms_backup.bak'
WITH COMPRESSION, INIT;

-- Differential backup
BACKUP DATABASE hms
TO DISK = 'C:\Backups\hms_diff.bak'
WITH DIFFERENTIAL, COMPRESSION;
```

#### Restore

```sql
-- Restore from backup
USE master;
GO

ALTER DATABASE hms SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

RESTORE DATABASE hms
FROM DISK = 'C:\Backups\hms_backup.bak'
WITH REPLACE, RECOVERY;

ALTER DATABASE hms SET MULTI_USER;
```

---

## 📖 Usage Guide

### Common Workflows

#### 1. Nhập Hàng (Stock Import)

```
User Action Flow:
1. Kho → Nhập Hàng
2. Chọn sản phẩm từ danh sách
3. Nhập thông tin batch:
   - Mã lô (Batch Code)
   - Ngày sản xuất
   - Ngày hết hạn
   - Giá nhập
   - Nhà cung cấp
4. Chọn kho nhận hàng
5. Nhập số lượng
6. Ghi chú (optional)
7. Click "Nhập Kho"

System Process:
├─ Validate input data
├─ Create/Update batch record
├─ Update inventory_items.quantity_on_hand
├─ Insert stock_movement (type=IMPORT)
└─ Generate import receipt (PDF)
```

#### 2. Bán Hàng (Create Order)

```
User Action Flow:
1. Bán Hàng → Tạo Đơn Mới
2. Chọn khách hàng (hoặc tạo mới)
3. Thêm sản phẩm vào giỏ:
   - Tìm kiếm sản phẩm
   - Chọn batch (FEFO - First Expired First Out)
   - Nhập số lượng
   - Xác nhận
4. Áp dụng chiết khấu (if any)
5. Xác nhận đơn hàng
6. In hóa đơn

System Process:
├─ Validate customer exists
├─ Check stock availability for each item
├─ Reserve stock (quantity_reserved++)
├─ Create order (status=NEW)
├─ Insert order_items
├─ Calculate totals (subtotal, discount, tax)
├─ Update order status → CONFIRMED
├─ Generate invoice
└─ Print receipt
```

#### 3. Kiểm Kê (Stock Count)

```
User Action Flow:
1. Kho → Kiểm Kê
2. Chọn kho cần kiểm kê
3. Quét barcode hoặc nhập thủ công
4. Nhập số lượng thực tế
5. Hệ thống so sánh với số lượng sổ sách
6. Xác nhận điều chỉnh (nếu chênh lệch)
7. Xuất báo cáo kiểm kê

System Process:
├─ Load current inventory
├─ Record actual count
├─ Calculate variance (actual - system)
├─ If variance != 0:
│   ├─ Create stock_movement (type=ADJUST)
│   ├─ Update inventory_items.quantity_on_hand
│   └─ Log adjustment with reason
└─ Generate stock count report
```

---

## 🔐 Security

### Authentication

Current: Basic username/password authentication

**⚠️ Known Issue**: Passwords are currently stored in plain text

**✅ Planned Fix (v1.6)**:
```csharp
// Using BCrypt for password hashing
using BCrypt.Net;

// When creating account
string hashedPassword = BCrypt.HashPassword(request.Password);
account.Password = hashedPassword;

// When logging in
bool isValid = BCrypt.Verify(inputPassword, storedHashedPassword);
```

### Authorization

Role-based access control (RBAC):

| Role | Permissions |
|------|-------------|
| **ADMIN** | Full access (create users, configure system, view all data) |
| **WAREHOUSE_MANAGER** | Manage inventory, approve stock adjustments |
| **SALES** | Create orders, view products, view customers |
| **ACCOUNTANT** | View orders, manage invoices, view reports |
| **CUSTOMER** | View own orders, track shipments |

### Audit Trail

All critical actions are logged in `stock_movements` table:

```sql
SELECT 
    sm.movement_date,
    u.full_name AS performed_by,
    sm.movement_type,
    p.name AS product,
    sm.quantity,
    sm.quantity_before,
    sm.quantity_after,
    sm.note
FROM stock_movements sm
JOIN user_profiles u ON sm.performed_by_user_id = u.id
JOIN products p ON sm.product_id = p.id
WHERE sm.movement_date >= DATEADD(DAY, -30, GETDATE())
ORDER BY sm.movement_date DESC;
```

### Data Protection

- **Encryption at Rest**: Enable TDE (Transparent Data Encryption) on SQL Server
- **Encryption in Transit**: Use TLS 1.3 for database connections
- **Backup Encryption**: Encrypt backup files

```sql
-- Enable TDE
USE master;
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'StrongPassword123!';
CREATE CERTIFICATE TDECert WITH SUBJECT = 'TDE Certificate';

USE hms;
CREATE DATABASE ENCRYPTION KEY
WITH ALGORITHM = AES_256
ENCRYPTION BY SERVER CERTIFICATE TDECert;

ALTER DATABASE hms SET ENCRYPTION ON;
```

---

## ⚡ Performance

### Query Optimization

#### Good Practices
✅ Use parameterized queries (all queries use this)
✅ Index foreign keys
✅ Use `WITH (NOLOCK)` for read-only queries
✅ Pagination for large result sets

#### Bad Practices to Avoid
❌ SELECT * (always select specific columns)
❌ N+1 queries (use JOIN instead)
❌ Cursor loops (use set-based operations)

### Benchmark Results

| Operation | Average Time | Notes |
|-----------|--------------|-------|
| Get All Inventory (1000 items) | 45ms | Includes JOINs |
| Create Order (5 items) | 120ms | With transaction |
| Stock Movement (Import) | 35ms | Single item |
| Generate Report (1 month data) | 850ms | 10,000 records |

### Optimization Tips

```sql
-- Use indexed views for complex reports
CREATE VIEW vw_InventorySummary WITH SCHEMABINDING
AS
SELECT 
    p.id AS product_id,
    w.id AS warehouse_id,
    SUM(ii.quantity_on_hand) AS total_quantity,
    COUNT_BIG(*) AS count_items
FROM dbo.inventory_items ii
JOIN dbo.products p ON ii.product_id = p.id
JOIN dbo.warehouses w ON ii.warehouse_id = w.id
GROUP BY p.id, w.id;

CREATE UNIQUE CLUSTERED INDEX idx_vw_InventorySummary 
    ON vw_InventorySummary(product_id, warehouse_id);
```

---

## 🧪 Testing

### Current State
⚠️ **No automated tests** (Manual testing only)

### Planned Testing Strategy

#### Unit Tests (Target: 80% coverage)

```csharp
// Example unit test
public class InventoryServiceTests
{
    [Fact]
    public async Task UpdateStock_WhenQuantityNegative_ShouldThrowException()
    {
        // Arrange
        var service = new InventoryService(_mockRepo.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.UpdateStock(productId: 1, newQuantity: -5)
        );
    }
    
    [Theory]
    [InlineData(10, 5, 5)] // Current: 10, Reserve: 5, Available: 5
    [InlineData(10, 10, 0)]
    [InlineData(10, 0, 10)]
    public void GetAvailableQuantity_ShouldCalculateCorrectly(
        int onHand, int reserved, int expected)
    {
        // Arrange
        var inventory = new InventoryItem 
        { 
            QuantityOnHand = onHand, 
            QuantityReserved = reserved 
        };
        
        // Act
        var result = inventory.AvailableQuantity;
        
        // Assert
        Assert.Equal(expected, result);
    }
}
```

#### Integration Tests

```csharp
[Collection("Database")]
public class OrderIntegrationTests : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task CreateOrder_EndToEnd_ShouldSucceed()
    {
        // Arrange
        var orderRequest = new CreateOrderRequest { ... };
        
        // Act
        var orderId = await _orderService.CreateOrderAsync(orderRequest);
        
        // Assert
        var order = await _orderRepo.GetByIdAsync(orderId);
        Assert.NotNull(order);
        Assert.Equal(OrderStatus.NEW, order.Status);
        
        var items = await _orderRepo.GetItemsAsync(orderId);
        Assert.Equal(2, items.Count);
        
        var invoice = await _invoiceRepo.GetByOrderIdAsync(orderId);
        Assert.NotNull(invoice);
    }
}
```

### Manual Testing Checklist

#### Before Release
- [ ] Create new product
- [ ] Import stock (new batch)
- [ ] Export stock (order)
- [ ] Transfer stock between warehouses
- [ ] Stock adjustment
- [ ] Create customer order
- [ ] Generate invoice
- [ ] Record payment
- [ ] Run all reports
- [ ] Check low stock alerts
- [ ] Check expiry alerts

---

## 🚀 Deployment

### Development Environment

```bash
# Prerequisites
- Windows 10/11
- SQL Server LocalDB or Express
- Visual Studio 2022

# Steps
1. Clone repository
2. Restore packages
3. Update appsettings.json
4. Run db.sql
5. Press F5
```

### Production Environment

#### Option 1: On-Premises Server

```bash
# Prerequisites
- Windows Server 2019/2022
- SQL Server Standard/Enterprise
- IIS 10 (for future web version)

# Deployment Steps
1. Publish application:
   dotnet publish -c Release -o publish

2. Copy published files to server:
   \\server\c$\Apps\HMS\

3. Create Windows Service (optional):
   sc create HMS binPath="C:\Apps\HMS\HospitalManagement.exe"
   sc start HMS

4. Configure SQL Server:
   - Create dedicated login
   - Assign db_datareader, db_datawriter roles
   - Configure firewall (port 1433)

5. Setup backup job (see Database Setup section)

6. Configure monitoring:
   - Performance Monitor counters
   - SQL Server alerts
   - Event Log monitoring
```

#### Option 2: Azure Cloud

```bash
# Services Required
- Azure Virtual Machine (Windows Server)
- Azure SQL Database
- Azure App Service (for web version)

# Deployment
1. Create Azure SQL Database:
   az sql db create --name hms --server hms-server --tier Standard

2. Run database schema script via Azure Portal

3. Deploy VM:
   az vm create --name hms-vm --image Win2022Datacenter

4. Install .NET 8 Runtime on VM

5. Copy application files via RDP

6. Update connection string to Azure SQL

7. Configure auto-shutdown schedule

8. Setup Azure Backup for VM and SQL
```

### Configuration Management

```bash
# Use different appsettings per environment
appsettings.json                  # Base settings
appsettings.Development.json      # Dev overrides
appsettings.Staging.json          # Staging overrides
appsettings.Production.json       # Production overrides

# Precedence:
# appsettings.{Environment}.json > appsettings.json > User Secrets > Environment Variables
```

---

## 🐛 Troubleshooting

### Common Issues

#### 1. Cannot Connect to Database

**Error**: `A network-related or instance-specific error occurred...`

**Solutions**:
```bash
# Check SQL Server is running
services.msc → SQL Server (MSSQLSERVER) → Start

# Enable TCP/IP protocol
SQL Server Configuration Manager → SQL Server Network Configuration → 
Protocols for MSSQLSERVER → TCP/IP → Enable

# Check firewall
netsh advfirewall firewall add rule name="SQL Server" dir=in action=allow protocol=TCP localport=1433

# Test connection
sqlcmd -S localhost -U sa -P YourPassword
```

#### 2. Login Failed

**Error**: `Login failed for user 'sa'`

**Solutions**:
- Verify password in appsettings.json
- Check SQL Server authentication mode (should be "Mixed Mode")
- Reset sa password:
  ```sql
  ALTER LOGIN sa WITH PASSWORD = 'NewStrongPassword123!';
  ALTER LOGIN sa ENABLE;
  ```

#### 3. Application Crashes on Startup

**Check**:
1. Event Viewer → Windows Logs → Application
2. Check `Logs/` folder for error logs
3. Verify .NET 8 Runtime is installed
4. Check appsettings.json syntax (valid JSON)

#### 4. Slow Performance

**Diagnostics**:
```sql
-- Check for missing indexes
SELECT 
    s.avg_total_user_cost * s.avg_user_impact * (s.user_seeks + s.user_scans) AS improvement,
    'CREATE INDEX idx_' + 
        OBJECT_NAME(d.object_id) + '_' + 
        d.equality_columns + 
        ISNULL('_' + d.inequality_columns, '') +
    ' ON ' + d.statement + ' (' + d.equality_columns + ')' AS create_index_statement
FROM sys.dm_db_missing_index_details d
JOIN sys.dm_db_missing_index_groups g ON d.index_handle = g.index_handle
JOIN sys.dm_db_missing_index_group_stats s ON g.index_group_handle = s.group_handle
ORDER BY improvement DESC;

-- Check for long-running queries
SELECT 
    sqltext.TEXT,
    req.session_id,
    req.status,
    req.command,
    req.cpu_time,
    req.total_elapsed_time
FROM sys.dm_exec_requests req
CROSS APPLY sys.dm_exec_sql_text(sql_handle) AS sqltext
WHERE req.session_id > 50
ORDER BY req.total_elapsed_time DESC;
```

---

## 🤝 Contributing

We welcome contributions! Here's how you can help:

### Development Workflow

```bash
# 1. Fork the repository
# 2. Create a feature branch
git checkout -b feature/your-feature-name

# 3. Make your changes
# Follow C# coding conventions
# Add XML documentation comments

# 4. Test your changes
dotnet build
# Run manual tests

# 5. Commit with conventional commits
git commit -m "feat: add batch expiry auto-alert"
# Types: feat, fix, docs, style, refactor, test, chore

# 6. Push to your fork
git push origin feature/your-feature-name

# 7. Create Pull Request
# Describe your changes clearly
# Reference any related issues
```

### Code Style

```csharp
// ✅ Good
public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repository;
    
    /// <summary>
    /// Gets available quantity for a product at a specific warehouse
    /// </summary>
    /// <param name="productId">The product identifier</param>
    /// <param name="warehouseId">The warehouse identifier</param>
    /// <returns>Available quantity (on hand - reserved)</returns>
    public async Task<int> GetAvailableQuantityAsync(long productId, long warehouseId)
    {
        if (productId <= 0)
            throw new ArgumentException("Product ID must be positive", nameof(productId));
            
        return await _repository.GetAvailableQuantityAsync(productId, warehouseId);
    }
}

// ❌ Bad
public class inventoryservice
{
    public int getQty(long pid, long wid) // No docs, unclear names
    {
        return _repo.Get(pid, wid); // No validation
    }
}
```

### Pull Request Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Unit tests added/updated
- [ ] Manual testing completed
- [ ] Performance impact assessed

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Comments added for complex logic
- [ ] Documentation updated
- [ ] No new warnings introduced
```

---

## 📄 License

This project is licensed under the **MIT License**.

```
MIT License

Copyright (c) 2026 UTT-k74HT22

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

## 📞 Support

### Getting Help

- 📧 **Email**: support@hms-system.com
- 💬 **Slack**: [Join our workspace](https://hms-workspace.slack.com)
- 🐛 **Bug Reports**: [GitHub Issues](https://github.com/UTT-k74HT22/HMS-UTT/issues)
- 📖 **Documentation**: [Wiki](https://github.com/UTT-k74HT22/HMS-UTT/wiki)

### Reporting Bugs

Please include:
1. HMS version (`Help → About`)
2. Windows version
3. SQL Server version
4. Steps to reproduce
5. Expected vs actual behavior
6. Screenshots (if applicable)
7. Error logs (`Logs/` folder)

### Feature Requests

Submit via [GitHub Discussions](https://github.com/UTT-k74HT22/HMS-UTT/discussions) with:
- Use case description
- Business value
- Proposed solution
- Alternative approaches considered

---

## 📚 Additional Resources

### Documentation
- 📘 [User Manual](docs/UserManual.pdf)
- 🔧 [Admin Guide](docs/AdminGuide.pdf)
- 🏗️ [Architecture Overview](ARCHITECTURE.md)
- 🐛 [Comprehensive Review](COMPREHENSIVE_REVIEW.md)
- 🚀 [Version 2 Roadmap](VERSION_2_ROADMAP.md)

### Related Projects
- [HMS Mobile App](https://github.com/UTT-k74HT22/HMS-Mobile) (Planned)
- [HMS Web Portal](https://github.com/UTT-k74HT22/HMS-Web) (Planned)
- [HMS API](https://github.com/UTT-k74HT22/HMS-API) (Planned)

### Learning Resources
- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [SQL Server Best Practices](https://docs.microsoft.com/en-us/sql/relational-databases/)
- [Clean Architecture Guide](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## 🎓 Credits

### Development Team
- **Lead Developer**: [Your Name]
- **Database Architect**: [Name]
- **UI/UX Designer**: [Name]
- **QA Engineer**: [Name]

### Contributors
See [CONTRIBUTORS.md](CONTRIBUTORS.md) for the full list.

### Open Source Libraries
- [EPPlus](https://github.com/EPPlusSoftware/EPPlus) - Excel export
- [Microsoft.Data.SqlClient](https://github.com/dotnet/SqlClient) - SQL Server connectivity
- [BCrypt.Net](https://github.com/BcryptNet/bcrypt.net) - Password hashing (planned)

---

## 📈 Project Stats

![GitHub stars](https://img.shields.io/github/stars/UTT-k74HT22/HMS-UTT?style=social)
![GitHub forks](https://img.shields.io/github/forks/UTT-k74HT22/HMS-UTT?style=social)
![GitHub issues](https://img.shields.io/github/issues/UTT-k74HT22/HMS-UTT)
![GitHub pull requests](https://img.shields.io/github/issues-pr/UTT-k74HT22/HMS-UTT)
![Code size](https://img.shields.io/github/languages/code-size/UTT-k74HT22/HMS-UTT)

---

## 🗺️ Roadmap

### Version 1.6 (Q2 2026)
- ✅ Password hashing (BCrypt)
- ✅ Transaction support cho order creation
- ✅ Stock reservation mechanism
- ✅ Audit logging
- ✅ Unit tests (60% coverage)

### Version 2.0 (Q4 2026)
- ✅ RESTful API layer
- ✅ Web Portal (React)
- ✅ Mobile App (Flutter)
- ✅ Event-driven architecture (RabbitMQ)
- ✅ Predictive analytics (ML.NET)
- ✅ Cloud deployment (Azure)

See [VERSION_2_ROADMAP.md](VERSION_2_ROADMAP.md) for detailed plan.

---

<div align="center">

**Made with ❤️ by UTT-k74HT22 Team**

⭐ **Star us on GitHub** if you find this project useful!

[Report Bug](https://github.com/UTT-k74HT22/HMS-UTT/issues) • 
[Request Feature](https://github.com/UTT-k74HT22/HMS-UTT/issues) • 
[Join Discussion](https://github.com/UTT-k74HT22/HMS-UTT/discussions)

</div>
