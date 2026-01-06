using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagement.configuration;
using HospitalManagement.service;
using HospitalManagement.service.impl;
using HospitalManagement.view;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;

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
            var services = new ServiceCollection();

            // Đăng ký Configuration để DBConfig có thể dùng
            services.AddSingleton(configuration);

            // Đăng ký DBConfig (Singleton vì cấu hình DB chỉ cần load 1 lần)
            services.AddSingleton<DBConfig>();

            // Đăng ký Repository
            services.AddScoped<IAccountRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new AccountRepositoryImpl(dbConfig.ConnectionString);
            });

            // Đăng ký Service (Giả sử bạn có class AuthService triển khai IAuthService)
            // Scoped: Tạo mới cho mỗi request (hoặc form lifetime), phù hợp logic business
            services.AddScoped<IAuthService, AuthServiceImpl>();

            // Đăng ký Form (Login)
            // Transient: Tạo mới mỗi khi được gọi
            services.AddTransient<LoginForm>();

            // 3. Build Provider
            ServiceProvider = services.BuildServiceProvider();

            try
            {
                // Lấy Login form từ Service Provider (nó sẽ tự động tiêm IAuthService vào)
                var loginForm = ServiceProvider.GetRequiredService<LoginForm>();
                
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