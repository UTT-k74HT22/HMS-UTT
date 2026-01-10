using System;
using System.Collections.Generic;
using System.Data;
using HospitalManagement.configuration;
using Microsoft.Data.SqlClient;
using HospitalManagement.entity;

namespace HospitalManagement.repository.impl
{
    public class PaymentRepositoryImpl : IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepositoryImpl(DBConfig dbConfig)
        {
            _connectionString = dbConfig.ConnectionString;
        }

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        // =================== FIND ALL ===================
        public List<Payment> FindAll()
        {
            string sql = @"
                SELECT id, invoice_id, payment_number, payment_date, amount, method, status, created_at
                FROM payments
                ORDER BY id DESC";

            var list = new List<Payment>();
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
        public Payment FindById(int id)
        {
            string sql = @"
                SELECT id, invoice_id, payment_number, payment_date, amount, method, status, created_at
                FROM payments
                WHERE id = @id";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
                return MapRow(reader);

            return null;
        }

        // =================== INSERT BY INVOICE ===================
        public int InsertByInvoiceId(int invoiceId, string paymentNumber, string method)
        {
            string sql = @"
                INSERT INTO payments (invoice_id, payment_number, payment_date, amount, method, status)
                SELECT i.id, @paymentNumber, GETDATE(), i.total_amount, @method, 'SUCCESS'
                FROM invoices i
                WHERE i.id = @invoiceId AND i.status = 'NEW'";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@paymentNumber", paymentNumber);
            cmd.Parameters.AddWithValue("@method", method);
            cmd.Parameters.AddWithValue("@invoiceId", invoiceId);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // =================== UPDATE ===================
        public void Update(Payment p)
        {
            string sql = @"
                UPDATE payments
                SET method = @method,
                    status = @status
                WHERE id = @id";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@method", p.Method);
            cmd.Parameters.AddWithValue("@status", p.Status);
            cmd.Parameters.AddWithValue("@id", p.Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // =================== DELETE (CANCEL) ===================
        public void Delete(int id)
        {
            string sql = @"
                UPDATE payments
                SET status = 'CANCELED'
                WHERE id = @id AND status = 'SUCCESS'";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // =================== EXISTS BY PAYMENT NUMBER ===================
        public bool ExistsByPaymentNumber(string paymentNumber)
        {
            string sql = "SELECT 1 FROM payments WHERE payment_number = @paymentNumber";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@paymentNumber", paymentNumber);

            conn.Open();
            var result = cmd.ExecuteScalar();
            return result != null;
        }

        // =================== MAPPER ===================
        private Payment MapRow(SqlDataReader rs)
        {
            return new Payment
            {
                Id = rs.GetInt32(rs.GetOrdinal("id")),
                InvoiceId = rs.GetInt32(rs.GetOrdinal("invoice_id")),
                PaymentNumber = rs.GetString(rs.GetOrdinal("payment_number")),
                PaymentDate = rs.IsDBNull(rs.GetOrdinal("payment_date"))
                    ? default
                    : rs.GetDateTime(rs.GetOrdinal("payment_date")),
                Amount = rs.GetDecimal(rs.GetOrdinal("amount")),
                Method = rs.GetString(rs.GetOrdinal("method")),
                Status = rs.GetString(rs.GetOrdinal("status")),
            };
        }
    }
}
