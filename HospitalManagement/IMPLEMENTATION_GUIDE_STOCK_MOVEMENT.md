# Hướng Dẫn Implementation - Stock Movement Management

## 📋 Tổng Quan
Module quản lý xuất nhập kho với tracking đầy đủ: quantity before/after, user, timestamp.

---

## 🗄️ 1. REPOSITORY IMPLEMENTATION

### File: `repository/impl/StockMovementRepositoryImpl.cs`

```csharp
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class StockMovementRepositoryImpl : IStockMovementRepository
    {
        private readonly string _connectionString;

        public StockMovementRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public long Create(CreateStockMovementRequest request)
        {
            string query = @"
                INSERT INTO stock_movement 
                    (movement_type, product_id, batch_id, warehouse_id, quantity, 
                     quantity_before, quantity_after, reference_type, reference_id,
                     performed_by_user_id, note, movement_date, created_at)
                OUTPUT INSERTED.id
                VALUES 
                    (@movementType, @productId, @batchId, @warehouseId, @quantity,
                     @quantityBefore, @quantityAfter, @referenceType, @referenceId,
                     @performedBy, @note, GETDATE(), GETDATE())";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@movementType", request.MovementType.ToString());
                command.Parameters.AddWithValue("@productId", request.ProductId);
                command.Parameters.AddWithValue("@batchId", request.BatchId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@warehouseId", request.WarehouseId);
                command.Parameters.AddWithValue("@quantity", request.Quantity);
                command.Parameters.AddWithValue("@quantityBefore", request.QuantityBefore ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@quantityAfter", request.QuantityAfter ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@referenceType", request.ReferenceType ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@referenceId", request.ReferenceId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@performedBy", request.PerformedByUserId);
                command.Parameters.AddWithValue("@note", request.Note ?? (object)DBNull.Value);

                connection.Open();
                return (long)(int)command.ExecuteScalar();
            }
        }

        public List<StockMovementResponse> GetAll()
        {
            var movements = new List<StockMovementResponse>();
            
            string query = @"
                SELECT 
                    sm.id,
                    sm.movement_type,
                    sm.movement_date,
                    p.id AS product_id,
                    p.code AS product_code,
                    p.name AS product_name,
                    p.unit,
                    b.id AS batch_id,
                    b.code AS batch_code,
                    w.id AS warehouse_id,
                    w.code AS warehouse_code,
                    w.name AS warehouse_name,
                    sm.quantity,
                    sm.quantity_before,
                    sm.quantity_after,
                    sm.reference_type,
                    sm.reference_id,
                    sm.performed_by_user_id,
                    a.username AS performed_by_username,
                    up.full_name AS performed_by_full_name,
                    sm.note
                FROM stock_movement sm
                INNER JOIN product p ON sm.product_id = p.id
                LEFT JOIN batch b ON sm.batch_id = b.id
                INNER JOIN warehouse w ON sm.warehouse_id = w.id
                LEFT JOIN user_profile up ON sm.performed_by_user_id = up.id
                LEFT JOIN account a ON up.account_id = a.id
                ORDER BY sm.movement_date DESC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movements.Add(MapToStockMovementResponse(reader));
                    }
                }
            }
            return movements;
        }

        public List<StockMovementResponse> GetByWarehouse(long warehouseId)
        {
            var movements = new List<StockMovementResponse>();
            
            string query = @"
                SELECT 
                    sm.id, sm.movement_type, sm.movement_date,
                    p.id AS product_id, p.code AS product_code, p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    sm.quantity, sm.quantity_before, sm.quantity_after,
                    sm.reference_type, sm.reference_id,
                    sm.performed_by_user_id,
                    a.username AS performed_by_username,
                    up.full_name AS performed_by_full_name,
                    sm.note
                FROM stock_movement sm
                INNER JOIN product p ON sm.product_id = p.id
                LEFT JOIN batch b ON sm.batch_id = b.id
                INNER JOIN warehouse w ON sm.warehouse_id = w.id
                LEFT JOIN user_profile up ON sm.performed_by_user_id = up.id
                LEFT JOIN account a ON up.account_id = a.id
                WHERE sm.warehouse_id = @warehouseId
                ORDER BY sm.movement_date DESC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@warehouseId", warehouseId);
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movements.Add(MapToStockMovementResponse(reader));
                    }
                }
            }
            return movements;
        }

        public List<StockMovementResponse> GetByProduct(long productId)
        {
            var movements = new List<StockMovementResponse>();
            
            string query = @"
                SELECT 
                    sm.id, sm.movement_type, sm.movement_date,
                    p.id AS product_id, p.code AS product_code, p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    sm.quantity, sm.quantity_before, sm.quantity_after,
                    sm.reference_type, sm.reference_id,
                    sm.performed_by_user_id,
                    a.username AS performed_by_username,
                    up.full_name AS performed_by_full_name,
                    sm.note
                FROM stock_movement sm
                INNER JOIN product p ON sm.product_id = p.id
                LEFT JOIN batch b ON sm.batch_id = b.id
                INNER JOIN warehouse w ON sm.warehouse_id = w.id
                LEFT JOIN user_profile up ON sm.performed_by_user_id = up.id
                LEFT JOIN account a ON up.account_id = a.id
                WHERE sm.product_id = @productId
                ORDER BY sm.movement_date DESC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@productId", productId);
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movements.Add(MapToStockMovementResponse(reader));
                    }
                }
            }
            return movements;
        }

        public List<StockMovementResponse> GetByMovementType(StockMovementType movementType)
        {
            var movements = new List<StockMovementResponse>();
            
            string query = @"
                SELECT 
                    sm.id, sm.movement_type, sm.movement_date,
                    p.id AS product_id, p.code AS product_code, p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    sm.quantity, sm.quantity_before, sm.quantity_after,
                    sm.reference_type, sm.reference_id,
                    sm.performed_by_user_id,
                    a.username AS performed_by_username,
                    up.full_name AS performed_by_full_name,
                    sm.note
                FROM stock_movement sm
                INNER JOIN product p ON sm.product_id = p.id
                LEFT JOIN batch b ON sm.batch_id = b.id
                INNER JOIN warehouse w ON sm.warehouse_id = w.id
                LEFT JOIN user_profile up ON sm.performed_by_user_id = up.id
                LEFT JOIN account a ON up.account_id = a.id
                WHERE sm.movement_type = @movementType
                ORDER BY sm.movement_date DESC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@movementType", movementType.ToString());
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movements.Add(MapToStockMovementResponse(reader));
                    }
                }
            }
            return movements;
        }

        public List<StockMovementResponse> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            var movements = new List<StockMovementResponse>();
            
            string query = @"
                SELECT 
                    sm.id, sm.movement_type, sm.movement_date,
                    p.id AS product_id, p.code AS product_code, p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    sm.quantity, sm.quantity_before, sm.quantity_after,
                    sm.reference_type, sm.reference_id,
                    sm.performed_by_user_id,
                    a.username AS performed_by_username,
                    up.full_name AS performed_by_full_name,
                    sm.note
                FROM stock_movement sm
                INNER JOIN product p ON sm.product_id = p.id
                LEFT JOIN batch b ON sm.batch_id = b.id
                INNER JOIN warehouse w ON sm.warehouse_id = w.id
                LEFT JOIN user_profile up ON sm.performed_by_user_id = up.id
                LEFT JOIN account a ON up.account_id = a.id
                WHERE sm.movement_date >= @fromDate 
                  AND sm.movement_date < @toDate
                ORDER BY sm.movement_date DESC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fromDate", fromDate);
                command.Parameters.AddWithValue("@toDate", toDate.AddDays(1)); // Include toDate
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movements.Add(MapToStockMovementResponse(reader));
                    }
                }
            }
            return movements;
        }

        public StockMovementResponse FindById(long id)
        {
            string query = @"
                SELECT 
                    sm.id, sm.movement_type, sm.movement_date,
                    p.id AS product_id, p.code AS product_code, p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    sm.quantity, sm.quantity_before, sm.quantity_after,
                    sm.reference_type, sm.reference_id,
                    sm.performed_by_user_id,
                    a.username AS performed_by_username,
                    up.full_name AS performed_by_full_name,
                    sm.note
                FROM stock_movement sm
                INNER JOIN product p ON sm.product_id = p.id
                LEFT JOIN batch b ON sm.batch_id = b.id
                INNER JOIN warehouse w ON sm.warehouse_id = w.id
                LEFT JOIN user_profile up ON sm.performed_by_user_id = up.id
                LEFT JOIN account a ON up.account_id = a.id
                WHERE sm.id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToStockMovementResponse(reader);
                    }
                }
            }
            return null;
        }

        public List<StockMovementResponse> GetHistoryByProductAndWarehouse(long productId, long warehouseId)
        {
            var movements = new List<StockMovementResponse>();
            
            string query = @"
                SELECT 
                    sm.id, sm.movement_type, sm.movement_date,
                    p.id AS product_id, p.code AS product_code, p.name AS product_name, p.unit,
                    b.id AS batch_id, b.code AS batch_code,
                    w.id AS warehouse_id, w.code AS warehouse_code, w.name AS warehouse_name,
                    sm.quantity, sm.quantity_before, sm.quantity_after,
                    sm.reference_type, sm.reference_id,
                    sm.performed_by_user_id,
                    a.username AS performed_by_username,
                    up.full_name AS performed_by_full_name,
                    sm.note
                FROM stock_movement sm
                INNER JOIN product p ON sm.product_id = p.id
                LEFT JOIN batch b ON sm.batch_id = b.id
                INNER JOIN warehouse w ON sm.warehouse_id = w.id
                LEFT JOIN user_profile up ON sm.performed_by_user_id = up.id
                LEFT JOIN account a ON up.account_id = a.id
                WHERE sm.product_id = @productId
                  AND sm.warehouse_id = @warehouseId
                ORDER BY sm.movement_date DESC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@warehouseId", warehouseId);
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movements.Add(MapToStockMovementResponse(reader));
                    }
                }
            }
            return movements;
        }

        public long InsertWithQuantityTracking(CreateStockMovementRequest request)
        {
            // This is the same as Create() but typically used within a transaction
            return Create(request);
        }

        private StockMovementResponse MapToStockMovementResponse(SqlDataReader reader)
        {
            return new StockMovementResponse
            {
                Id = reader.GetInt64(reader.GetOrdinal("id")),
                MovementType = Enum.Parse<StockMovementType>(reader.GetString(reader.GetOrdinal("movement_type"))),
                MovementDate = reader.GetDateTime(reader.GetOrdinal("movement_date")),
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
                WarehouseId = reader.GetInt64(reader.GetOrdinal("warehouse_id")),
                WarehouseCode = reader.GetString(reader.GetOrdinal("warehouse_code")),
                WarehouseName = reader.GetString(reader.GetOrdinal("warehouse_name")),
                Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                QuantityBefore = reader.IsDBNull(reader.GetOrdinal("quantity_before")) 
                    ? null 
                    : reader.GetInt32(reader.GetOrdinal("quantity_before")),
                QuantityAfter = reader.IsDBNull(reader.GetOrdinal("quantity_after")) 
                    ? null 
                    : reader.GetInt32(reader.GetOrdinal("quantity_after")),
                ReferenceType = reader.IsDBNull(reader.GetOrdinal("reference_type")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("reference_type")),
                ReferenceId = reader.IsDBNull(reader.GetOrdinal("reference_id")) 
                    ? null 
                    : reader.GetInt64(reader.GetOrdinal("reference_id")),
                PerformedByUserId = reader.IsDBNull(reader.GetOrdinal("performed_by_user_id")) 
                    ? null 
                    : reader.GetInt64(reader.GetOrdinal("performed_by_user_id")),
                PerformedByUsername = reader.IsDBNull(reader.GetOrdinal("performed_by_username")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("performed_by_username")),
                PerformedByFullName = reader.IsDBNull(reader.GetOrdinal("performed_by_full_name")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("performed_by_full_name")),
                Note = reader.IsDBNull(reader.GetOrdinal("note")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("note"))
            };
        }
    }
}
```

---

## 💼 2. SERVICE IMPLEMENTATION

### File: `service/impl/StockMovementServiceImpl.cs`

```csharp
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.service.impl
{
    public class StockMovementServiceImpl : IStockMovementService
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly string _connectionString;

        public StockMovementServiceImpl(
            IStockMovementRepository stockMovementRepository,
            IInventoryRepository inventoryRepository,
            string connectionString)
        {
            _stockMovementRepository = stockMovementRepository;
            _inventoryRepository = inventoryRepository;
            _connectionString = connectionString;
        }

        public void CreateMovement(CreateStockMovementRequest request)
        {
            // Validate request
            ValidateCreateMovementRequest(request);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // STEP 1: Get or create inventory item
                        var inventoryInfo = _inventoryRepository.GetOrCreateInventoryItem(
                            request.ProductId, 
                            request.BatchId ?? 0, 
                            request.WarehouseId);

                        int quantityBefore = inventoryInfo.CurrentQuantity;
                        int quantityAfter;

                        // STEP 2: Calculate new quantity based on movement type
                        switch (request.MovementType)
                        {
                            case StockMovementType.IMPORT:
                                quantityAfter = quantityBefore + request.Quantity;
                                break;

                            case StockMovementType.EXPORT:
                                if (quantityBefore < request.Quantity)
                                {
                                    throw new Exception(
                                        $"Không đủ hàng để xuất. Tồn kho hiện tại: {quantityBefore}, yêu cầu xuất: {request.Quantity}");
                                }
                                quantityAfter = quantityBefore - request.Quantity;
                                break;

                            case StockMovementType.ADJUST:
                                // Điều chỉnh có thể cộng hoặc trừ
                                quantityAfter = quantityBefore + request.Quantity;
                                break;

                            case StockMovementType.TRANSFER:
                                // Transfer out (giảm)
                                if (quantityBefore < request.Quantity)
                                {
                                    throw new Exception("Không đủ hàng để chuyển kho");
                                }
                                quantityAfter = quantityBefore - request.Quantity;
                                break;

                            default:
                                throw new Exception($"Loại giao dịch không hợp lệ: {request.MovementType}");
                        }

                        // STEP 3: Update inventory
                        _inventoryRepository.UpdateQuantity(inventoryInfo.InventoryItemId, quantityAfter);

                        // STEP 4: Insert stock movement with tracking
                        request.QuantityBefore = quantityBefore;
                        request.QuantityAfter = quantityAfter;
                        _stockMovementRepository.InsertWithQuantityTracking(request);

                        // STEP 5: Commit transaction
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<StockMovementResponse> GetAll()
        {
            return _stockMovementRepository.GetAll();
        }

        public List<StockMovementResponse> GetByWarehouse(long warehouseId)
        {
            if (warehouseId <= 0)
                throw new ArgumentException("Warehouse ID không hợp lệ");

            return _stockMovementRepository.GetByWarehouse(warehouseId);
        }

        public List<StockMovementResponse> GetByProduct(long productId)
        {
            if (productId <= 0)
                throw new ArgumentException("Product ID không hợp lệ");

            return _stockMovementRepository.GetByProduct(productId);
        }

        public List<StockMovementResponse> GetByMovementType(StockMovementType movementType)
        {
            return _stockMovementRepository.GetByMovementType(movementType);
        }

        public List<StockMovementResponse> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
                throw new ArgumentException("Ngày bắt đầu không thể sau ngày kết thúc");

            return _stockMovementRepository.GetByDateRange(fromDate, toDate);
        }

        public List<StockMovementResponse> GetHistoryByProductAndWarehouse(long productId, long warehouseId)
        {
            if (productId <= 0)
                throw new ArgumentException("Product ID không hợp lệ");
            
            if (warehouseId <= 0)
                throw new ArgumentException("Warehouse ID không hợp lệ");

            return _stockMovementRepository.GetHistoryByProductAndWarehouse(productId, warehouseId);
        }

        private void ValidateCreateMovementRequest(CreateStockMovementRequest request)
        {
            if (request.ProductId <= 0)
                throw new ArgumentException("Product ID không hợp lệ");

            if (request.WarehouseId <= 0)
                throw new ArgumentException("Warehouse ID không hợp lệ");

            if (request.Quantity <= 0)
                throw new ArgumentException("Số lượng phải lớn hơn 0");

            if (request.PerformedByUserId <= 0)
                throw new ArgumentException("User ID không hợp lệ");

            if (request.MovementType == null)
                throw new ArgumentException("Loại giao dịch không được để trống");
        }
    }
}
```

---

## 🎮 3. CONTROLLER IMPLEMENTATION

### File: `controller/StockMovementController.cs`

```csharp
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.service;

namespace HospitalManagement.controller
{
    public class StockMovementController
    {
        private readonly IStockMovementService _stockMovementService;

        public StockMovementController(IStockMovementService stockMovementService)
        {
            _stockMovementService = stockMovementService;
        }

        /// <summary>
        /// [CHỨC NĂNG 1] Nhập kho
        /// FLOW:
        /// 1. Validate request (product, warehouse, quantity > 0)
        /// 2. BEGIN TRANSACTION
        /// 3. Get/Create inventory_item
        /// 4. Calculate: quantity_after = quantity_before + quantity
        /// 5. Update inventory_item.quantity_on_hand
        /// 6. Insert stock_movement with before/after tracking
        /// 7. COMMIT
        /// </summary>
        public void ImportStock(CreateStockMovementRequest request)
        {
            try
            {
                request.MovementType = StockMovementType.IMPORT;
                _stockMovementService.CreateMovement(request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi nhập kho: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 2] Xuất kho
        /// FLOW:
        /// 1. Validate request
        /// 2. BEGIN TRANSACTION
        /// 3. Get current quantity
        /// 4. Check: quantity_before >= quantity (đủ hàng để xuất)
        /// 5. Calculate: quantity_after = quantity_before - quantity
        /// 6. Update inventory
        /// 7. Insert stock_movement
        /// 8. COMMIT (hoặc ROLLBACK nếu không đủ hàng)
        /// </summary>
        public void ExportStock(CreateStockMovementRequest request)
        {
            try
            {
                request.MovementType = StockMovementType.EXPORT;
                _stockMovementService.CreateMovement(request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xuất kho: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 3] Điều chỉnh tồn kho
        /// FLOW:
        /// 1. Dùng khi kiểm kê phát hiện chênh lệch
        /// 2. quantity có thể dương (thừa) hoặc âm (thiếu)
        /// 3. Cập nhật và ghi log
        /// </summary>
        public void AdjustStock(CreateStockMovementRequest request)
        {
            try
            {
                request.MovementType = StockMovementType.ADJUST;
                _stockMovementService.CreateMovement(request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi điều chỉnh tồn kho: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 4] Chuyển kho
        /// FLOW:
        /// 1. Tạo 2 giao dịch:
        ///    - TRANSFER OUT: Giảm tồn kho nguồn
        ///    - TRANSFER IN: Tăng tồn kho đích
        /// 2. Reference đến nhau qua reference_id
        /// </summary>
        public void TransferStock(long productId, long batchId, long fromWarehouseId, 
                                  long toWarehouseId, int quantity, long userId, string note)
        {
            try
            {
                // Transfer OUT from source warehouse
                var outRequest = new CreateStockMovementRequest
                {
                    MovementType = StockMovementType.TRANSFER,
                    ProductId = productId,
                    BatchId = batchId,
                    WarehouseId = fromWarehouseId,
                    Quantity = quantity,
                    PerformedByUserId = userId,
                    Note = $"Chuyển đến kho {toWarehouseId}: {note}",
                    ReferenceType = "TRANSFER_OUT"
                };
                _stockMovementService.CreateMovement(outRequest);

                // Transfer IN to destination warehouse
                var inRequest = new CreateStockMovementRequest
                {
                    MovementType = StockMovementType.IMPORT,
                    ProductId = productId,
                    BatchId = batchId,
                    WarehouseId = toWarehouseId,
                    Quantity = quantity,
                    PerformedByUserId = userId,
                    Note = $"Nhận từ kho {fromWarehouseId}: {note}",
                    ReferenceType = "TRANSFER_IN"
                };
                _stockMovementService.CreateMovement(inRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi chuyển kho: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 5] Xem tất cả giao dịch
        /// </summary>
        public List<StockMovementResponse> GetAllMovements()
        {
            try
            {
                return _stockMovementService.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách giao dịch: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 6] Lọc giao dịch theo kho
        /// </summary>
        public List<StockMovementResponse> GetMovementsByWarehouse(long warehouseId)
        {
            try
            {
                return _stockMovementService.GetByWarehouse(warehouseId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy giao dịch theo kho: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 7] Lọc giao dịch theo sản phẩm
        /// </summary>
        public List<StockMovementResponse> GetMovementsByProduct(long productId)
        {
            try
            {
                return _stockMovementService.GetByProduct(productId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy giao dịch theo sản phẩm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 8] Lọc theo loại giao dịch
        /// </summary>
        public List<StockMovementResponse> GetMovementsByType(StockMovementType movementType)
        {
            try
            {
                return _stockMovementService.GetByMovementType(movementType);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy giao dịch theo loại: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 9] Lọc theo khoảng thời gian
        /// </summary>
        public List<StockMovementResponse> GetMovementsByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return _stockMovementService.GetByDateRange(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy giao dịch theo thời gian: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 10] Xem lịch sử xuất nhập của 1 sản phẩm tại 1 kho
        /// </summary>
        public List<StockMovementResponse> GetProductWarehouseHistory(long productId, long warehouseId)
        {
            try
            {
                return _stockMovementService.GetHistoryByProductAndWarehouse(productId, warehouseId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy lịch sử: {ex.Message}", ex);
            }
        }
    }
}
```

---

## 📊 4. SQL SCHEMA (SQL Server)

```sql
-- Table: stock_movement
CREATE TABLE stock_movement (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    movement_type NVARCHAR(20) NOT NULL CHECK (movement_type IN ('IMPORT', 'EXPORT', 'ADJUST', 'TRANSFER')),
    product_id BIGINT NOT NULL,
    batch_id BIGINT,
    warehouse_id BIGINT NOT NULL,
    quantity INT NOT NULL,
    quantity_before INT,
    quantity_after INT,
    reference_type NVARCHAR(50),
    reference_id BIGINT,
    performed_by_user_id BIGINT,
    note NVARCHAR(500),
    movement_date DATETIME2 DEFAULT GETDATE(),
    created_at DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (product_id) REFERENCES product(id),
    FOREIGN KEY (batch_id) REFERENCES batch(id),
    FOREIGN KEY (warehouse_id) REFERENCES warehouse(id),
    FOREIGN KEY (performed_by_user_id) REFERENCES user_profile(id)
);

CREATE INDEX idx_stock_movement_product ON stock_movement(product_id);
CREATE INDEX idx_stock_movement_warehouse ON stock_movement(warehouse_id);
CREATE INDEX idx_stock_movement_type ON stock_movement(movement_type);
CREATE INDEX idx_stock_movement_date ON stock_movement(movement_date);
```

---

## 🔄 5. TRANSACTION FLOW

### IMPORT Flow:
```
[UI nhập số lượng] → [Controller.ImportStock]
    → [Service.CreateMovement] 
        → [BEGIN TRANSACTION]
            → [GetOrCreateInventoryItem] → Get current quantity (100)
            → [Calculate] → after = 100 + 50 = 150
            → [UpdateQuantity(150)]
            → [InsertStockMovement(before:100, after:150)]
        → [COMMIT]
```

### EXPORT Flow (có validation):
```
[UI xuất hàng] → [Controller.ExportStock]
    → [Service.CreateMovement]
        → [BEGIN TRANSACTION]
            → [Get current quantity] → 150
            → [Validate] → 150 >= 80? YES
            → [Calculate] → after = 150 - 80 = 70
            → [UpdateQuantity(70)]
            → [InsertStockMovement(before:150, after:70)]
        → [COMMIT]
```

### EXPORT Flow (không đủ hàng):
```
[UI xuất 200] → [Service]
    → [Get current: 70]
    → [Check: 70 >= 200?] NO
    → [ROLLBACK]
    → [Throw Exception: "Không đủ hàng"]
```

---

## 🎯 6. BUSINESS RULES

1. **IMPORT**: Luôn tăng tồn kho
2. **EXPORT**: Phải check đủ hàng trước khi xuất
3. **ADJUST**: Có thể + hoặc - (dùng khi kiểm kê)
4. **TRANSFER**: Tạo 2 giao dịch (OUT + IN)
5. **Tracking**: Luôn ghi quantity_before và quantity_after
6. **Transaction**: Tất cả phải trong transaction để đảm bảo data consistency

---

## ✅ 7. TESTING CHECKLIST

- [ ] Nhập kho thành công
- [ ] Xuất kho đủ hàng
- [ ] Xuất kho không đủ hàng (phải lỗi)
- [ ] Điều chỉnh tồn kho (cộng/trừ)
- [ ] Chuyển kho thành công
- [ ] Xem lịch sử theo sản phẩm
- [ ] Xem lịch sử theo kho
- [ ] Lọc theo loại giao dịch
- [ ] Lọc theo khoảng thời gian
- [ ] Kiểm tra quantity_before/after chính xác

---

## 🎯 8. NOTES CHO DEVELOPER

1. **Transaction**: Bắt buộc dùng transaction cho CreateMovement
2. **Rollback**: Nếu không đủ hàng, rollback toàn bộ
3. **Tracking**: quantity_before/after giúp audit trail
4. **Reference**: Dùng reference_type/id để liên kết các giao dịch (VD: Transfer, Sale Order)
5. **Performance**: Index trên movement_date, product_id, warehouse_id
6. **Audit**: Lưu performed_by_user_id để biết ai thực hiện
7. **Validation**: Luôn validate quantity > 0 và đủ hàng trước khi xuất
