using System.Data;
using HospitalManagement.dto.request.Order;
using HospitalManagement.dto.response.Order;
using HospitalManagement.entity.enums;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class OrderRepositoryImpl : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        /* =========================================================
           CREATE ORDER
           ========================================================= */
        public long InsertOrder(
            long? customerId,
            string shippingAddress,
            string note,
            decimal discount,
            long employeeId)
        {
            const string sql = @"
                INSERT INTO orders
                (customer_id, order_number, order_date,
                 shipping_address, note, discount, tax,
                 created_by_user_id, status)
                VALUES
                (@customerId,
                 CONCAT('ORD-', DATEDIFF(SECOND,'1970-01-01',GETDATE())),
                 GETDATE(),
                 @shippingAddress,
                 @note,
                 @discount,
                 0,
                 @employeeId,
                 'NEW');

                SELECT SCOPE_IDENTITY();
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@customerId", (object?)customerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@shippingAddress", (object?)shippingAddress ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@note", (object?)note ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@discount", discount);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);

            conn.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        /* =========================================================
           INSERT ORDER ITEM (schema-safe)
           ========================================================= */
        public void InsertItem(long orderId, OrderItemRequest item)
        {
            const string sql = @"
        INSERT INTO order_items
        (order_id, product_id, batch_id, warehouse_id,
         quantity, unit_price, discount, line_total)
        VALUES
        (@orderId, @productId, @batchId, @warehouseId,
         @quantity, @unitPrice, 0, @lineTotal)
    ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.Parameters.AddWithValue("@productId", item.ProductId!.Value);
            cmd.Parameters.AddWithValue("@batchId", (object?)item.BatchId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@warehouseId", item.WarehouseId); 
            cmd.Parameters.AddWithValue("@quantity", item.Quantity);
            cmd.Parameters.AddWithValue("@unitPrice", item.UnitPrice);
            cmd.Parameters.AddWithValue("@lineTotal", item.UnitPrice * item.Quantity);

            conn.Open();
            cmd.ExecuteNonQuery();
        }


        /* =========================================================
           UPDATE TOTAL
           ========================================================= */
        public void UpdateOrderTotal(long orderId)
        {
            const string sql = @"
                UPDATE o
                SET subtotal = (
                        SELECT ISNULL(SUM(line_total), 0)
                        FROM order_items
                        WHERE order_id = o.id
                    ),
                    total_amount = subtotal - discount
                FROM orders o
                WHERE o.id = @orderId
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@orderId", orderId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        /* =========================================================
           STATUS
           ========================================================= */
        public void UpdateStatus(long orderId, string status)
        {
            const string sql = "UPDATE orders SET status = @status WHERE id = @orderId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@orderId", orderId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void CancelOrder(long orderId)
        {
            UpdateStatus(orderId, OrderStatus.CANCELED.ToString());
        }

        /* =========================================================
           FIND ALL (LIST VIEW)
           ========================================================= */
        public List<OrderResponse> FindAll()
        {
            const string sql = @"
                SELECT
                    o.id,
                    o.order_number,
                    o.order_date,
                    o.status,
                    o.total_amount,
                    o.note,
                    o.shipping_address,
                    u.full_name AS creator_name,
                    u.phone     AS creator_phone,
                    u.email     AS creator_email
                FROM orders o
                LEFT JOIN user_profiles u
                       ON o.created_by_user_id = u.id
                ORDER BY o.order_date DESC
            ";

            var list = new List<OrderResponse>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                list.Add(MapList(reader));

            return list;
        }

        /* =========================================================
           FIND BY ID (DETAIL VIEW)
           ========================================================= */
        public OrderResponse FindById(long orderId)
        {
            const string sql = @"
                SELECT
                    o.id,
                    o.order_number,
                    o.order_date,
                    o.status,
                    o.total_amount,
                    o.note,
                    o.shipping_address,
                    o.customer_id,
                    cus.full_name AS customer_name,
                    cus.phone     AS customer_phone,
                    cus.email     AS customer_email,
                    cre.full_name AS creator_name,
                    cre.phone     AS creator_phone,
                    cre.email     AS creator_email
                FROM orders o
                LEFT JOIN user_profiles cus
                       ON o.customer_id = cus.id
                LEFT JOIN user_profiles cre
                       ON o.created_by_user_id = cre.id
                WHERE o.id = @orderId
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@orderId", orderId);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                throw new Exception($"Không tìm thấy đơn hàng với ID {orderId}");

            return MapDetail(reader);
        }

        /* =========================================================
           GET ORDER ITEMS
           ========================================================= */
        public List<OrderItemResponse> GetItems(long orderId)
        {
            const string sql = @"
                SELECT
                    oi.id,
                    oi.product_id,
                    oi.batch_id,
                    oi.quantity,
                    oi.unit_price,
                    oi.line_total,
                    oi.discount,
                    p.name AS product_name
                FROM order_items oi
                JOIN products p ON oi.product_id = p.id
                WHERE oi.order_id = @orderId
            ";

            var list = new List<OrderItemResponse>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@orderId", orderId);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new OrderItemResponse
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
                    ProductName = reader.GetString(reader.GetOrdinal("product_name")),
                    BatchId = reader.IsDBNull(reader.GetOrdinal("batch_id"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("batch_id")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("unit_price")),
                    LineTotal = reader.GetDecimal(reader.GetOrdinal("line_total"))
                });
            }

            return list;
        }
        private static OrderResponse MapList(SqlDataReader r) => new()
        {
            Id = r.GetInt32(r.GetOrdinal("id")),
            OrderNumber = r.GetString(r.GetOrdinal("order_number")),
            OrderDate = r.GetDateTime(r.GetOrdinal("order_date")),
            Status = r.GetString(r.GetOrdinal("status")),
            TotalAmount = r.GetDecimal(r.GetOrdinal("total_amount")),

            Note = r.IsDBNull(r.GetOrdinal("note"))
                ? null
                : r.GetString(r.GetOrdinal("note")),

            ShippingAddress = r.IsDBNull(r.GetOrdinal("shipping_address"))
                ? null
                : r.GetString(r.GetOrdinal("shipping_address")),

            CreatorName = r.IsDBNull(r.GetOrdinal("creator_name"))
                ? null
                : r.GetString(r.GetOrdinal("creator_name")),

            CreatorPhone = r.IsDBNull(r.GetOrdinal("creator_phone"))
                ? null
                : r.GetString(r.GetOrdinal("creator_phone")),

            CreatorEmail = r.IsDBNull(r.GetOrdinal("creator_email"))
                ? null
                : r.GetString(r.GetOrdinal("creator_email"))
        };
        private static OrderResponse MapDetail(SqlDataReader r) => new()
        {
            Id = r.GetInt32(r.GetOrdinal("id")),
            OrderNumber = r.GetString(r.GetOrdinal("order_number")),
            OrderDate = r.GetDateTime(r.GetOrdinal("order_date")),
            Status = r.GetString(r.GetOrdinal("status")),
            TotalAmount = r.GetDecimal(r.GetOrdinal("total_amount")),
            Note = r.IsDBNull(r.GetOrdinal("note")) ? null : r.GetString(r.GetOrdinal("note")),
            ShippingAddress = r.IsDBNull(r.GetOrdinal("shipping_address"))
                ? null
                : r.GetString(r.GetOrdinal("shipping_address")),

            CustomerId = r.IsDBNull(r.GetOrdinal("customer_id"))
                ? null
                : r.GetInt32(r.GetOrdinal("customer_id")),

            CustomerName = r.IsDBNull(r.GetOrdinal("customer_name"))
                ? null
                : r.GetString(r.GetOrdinal("customer_name")),

            CustomerPhone = r.IsDBNull(r.GetOrdinal("customer_phone"))
                ? null
                : r.GetString(r.GetOrdinal("customer_phone")),

            CustomerEmail = r.IsDBNull(r.GetOrdinal("customer_email"))
                ? null
                : r.GetString(r.GetOrdinal("customer_email")),

            CreatorName = r.IsDBNull(r.GetOrdinal("creator_name"))
                ? null
                : r.GetString(r.GetOrdinal("creator_name")),

            CreatorPhone = r.IsDBNull(r.GetOrdinal("creator_phone"))
                ? null
                : r.GetString(r.GetOrdinal("creator_phone")),

            CreatorEmail = r.IsDBNull(r.GetOrdinal("creator_email"))
                ? null
                : r.GetString(r.GetOrdinal("creator_email"))
        };
    }
}
