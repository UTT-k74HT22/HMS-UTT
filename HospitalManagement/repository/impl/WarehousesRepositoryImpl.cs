using HospitalManagement.entity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace HospitalManagement.repository.impl
{
    public class WarehousesRepositoryImpl : IWarehousesRepository
    {
        private readonly string _connectionString;

        public WarehousesRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ==================== GET ALL ====================
        public List<Warehouse> GetAll()
        {
            var list = new List<Warehouse>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM warehouses";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapToWarehouse(reader));
            }
            return list;
        }

        // ==================== GET ACTIVE ====================
        public List<Warehouse> GetAllActive()
        {
            var list = new List<Warehouse>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM warehouses WHERE is_active = 1";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapToWarehouse(reader));
            }
            return list;
        }

        // ==================== GET BY CODE ====================
        public Warehouse? GetByCode(string code)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM warehouses WHERE code = @code";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", code);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapToWarehouse(reader) : null;
        }

        // ==================== CREATE ====================
        public void Create(Warehouse warehouse)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                INSERT INTO warehouses
                (code, name, address, phone, manager_name, is_active)
                VALUES
                (@code, @name, @address, @phone, @manager_name, @is_active)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", warehouse.Code);
            cmd.Parameters.AddWithValue("@name", warehouse.Name);
            cmd.Parameters.AddWithValue("@address", (object?)warehouse.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object?)warehouse.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@manager_name", (object?)warehouse.ManagerName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@is_active", warehouse.IsActive);

            cmd.ExecuteNonQuery();
        }

        // ==================== UPDATE ====================
        public void Update(Warehouse warehouse)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                UPDATE warehouses
                SET 
                    name = @name,
                    address = @address,
                    phone = @phone,
                    manager_name = @manager_name,
                    is_active = @is_active,
                    updated_at = @updated_at
                WHERE code = @code";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", warehouse.Code);
            cmd.Parameters.AddWithValue("@name", warehouse.Name);
            cmd.Parameters.AddWithValue("@address", (object?)warehouse.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object?)warehouse.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@manager_name", (object?)warehouse.ManagerName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@is_active", warehouse.IsActive);
            cmd.Parameters.AddWithValue("@updated_at", warehouse.UpdatedAt);

            cmd.ExecuteNonQuery();
        }

        // ==================== SOFT DELETE ====================
        public void SoftDelete(string code)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = @"
                UPDATE warehouses
                SET is_active = 0, updated_at = SYSDATETIME()
                WHERE code = @code";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", code);
            cmd.ExecuteNonQuery();
        }

        // ==================== EXISTS ====================
        public bool ExistsByCode(string code)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT COUNT(1) FROM warehouses WHERE code = @code";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", code);

            return (int)cmd.ExecuteScalar() > 0;
        }

        // ==================== MAPPER ====================
        private Warehouse MapToWarehouse(SqlDataReader reader)
        {
            return new Warehouse
            {
                Id = Convert.ToInt32(reader["id"]),
                Code = reader["code"].ToString()!,
                Name = reader["name"].ToString()!,
                Address = reader["address"] as string,
                Phone = reader["phone"] as string,
                ManagerName = reader["manager_name"] as string,
                IsActive = Convert.ToBoolean(reader["is_active"]),
                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                UpdatedAt = Convert.ToDateTime(reader["updated_at"])
            };
        }
    }
}
