# 🚀 HMS VERSION 2.0 - DEVELOPMENT ROADMAP

**Document Type**: Strategic Development Plan  
**Target Audience**: Development Team, Stakeholders, Project Managers  
**Timeline**: 6 months phased rollout  
**Last Updated**: January 13, 2026

---

## 🎯 VISION & OBJECTIVES

### Vision Statement
Transform HMS từ một desktop inventory management system thành một **enterprise-grade, cloud-native, scalable pharmacy & hospital supply chain platform** với khả năng:
- Real-time inventory tracking across multiple facilities
- Predictive analytics for stock optimization
- Mobile-first approach cho warehouse operations
- Integration-ready architecture
- AI-powered demand forecasting

### Business Objectives
1. ✅ **Reduce stock-outs** by 80% (từ 20% → 4%)
2. ✅ **Improve order fulfillment time** from 24h → 4h
3. ✅ **Increase inventory accuracy** from 85% → 99.5%
4. ✅ **Support 10x scale**: From 5 warehouses → 50 warehouses
5. ✅ **Reduce operational cost** by 30% through automation

---

## 📊 CURRENT STATE vs FUTURE STATE

| Aspect | Current (V1) | Target (V2) | Impact |
|--------|--------------|-------------|--------|
| **Architecture** | Monolithic WinForms | Microservices + Web/Mobile | Scalability |
| **Database** | Single SQL Server | Distributed DB + Cache | Performance |
| **Deployment** | Desktop install | Cloud-native (Azure/AWS) | Accessibility |
| **Real-time** | Batch updates | Event-driven architecture | Responsiveness |
| **Analytics** | Basic reports | BI Dashboard + Predictive ML | Insights |
| **Integration** | None | REST API + Webhooks | Ecosystem |
| **Security** | Basic auth | OAuth2 + MFA + Audit | Compliance |
| **Testing** | Manual | Automated CI/CD | Quality |

---

## 🏗️ ARCHITECTURE TRANSFORMATION

### Current Architecture (V1)
```
┌─────────────────────────────────────────┐
│         WinForms Desktop App            │
│  ┌──────────┐  ┌──────────┐            │
│  │Controller│→ │ Service  │            │
│  └──────────┘  └──────────┘            │
│                      ↓                   │
│              ┌──────────────┐           │
│              │  Repository  │           │
│              └──────────────┘           │
│                      ↓                   │
└──────────────────────┼──────────────────┘
                       ↓
              ┌─────────────────┐
              │  SQL Server     │
              └─────────────────┘
```

### Target Architecture (V2)
```
┌────────────────── API Gateway (Kong/APIM) ──────────────────┐
│                                                              │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │
│  │ Web Portal   │  │ Mobile App   │  │ Admin Panel  │     │
│  │  (React)     │  │ (Flutter)    │  │  (Blazor)    │     │
│  └──────────────┘  └──────────────┘  └──────────────┘     │
│                                                              │
└───────────────────────────┬──────────────────────────────────┘
                            ↓
┌────────────── Microservices Layer ───────────────────┐
│                                                       │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────┐ │
│  │ Inventory    │  │ Order        │  │ Auth      │ │
│  │ Service      │  │ Service      │  │ Service   │ │
│  │ (C# .NET 8)  │  │ (C# .NET 8)  │  │ (C#/.NET) │ │
│  └──────────────┘  └──────────────┘  └───────────┘ │
│                                                       │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────┐ │
│  │ Notification │  │ Analytics    │  │ Reporting │ │
│  │ Service      │  │ Service      │  │ Service   │ │
│  └──────────────┘  └──────────────┘  └───────────┘ │
│                                                       │
└────────────────────────┬──────────────────────────────┘
                         ↓
┌────────── Message Broker (RabbitMQ/Azure Service Bus) ──────┐
│  Events: OrderCreated, StockUpdated, BatchExpiring, etc.    │
└──────────────────────────────────────────────────────────────┘
                         ↓
┌──────────────── Data Layer ──────────────────────────┐
│                                                       │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────┐ │
│  │ SQL Server   │  │ Redis Cache  │  │ MongoDB   │ │
│  │ (OLTP)       │  │ (Session)    │  │ (Logs)    │ │
│  └──────────────┘  └──────────────┘  └───────────┘ │
│                                                       │
│  ┌──────────────┐  ┌──────────────┐                 │
│  │ Azure Blob   │  │ SQL DW       │                 │
│  │ (Files)      │  │ (Analytics)  │                 │
│  └──────────────┘  └──────────────┘                 │
│                                                       │
└───────────────────────────────────────────────────────┘
```

---

## 🎯 PHASED ROLLOUT PLAN

### 📅 **PHASE 1: Foundation & Critical Fixes (Month 1-2)**

**Goal**: Stabilize V1, fix critical bugs, prepare infrastructure

#### Week 1-2: Critical Bug Fixes
- [ ] **Fix Race Conditions**
  - Implement row-level locking cho inventory updates
  - Add optimistic concurrency với `rowversion`
  - Unit tests cho concurrent scenarios
  
- [ ] **Security Hardening**
  - Migrate passwords to BCrypt
  - Implement password policy (min length, complexity)
  - Add password reset functionality
  - Enable HTTPS only
  
- [ ] **Transaction Management**
  - Wrap Order creation trong transaction
  - Add compensating transactions cho failures
  - Implement distributed transaction coordinator

#### Week 3-4: Infrastructure Setup
- [ ] **DevOps Foundation**
  - Setup Azure DevOps / GitHub Actions
  - Create CI/CD pipelines
  - Containerize application (Docker)
  - Setup Kubernetes cluster (AKS/EKS)
  
- [ ] **Monitoring & Logging**
  - Integrate Serilog với structured logging
  - Setup Application Insights / New Relic
  - Configure alerts (stock low, errors, performance)
  - Create dashboards (Grafana)
  
- [ ] **Database Enhancements**
  ```sql
  -- Add audit columns
  ALTER TABLE products 
      ADD deleted_at DATETIME2 NULL,
          deleted_by INT NULL,
          row_version ROWVERSION;
  
  -- Add stock reservation
  ALTER TABLE inventory_items
      ADD quantity_allocated INT DEFAULT 0,
          quantity_available AS (quantity_on_hand - quantity_reserved - quantity_allocated);
  
  -- Create audit trail
  CREATE TABLE audit_logs (
      id BIGINT IDENTITY PRIMARY KEY,
      entity_type VARCHAR(50),
      entity_id BIGINT,
      action VARCHAR(20),
      old_values NVARCHAR(MAX),
      new_values NVARCHAR(MAX),
      user_id INT,
      ip_address VARCHAR(50),
      created_at DATETIME2 DEFAULT SYSDATETIME()
  );
  ```

#### Week 5-6: Business Logic Improvements
- [ ] **Stock Reservation System**
  ```csharp
  public class StockReservationService
  {
      public async Task<ReservationResult> ReserveStock(
          long productId, 
          long warehouseId, 
          int quantity, 
          string orderId,
          TimeSpan expirationTime)
      {
          using var transaction = await _db.BeginTransactionAsync();
          
          // 1. Lock row
          var inventory = await _db.InventoryItems
              .FromSqlRaw(@"
                  SELECT * FROM inventory_items WITH (UPDLOCK, ROWLOCK)
                  WHERE product_id = {0} AND warehouse_id = {1}
              ", productId, warehouseId)
              .FirstOrDefaultAsync();
          
          // 2. Check availability
          var available = inventory.QuantityOnHand - 
                         inventory.QuantityReserved - 
                         inventory.QuantityAllocated;
          
          if (available < quantity)
              throw new InsufficientStockException();
          
          // 3. Reserve
          inventory.QuantityReserved += quantity;
          
          // 4. Create reservation record
          var reservation = new StockReservation
          {
              ProductId = productId,
              WarehouseId = warehouseId,
              Quantity = quantity,
              OrderId = orderId,
              ExpiresAt = DateTime.UtcNow.Add(expirationTime),
              Status = ReservationStatus.Active
          };
          
          await _db.StockReservations.AddAsync(reservation);
          await _db.SaveChangesAsync();
          await transaction.CommitAsync();
          
          return new ReservationResult { ReservationId = reservation.Id };
      }
  }
  ```

- [ ] **Order Fulfillment Automation**
  - Auto-deduct inventory khi order status → SHIPPED
  - FIFO batch selection (first expiry, first out)
  - Auto-split orders nếu stock ở multiple warehouses

- [ ] **Expiry Management**
  - Background job check expiring batches daily
  - Auto-alert 90/60/30 days before expiry
  - Block sales of expired batches

#### Week 7-8: Testing & Documentation
- [ ] Unit Tests (Target: 60% coverage)
  - InventoryService tests
  - OrderService tests
  - StockMovementService tests
  
- [ ] Integration Tests
  - Order creation end-to-end
  - Stock movement workflows
  - API endpoint tests
  
- [ ] Performance Tests
  - Load test: 1000 concurrent orders
  - Stress test: Database connection pool
  - Benchmark: Query optimization

---

### 📅 **PHASE 2: API-First Architecture (Month 3)**

**Goal**: Expose RESTful APIs, enable third-party integrations

#### Week 9-10: API Development
- [ ] **Create ASP.NET Core Web API Project**
  ```csharp
  // Startup.cs
  services.AddControllers()
      .AddJsonOptions(opts => {
          opts.JsonSerializerOptions.PropertyNamingPolicy = 
              JsonNamingPolicy.CamelCase;
      });
  
  services.AddApiVersioning(opts => {
      opts.DefaultApiVersion = new ApiVersion(2, 0);
      opts.AssumeDefaultVersionWhenUnspecified = true;
      opts.ReportApiVersions = true;
  });
  
  services.AddSwaggerGen(c => {
      c.SwaggerDoc("v2", new OpenApiInfo { 
          Title = "HMS API", 
          Version = "v2.0" 
      });
      c.AddSecurityDefinition("Bearer", ...);
  });
  ```

- [ ] **API Endpoints Design**
  ```
  # Inventory APIs
  GET    /api/v2/inventory                    # List all inventory
  GET    /api/v2/inventory/{id}               # Get by ID
  GET    /api/v2/inventory/warehouse/{id}     # By warehouse
  GET    /api/v2/inventory/product/{id}       # By product
  GET    /api/v2/inventory/low-stock          # Low stock alerts
  POST   /api/v2/inventory/reserve            # Reserve stock
  DELETE /api/v2/inventory/reserve/{id}       # Release reservation
  
  # Order APIs
  GET    /api/v2/orders                       # List orders
  GET    /api/v2/orders/{id}                  # Get order
  POST   /api/v2/orders                       # Create order
  PUT    /api/v2/orders/{id}/confirm          # Confirm order
  PUT    /api/v2/orders/{id}/cancel           # Cancel order
  GET    /api/v2/orders/{id}/items            # Get order items
  
  # Stock Movement APIs
  GET    /api/v2/stock-movements              # List movements
  POST   /api/v2/stock-movements/import       # Import stock
  POST   /api/v2/stock-movements/export       # Export stock
  POST   /api/v2/stock-movements/transfer     # Transfer stock
  POST   /api/v2/stock-movements/adjust       # Adjust stock
  ```

#### Week 11-12: Authentication & Authorization
- [ ] **Implement OAuth 2.0 + JWT**
  ```csharp
  services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(opts => {
          opts.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = Configuration["Jwt:Issuer"],
              ValidAudience = Configuration["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])
              )
          };
      });
  ```

- [ ] **Role-Based Access Control (RBAC)**
  ```csharp
  [Authorize(Roles = "Admin,WarehouseManager")]
  [HttpPost("stock-movements/adjust")]
  public async Task<IActionResult> AdjustStock([FromBody] AdjustStockRequest req)
  {
      // Only admins can adjust stock
  }
  
  [Authorize(Policy = "CanViewInventory")]
  [HttpGet("inventory")]
  public async Task<IActionResult> GetInventory()
  {
      // Custom policy
  }
  ```

- [ ] **API Rate Limiting**
  ```csharp
  services.AddRateLimiter(opts => {
      opts.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
          ctx => RateLimitPartition.GetFixedWindowLimiter(
              partitionKey: ctx.User.Identity?.Name ?? ctx.Request.Headers.Host.ToString(),
              factory: partition => new FixedWindowRateLimiterOptions
              {
                  AutoReplenishment = true,
                  PermitLimit = 100,
                  QueueLimit = 0,
                  Window = TimeSpan.FromMinutes(1)
              }
          )
      );
  });
  ```

---

### 📅 **PHASE 3: Event-Driven Architecture (Month 4)**

**Goal**: Implement asynchronous processing, improve scalability

#### Week 13-14: Message Broker Setup
- [ ] **Setup RabbitMQ / Azure Service Bus**
  ```yaml
  # docker-compose.yml
  rabbitmq:
    image: rabbitmq:3-management
    ports:
      - "5672:5672"
      - "15672:15672"
    environment:
      RABBITMQ_DEFAULT_USER: admin
      RABBITMQ_DEFAULT_PASS: ${RABBITMQ_PASSWORD}
  ```

- [ ] **Define Domain Events**
  ```csharp
  // Events
  public record OrderCreatedEvent(long OrderId, long CustomerId, decimal Total);
  public record OrderConfirmedEvent(long OrderId);
  public record StockUpdatedEvent(long ProductId, long WarehouseId, int NewQuantity);
  public record BatchExpiringEvent(long BatchId, DateTime ExpiryDate);
  public record LowStockAlertEvent(long ProductId, long WarehouseId, int CurrentQty);
  ```

#### Week 15-16: Event Handlers
- [ ] **Order Confirmed → Reserve Stock**
  ```csharp
  public class OrderConfirmedHandler : IEventHandler<OrderConfirmedEvent>
  {
      public async Task Handle(OrderConfirmedEvent evt)
      {
          var order = await _orderRepo.GetByIdAsync(evt.OrderId);
          
          foreach (var item in order.Items)
          {
              await _stockService.ReserveStock(
                  item.ProductId, 
                  item.WarehouseId, 
                  item.Quantity,
                  order.Id
              );
          }
      }
  }
  ```

- [ ] **Stock Updated → Send Alerts**
  ```csharp
  public class StockUpdatedHandler : IEventHandler<StockUpdatedEvent>
  {
      public async Task Handle(StockUpdatedEvent evt)
      {
          var inventory = await _inventoryRepo.GetAsync(
              evt.ProductId, 
              evt.WarehouseId
          );
          
          if (inventory.QuantityOnHand <= inventory.MinThreshold)
          {
              await _eventBus.PublishAsync(new LowStockAlertEvent(
                  evt.ProductId,
                  evt.WarehouseId,
                  inventory.QuantityOnHand
              ));
          }
      }
  }
  ```

- [ ] **Low Stock Alert → Notify & Auto-Reorder**
  ```csharp
  public class LowStockAlertHandler : IEventHandler<LowStockAlertEvent>
  {
      public async Task Handle(LowStockAlertEvent evt)
      {
          // 1. Send notification
          await _notificationService.SendAsync(new Notification
          {
              Type = NotificationType.LowStock,
              Severity = Severity.Warning,
              Title = "Low Stock Alert",
              Message = $"Product {evt.ProductId} at warehouse {evt.WarehouseId} " +
                       $"has only {evt.CurrentQty} units remaining"
          });
          
          // 2. Auto-create purchase order (optional)
          if (_settings.AutoReorderEnabled)
          {
              await _purchaseOrderService.CreateAutoReorderAsync(
                  evt.ProductId, 
                  evt.WarehouseId
              );
          }
      }
  }
  ```

---

### 📅 **PHASE 4: Advanced Features (Month 5)**

**Goal**: AI/ML integration, advanced analytics, mobile app

#### Week 17-18: Predictive Analytics
- [ ] **Demand Forecasting Model**
  ```python
  # Python ML Service (Flask API)
  from sklearn.ensemble import RandomForestRegressor
  import pandas as pd
  
  class DemandForecaster:
      def train(self, historical_data):
          # Features: month, day_of_week, is_holiday, promotions, etc.
          X = historical_data[['month', 'day_of_week', 'is_holiday']]
          y = historical_data['quantity_sold']
          
          self.model = RandomForestRegressor(n_estimators=100)
          self.model.fit(X, y)
      
      def predict(self, product_id, days_ahead=30):
          # Predict next 30 days demand
          predictions = []
          for day in range(days_ahead):
              features = self._build_features(product_id, day)
              pred = self.model.predict([features])[0]
              predictions.append(pred)
          
          return predictions
  ```

- [ ] **Optimal Stock Level Calculator**
  ```csharp
  public class StockOptimizationService
  {
      public async Task<OptimalStockLevel> CalculateOptimalLevel(
          long productId, 
          long warehouseId)
      {
          // 1. Get historical data (6 months)
          var history = await _repo.GetSalesHistory(productId, warehouseId, 180);
          
          // 2. Calculate metrics
          var avgDailySales = history.Average(x => x.QuantitySold);
          var stdDev = CalculateStdDev(history.Select(x => x.QuantitySold));
          
          // 3. Lead time (days to reorder)
          var leadTime = 7; // days
          
          // 4. Service level (99% - want to avoid stockout 99% of time)
          var zScore = 2.33; // 99% service level
          
          // 5. Safety stock = Z × σ × √L
          var safetyStock = (int)(zScore * stdDev * Math.Sqrt(leadTime));
          
          // 6. Reorder point = (Avg daily sales × Lead time) + Safety stock
          var reorderPoint = (int)(avgDailySales * leadTime) + safetyStock;
          
          // 7. Economic Order Quantity
          var annualDemand = avgDailySales * 365;
          var orderingCost = 50; // per order
          var holdingCost = 5; // per unit per year
          var eoq = (int)Math.Sqrt((2 * annualDemand * orderingCost) / holdingCost);
          
          return new OptimalStockLevel
          {
              MinThreshold = reorderPoint,
              MaxThreshold = reorderPoint + eoq,
              SafetyStock = safetyStock,
              ReorderQuantity = eoq
          };
      }
  }
  ```

#### Week 19-20: Mobile App (Flutter)
- [ ] **Warehouse Manager Mobile App**
  ```
  Features:
  - Barcode scanning cho stock movements
  - Quick stock check
  - Receive notifications (low stock, expiry alerts)
  - Approve/Reject orders
  - Real-time inventory dashboard
  - Offline mode với sync
  ```

- [ ] **Key Screens**
  ```
  1. Dashboard
     ├─ Today's movements
     ├─ Low stock alerts
     ├─ Expiring batches
     └─ Pending approvals
  
  2. Stock Management
     ├─ Scan barcode → Quick view
     ├─ Import stock (camera + form)
     ├─ Export stock
     └─ Stock transfer
  
  3. Orders
     ├─ Pending orders list
     ├─ Order details
     └─ Fulfill order (pick list)
  
  4. Reports
     ├─ Daily summary
     ├─ Stock movement history
     └─ Warehouse performance
  ```

---

### 📅 **PHASE 5: Analytics & BI (Month 6)**

**Goal**: Data warehouse, business intelligence, executive dashboards

#### Week 21-22: Data Warehouse
- [ ] **Setup Azure Synapse / SQL Data Warehouse**
  ```sql
  -- Star Schema Design
  
  -- Fact Table
  CREATE TABLE fact_inventory_movements (
      movement_id BIGINT,
      date_key INT,
      product_key INT,
      warehouse_key INT,
      batch_key INT,
      movement_type VARCHAR(20),
      quantity INT,
      quantity_before INT,
      quantity_after INT,
      unit_cost DECIMAL(18,2),
      total_value DECIMAL(18,2)
  );
  
  -- Dimension Tables
  CREATE TABLE dim_date (
      date_key INT PRIMARY KEY,
      full_date DATE,
      year INT,
      quarter INT,
      month INT,
      week INT,
      day_of_week INT,
      is_weekend BIT,
      is_holiday BIT
  );
  
  CREATE TABLE dim_product (
      product_key INT PRIMARY KEY,
      product_id INT,
      product_code VARCHAR(50),
      product_name NVARCHAR(150),
      category_name NVARCHAR(100),
      manufacturer_name NVARCHAR(150)
  );
  
  CREATE TABLE dim_warehouse (
      warehouse_key INT PRIMARY KEY,
      warehouse_id INT,
      warehouse_code VARCHAR(50),
      warehouse_name NVARCHAR(150),
      city NVARCHAR(100),
      region NVARCHAR(100)
  );
  ```

- [ ] **ETL Pipeline (Azure Data Factory)**
  ```json
  {
    "name": "DailyInventoryETL",
    "type": "Copy",
    "source": {
      "type": "SqlServerSource",
      "sqlReaderQuery": "SELECT * FROM stock_movements WHERE created_at >= @{adddays(utcnow(), -1)}"
    },
    "sink": {
      "type": "AzureSqlDWSink",
      "writeBatchSize": 10000
    },
    "schedule": {
      "frequency": "Day",
      "interval": 1,
      "startTime": "2026-01-01T00:00:00Z"
    }
  }
  ```

#### Week 23-24: BI Dashboards
- [ ] **Power BI / Tableau Dashboards**
  
  **Executive Dashboard**:
  ```
  KPIs:
  - Total Inventory Value: $1.2M
  - Stock Turnover Ratio: 8.5
  - Stock Accuracy: 99.2%
  - Fill Rate: 96.8%
  
  Charts:
  - Inventory value trend (last 12 months)
  - Top 20 products by value
  - Warehouse utilization %
  - Stock movement heatmap
  ```
  
  **Warehouse Manager Dashboard**:
  ```
  Metrics:
  - Today's movements: 145 (↑12%)
  - Low stock items: 23 (critical: 5)
  - Expiring in 30 days: 18 batches
  - Pending orders: 34
  
  Tables:
  - Low stock alerts (sortable)
  - Expiring batches (with action buttons)
  - Slow-moving items
  - Stock accuracy by category
  ```
  
  **Procurement Dashboard**:
  ```
  Insights:
  - Recommended reorders: 15 items
  - Avg lead time: 6.2 days
  - Supplier performance
  - Cost savings opportunities
  ```

---

## 🔐 SECURITY ENHANCEMENTS (V2)

### Authentication & Authorization
- [ ] Multi-Factor Authentication (MFA)
- [ ] Single Sign-On (SSO) with Azure AD
- [ ] API Key management cho integrations
- [ ] Session management with Redis
- [ ] Password policies enforcement

### Data Security
- [ ] Encryption at rest (TDE - Transparent Data Encryption)
- [ ] Encryption in transit (TLS 1.3)
- [ ] Field-level encryption cho sensitive data
- [ ] Data masking cho PII
- [ ] Automated backup with encryption

### Compliance
- [ ] GDPR compliance (data retention, right to be forgotten)
- [ ] HIPAA compliance (audit trail, access logs)
- [ ] SOC 2 Type II requirements
- [ ] Regular security audits
- [ ] Penetration testing

---

## 📈 PERFORMANCE OPTIMIZATION (V2)

### Database Optimization
```sql
-- Partitioning cho large tables
CREATE PARTITION FUNCTION pf_StockMovements (DATETIME2)
AS RANGE RIGHT FOR VALUES 
('2025-01-01', '2025-02-01', '2025-03-01', ...);

CREATE PARTITION SCHEME ps_StockMovements
AS PARTITION pf_StockMovements
ALL TO ([PRIMARY]);

-- Columnstore index for analytics
CREATE NONCLUSTERED COLUMNSTORE INDEX nci_stock_movements_analytics
ON stock_movements (movement_date, product_id, warehouse_id, quantity)
WHERE movement_date >= '2024-01-01';

-- Filtered indexes
CREATE INDEX idx_active_orders 
ON orders(customer_id, order_date)
WHERE status IN ('NEW', 'CONFIRMED', 'PROCESSING');
```

### Caching Strategy
```csharp
// 1. Distributed Cache (Redis)
public class CachedInventoryService : IInventoryService
{
    private readonly IInventoryService _inner;
    private readonly IDistributedCache _cache;
    
    public async Task<InventoryResponse> GetByIdAsync(long id)
    {
        var cacheKey = $"inventory:{id}";
        var cached = await _cache.GetStringAsync(cacheKey);
        
        if (cached != null)
            return JsonSerializer.Deserialize<InventoryResponse>(cached);
        
        var result = await _inner.GetByIdAsync(id);
        
        await _cache.SetStringAsync(cacheKey, 
            JsonSerializer.Serialize(result),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }
        );
        
        return result;
    }
}

// 2. Memory Cache cho static data
services.AddMemoryCache();
services.AddSingleton<ICategoryCache, CategoryCache>();

public class CategoryCache : ICategoryCache
{
    private readonly IMemoryCache _cache;
    
    public async Task<List<Category>> GetAllAsync()
    {
        return await _cache.GetOrCreateAsync("categories", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
            return await _categoryRepo.GetAllAsync();
        });
    }
}
```

### Query Optimization
```csharp
// Use projection instead of full entity
public async Task<List<ProductListDto>> GetProductsAsync()
{
    return await _context.Products
        .Where(p => p.DeletedAt == null)
        .Select(p => new ProductListDto
        {
            Id = p.Id,
            Code = p.Code,
            Name = p.Name,
            CategoryName = p.Category.Name,
            StandardPrice = p.StandardPrice
        })
        .AsNoTracking()  // Read-only, no change tracking
        .ToListAsync();
}

// Batch queries instead of N+1
public async Task<List<OrderWithItemsDto>> GetOrdersWithItemsAsync()
{
    return await _context.Orders
        .Include(o => o.Items)
            .ThenInclude(i => i.Product)
        .Where(o => o.DeletedAt == null)
        .AsSplitQuery()  // Separate queries to avoid cartesian explosion
        .ToListAsync();
}
```

---

## 🧪 TESTING STRATEGY (V2)

### Test Pyramid
```
        ┌─────────────┐
        │   E2E Tests │  5%
        │   (Selenium)│
        └─────────────┘
       ┌───────────────┐
       │Integration Tests│ 15%
       │  (TestContainers)│
       └───────────────┘
     ┌──────────────────────┐
     │    Unit Tests        │ 80%
     │    (xUnit + Moq)     │
     └──────────────────────┘
```

### Unit Test Example
```csharp
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockRepo;
    private readonly Mock<IInventoryService> _mockInventory;
    private readonly OrderService _sut;
    
    [Fact]
    public async Task CreateOrder_WhenStockAvailable_ShouldSucceed()
    {
        // Arrange
        var request = new CreateOrderRequest
        {
            CustomerId = 1,
            Items = new List<OrderItemRequest>
            {
                new() { ProductId = 10, Quantity = 5 }
            }
        };
        
        _mockInventory
            .Setup(x => x.GetAvailableQuantityAsync(10, It.IsAny<long>()))
            .ReturnsAsync(10);
        
        _mockRepo
            .Setup(x => x.InsertOrderAsync(It.IsAny<Order>()))
            .ReturnsAsync(1001);
        
        // Act
        var result = await _sut.CreateOrderAsync(request);
        
        // Assert
        Assert.Equal(1001, result.OrderId);
        _mockRepo.Verify(x => x.InsertOrderAsync(It.IsAny<Order>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateOrder_WhenInsufficientStock_ShouldThrowException()
    {
        // Arrange
        _mockInventory
            .Setup(x => x.GetAvailableQuantityAsync(10, It.IsAny<long>()))
            .ReturnsAsync(3); // Only 3 available, requested 5
        
        // Act & Assert
        await Assert.ThrowsAsync<InsufficientStockException>(
            () => _sut.CreateOrderAsync(request)
        );
    }
}
```

### Integration Test Example
```csharp
public class OrderIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    [Fact]
    public async Task POST_CreateOrder_Returns201Created()
    {
        // Arrange
        var request = new CreateOrderRequest { ... };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/v2/orders", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var order = await response.Content.ReadFromJsonAsync<OrderResponse>();
        order.Id.Should().BeGreaterThan(0);
    }
}
```

---

## 🚀 DEPLOYMENT STRATEGY

### Environment Strategy
```
Development → Staging → Production
    ↓           ↓           ↓
  (Local)    (Azure)     (Azure)
             (Test Data) (Real Data)
```

### Blue-Green Deployment
```
┌────────────────────────────────────┐
│      Load Balancer / Traffic Mgr    │
│         (Azure Front Door)          │
└────────────┬───────────────────────┘
             │
      ┌──────┴──────┐
      │             │
┌─────▼─────┐ ┌─────▼─────┐
│  Blue     │ │  Green    │
│ (Current) │ │  (New)    │
│  v1.5.0   │ │  v2.0.0   │
└───────────┘ └───────────┘

Steps:
1. Deploy v2.0.0 to Green environment
2. Run smoke tests on Green
3. Route 10% traffic to Green (canary)
4. Monitor metrics for 1 hour
5. If OK: Route 100% to Green
6. If NOK: Instant rollback to Blue
```

### Kubernetes Deployment
```yaml
# deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: hms-inventory-service
spec:
  replicas: 3
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  template:
    spec:
      containers:
      - name: inventory-service
        image: hmsacr.azurecr.io/inventory-service:v2.0.0
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /health/live
            port: 8080
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: inventory-service
spec:
  type: LoadBalancer
  ports:
  - port: 80
    targetPort: 8080
  selector:
    app: inventory-service
```

---

## 💰 COST ESTIMATION (V2)

### Infrastructure Costs (Monthly)
| Service | Specs | Cost (USD) |
|---------|-------|------------|
| Azure Kubernetes Service | 3 nodes (D4s_v3) | $480 |
| Azure SQL Database | Business Critical, 4 vCores | $1,200 |
| Azure Redis Cache | Premium P1 (6GB) | $250 |
| Azure Service Bus | Premium | $665 |
| Azure Blob Storage | 500GB | $10 |
| Azure Application Insights | 10GB/month | $23 |
| Azure Data Factory | 10 pipelines | $50 |
| Azure Synapse Analytics | Pay-as-you-go | $200 |
| **Total** | | **~$2,878/month** |

### Development Costs (One-time)
| Phase | Duration | Team | Cost (USD) |
|-------|----------|------|------------|
| Phase 1: Foundation | 2 months | 3 devs | $36,000 |
| Phase 2: API | 1 month | 2 devs | $12,000 |
| Phase 3: Events | 1 month | 2 devs | $12,000 |
| Phase 4: ML/Mobile | 1 month | 3 devs | $18,000 |
| Phase 5: BI | 1 month | 2 devs | $12,000 |
| **Total** | **6 months** | | **$90,000** |

---

## 📊 SUCCESS METRICS

### Technical KPIs
- [ ] API Response Time: p95 < 200ms
- [ ] Database Query Time: p95 < 50ms
- [ ] System Uptime: 99.9%
- [ ] Error Rate: < 0.1%
- [ ] Unit Test Coverage: > 80%
- [ ] Security Vulnerabilities: 0 critical

### Business KPIs
- [ ] Order Fulfillment Time: < 4 hours
- [ ] Stock Accuracy: > 99.5%
- [ ] Stock-out Rate: < 5%
- [ ] Inventory Turnover: 10+
- [ ] Customer Satisfaction: > 4.5/5
- [ ] Operational Cost Reduction: 30%

---

## 🎓 TRAINING & CHANGE MANAGEMENT

### Training Plan
1. **Week 1-2**: Admin training (new features, dashboards)
2. **Week 3-4**: Warehouse staff training (mobile app, workflows)
3. **Week 5-6**: Developer training (API usage, troubleshooting)
4. **Ongoing**: Monthly knowledge sharing sessions

### Documentation
- [ ] API Documentation (Swagger/OpenAPI)
- [ ] User Manual (web & mobile)
- [ ] Admin Guide
- [ ] Developer Guide
- [ ] Troubleshooting Guide
- [ ] Video Tutorials

---

## 🔮 FUTURE ROADMAP (Beyond V2)

### V2.5 (Month 9-12)
- Blockchain cho supply chain traceability
- IoT integration (RFID, temperature sensors)
- Voice-activated warehouse operations (Alexa/Google)
- AR-guided picking (HoloLens)

### V3.0 (Year 2)
- Multi-tenant SaaS platform
- Marketplace cho suppliers
- Customer self-service portal
- Global expansion (multi-currency, multi-language)

---

## 📞 SUPPORT & GOVERNANCE

### Support Tiers
- **L1**: Help desk (tickets, email) - Response: 24h
- **L2**: Technical support (bugs, issues) - Response: 4h
- **L3**: Development team (critical bugs) - Response: 1h

### Release Cadence
- **Major releases**: Quarterly (v2.1, v2.2, v2.3...)
- **Minor releases**: Monthly (bug fixes, small features)
- **Hotfixes**: As needed (critical issues)

---

## ✅ CONCLUSION

Version 2.0 transformation sẽ đưa HMS từ một desktop app đơn giản thành một **enterprise-grade platform** với:
- ✅ Cloud-native architecture
- ✅ API-first approach
- ✅ Event-driven scalability
- ✅ AI-powered insights
- ✅ Mobile-first operations
- ✅ Real-time analytics

**Timeline**: 6 months  
**Budget**: ~$90K + $2.9K/month infrastructure  
**ROI**: 30% cost reduction + 10x scalability = Break-even in 12 months

**Next Action**: Approve phase 1 budget and kickoff sprint planning.

---

**Document Owner**: Senior Architect  
**Approvers**: CTO, Product Manager, Engineering Manager  
**Last Review**: January 13, 2026
