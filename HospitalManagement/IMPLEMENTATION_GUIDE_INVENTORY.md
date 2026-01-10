# Hướng Dẫn Implementation - Inventory Management

## 📋 Tổng Quan
Module quản lý tồn kho với tracking theo Product + Batch + Warehouse.

---

## 🗄️ 1. REPOSITORY IMPLEMENTATION

### File: `repository/impl/InventoryRepositoryImpl.cs`

```csharp
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class InventoryRepositoryImpl : IInventoryRepository
    {
        private readonly string _connectionString;

        public InventoryRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<InventoryResponse> GetAll()
        {
            var inventories = new List<InventoryResponse>();
            
            string query = @"
                SELECT 
                    ii.id,
                    p.id AS product_id,
                    p.code AS product_code,
                    p.name AS product_name,
                    p.unit,
                    b.id AS batch_id,
                    b.code AS batch_code,
                    b.expiry_date,
                    w.id AS warehouse_id,
                    w.code AS warehouse_code,
                    w.name AS warehouse_name,
                    ii.quantity_on_hand,
                    ii.quantity_reserved,
                    (ii.quantity_on_hand - ii.quantity_reserved) AS quantity_available,
                    ii.min_threshold,
                    ii.max_threshold,
                    CASE WHEN ii.quantity_on_hand <= ii.min_threshold THEN 1 ELSE 0 END AS is_low_stock,
                    CASE WHEN ii.quantity_on_hand >= ii.max_threshold THEN 1 ELSE 0 END AS is_over_stock,
                    CASE WHEN b.expiry_date IS NOT NULL 
                         AND DATEDIFF(MONTH, GETDATE(), b.expiry_date) <= 3 
                         THEN 1 ELSE 0 END AS is_near_expiry,
                    ii.last_stock_check
                FROM inventory_item ii
                INNER JOIN product p ON ii.product_id = p.id
                LEFT JOIN batch b ON ii.batch_id = b.id
                INNER JOIN warehouse w ON ii.warehouse_id = w.id
                WHERE p.deleted_at IS NULL
                  AND w.deleted_at IS NULL
                ORDER BY p.name, w.name";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventories.Add(MapToInventoryResponse(reader));
                    }
                }
            }
            return inventories;
        }

        public List<InventoryResponse> GetByWarehouse(long warehouseId)
        {
            var inventories = new List<InventoryResponse>();
            
            string query = @"
                SELECT 
                    ii.id, p.id AS product_id, p.code AS product_code, 
                    p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code, b.expiry_date,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    ii.quantity_on_hand, ii.quantity_reserved,
                    (ii.quantity_on_hand - ii.quantity_reserved) AS quantity_available,
                    ii.min_threshold, ii.max_threshold,
                    CASE WHEN ii.quantity_on_hand <= ii.min_threshold THEN 1 ELSE 0 END AS is_low_stock,
                    CASE WHEN ii.quantity_on_hand >= ii.max_threshold THEN 1 ELSE 0 END AS is_over_stock,
                    CASE WHEN b.expiry_date IS NOT NULL 
                         AND DATEDIFF(MONTH, GETDATE(), b.expiry_date) <= 3 
                         THEN 1 ELSE 0 END AS is_near_expiry,
                    ii.last_stock_check
                FROM inventory_item ii
                INNER JOIN product p ON ii.product_id = p.id
                LEFT JOIN batch b ON ii.batch_id = b.id
                INNER JOIN warehouse w ON ii.warehouse_id = w.id
                WHERE ii.warehouse_id = @warehouseId
                  AND p.deleted_at IS NULL
                  AND w.deleted_at IS NULL
                ORDER BY p.name";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@warehouseId", warehouseId);
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventories.Add(MapToInventoryResponse(reader));
                    }
                }
            }
            return inventories;
        }

        public List<InventoryResponse> GetByProduct(long productId)
        {
            var inventories = new List<InventoryResponse>();
            
            string query = @"
                SELECT 
                    ii.id, p.id AS product_id, p.code AS product_code,
                    p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code, b.expiry_date,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    ii.quantity_on_hand, ii.quantity_reserved,
                    (ii.quantity_on_hand - ii.quantity_reserved) AS quantity_available,
                    ii.min_threshold, ii.max_threshold,
                    CASE WHEN ii.quantity_on_hand <= ii.min_threshold THEN 1 ELSE 0 END AS is_low_stock,
                    CASE WHEN ii.quantity_on_hand >= ii.max_threshold THEN 1 ELSE 0 END AS is_over_stock,
                    CASE WHEN b.expiry_date IS NOT NULL 
                         AND DATEDIFF(MONTH, GETDATE(), b.expiry_date) <= 3 
                         THEN 1 ELSE 0 END AS is_near_expiry,
                    ii.last_stock_check
                FROM inventory_item ii
                INNER JOIN product p ON ii.product_id = p.id
                LEFT JOIN batch b ON ii.batch_id = b.id
                INNER JOIN warehouse w ON ii.warehouse_id = w.id
                WHERE ii.product_id = @productId
                  AND p.deleted_at IS NULL
                  AND w.deleted_at IS NULL
                ORDER BY w.name";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@productId", productId);
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventories.Add(MapToInventoryResponse(reader));
                    }
                }
            }
            return inventories;
        }

        public InventoryResponse FindByProductAndWarehouse(long productId, long warehouseId, long? batchId)
        {
            string query = @"
                SELECT 
                    ii.id, p.id AS product_id, p.code AS product_code,
                    p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code, b.expiry_date,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    ii.quantity_on_hand, ii.quantity_reserved,
                    (ii.quantity_on_hand - ii.quantity_reserved) AS quantity_available,
                    ii.min_threshold, ii.max_threshold,
                    CASE WHEN ii.quantity_on_hand <= ii.min_threshold THEN 1 ELSE 0 END AS is_low_stock,
                    CASE WHEN ii.quantity_on_hand >= ii.max_threshold THEN 1 ELSE 0 END AS is_over_stock,
                    CASE WHEN b.expiry_date IS NOT NULL 
                         AND DATEDIFF(MONTH, GETDATE(), b.expiry_date) <= 3 
                         THEN 1 ELSE 0 END AS is_near_expiry,
                    ii.last_stock_check
                FROM inventory_item ii
                INNER JOIN product p ON ii.product_id = p.id
                LEFT JOIN batch b ON ii.batch_id = b.id
                INNER JOIN warehouse w ON ii.warehouse_id = w.id
                WHERE ii.product_id = @productId
                  AND ii.warehouse_id = @warehouseId
                  AND (@batchId IS NULL OR ii.batch_id = @batchId)
                  AND p.deleted_at IS NULL
                  AND w.deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@warehouseId", warehouseId);
                command.Parameters.AddWithValue("@batchId", batchId ?? (object)DBNull.Value);
                
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToInventoryResponse(reader);
                    }
                }
            }
            return null;
        }

        public List<InventoryResponse> GetLowStockItems()
        {
            var inventories = new List<InventoryResponse>();
            
            string query = @"
                SELECT 
                    ii.id, p.id AS product_id, p.code AS product_code,
                    p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code, b.expiry_date,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    ii.quantity_on_hand, ii.quantity_reserved,
                    (ii.quantity_on_hand - ii.quantity_reserved) AS quantity_available,
                    ii.min_threshold, ii.max_threshold,
                    1 AS is_low_stock,
                    CASE WHEN ii.quantity_on_hand >= ii.max_threshold THEN 1 ELSE 0 END AS is_over_stock,
                    CASE WHEN b.expiry_date IS NOT NULL 
                         AND DATEDIFF(MONTH, GETDATE(), b.expiry_date) <= 3 
                         THEN 1 ELSE 0 END AS is_near_expiry,
                    ii.last_stock_check
                FROM inventory_item ii
                INNER JOIN product p ON ii.product_id = p.id
                LEFT JOIN batch b ON ii.batch_id = b.id
                INNER JOIN warehouse w ON ii.warehouse_id = w.id
                WHERE ii.quantity_on_hand <= ii.min_threshold
                  AND p.deleted_at IS NULL
                  AND w.deleted_at IS NULL
                ORDER BY ii.quantity_on_hand ASC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventories.Add(MapToInventoryResponse(reader));
                    }
                }
            }
            return inventories;
        }

        public List<InventoryResponse> GetNearExpiryItems()
        {
            var inventories = new List<InventoryResponse>();
            
            string query = @"
                SELECT 
                    ii.id, p.id AS product_id, p.code AS product_code,
                    p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code, b.expiry_date,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    ii.quantity_on_hand, ii.quantity_reserved,
                    (ii.quantity_on_hand - ii.quantity_reserved) AS quantity_available,
                    ii.min_threshold, ii.max_threshold,
                    CASE WHEN ii.quantity_on_hand <= ii.min_threshold THEN 1 ELSE 0 END AS is_low_stock,
                    CASE WHEN ii.quantity_on_hand >= ii.max_threshold THEN 1 ELSE 0 END AS is_over_stock,
                    1 AS is_near_expiry,
                    ii.last_stock_check
                FROM inventory_item ii
                INNER JOIN product p ON ii.product_id = p.id
                INNER JOIN batch b ON ii.batch_id = b.id
                INNER JOIN warehouse w ON ii.warehouse_id = w.id
                WHERE b.expiry_date IS NOT NULL
                  AND DATEDIFF(MONTH, GETDATE(), b.expiry_date) <= 3
                  AND DATEDIFF(DAY, GETDATE(), b.expiry_date) >= 0
                  AND p.deleted_at IS NULL
                  AND w.deleted_at IS NULL
                ORDER BY b.expiry_date ASC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventories.Add(MapToInventoryResponse(reader));
                    }
                }
            }
            return inventories;
        }

        public void UpdateThresholds(long inventoryItemId, UpdateInventoryThresholdRequest request)
        {
            string query = @"
                UPDATE inventory_item
                SET min_threshold = @minThreshold,
                    max_threshold = @maxThreshold,
                    updated_at = GETDATE()
                WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", inventoryItemId);
                command.Parameters.AddWithValue("@minThreshold", request.MinThreshold ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@maxThreshold", request.MaxThreshold ?? (object)DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int GetTotalQuantityByProduct(long productId)
        {
            string query = @"
                SELECT ISNULL(SUM(quantity_on_hand), 0)
                FROM inventory_item
                WHERE product_id = @productId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@productId", productId);
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public bool HasStock(long productId, long warehouseId, int requiredQuantity)
        {
            string query = @"
                SELECT ISNULL(SUM(quantity_on_hand - quantity_reserved), 0)
                FROM inventory_item
                WHERE product_id = @productId
                  AND warehouse_id = @warehouseId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@warehouseId", warehouseId);
                connection.Open();
                
                int availableQuantity = (int)command.ExecuteScalar();
                return availableQuantity >= requiredQuantity;
            }
        }

        public InventoryItemInfo GetOrCreateInventoryItem(long productId, long batchId, long warehouseId)
        {
            // Try to find existing
            string findQuery = @"
                SELECT id, quantity_on_hand
                FROM inventory_item
                WHERE product_id = @productId
                  AND batch_id = @batchId
                  AND warehouse_id = @warehouseId";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(findQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@batchId", batchId);
                    command.Parameters.AddWithValue("@warehouseId", warehouseId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new InventoryItemInfo
                            {
                                InventoryItemId = reader.GetInt64(0),
                                CurrentQuantity = reader.GetInt32(1)
                            };
                        }
                    }
                }

                // Create new if not exists
                string insertQuery = @"
                    INSERT INTO inventory_item 
                        (product_id, batch_id, warehouse_id, quantity_on_hand, quantity_reserved, 
                         min_threshold, max_threshold, created_at)
                    OUTPUT INSERTED.id
                    VALUES 
                        (@productId, @batchId, @warehouseId, 0, 0, 10, 1000, GETDATE())";

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@batchId", batchId);
                    command.Parameters.AddWithValue("@warehouseId", warehouseId);

                    long newId = (long)(int)command.ExecuteScalar();
                    return new InventoryItemInfo
                    {
                        InventoryItemId = newId,
                        CurrentQuantity = 0
                    };
                }
            }
        }

        public void UpdateQuantity(long inventoryItemId, int newQuantity)
        {
            string query = @"
                UPDATE inventory_item
                SET quantity_on_hand = @newQuantity,
                    last_stock_check = GETDATE(),
                    updated_at = GETDATE()
                WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", inventoryItemId);
                command.Parameters.AddWithValue("@newQuantity", newQuantity);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int GetCurrentQuantity(long productId, long batchId, long warehouseId)
        {
            string query = @"
                SELECT ISNULL(quantity_on_hand, 0)
                FROM inventory_item
                WHERE product_id = @productId
                  AND batch_id = @batchId
                  AND warehouse_id = @warehouseId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@batchId", batchId);
                command.Parameters.AddWithValue("@warehouseId", warehouseId);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != null ? (int)result : 0;
            }
        }

        public void UpdateStock(long productId, long batchId, long warehouseId, int newQuantity)
        {
            string query = @"
                UPDATE inventory_item
                SET quantity_on_hand = @newQuantity,
                    last_stock_check = GETDATE(),
                    updated_at = GETDATE()
                WHERE product_id = @productId
                  AND batch_id = @batchId
                  AND warehouse_id = @warehouseId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@batchId", batchId);
                command.Parameters.AddWithValue("@warehouseId", warehouseId);
                command.Parameters.AddWithValue("@newQuantity", newQuantity);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void InsertStockMovement(long productId, long batchId, long warehouseId,
                                        int quantity, int before, int after,
                                        long userId, string note, string movementType)
        {
            // This will be called from StockMovementRepository
            // Left empty as it's handled by StockMovementRepository
        }

        private InventoryResponse MapToInventoryResponse(SqlDataReader reader)
        {
            return new InventoryResponse
            {
                Id = reader.GetInt64(reader.GetOrdinal("id")),
                ProductId = reader.GetInt64(reader.GetOrdinal("product_id")),
                ProductCode = reader.GetString(reader.GetOrdinal("product_code")),
                ProductName = reader.GetString(reader.GetOrdinal("product_name")),
                Unit = reader.GetString(reader.GetOrdinal("unit")),
                BatchId = reader.IsDBNull(reader.GetOrdinal("batch_id")) 
                    ? null 
                    : reader.GetInt64(reader.GetOrdinal("batch_id")),
                BatchCode = reader.IsDBNull(reader.GetOrdinal("batch_code")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("batch_code")),
                ExpiryDate = reader.IsDBNull(reader.GetOrdinal("expiry_date")) 
                    ? null 
                    : reader.GetDateTime(reader.GetOrdinal("expiry_date")),
                WarehouseId = reader.GetInt64(reader.GetOrdinal("warehouse_id")),
                WarehouseCode = reader.GetString(reader.GetOrdinal("warehouse_code")),
                WarehouseName = reader.GetString(reader.GetOrdinal("warehouse_name")),
                QuantityOnHand = reader.GetInt32(reader.GetOrdinal("quantity_on_hand")),
                QuantityReserved = reader.GetInt32(reader.GetOrdinal("quantity_reserved")),
                QuantityAvailable = reader.GetInt32(reader.GetOrdinal("quantity_available")),
                MinThreshold = reader.IsDBNull(reader.GetOrdinal("min_threshold")) 
                    ? null 
                    : reader.GetInt32(reader.GetOrdinal("min_threshold")),
                MaxThreshold = reader.IsDBNull(reader.GetOrdinal("max_threshold")) 
                    ? null 
                    : reader.GetInt32(reader.GetOrdinal("max_threshold")),
                IsLowStock = reader.GetInt32(reader.GetOrdinal("is_low_stock")) == 1,
                IsOverStock = reader.GetInt32(reader.GetOrdinal("is_over_stock")) == 1,
                IsNearExpiry = reader.GetInt32(reader.GetOrdinal("is_near_expiry")) == 1,
                LastStockCheck = reader.IsDBNull(reader.GetOrdinal("last_stock_check")) 
                    ? null 
                    : reader.GetDateTime(reader.GetOrdinal("last_stock_check"))
            };
        }
    }
}
```

---

## 💼 2. SERVICE IMPLEMENTATION

### File: `service/impl/InventoryServiceImpl.cs`

```csharp
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl
{
    public class InventoryServiceImpl : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryServiceImpl(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public List<InventoryResponse> GetAll()
        {
            return _inventoryRepository.GetAll();
        }

        public List<InventoryResponse> GetByWarehouse(long warehouseId)
        {
            if (warehouseId <= 0)
                throw new ArgumentException("Warehouse ID không hợp lệ");

            return _inventoryRepository.GetByWarehouse(warehouseId);
        }

        public List<InventoryResponse> GetByProduct(long productId)
        {
            if (productId <= 0)
                throw new ArgumentException("Product ID không hợp lệ");

            return _inventoryRepository.GetByProduct(productId);
        }

        public List<InventoryResponse> GetLowStockItems()
        {
            return _inventoryRepository.GetLowStockItems();
        }

        public List<InventoryResponse> GetNearExpiryItems()
        {
            return _inventoryRepository.GetNearExpiryItems();
        }

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

            _inventoryRepository.UpdateThresholds(inventoryItemId, request);
        }

        public int GetTotalQuantityByProduct(long productId)
        {
            return _inventoryRepository.GetTotalQuantityByProduct(productId);
        }

        public bool HasStock(long productId, long warehouseId, int requiredQuantity)
        {
            return _inventoryRepository.HasStock(productId, warehouseId, requiredQuantity);
        }

        public void UpdateStock(long productId, long batchId, long warehouseId, int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Số lượng không thể âm");

            _inventoryRepository.UpdateStock(productId, batchId, warehouseId, newQuantity);
        }

        public void InsertStockMovement(long productId, long batchId, long warehouseId, 
                                        int quantity, int before, int after, 
                                        long userId, string note, string movementType)
        {
            // This is typically called from StockMovementService
            _inventoryRepository.InsertStockMovement(productId, batchId, warehouseId, 
                                                     quantity, before, after, 
                                                     userId, note, movementType);
        }

        public int GetCurrentQuantity(long productId, long batchId, long warehouseId)
        {
            return _inventoryRepository.GetCurrentQuantity(productId, batchId, warehouseId);
        }
    }
}
```

---

## 📊 3. SQL SCHEMA (SQL Server)

```sql
-- Table: inventory_item
CREATE TABLE inventory_item (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    product_id BIGINT NOT NULL,
    batch_id BIGINT,
    warehouse_id BIGINT NOT NULL,
    quantity_on_hand INT NOT NULL DEFAULT 0,
    quantity_reserved INT NOT NULL DEFAULT 0,
    min_threshold INT,
    max_threshold INT,
    last_stock_check DATETIME2,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2,
    FOREIGN KEY (product_id) REFERENCES product(id),
    FOREIGN KEY (batch_id) REFERENCES batch(id),
    FOREIGN KEY (warehouse_id) REFERENCES warehouse(id),
    UNIQUE (product_id, batch_id, warehouse_id)
);

CREATE INDEX idx_inventory_product ON inventory_item(product_id);
CREATE INDEX idx_inventory_warehouse ON inventory_item(warehouse_id);
CREATE INDEX idx_inventory_low_stock ON inventory_item(quantity_on_hand);
```

---

## 🔄 4. BUSINESS RULES

1. **Unique Constraint**: Một sản phẩm + batch + kho chỉ có 1 record
2. **Quantity Available**: = quantity_on_hand - quantity_reserved
3. **Low Stock**: quantity_on_hand <= min_threshold
4. **Near Expiry**: Hạn sử dụng còn <= 3 tháng
5. **Negative Stock**: Không cho phép số lượng âm

---

## ✅ 5. TESTING CHECKLIST

- [ ] Hiển thị tất cả tồn kho
- [ ] Lọc theo kho
- [ ] Lọc theo sản phẩm
- [ ] Xem danh sách sắp hết hàng
- [ ] Xem danh sách sắp hết hạn
- [ ] Cập nhật ngưỡng min/max
- [ ] Kiểm tra còn hàng trước khi xuất

---

## 🎯 6. NOTES

1. **Performance**: Index trên product_id, warehouse_id
2. **Computed Column**: quantity_available tính trong query
3. **Alert System**: Có thể tạo trigger/job để gửi cảnh báo low stock
4. **Batch Tracking**: Batch có thể NULL (cho sản phẩm không theo lô)
