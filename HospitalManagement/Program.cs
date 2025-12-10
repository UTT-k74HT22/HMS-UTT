using System;
using System.Linq;
using System.Windows.Forms;
using HospitalManagement.configuration;
using HospitalManagement.repository.impl;
using HospitalManagement.service.impl;
using HospitalManagement.controller;
using HospitalManagement.view.Auth;
using Microsoft.Extensions.Configuration;

namespace HospitalManagement
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Console.WriteLine("=== START SYSTEM ===");

            // 1. Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2. DB Config
            var dbConfig = new DBConfig(configuration);

            try
            {
                // 3. Init DB + Create tables if not exist
                Console.WriteLine("Checking database...");

                var createdTables = DbInitializer.Initialize(dbConfig.ConnectionString);

                if (createdTables.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✅ Tables created:");
                    foreach (var table in createdTables)
                    {
                        Console.WriteLine(" - " + table);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("✅ Tables already exist.");
                }

                Console.ResetColor();

                // 4. Init Repository
                var accountRepo = new AccountRepositoryImpl(dbConfig.ConnectionString);

                // 5. Init Service
                var authService = new AuthServiceImpl(accountRepo);

                // 6. Init Controller
                var authController = new AuthController(authService);

                // 7. Start UI
                Application.Run(new LoginForm(authController));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ SYSTEM START FAILED:");
                Console.WriteLine(ex.Message);
                Console.ResetColor();

                MessageBox.Show(
                    "System failed to start:\n" + ex.Message,
                    "Startup Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
