using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;

namespace HospitalManagement.repository.impl
{
    public class CustomerProfileRepositoryImpl : ICustomerProfileRepository
    {
        private readonly string _connectionString;

        public CustomerProfileRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public string ConnectionString => _connectionString;

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        
        public void Insert(SqlConnection conn, SqlTransaction transaction, CustomerProfile profile)
        {
            Console.WriteLine($"[CustomerProfileRepo] Insert (with transaction): Inserting customer profile_id={profile.ProfileId}");
            string sql = """
                         INSERT INTO customer_profiles (profile_id, customer_type, tax_code, created_at, updated_at)
                         VALUES (@profile_id, @customer_type, @tax_code, @created_at, @updated_at)
                         """;

            using var command = new SqlCommand(sql, conn, transaction);
            command.Parameters.AddWithValue("@profile_id", profile.ProfileId);
            command.Parameters.AddWithValue("@customer_type", profile.CustomerType);
            command.Parameters.AddWithValue("@tax_code", (object?)profile.TaxCode ?? DBNull.Value);
            command.Parameters.AddWithValue("@created_at", DateTime.UtcNow);
            command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);

            command.ExecuteNonQuery();
            Console.WriteLine($"[CustomerProfileRepo] Insert: Customer profile inserted");
        }

        /* ==================== 1) Get all customers ==================== */
        public List<CustomerProfileResponse> GetAll()
        {
            var list = new List<CustomerProfileResponse>();
            var sql = @"
                SELECT up.id AS profile_id, up.code, up.full_name, up.phone, up.email, up.address, up.status,
                       cp.customer_type, cp.tax_code
                FROM dbo.user_profiles up
                INNER JOIN dbo.customer_profiles cp ON cp.profile_id = up.id";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(MapToResponse(reader));
                    }
                }
            }

            return list;
        }

        /* ==================== 2) Get by Profile ID ==================== */
        public CustomerProfileResponse? GetByProfileId(int profileId)
        {
            var sql = @"
                SELECT up.id AS profile_id, up.code, up.full_name, up.phone, up.email, up.address, up.status,
                       cp.customer_type, cp.tax_code
                FROM dbo.user_profiles up
                INNER JOIN dbo.customer_profiles cp ON cp.profile_id = up.id
                WHERE up.id = @profileId";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@profileId", profileId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapToResponse(reader);
                }
            }

            return null;
        }

        /* ==================== 3) Get by Code ==================== */
        public CustomerProfileResponse? GetByCode(string code)
        {
            var sql = @"
                SELECT up.id AS profile_id, up.code, up.full_name, up.phone, up.email, up.address, up.status,
                       cp.customer_type, cp.tax_code
                FROM dbo.user_profiles up
                INNER JOIN dbo.customer_profiles cp ON cp.profile_id = up.id
                WHERE up.code = @code";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@code", code);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapToResponse(reader);
                }
            }

            return null;
        }

        /* ==================== 4) Update customer ==================== */
        public bool Update(CustomerProfileResponse model)
        {
            var sql = @"
                UPDATE up
                SET full_name = @fullName,
                    phone = @phone,
                    email = @email,
                    address = @address,
                    status = @status
                FROM dbo.user_profiles up
                WHERE up.id = @profileId;

                UPDATE cp
                SET customer_type = @customerType,
                    tax_code = @taxCode
                FROM dbo.customer_profiles cp
                WHERE cp.profile_id = @profileId;
            ";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@profileId", model.ProfileId);
                cmd.Parameters.AddWithValue("@fullName", model.FullName);
                cmd.Parameters.AddWithValue("@phone", (object?)model.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@email", (object?)model.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@address", (object?)model.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", model.Status.ToString());
                cmd.Parameters.AddWithValue("@customerType", model.CustomerType.ToString());
                cmd.Parameters.AddWithValue("@taxCode", (object?)model.TaxCode ?? DBNull.Value);

                conn.Open();
                var affected = cmd.ExecuteNonQuery();
                return affected > 0;
            }
        }

        /* ==================== 5) Soft delete ==================== */
        public bool SoftDelete(int profileId)
        {
            var sql = @"UPDATE dbo.user_profiles
                        SET status = 'INACTIVE'
                        WHERE id = @profileId";

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@profileId", profileId);
                conn.Open();
                var affected = cmd.ExecuteNonQuery();
                return affected > 0;
            }
        }

        /* ==================== Mapping ==================== */
        private CustomerProfileResponse MapToResponse(SqlDataReader reader)
        {
            return new CustomerProfileResponse
            {
                ProfileId = reader.GetInt32(reader.GetOrdinal("profile_id")),
                Code = reader.GetString(reader.GetOrdinal("code")),
                FullName = reader.GetString(reader.GetOrdinal("full_name")),
                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address")),
                Status = Enum.Parse<ProfileStatus>(reader.GetString(reader.GetOrdinal("status"))),
                CustomerType = Enum.Parse<CustomerType>(reader.GetString(reader.GetOrdinal("customer_type"))),
                TaxCode = reader.IsDBNull(reader.GetOrdinal("tax_code")) ? null : reader.GetString(reader.GetOrdinal("tax_code"))
            };
        }
    }
}
