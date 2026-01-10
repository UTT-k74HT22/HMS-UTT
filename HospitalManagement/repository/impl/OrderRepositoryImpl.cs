using System.Data;
using HospitalManagement.dto.request.Order;
using HospitalManagement.dto.response.Order;
using HospitalManagement.entity;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl;

public class OrderRepositoryImpl : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ===== Tạo đơn hàng =====
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
                 shipping_address, note, discount, tax, created_by_user_id, status)
                VALUES
                (@customerId, CONCAT('ORD-', DATEDIFF(SECOND,'1970-01-01',GETDATE())),
                 GETDATE(), @shippingAddress, @note, @discount, 0, @employeeId, 'NEW');

                SELECT SCOPE_IDENTITY();
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@customerId", (object?)customerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@shippingAddress", shippingAddress);
            cmd.Parameters.AddWithValue("@note", note ?? string.Empty);
            cmd.Parameters.AddWithValue("@discount", discount);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);

            conn.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        // ===== Thêm sản phẩm vào đơn =====
        public void InsertItem(long orderId, OrderItemRequest item)
        {
            const string sql = @"
                INSERT INTO order_items
                (order_id, product_id, batch_id, quantity, unit_price, line_total, warehouse_id, note)
                VALUES
                (@orderId, @productId, @batchId, @quantity, @unitPrice, @lineTotal, @warehouseId, @note)
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.Parameters.AddWithValue("@productId", item.ProductId!.Value);
            cmd.Parameters.AddWithValue("@batchId", (object?)item.BatchId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@quantity", item.Quantity);
            cmd.Parameters.AddWithValue("@unitPrice", item.UnitPrice);
            cmd.Parameters.AddWithValue("@lineTotal", item.UnitPrice * item.Quantity);
            cmd.Parameters.AddWithValue("@warehouseId", (object?)item.WarehouseId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@note", item.Note ?? string.Empty);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ===== Cập nhật tổng tiền =====
        public void UpdateOrderTotal(long orderId)
        {
            const string sql = @"
                UPDATE o
                SET subtotal = (
                    SELECT ISNULL(SUM(line_total), 0)
                    FROM order_items WHERE order_id = o.id
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

        // ===== Cập nhật trạng thái =====
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

        // ===== Hủy đơn (soft delete) =====
        public void CancelOrder(long orderId)
        {
            UpdateStatus(orderId, OrderStatus.CANCELED.ToString());
        }

        // ===== Lấy tất cả đơn =====
        public List<OrderResponse> FindAll()
        {
            const string sql = @"
                SELECT 
                    o.id, o.order_number, o.status, o.total_amount,
                    o.order_date, o.note, o.shipping_address,
                    u.full_name AS creator_name,
                    u.phone     AS creator_phone,
                    u.email     AS creator_email
                FROM orders o
                LEFT JOIN user_profiles u ON o.created_by_user_id = u.id
                ORDER BY o.order_date DESC
            ";

            var list = new List<OrderResponse>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                list.Add(Map(reader));

            return list;
        }

        // ===== Lấy đơn theo ID =====
        public OrderResponse FindById(long orderId)
        {
            const string sql = @"
                SELECT
                    o.id, o.order_number, o.order_date, o.status,
                    o.total_amount, o.note, o.shipping_address,
                    u.full_name AS customer_name,
                    u.phone     AS customer_phone,
                    u.email     AS customer_email,
                    c.full_name AS creator_name,
                    c.phone     AS creator_phone,
                    c.email     AS creator_email
                FROM orders o
                JOIN user_profiles u ON o.customer_id = u.id
                LEFT JOIN user_profiles c ON o.created_by_user_id = c.id
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

        // ===== Lấy danh sách sản phẩm =====
        public List<OrderItemResponse> GetItems(long orderId)
        {
            const string sql = @"
                SELECT
                    oi.id, oi.product_id, oi.batch_id, oi.warehouse_id,
                    oi.quantity, oi.unit_price, oi.line_total, oi.note,
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
                    Id = reader.GetInt64("id"),
                    ProductId = reader.GetInt64("product_id"),
                    ProductName = reader.GetString("product_name"),
                    BatchId = reader.IsDBNull("batch_id") ? null : reader.GetInt64("batch_id"),
                    WarehouseId = reader.IsDBNull("warehouse_id") ? null : reader.GetInt64("warehouse_id"),
                    Quantity = reader.GetInt32("quantity"),
                    UnitPrice = reader.GetDecimal("unit_price"),
                    LineTotal = reader.GetDecimal("line_total"),
                    Note = reader.GetString("note")
                });
            }

            return list;
        }

        // ===== Mapper dùng chung =====
        private static OrderResponse Map(SqlDataReader r) => new()
        {
            Id = r.GetInt64("id"),
            OrderNumber = r.GetString("order_number"),
            OrderDate = r.GetDateTime("order_date"),
            Status = r.GetString("status"),
            TotalAmount = r.GetDecimal("total_amount"),
            Note = r.IsDBNull("note") ? "-" : r.GetString("note"),
            ShippingAddress = r.GetString("shipping_address"),
            CreatorName = r.GetString("creator_name"),
            CreatorEmail = r.GetString("creator_email"),
            CreatorPhone = r.GetString("creator_phone")
        };

        private static OrderResponse MapDetail(SqlDataReader r) => new()
        {
            Id = r.GetInt64("id"),
            OrderNumber = r.GetString("order_number"),
            OrderDate = r.GetDateTime("order_date"),
            Status = r.GetString("status"),
            TotalAmount = r.GetDecimal("total_amount"),
            Note = r.GetString("note"),
            ShippingAddress = r.GetString("shipping_address"),
            CustomerName = r.GetString("customer_name"),
            CustomerPhone = r.GetString("customer_phone"),
            CustomerEmail = r.GetString("customer_email"),
            CreatorName = r.GetString("creator_name"),
            CreatorEmail = r.GetString("creator_email"),
            CreatorPhone = r.GetString("creator_phone")
        };
    }