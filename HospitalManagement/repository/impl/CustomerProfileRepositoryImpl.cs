using HospitalManagement.entity;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class CustomerProfileRepositoryImpl : ICustomerProfileRepository
    {
        private readonly string _connectionString;

        public CustomerProfileRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(SqlConnection conn, CustomerProfile profile)
        {
            Console.WriteLine($"[CustomerProfileRepo] Insert: Inserting customer profile_id={profile.ProfileId}");
            string sql = """
                INSERT INTO customer_profiles (profile_id, customer_type, tax_code, created_at, updated_at)
                VALUES (@profile_id, @customer_type, @tax_code, @created_at, @updated_at)
                """;

            using var command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@profile_id", profile.ProfileId);
            command.Parameters.AddWithValue("@customer_type", profile.CustomerType);
            command.Parameters.AddWithValue("@tax_code", (object?)profile.TaxCode ?? DBNull.Value);
            command.Parameters.AddWithValue("@created_at", DateTime.UtcNow);
            command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);

            command.ExecuteNonQuery();
            Console.WriteLine($"[CustomerProfileRepo] Insert: Customer profile inserted");
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

        public CustomerProfile? FindByProfileId(long profileId)
        {
            string sql = """
                SELECT id, profile_id, customer_type, tax_code, created_at, updated_at
                FROM customer_profiles
                WHERE profile_id = @profile_id AND deleted_at IS NULL
                """;

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@profile_id", profileId);
            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return MapToEntity(reader);
            }
            return null;
        }

        public void Update(SqlConnection conn, CustomerProfile profile)
        {
            string sql = """
                UPDATE customer_profiles
                SET customer_type = @customer_type,
                    tax_code = @tax_code,
                    updated_at = @updated_at
                WHERE profile_id = @profile_id
                """;

            using var command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@customer_type", profile.CustomerType);
            command.Parameters.AddWithValue("@tax_code", (object?)profile.TaxCode ?? DBNull.Value);
            command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
            command.Parameters.AddWithValue("@profile_id", profile.ProfileId);

            command.ExecuteNonQuery();
        }

        private CustomerProfile MapToEntity(SqlDataReader reader)
        {
            return new CustomerProfile
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                ProfileId = reader.GetInt32(reader.GetOrdinal("profile_id")),
                CustomerType = reader.GetString(reader.GetOrdinal("customer_type")),
                TaxCode = reader.IsDBNull(reader.GetOrdinal("tax_code")) ? null : reader.GetString(reader.GetOrdinal("tax_code")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
            };
        }
    }
}
