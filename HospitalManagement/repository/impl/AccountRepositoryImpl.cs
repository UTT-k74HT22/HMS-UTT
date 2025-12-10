using HospitalManagement.entity;
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
            var list = new List<Account>();
            const string sql = "SELECT * FROM Accounts";

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public Account? FindByUsername(string username)
        {
            const string sql = "SELECT * FROM Accounts WHERE username = @username";

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public void Save(Account acc)
        {
            const string sql =
                @"INSERT INTO Accounts (username, password, role, is_active)
                  VALUES (@u, @p, @r, @a)";

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", acc.Username);
            cmd.Parameters.AddWithValue("@p", acc.Password);
            cmd.Parameters.AddWithValue("@r", acc.Role);
            cmd.Parameters.AddWithValue("@a", acc.Active);

            cmd.ExecuteNonQuery();
        }

        public void Update(Account acc)
        {
            const string sql =
                @"UPDATE Accounts SET password=@p, role=@r, is_active=@a WHERE id=@id";

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@p", acc.Password);
            cmd.Parameters.AddWithValue("@r", acc.Role);
            cmd.Parameters.AddWithValue("@a", acc.Active);
            cmd.Parameters.AddWithValue("@id", acc.Id);

            cmd.ExecuteNonQuery();
        }

        public void DeleteById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM Accounts WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public bool ExistsByUsername(string username)
        {
            const string sql = "SELECT 1 FROM Accounts WHERE username = @username";

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

        private Account Map(SqlDataReader rs)
        {
            return new Account
            {
                Id = rs.GetInt32(rs.GetOrdinal("id")),
                Username = rs.GetString(rs.GetOrdinal("username")),
                Password = rs.GetString(rs.GetOrdinal("password")),
                Fullname = rs.IsDBNull(rs.GetOrdinal("fullname")) ? "" : rs.GetString(rs.GetOrdinal("fullname")),
                Role = rs.GetString(rs.GetOrdinal("role")),
                Active = rs.GetBoolean(rs.GetOrdinal("is_active"))
            };
        }
    }
}
