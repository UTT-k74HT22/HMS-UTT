using HospitalManagement.entity;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class UserProfileRepositoryImpl : IUserProfileRepository
    {
        private readonly string _connectionString;

        public UserProfileRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public long Insert(SqlConnection conn, UserProfile profile)
        {
            string sql = """
                INSERT INTO user_profiles (account_id, code, full_name, phone, email, address, status, created_at, updated_at)
                OUTPUT INSERTED.id
                VALUES (@account_id, @code, @full_name, @phone, @email, @address, @status, @created_at, @updated_at)
                """;

            using var command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@account_id", profile.AccountId);
            command.Parameters.AddWithValue("@code", profile.Code);
            command.Parameters.AddWithValue("@full_name", profile.FullName);
            command.Parameters.AddWithValue("@phone", (object?)profile.Phone ?? DBNull.Value);
            command.Parameters.AddWithValue("@email", (object?)profile.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@address", (object?)profile.Address ?? DBNull.Value);
            command.Parameters.AddWithValue("@status", profile.Status);
            command.Parameters.AddWithValue("@created_at", DateTime.UtcNow);
            command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);

            return (long)(int)command.ExecuteScalar();
        }

        public UserProfile? FindByAccountId(long accountId)
        {
            string sql = """
                SELECT id, account_id, code, full_name, phone, email, address, status, created_at, updated_at
                FROM user_profiles
                WHERE account_id = @account_id
                """;

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@account_id", accountId);
            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return MapToEntity(reader);
            }
            return null;
        }

        public UserProfile? FindById(long id)
        {
            string sql = """
                SELECT id, account_id, code, full_name, phone, email, address, status, created_at, updated_at
                FROM user_profiles
                WHERE id = @id
                """;

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return MapToEntity(reader);
            }
            return null;
        }

        public void Update(SqlConnection conn, UserProfile profile)
        {
            string sql = """
                UPDATE user_profiles
                SET full_name = @full_name, 
                    phone = @phone, 
                    email = @email, 
                    address = @address, 
                    status = @status,
                    updated_at = @updated_at
                WHERE id = @id
                """;

            using var command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@full_name", profile.FullName);
            command.Parameters.AddWithValue("@phone", (object?)profile.Phone ?? DBNull.Value);
            command.Parameters.AddWithValue("@email", (object?)profile.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@address", (object?)profile.Address ?? DBNull.Value);
            command.Parameters.AddWithValue("@status", profile.Status);
            command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
            command.Parameters.AddWithValue("@id", profile.Id);

            command.ExecuteNonQuery();
        }

        public bool ExistsByEmail(string email)
        {
            string sql = """
                SELECT COUNT(*) 
                FROM user_profiles 
                WHERE email = @email
                """;

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            return (int)command.ExecuteScalar() > 0;
        }

        public bool ExistsByPhone(string phone)
        {
            string sql = """
                SELECT COUNT(*) 
                FROM user_profiles 
                WHERE phone = @phone
                """;

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@phone", phone);

            return (int)command.ExecuteScalar() > 0;
        }

        public string GenerateCode(string prefix)
        {
            string sql = """
                SELECT MAX(CAST(SUBSTRING(code, LEN(@prefix) + 1, LEN(code)) AS INT))
                FROM user_profiles
                WHERE code LIKE @prefix + '%'
                """;

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@prefix", prefix);

            var result = command.ExecuteScalar();
            int nextNumber = (result == DBNull.Value || result == null) ? 1 : (int)result + 1;

            return $"{prefix}{nextNumber:D4}";
        }

        private UserProfile MapToEntity(SqlDataReader reader)
        {
            return new UserProfile
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                AccountId = reader.GetInt32(reader.GetOrdinal("account_id")),
                Code = reader.GetString(reader.GetOrdinal("code")),
                FullName = reader.GetString(reader.GetOrdinal("full_name")),
                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address")),
                Status = reader.GetString(reader.GetOrdinal("status")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
            };
        }
    }
}
