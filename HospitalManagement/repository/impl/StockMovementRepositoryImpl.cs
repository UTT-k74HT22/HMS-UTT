using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl;
public class StockMovementRepositoryImpl : IStockMovementRepository
{
    private readonly string _connectionString;

    public StockMovementRepositoryImpl(string connectionString)
    {
        _connectionString = connectionString;
    }

    public long Create(CreateStockMovementRequest request)
    {
        const string query = @"
                            INSERT INTO dbo.stock_movements
                            (
                                movement_type, product_id, batch_id, warehouse_id, quantity,
                                quantity_before, quantity_after,
                                reference_type, reference_id, performed_by_user_id, note
                            )
                            OUTPUT INSERTED.id
                            VALUES
                            (
                                @movementType, @productId, @batchId, @warehouseId, @quantity,
                                @quantityBefore, @quantityAfter,
                                @referenceType, @referenceId, @performedBy, @note
                            );";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(query, connection);

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
        return Convert.ToInt64(command.ExecuteScalar());
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
                    b.batch_code AS batch_code,
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
                FROM stock_movements sm
                INNER JOIN products p ON sm.product_id = p.id
                LEFT JOIN batches b ON sm.batch_id = b.id
                INNER JOIN warehouses w ON sm.warehouse_id = w.id
                LEFT JOIN user_profiles up ON sm.performed_by_user_id = up.id
                LEFT JOIN accounts a ON up.account_id = a.id
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
                FROM inventory_items sm
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
                FROM inventory_items sm
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
        throw new NotImplementedException();
    }

    public List<StockMovementResponse> GetByDateRange(DateTime fromDate, DateTime toDate)
    {
        throw new NotImplementedException();
    }

    public StockMovementResponse FindById(long id)
    {
        throw new NotImplementedException();
    }

    public List<StockMovementResponse> GetHistoryByProductAndWarehouse(long productId, long warehouseId)
    {
        throw new NotImplementedException();
    }

    public long InsertWithQuantityTracking(CreateStockMovementRequest request)
    {
        return Create(request);
    }

    private StockMovementResponse MapToStockMovementResponse(SqlDataReader reader)
    {
        return new StockMovementResponse
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            MovementType = Enum.Parse<StockMovementType>(reader.GetString(reader.GetOrdinal("movement_type"))),
            MovementDate = reader.GetDateTime(reader.GetOrdinal("movement_date")),
            ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
            ProductCode = reader.GetString(reader.GetOrdinal("product_code")),
            ProductName = reader.GetString(reader.GetOrdinal("product_name")),
            Unit = reader.GetString(reader.GetOrdinal("unit")),
            BatchId = reader.IsDBNull(reader.GetOrdinal("batch_id"))
                ? null
                : reader.GetInt32(reader.GetOrdinal("batch_id")),
            BatchCode = reader.IsDBNull(reader.GetOrdinal("batch_code"))
                ? null
                : reader.GetString(reader.GetOrdinal("batch_code")),
            WarehouseId = reader.GetInt32(reader.GetOrdinal("warehouse_id")),
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
                : reader.GetInt32(reader.GetOrdinal("reference_id")),
            PerformedByUserId = reader.IsDBNull(reader.GetOrdinal("performed_by_user_id"))
                ? null
                : reader.GetInt32(reader.GetOrdinal("performed_by_user_id")),
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