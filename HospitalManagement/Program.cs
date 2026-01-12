﻿﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagement.configuration;
using HospitalManagement.view;
using OfficeOpenXml;

namespace HospitalManagement
{
    static class Program
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ExcelPackage.License.SetNonCommercialOrganization("HospitalManagement");
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show(e.Exception.ToString(), "ThreadException",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                MessageBox.Show(e.ExceptionObject?.ToString() ?? "Unknown", "UnhandledException",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Exception: " + e.ExceptionObject);
            };

            // 1. Xây dựng Configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            // 2. DI
            var services = new ServiceCollection();
            services.ConfigureServices(configuration);

            // 3. Build Provider
            ServiceProvider = services.BuildServiceProvider();

            try
            {
                var loginForm = ServiceProvider.GetRequiredService<LoginForm>();
                Application.Run(loginForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi động: {ex}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}