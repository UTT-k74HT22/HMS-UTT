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
            Console.WriteLine($"[Repo] Insert: Inserting account username={account.Username}");
            string sql = """
                         INSERT INTO accounts (username, password, role, is_active, created_at, updated_at)
                         OUTPUT INSERTED.id
                         VALUES (@username, @password, @role, @is_active, @created_at, @updated_at);
                         """;
            using (var command = new SqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@username", account.Username);
                command.Parameters.AddWithValue("@password", account.Password);
                command.Parameters.AddWithValue("@role", account.Role.ToString());
                command.Parameters.AddWithValue("@is_active", account.IsActive);
                command.Parameters.AddWithValue("@created_at", DateTime.UtcNow);
                command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
                
                Console.WriteLine($"[Repo] Insert: Executing SQL insert...");
                var result = command.ExecuteScalar();
                long id = Convert.ToInt64(result);
                Console.WriteLine($"[Repo] Insert: Account inserted with ID={id}");
                return id;
            }
        }
        
        public long Insert(SqlConnection conn, SqlTransaction transaction, Account account)
        {
            Console.WriteLine($"[Repo] Insert (with transaction): Inserting account username={account.Username}");
            string sql = """
                         INSERT INTO accounts (username, password, role, is_active, created_at, updated_at)
                         OUTPUT INSERTED.id
                         VALUES (@username, @password, @role, @is_active, @created_at, @updated_at);
                         """;
            using (var command = new SqlCommand(sql, conn, transaction))
            {
                command.Parameters.AddWithValue("@username", account.Username);
                command.Parameters.AddWithValue("@password", account.Password);
                command.Parameters.AddWithValue("@role", account.Role.ToString());
                command.Parameters.AddWithValue("@is_active", account.IsActive);
                command.Parameters.AddWithValue("@created_at", DateTime.UtcNow);
                command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
                
                Console.WriteLine($"[Repo] Insert: Executing SQL insert with transaction...");
                var result = command.ExecuteScalar();
                long id = Convert.ToInt64(result);
                Console.WriteLine($"[Repo] Insert: Account inserted with ID={id}");
                return id;
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
                UPDATE accounts
                SET is_active = 0
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
            string sql = """
                             UPDATE accounts
                             SET last_login_at = @now
                             WHERE id = @id
                         """;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@now", DateTime.UtcNow);
                command.Parameters.AddWithValue("@id", accountId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        public long? FindUserIdByAccountId(long accountId)
        {
            const string sql = @"
        SELECT id
        FROM user_profiles
        WHERE account_id = @accountId
    ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@accountId", accountId);

            conn.Open();
            var result = cmd.ExecuteScalar();

            if (result == null || result == DBNull.Value)
                return null;

            return Convert.ToInt64(result); // user_profiles.id
        }

        public long CreateUserProfile(long accountId, string username, string role)
        {
            string sql = """
                             INSERT INTO user_profiles
                             (
                                 account_id,
                                 code,
                                 full_name,
                                 status,
                                 created_at,
                                 updated_at
                             )
                             OUTPUT INSERTED.id
                             VALUES
                             (
                                 @accountId,
                                 @code,
                                 @fullName,
                                 'ACTIVE',
                                 SYSDATETIME(),
                                 SYSDATETIME()
                             )
                         """;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            string codePrefix = role == "ADMIN" ? "ADM" :
                role == "EMPLOYEE" ? "EMP" : "CUS";

            cmd.Parameters.AddWithValue("@accountId", accountId);
            cmd.Parameters.AddWithValue("@code", $"{codePrefix}-{accountId}");
            cmd.Parameters.AddWithValue("@fullName", username);

            conn.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
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
