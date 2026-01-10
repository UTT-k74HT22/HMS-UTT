using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagement.controller;
using HospitalManagement.dto.response;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;
using HospitalManagement.service;
using HospitalManagement.service.impl;
using HospitalManagement.view;

namespace HospitalManagement.configuration
{
    /// <summary>
    /// Centralized service configuration để tránh làm phình to Program.cs
    /// Quản lý tất cả các đăng ký DI ở một nơi
    /// </summary>
    public static class ServiceConfigurator
    {
        /// <summary>
        /// Đăng ký tất cả services vào ServiceCollection
        /// </summary>
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuration
            services.AddSingleton(configuration);

            // Database Configuration
            ConfigureDatabase(services);

            // Repositories
            ConfigureRepositories(services);

            // Services
            ConfigureBusinessServices(services);

            // Controllers
            ConfigureControllers(services);

            // Views/Forms
            ConfigureViews(services);

            return services;
        }

        private static void ConfigureDatabase(IServiceCollection services)
        {
            // DBConfig (Singleton vì chỉ cần load 1 lần)
            services.AddSingleton<DBConfig>();
        }

        private static void ConfigureRepositories(IServiceCollection services)
        {
            // Account Repository
            services.AddScoped<IAccountRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new AccountRepositoryImpl(dbConfig.ConnectionString);
            });

            // UserProfile Repository
            services.AddScoped<IUserProfileRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new UserProfileRepositoryImpl(dbConfig.ConnectionString);
            });

            // EmployeeProfile Repository
            services.AddScoped<IEmployeeProfileRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new EmployeeRepositoryImpl(dbConfig.ConnectionString);
            });

            // CustomerProfile Repository
            services.AddScoped<ICustomerProfileRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new CustomerProfileRepositoryImpl(dbConfig.ConnectionString);
            });

            // TODO: Thêm các repository khác
            // services.AddScoped<IProductRepository, ProductRepositoryImpl>();
        }

        private static void ConfigureBusinessServices(IServiceCollection services)
        {
            // Auth Service
            services.AddScoped<IAuthService, AuthServiceImpl>();

            // Account Service (with connection string for transaction)
            services.AddScoped<IAccountService>(provider =>
            {
                var accountRepo = provider.GetRequiredService<IAccountRepository>();
                var userProfileRepo = provider.GetRequiredService<IUserProfileRepository>();
                var employeeProfileRepo = provider.GetRequiredService<IEmployeeProfileRepository>();
                var customerProfileRepo = provider.GetRequiredService<ICustomerProfileRepository>();
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new AccountServiceImpl(accountRepo, userProfileRepo, employeeProfileRepo, 
                    customerProfileRepo, dbConfig.ConnectionString);
            });
            
            // Employee Service
            services.AddScoped<IEmployeeService, EmployeeServiceImpl>();

            // TODO: Thêm các service khác
            // services.AddScoped<ProductService, ProductServiceImpl>();
        }

        private static void ConfigureControllers(IServiceCollection services)
        {
            // Account Controller
            services.AddScoped<AccountController>();
            services.AddScoped<EmployeeController>();

            // TODO: Thêm các controller khác
            // services.AddScoped<EmployeeController>();
            // services.AddScoped<CustomerController>();
            // services.AddScoped<ProductController>();
        }

        private static void ConfigureViews(IServiceCollection services)
        {
            // Login Form (Transient vì có thể tạo mới nhiều lần)
            services.AddTransient<LoginForm>();

            // TODO: Thêm các form/dialog khác nếu cần DI
            // services.AddTransient<AddAccountDialog>();
            // services.AddTransient<EditAccountDialog>();
        }
    }
}

