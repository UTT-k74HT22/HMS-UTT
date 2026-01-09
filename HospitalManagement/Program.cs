﻿﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagement.configuration;
using HospitalManagement.view;

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
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Xây dựng Configuration (Đọc appsettings.json)
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            // 2. Thiết lập Dependency Injection (Service Collection)
            // Sử dụng ServiceConfigurator để tránh phình to Program.cs
            var services = new ServiceCollection();
            services.ConfigureServices(configuration);

            // 3. Build Provider
            ServiceProvider = services.BuildServiceProvider();

            try
            {
                // Lấy Login form từ Service Provider (nó sẽ tự động tiêm IAuthService vào)
                var loginForm = ServiceProvider.GetRequiredService<LoginForm>();
                
                //TEST
                
                // Chạy ứng dụng
                Application.Run(loginForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi động: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}