using HospitalManagement.dto.response.ReportDetailResponse;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace HospitalManagement.repository.impl
{
    public class  ReportDetailRepositoryImpl : IReportDetailRepository
    {
        private readonly string _connectionString;

        public ReportDetailRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<InventoryItemResponse> GetInventory(int lowStockThreshold = 10)
        {
            var list = new List<InventoryItemResponse>();
            string query = @"
                SELECT w.name AS Warehouse,
                       p.name AS Product,
                       ISNULL(b.batch_code,'-') AS Batch,
                       i.quantity_on_hand AS Quantity
                FROM dbo.inventory_items i
                LEFT JOIN dbo.products p ON i.product_id = p.id
                LEFT JOIN dbo.batches b ON i.batch_id = b.id
                LEFT JOIN dbo.warehouses w ON i.warehouse_id = w.id
                ORDER BY w.name, p.name";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int qty = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0;
                list.Add(new InventoryItemResponse
                {
                    Warehouse = reader["Warehouse"].ToString(),
                    Product = reader["Product"].ToString(),
                    Batch = reader["Batch"].ToString(),
                    Quantity = qty,
                    IsLowStock = qty <= lowStockThreshold
                });
            }
            return list;
        }

        public List<BestSellingProductResponse> GetBestSellingProducts(int top = 10, int? month = null, int? year = null)
        {
            var list = new List<BestSellingProductResponse>();
            string query = @"
                SELECT TOP (@Top) 
                       p.name AS Product,
                       p.code AS ProductCode,
                       SUM(oi.quantity) AS TotalSold,
                       SUM(oi.line_total) AS TotalRevenue,
                       MONTH(o.order_date) AS Month,
                       YEAR(o.order_date) AS Year
                FROM dbo.order_items oi
                INNER JOIN dbo.orders o ON oi.order_id = o.id
                INNER JOIN dbo.products p ON oi.product_id = p.id
                WHERE (@Month IS NULL OR MONTH(o.order_date) = @Month)
                  AND (@Year IS NULL OR YEAR(o.order_date) = @Year)
                GROUP BY p.name, p.code, MONTH(o.order_date), YEAR(o.order_date)
                ORDER BY TotalSold DESC";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Top", top);
            cmd.Parameters.AddWithValue("@Month", (object?)month ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Year", (object?)year ?? DBNull.Value);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new BestSellingProductResponse
                {
                    Product = reader["Product"].ToString(),
                    ProductCode = reader["ProductCode"].ToString(),
                    TotalSold = reader["TotalSold"] != DBNull.Value ? (int)reader["TotalSold"] : 0,
                    TotalRevenue = reader["TotalRevenue"] != DBNull.Value ? (decimal)reader["TotalRevenue"] : 0,
                    Month = reader["Month"] != DBNull.Value ? (int)reader["Month"] : 0,
                    Year = reader["Year"] != DBNull.Value ? (int)reader["Year"] : 0
                });
            }
            return list;
        }

        public List<CustomerResponse> GetCustomers(int? month = null, int? year = null)
        {
            var list = new List<CustomerResponse>();
            string query = @"
                SELECT up.full_name AS Customer,
                       cp.customer_type AS Type,
                       COUNT(o.id) AS TotalOrders,
                       ISNULL(SUM(o.total_amount),0) AS TotalSpent,
                       MONTH(o.order_date) AS Month,
                       YEAR(o.order_date) AS Year
                FROM dbo.user_profiles up
                INNER JOIN dbo.customer_profiles cp ON up.id = cp.profile_id
                LEFT JOIN dbo.orders o ON o.customer_id = up.id
                WHERE (@Month IS NULL OR MONTH(o.order_date) = @Month)
                  AND (@Year IS NULL OR YEAR(o.order_date) = @Year)
                GROUP BY up.full_name, cp.customer_type, MONTH(o.order_date), YEAR(o.order_date)
                ORDER BY TotalSpent DESC";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Month", (object?)month ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Year", (object?)year ?? DBNull.Value);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CustomerResponse
                {
                    Customer = reader["Customer"].ToString(),
                    Type = reader["Type"].ToString(),
                    TotalOrders = reader["TotalOrders"] != DBNull.Value ? (int)reader["TotalOrders"] : 0,
                    TotalSpent = reader["TotalSpent"] != DBNull.Value ? (decimal)reader["TotalSpent"] : 0,
                    Month = reader["Month"] != DBNull.Value ? (int)reader["Month"] : 0,
                    Year = reader["Year"] != DBNull.Value ? (int)reader["Year"] : 0
                });
            }
            return list;
        }

        public List<OrderStatusResponse> GetOrdersByStatus(int? month = null, int? year = null)
        {
            var list = new List<OrderStatusResponse>();
            string query = @"
                SELECT status AS Status,
                       COUNT(*) AS TotalOrders,
                       ISNULL(SUM(total_amount),0) AS TotalValue,
                       MONTH(order_date) AS Month,
                       YEAR(order_date) AS Year
                FROM dbo.orders
                WHERE (@Month IS NULL OR MONTH(order_date) = @Month)
                  AND (@Year IS NULL OR YEAR(order_date) = @Year)
                GROUP BY status, MONTH(order_date), YEAR(order_date)
                ORDER BY TotalValue DESC";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Month", (object?)month ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Year", (object?)year ?? DBNull.Value);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new OrderStatusResponse
                {
                    Status = reader["Status"].ToString(),
                    TotalOrders = reader["TotalOrders"] != DBNull.Value ? (int)reader["TotalOrders"] : 0,
                    TotalValue = reader["TotalValue"] != DBNull.Value ? (decimal)reader["TotalValue"] : 0,
                    Month = reader["Month"] != DBNull.Value ? (int)reader["Month"] : 0,
                    Year = reader["Year"] != DBNull.Value ? (int)reader["Year"] : 0
                });
            }
            return list;
        }
    }
}
