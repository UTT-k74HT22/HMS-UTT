using System;
using System.Collections.Generic;
using HospitalManagement.configuration;
using HospitalManagement.entity;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class InvoiceRepositoryImpl : IInvoiceRepository
    {
        private readonly string _connectionString;

        public InvoiceRepositoryImpl(DBConfig dbConfig)
        {
            _connectionString = dbConfig.ConnectionString;
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_connectionString);

        // =================== FIND ALL ===================
        public List<Invoice> FindAll()
        {
            string sql = @"
                SELECT id, order_id, invoice_number, issue_date, due_date,
                       total_amount, paid_amount, status, created_at
                FROM invoices
                ORDER BY id DESC";

            var list = new List<Invoice>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapRow(reader));
            }

            return list;
        }

        // =================== FIND BY ID ===================
        public Invoice FindById(long id)
        {
            string sql = @"
                SELECT id, order_id, invoice_number, issue_date, due_date,
                       total_amount, paid_amount, status, created_at
                FROM invoices
                WHERE id = @id";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            return reader.Read() ? MapRow(reader) : null;
        }

        // =================== INSERT BY ORDER ===================
        public int InsertByOrderId(int orderId, string invoiceNumber)
        {
            string sql = @"
                INSERT INTO invoices (
                    order_id, invoice_number, issue_date,
                    total_amount, paid_amount, status
                )
                SELECT o.id, @invoiceNumber, GETDATE(),
                       o.total_amount, 0, 'NEW'
                FROM orders o
                WHERE o.id = @orderId
                  AND NOT EXISTS (
                        SELECT 1 FROM invoices i
                        WHERE i.order_id = o.id
                  )";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.Parameters.AddWithValue("@invoiceNumber", invoiceNumber);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // =================== UPDATE ===================
        public void Update(Invoice i)
        {
            string sql = @"
                UPDATE invoices
                SET due_date = @dueDate,
                    paid_amount = @paidAmount,
                    status = @status
                WHERE id = @id";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@dueDate", (object?)i.DueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@paidAmount", i.PaidAmount);
            cmd.Parameters.AddWithValue("@status", i.Status);
            cmd.Parameters.AddWithValue("@id", i.Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // =================== CANCEL ===================
        public void Cancel(long id)
        {
            string sql = @"
                UPDATE invoices
                SET status = 'CANCELED'
                WHERE id = @id
                  AND status <> 'PAID'";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // =================== AVAILABLE ORDER IDS ===================
        public List<int> FindAvailableOrderIds()
        {
            string sql = @"
                SELECT o.id
                FROM orders o
                WHERE NOT EXISTS (
                    SELECT 1 FROM invoices i
                    WHERE i.order_id = o.id
                )
                ORDER BY o.id";

            var result = new List<int>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader.GetInt32(0));
            }

            return result;
        }

        // =================== MAPPER ===================
        private Invoice MapRow(SqlDataReader rs)
        {
            return new Invoice
            {
                Id = rs.GetInt32(rs.GetOrdinal("id")),
                OrderId = rs.GetInt32(rs.GetOrdinal("order_id")),
                InvoiceNumber = rs.GetString(rs.GetOrdinal("invoice_number")),
                IssueDate = rs.GetDateTime(rs.GetOrdinal("issue_date")),
                DueDate = rs.IsDBNull(rs.GetOrdinal("due_date"))
                    ? null
                    : rs.GetDateTime(rs.GetOrdinal("due_date")),
                TotalAmount = rs.GetDecimal(rs.GetOrdinal("total_amount")),
                PaidAmount = rs.GetDecimal(rs.GetOrdinal("paid_amount")),
                Status = rs.GetString(rs.GetOrdinal("status"))
            };
        }
    }
}
