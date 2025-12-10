using System.Data;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.configuration
{
    public static class DbInitializer
    {
        public static List<string> Initialize(string connectionString)
        {
            var createdTables = new List<string>();

            using var conn = new SqlConnection(connectionString);
            conn.Open();

            var commands = new Dictionary<string, string>
            {
                {
                    "Accounts", """
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Accounts')
                    BEGIN
                        CREATE TABLE Accounts (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            username NVARCHAR(50) UNIQUE NOT NULL,
                            password NVARCHAR(100) NOT NULL,
                            fullname NVARCHAR(100) NULL,
                            role NVARCHAR(20) NOT NULL,
                            is_active BIT NOT NULL DEFAULT 1,
                            created_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
                            updated_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
                        );
                    END
                    """
                }
                // add other tables here
            };

            foreach (var kv in commands)
            {
                var name = kv.Key;
                var sql = kv.Value;

                using var checkCmd = new SqlCommand($"SELECT COUNT(*) FROM sys.tables WHERE name = '{name}'", conn);
                var existedBefore = (int)checkCmd.ExecuteScalar() > 0;

                using var cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                using var checkAfterCmd = new SqlCommand($"SELECT COUNT(*) FROM sys.tables WHERE name = '{name}'", conn);
                var existedAfter = (int)checkAfterCmd.ExecuteScalar() > 0;

                if (!existedBefore && existedAfter) createdTables.Add(name);
            }

            return createdTables;
        }
    }
}
