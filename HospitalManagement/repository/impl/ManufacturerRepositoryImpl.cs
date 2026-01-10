using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using HospitalManagement.configuration;
using HospitalManagement.entity;
using HospitalManagement.dto.response;
using HospitalManagement.repository;

namespace HospitalManagement.repository.impl
{
    public class ManufacturerRepositoryImpl : IManufacturerRepository
    {
        // =====================================================
        // CONNECTION
        // =====================================================
        private readonly string _connectionString;

        public ManufacturerRepositoryImpl(DBConfig dbConfig)
        {
            _connectionString = dbConfig.ConnectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // =====================================================
        // PUBLIC METHODS
        // =====================================================
        public List<Manufacturer> FindAll()
        {
            string sql = @"
                SELECT id, code, name, country, address, phone, email, contact_person,
                       created_at, updated_at
                FROM manufacturers
                ORDER BY id DESC";

            return QueryForList(sql, MapRow);
        }

        public Manufacturer FindById(int id)
        {
            string sql = @"
                SELECT id, code, name, country, address, phone, email, contact_person,
                       created_at, updated_at
                FROM manufacturers
                WHERE id = @id";

            return QueryForObject(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@id", id);
            }, MapRow);
        }
        
        public List<Manufacturer> SearchByCode(string code)
        {
            string sql = @"
        SELECT id, code, name, country, address, phone, email, contact_person, created_at, updated_at
        FROM manufacturers
        WHERE code LIKE @code
        ORDER BY id DESC";

            return QueryForList(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@code", $"%{code}%"); // tìm gần đúng, có thể xóa % để tìm chính xác
            }, MapRow);
        }


        public long Insert(Manufacturer m)
        {
            string sql = @"
                INSERT INTO manufacturers
                (code, name, country, address, phone, email, contact_person)
                OUTPUT INSERTED.id
                VALUES (@code, @name, @country, @address, @phone, @email, @contactPerson)";

            return ExecuteInsertReturnId(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@code", m.Code);
                cmd.Parameters.AddWithValue("@name", m.Name);
                cmd.Parameters.AddWithValue("@country", m.Country);
                cmd.Parameters.AddWithValue("@address", m.Address);
                cmd.Parameters.AddWithValue("@phone", m.Phone);
                cmd.Parameters.AddWithValue("@email", m.Email);
                cmd.Parameters.AddWithValue("@contactPerson", m.ContactPerson);
            });
        }

        public void Update(Manufacturer m)
        {
            string sql = @"
                UPDATE manufacturers
                SET code = @code,
                    name = @name,
                    country = @country,
                    address = @address,
                    phone = @phone,
                    email = @email,
                    contact_person = @contactPerson
                WHERE id = @id";

            ExecuteUpdate(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@code", m.Code);
                cmd.Parameters.AddWithValue("@name", m.Name);
                cmd.Parameters.AddWithValue("@country", m.Country);
                cmd.Parameters.AddWithValue("@address", m.Address);
                cmd.Parameters.AddWithValue("@phone", m.Phone);
                cmd.Parameters.AddWithValue("@email", m.Email);
                cmd.Parameters.AddWithValue("@contactPerson", m.ContactPerson);
                cmd.Parameters.AddWithValue("@id", m.Id);
            });
        }

        public void DeleteById(int id)
        {
            string sql = "DELETE FROM manufacturers WHERE id = @id";

            ExecuteUpdate(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@id", id);
            });
        }

        public bool ExistsByCode(string code)
        {
            string sql = "SELECT 1 FROM manufacturers WHERE code = @code";

            return Exists(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@code", code);
            });
        }

        public List<ManufacturerResponse> FindAllActive()
        {
            string sql = @"
                SELECT id, code, name, country, address, phone, email, contact_person
                FROM manufacturers
                ORDER BY name";

            return QueryForList(sql, MapRowToDto);
        }

        // =====================================================
        // INTERNAL "BASE REPOSITORY" METHODS
        // =====================================================
        private List<T> QueryForList<T>(
            string sql,
            Func<SqlDataReader, T> mapper)
        {
            var result = new List<T>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var rs = cmd.ExecuteReader();

            while (rs.Read())
            {
                result.Add(mapper(rs));
            }

            return result;
        }

        private List<T> QueryForList<T>(
            string sql,
            Action<SqlCommand> paramSetter,
            Func<SqlDataReader, T> mapper)
        {
            var result = new List<T>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            paramSetter?.Invoke(cmd);
            conn.Open();

            using var rs = cmd.ExecuteReader();
            while (rs.Read())
            {
                result.Add(mapper(rs));
            }

            return result;
        }

        private T QueryForObject<T>(
            string sql,
            Action<SqlCommand> paramSetter,
            Func<SqlDataReader, T> mapper)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            paramSetter?.Invoke(cmd);
            conn.Open();

            using var rs = cmd.ExecuteReader();
            if (rs.Read())
            {
                return mapper(rs);
            }

            return default;
        }

        private long ExecuteInsertReturnId(
            string sql,
            Action<SqlCommand> paramSetter)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            paramSetter?.Invoke(cmd);
            conn.Open();

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private int ExecuteUpdate(
            string sql,
            Action<SqlCommand> paramSetter)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            paramSetter?.Invoke(cmd);
            conn.Open();

            return cmd.ExecuteNonQuery();
        }

        private bool Exists(
            string sql,
            Action<SqlCommand> paramSetter)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            paramSetter?.Invoke(cmd);
            conn.Open();

            return cmd.ExecuteScalar() != null;
        }

        // =====================================================
        // MAPPERS
        // =====================================================
        private Manufacturer MapRow(SqlDataReader rs)
        {
            return new Manufacturer
            {
                Id = rs.GetInt32(rs.GetOrdinal("id")),
                Code = rs.GetString(rs.GetOrdinal("code")),
                Name = rs.GetString(rs.GetOrdinal("name")),

                Country = rs.IsDBNull(rs.GetOrdinal("country"))
                    ? null
                    : rs.GetString(rs.GetOrdinal("country")),

                Address = rs.IsDBNull(rs.GetOrdinal("address"))
                    ? null
                    : rs.GetString(rs.GetOrdinal("address")),

                Phone = rs.IsDBNull(rs.GetOrdinal("phone"))
                    ? null
                    : rs.GetString(rs.GetOrdinal("phone")),

                Email = rs.IsDBNull(rs.GetOrdinal("email"))
                    ? null
                    : rs.GetString(rs.GetOrdinal("email")),

                ContactPerson = rs.IsDBNull(rs.GetOrdinal("contact_person"))
                    ? null
                    : rs.GetString(rs.GetOrdinal("contact_person")),

                CreatedAt = rs.IsDBNull(rs.GetOrdinal("created_at"))
                    ? default
                    : rs.GetDateTime(rs.GetOrdinal("created_at")),

                UpdatedAt = rs.IsDBNull(rs.GetOrdinal("updated_at"))
                    ? default
                    : rs.GetDateTime(rs.GetOrdinal("updated_at"))

            };
        }
        
        private ManufacturerResponse MapRowToDto(SqlDataReader rs)
        {
            return new ManufacturerResponse
            {
                Id = rs.GetInt32(rs.GetOrdinal("id")),

                Code = rs.GetString(rs.GetOrdinal("code")),
                Name = rs.GetString(rs.GetOrdinal("name")),

                Country = rs.IsDBNull(rs.GetOrdinal("country"))
                    ? ""
                    : rs.GetString(rs.GetOrdinal("country")),
                
                Address = rs.IsDBNull(rs.GetOrdinal("address"))
                    ? null
                    : rs.GetString(rs.GetOrdinal("address")),

                Phone = rs.IsDBNull(rs.GetOrdinal("phone"))
                    ? ""
                    : rs.GetString(rs.GetOrdinal("phone")),

                Email = rs.IsDBNull(rs.GetOrdinal("email"))
                    ? ""
                    : rs.GetString(rs.GetOrdinal("email")),
                
                ContactPerson = rs.IsDBNull(rs.GetOrdinal("contact_person"))
                    ? null
                    : rs.GetString(rs.GetOrdinal("contact_person")),
            };
        }

    }
}
