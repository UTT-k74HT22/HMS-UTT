using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl;

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
        string query = """
                       SELECT
                           ii.id,
                           p.id AS product_id,
                           p.code AS product_code,
                           p.name AS product_name,
                           p.unit,
                           b.id AS batch_id,
                           b.batch_code AS batch_code,
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
                           CASE WHEN b.expiry_date IS NOT NULL AND DATEDIFF(MONTH, GETDATE(), b.expiry_date) <= 3 THEN 1 ELSE 0 END AS is_near_expiry,
                           ii.last_stock_check
                       FROM inventory_items ii
                       INNER JOIN products p ON ii.product_id = p.id
                       LEFT JOIN batches b ON ii.batch_id = b.id
                       INNER JOIN warehouses w ON ii.warehouse_id = w.id
                       ORDER BY p.name, w.name ASC
                       """;
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                inventories.Add(MapToResponse(reader));
            }
        }

        return inventories;
    }

    public List<InventoryResponse> GetByWarehouse(long warehouseId)
    {
        var inventories = new List<InventoryResponse>();
        string sql = """
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
                     ORDER BY p.name
                     """;
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@warehouseId", warehouseId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                inventories.Add(MapToResponse(reader));
            }
            return inventories;
        }
    }

    public List<InventoryResponse> GetByProduct(long productId)
    {
        var inventories = new List<InventoryResponse>();

        string query = """
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
                        ORDER BY w.name
                        """;

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@productId", productId);
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    inventories.Add(MapToResponse(reader));
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
                    return MapToResponse(reader);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Lấy danh sách các mặt hàng có tồn kho thấp hơn ngưỡng tối thiểu
    /// </summary>
    /// <returns>Trả về danh sách đã lấy</returns>
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
                    inventories.Add(MapToResponse(reader));
                }
            }
        }
        return inventories;   
    }

    /// <summary>
    /// Danh sách các mặt hàng sắp hết hạn (trong vòng 3 tháng)
    /// </summary>
    /// <returns></returns>
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
                    inventories.Add(MapToResponse(reader));
                }
            }
        }

        return inventories;
    }

    /// <summary>
    /// Cập nhật ngưỡng tồn kho cho mặt hàng
    /// </summary>
    /// <param name="inventoryItemId"></param>
    /// <param name="request"></param>
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
        throw new NotImplementedException();
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

    public void InsertStockMovement(long productId, long batchId, long warehouseId, int quantity, int before, int after,
        long userId, string note, string movementType)
    {
        throw new NotImplementedException();
    }

    private InventoryResponse MapToResponse(SqlDataReader reader)
    {
        return new InventoryResponse
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),

            ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
            ProductCode = reader.GetString(reader.GetOrdinal("product_code")),
            ProductName = reader.GetString(reader.GetOrdinal("product_name")),
            Unit = reader.IsDBNull(reader.GetOrdinal("unit"))
                ? null
                : reader.GetString(reader.GetOrdinal("unit")),

            BatchId = reader.IsDBNull(reader.GetOrdinal("batch_id"))
                ? null
                : reader.GetInt32(reader.GetOrdinal("batch_id")),

            BatchCode = reader.IsDBNull(reader.GetOrdinal("batch_code"))
                ? null
                : reader.GetString(reader.GetOrdinal("batch_code")),

            ExpiryDate = reader.IsDBNull(reader.GetOrdinal("expiry_date"))
                ? null
                : reader.GetDateTime(reader.GetOrdinal("expiry_date")),

            WarehouseId = reader.GetInt32(reader.GetOrdinal("warehouse_id")),
            WarehouseCode = reader.GetString(reader.GetOrdinal("warehouse_code")),
            WarehouseName = reader.GetString(reader.GetOrdinal("warehouse_name")),

            QuantityOnHand = reader.GetInt32(reader.GetOrdinal("quantity_on_hand")),
            QuantityReserved = reader.GetInt32(reader.GetOrdinal("quantity_reserved")),
            QuantityAvailable = reader.GetInt32(reader.GetOrdinal("quantity_available")),

            MinThreshold = reader.GetInt32(reader.GetOrdinal("min_threshold")),
            MaxThreshold = reader.GetInt32(reader.GetOrdinal("max_threshold")),

            IsLowStock = reader.GetInt32(reader.GetOrdinal("is_low_stock")) == 1,
            IsOverStock = reader.GetInt32(reader.GetOrdinal("is_over_stock")) == 1,
            IsNearExpiry = reader.GetInt32(reader.GetOrdinal("is_near_expiry")) == 1,

            LastStockCheck = reader.IsDBNull(reader.GetOrdinal("last_stock_check"))
                ? null
                : reader.GetDateTime(reader.GetOrdinal("last_stock_check"))
        };
    }
}