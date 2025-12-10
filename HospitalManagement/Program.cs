using HospitalManagement.configuration;
using HospitalManagement.repository.impl;
using Microsoft.Extensions.Configuration;

internal static class Program
{
    static void Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = builder.Build();
        var dbConfig = new DBConfig(configuration);

        try
        {
            var createdTables = DbInitializer.Initialize(dbConfig.ConnectionString);
            if (createdTables.Any())
            {
                Console.WriteLine("Created: " + string.Join(", ", createdTables));
            }
            else Console.WriteLine("No new tables.");

            // Quick test repository
            var repo = new AccountRepositoryImpl(dbConfig.ConnectionString);
            var accounts = repo.FindAll();
            Console.WriteLine($"Accounts found: {accounts.Count}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + ex);
            Console.ResetColor();
        }

        Console.ReadLine();
    }
}