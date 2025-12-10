using Microsoft.Extensions.Configuration;

namespace HospitalManagement.configuration
{
    public class DBConfig
    {
        public string ConnectionString { get; }

        public DBConfig(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("DefaultConnection not found in config.");
        }
    }
}
