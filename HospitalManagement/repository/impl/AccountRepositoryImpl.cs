using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class AccountRepositoryImpl : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Account> FindAll()
        {
            var accounts = new List<Account>();
            string query = """
                           SELECT id, username, password, role, is_active
                           FROM accounts
                           """;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using var command = new SqlCommand(query, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    accounts.Add(Map(reader));
                }
            }
            return accounts;
        }

        public Account FindByUsername(string username)
        {
            string sql = """
                         SELECT id, username, password, role, is_active
                         FROM accounts
                         WHERE username = @username
                         """;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@username", username);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            return null;
        }

        public Account FindById(long id)
        {
            string sql = """
                         SELECT id, username, password, role, is_active
                         FROM accounts
                         WHERE id = @id
                         """;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            return null;        
        }

        public long Insert(SqlConnection conn, Account account)
        {
            string sql = """
                         INSERT INTO accounts (username, password, role, is_active, created_at, updated_at)
                         OUTPUT INSERTED.id
                         VALUES (@username, @password, @role, @is_active, @created_at, @updated_at);
                         """;
            using (var command = new SqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@username", account.Username);
                command.Parameters.AddWithValue("@password", account.Password);
                command.Parameters.AddWithValue("@role", account.Role);
                command.Parameters.AddWithValue("@is_active", account.IsActive);
                command.Parameters.AddWithValue("@created_at", DateTime.UtcNow);
                command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
                return (long)command.ExecuteScalar();
            }
        }

        public void UpdateRoleAndStatus(long id, RoleType role, bool active)
        {
            string sql = """
                         UPDATE accounts
                         SET role = @role, is_active = @is_active, updated_at = @updated_at
                         WHERE id = @id
                         """;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@role", role.ToString());
                command.Parameters.AddWithValue("@is_active", active);
                command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteById(long id)
        {
            string query = @"
                DELETE FROM accounts
                WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }        
        }

        public bool ExistsByUsername(string username)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM accounts 
                WHERE username = @username";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                connection.Open();
                return (int)command.ExecuteScalar() > 0;
            }
        }

        public bool ExistsByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public void UpdatePassword(long accountId, string hashedPassword)
        {
            throw new NotImplementedException();
        }

        public void UpdateLastLogin(long accountId)
        {
            throw new NotImplementedException();
        }

        public long? FindUserIdByAccountId(long accountId)
        {
            string query = @"
                SELECT id 
                FROM user_profiles 
                WHERE account_id = @accountId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@accountId", accountId);
                connection.Open();
                
                var result = command.ExecuteScalar();
                return result != null ? (long?)(int)result : null;
            }
        }
        
        
        private Account Map(SqlDataReader reader)
        {
            return new Account
            {
                Id = Convert.ToInt64(reader["id"]),
                Username = reader.GetString(reader.GetOrdinal("username")),
                Password = reader.GetString(reader.GetOrdinal("password")),
                Role = Enum.Parse<RoleType>(reader.GetString(reader.GetOrdinal("role"))),
                IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                // CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                // LastLoginAt = reader.IsDBNull(reader.GetOrdinal("last_login_at")) 
                //     ? null 
                //     : reader.GetDateTime(reader.GetOrdinal("last_login_at"))
            };
        }
    }
}
