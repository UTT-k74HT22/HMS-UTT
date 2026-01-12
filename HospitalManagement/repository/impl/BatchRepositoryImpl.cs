using System;
using System.Collections.Generic;
using System.Data;
using HospitalManagement.dto.request.Batch;
using HospitalManagement.dto.response;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class BatchRepositoryImpl : IBatchRepository
    {
        private readonly string _connectionString;

        public BatchRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // ================= INSERT =================
        public long Insert(CreateBatchRequest request)
        {
            if (ExistsBatchCode(request.BatchCode))
                throw new Exception($"Batch code already exists: {request.BatchCode}");

            const string sql = @"
                INSERT INTO batches
                (batch_code, product_id, import_price,
                 manufacture_date, expiry_date, supplier_name, status)
                VALUES
                (@batchCode, @productId, @importPrice,
                 @manufactureDate, @expiryDate, @supplierName, @status);

                SELECT SCOPE_IDENTITY();
            ";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@batchCode", request.BatchCode);
            cmd.Parameters.AddWithValue("@productId", request.ProductId);
            cmd.Parameters.AddWithValue("@importPrice", request.ImportPrice);
            cmd.Parameters.AddWithValue("@manufactureDate",
                (object?)request.ManufactureDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@expiryDate",
                (object?)request.ExpiryDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@supplierName",
                (object?)request.SupplierName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", request.Status);

            conn.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        // ================= UPDATE =================
        public void Update(long batchId, UpdateBatchRequest request)
        {
            const string sql = @"
                UPDATE batches
                SET expiry_date = @expiryDate,
                    status = @status
                WHERE id = @id
            ";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@expiryDate",
                (object?)request.ExpiryDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", request.Status);
            cmd.Parameters.AddWithValue("@id", batchId);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                throw new Exception($"Batch not found with id: {batchId}");
        }

        // ================= EXISTS =================
        public bool ExistsBatchCode(string batchCode)
        {
            const string sql = "SELECT 1 FROM batches WHERE batch_code = @code";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", batchCode);

            conn.Open();
            return cmd.ExecuteScalar() != null;
        }

        // ================= DISABLE =================
        public void Disable(long batchId)
        {
            const string sql = @"
                UPDATE batches
                SET status = 'BLOCKED'
                WHERE id = @id
            ";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", batchId);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                throw new Exception($"Batch not found with id: {batchId}");
        }

        // ================= FIND ALL =================
        public List<BatchResponse> FindAll()
        {
            const string sql = @"
                SELECT
                    b.id,
                    b.batch_code,
                    b.product_id,
                    p.name AS productName,
                    b.import_price,
                    b.manufacture_date,
                    b.expiry_date,
                    b.supplier_name,
                    b.status
                FROM batches b
                JOIN products p ON b.product_id = p.id
                ORDER BY b.expiry_date
            ";

            return QueryList(sql);
        }

        // ================= FIND BY ID =================
        public BatchResponse? FindById(long id)
        {
            const string sql = @"
                SELECT
                    b.id,
                    b.batch_code,
                    b.expiry_date,
                    b.status
                FROM batches b
                WHERE b.id = @id
            ";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            using var rs = cmd.ExecuteReader();

            return rs.Read() ? Map(rs) : null;
        }

        // ================= FIND BY PRODUCT =================
        public List<BatchResponse> FindByProduct(long productId)
        {
            const string sql = @"
                SELECT
                    b.id,
                    b.batch_code,
                    b.product_id,
                    p.name AS productName,
                    b.import_price,
                    b.manufacture_date,
                    b.expiry_date,
                    b.supplier_name,
                    b.status
                FROM batches b
                JOIN products p ON b.product_id = p.id
                WHERE b.product_id = @productId
                ORDER BY b.expiry_date
            ";

            return QueryList(sql, ("@productId", productId));
        }

        // ================= FIND BY BATCH CODE =================
        public List<BatchResponse> FindByBatchCode(string keyword)
        {
            const string sql = @"
                SELECT
                    b.id,
                    b.batch_code,
                    b.product_id,
                    p.name AS productName,
                    b.import_price,
                    b.manufacture_date,
                    b.expiry_date,
                    b.supplier_name,
                    b.status
                FROM batches b
                JOIN products p ON b.product_id = p.id
                WHERE b.batch_code LIKE @code
                ORDER BY b.expiry_date
            ";

            return QueryList(sql, ("@code", $"%{keyword}%"));
        }

        // ================= FIND DETAIL =================
        public BatchResponse? FindDetail(long batchId)
        {
            const string sql = @"
                SELECT
                    b.id,
                    b.batch_code,
                    b.product_id,
                    p.name AS productName,
                    b.import_price,
                    b.manufacture_date,
                    b.expiry_date,
                    b.supplier_name,
                    b.status
                FROM batches b
                JOIN products p ON b.product_id = p.id
                WHERE b.id = @id
            ";

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", batchId);

            conn.Open();
            using var rs = cmd.ExecuteReader();

            return rs.Read() ? Map(rs) : null;
        }

        // ================= HELPER =================
        private List<BatchResponse> QueryList(
            string sql,
            params (string, object)[] parameters)
        {
            var list = new List<BatchResponse>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            foreach (var (name, value) in parameters)
                cmd.Parameters.AddWithValue(name, value);

            conn.Open();
            using var rs = cmd.ExecuteReader();

            while (rs.Read())
                list.Add(Map(rs));

            return list;
        }

        private BatchResponse Map(IDataReader rs)
        {
            return new BatchResponse
            {
                Id = Convert.ToInt64(rs["id"]),
                BatchCode = rs["batch_code"]?.ToString(),
                ProductId = rs["product_id"] == DBNull.Value
                    ? null
                    : Convert.ToInt64(rs["product_id"]),
                ProductName = rs["productName"]?.ToString(),
                ImportPrice = rs["import_price"] == DBNull.Value
                    ? 0
                    : Convert.ToDecimal(rs["import_price"]),
                ManufactureDate = rs["manufacture_date"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(rs["manufacture_date"]),
                ExpiryDate = rs["expiry_date"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(rs["expiry_date"]),
                SupplierName = rs["supplier_name"]?.ToString(),
                Status = rs["status"]?.ToString()
            };
        }
    }
}
