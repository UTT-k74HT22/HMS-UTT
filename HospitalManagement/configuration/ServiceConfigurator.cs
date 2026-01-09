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

            services.AddScoped<IEmployeeProfileRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new EmployeeRepositoryImpl(dbConfig.ConnectionString);
            });

            // TODO: Thêm các repository khác
            // services.AddScoped<IEmployeeRepository, EmployeeRepositoryImpl>();
            // services.AddScoped<ICustomerRepository, CustomerRepositoryImpl>();
            // services.AddScoped<IProductRepository, ProductRepositoryImpl>();
        }

        private static void ConfigureBusinessServices(IServiceCollection services)
        {
            // Auth Service
            services.AddScoped<IAuthService, AuthServiceImpl>();

            // Account Service
            services.AddScoped<AccountService, AccountServiceImpl>();
            
            // Employee Service
            services.AddScoped<EmployeeService, EmployeeServiceImpl>();

            // TODO: Thêm các service khác
            // services.AddScoped<EmployeeService, EmployeeServiceImpl>();
            // services.AddScoped<CustomerService, CustomerServiceImpl>();
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

