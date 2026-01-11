using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagement.controller;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;
using HospitalManagement.service;
using HospitalManagement.service.impl;
using HospitalManagement.Service.Impl;
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

            // Inventory & Product Repositories
            services.AddScoped<IInventoryRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new InventoryRepositoryImpl(dbConfig.ConnectionString);
            });

            services.AddScoped<IWarehousesRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new WarehousesRepositoryImpl(dbConfig.ConnectionString);
            });

            services.AddScoped<IProductRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new ProductRepositoryImpl(dbConfig.ConnectionString);
            });

            services.AddScoped<ICategoryRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new CategoryRepositoryImpl(dbConfig.ConnectionString);
            });

            services.AddScoped<IManufacturerRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new ManufacturerRepositoryImpl(dbConfig.ConnectionString);
            });

            services.AddScoped<IBatchRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new BatchRepositoryImpl(dbConfig.ConnectionString);
            });

            services.AddScoped<IStockMovementRepository>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new StockMovementRepositoryImpl(dbConfig.ConnectionString);
            });
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

            // Inventory & Product Services
            services.AddScoped<IInventoryService, InventoryServiceImpl>();
            services.AddScoped<IWarehousesService, WarehousesServiceImpl>();
            
            services.AddScoped<IProductService>(provider =>
            {
                var dbConfig = provider.GetRequiredService<DBConfig>();
                return new ProductServiceImpl(dbConfig.ConnectionString);
            });
            
            services.AddScoped<IBatchService>(provider =>
            {
                var batchRepo = provider.GetRequiredService<IBatchRepository>();
                return new BatchServiceImpl(batchRepo);
            });
            
            services.AddScoped<IStockMovementService, StockMovementServiceImpl>();
        }

        private static void ConfigureControllers(IServiceCollection services)
        {
            // Account & Employee Controllers
            services.AddScoped<AccountController>();
            services.AddScoped<EmployeeController>();
            
            // Inventory & Stock Management Controllers
            services.AddScoped<InventoryController>();
            services.AddScoped<WarehousesController>();
            
            services.AddScoped<ProductController>(provider =>
            {
                var productService = provider.GetRequiredService<IProductService>();
                return new ProductController(productService);
            });
            
            services.AddScoped<BatchController>(provider =>
            {
                var batchService = provider.GetRequiredService<IBatchService>();
                var productService = provider.GetRequiredService<IProductService>();
                return new BatchController(batchService, productService);
            });
            
            services.AddScoped<StockMovementController>();
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

