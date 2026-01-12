using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagement.controller;
using HospitalManagement.configuration;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;
using HospitalManagement.service;
using HospitalManagement.service.impl;
using HospitalManagement.Service.Impl;
using HospitalManagement.utils.importer.services;
using HospitalManagement.view;

namespace HospitalManagement.configuration
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ================= CONFIG =================
            services.AddSingleton(configuration);
            services.AddSingleton<DBConfig>();

            // ================= REPOSITORIES =================

            services.AddScoped<IAccountRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new AccountRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<IUserProfileRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new UserProfileRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<IEmployeeProfileRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new EmployeeRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<ICustomerProfileRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new CustomerProfileRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<IInventoryRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new InventoryRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<IWarehousesRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new WarehousesRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<IProductRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new ProductRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<ICategoryRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new CategoryRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<IManufacturerRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new ManufacturerRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<IBatchRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new BatchRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<IStockMovementRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new StockMovementRepositoryImpl(db.ConnectionString);
            });

            services.AddScoped<IOrderRepository>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new OrderRepositoryImpl(db.ConnectionString);
            });

            // ================= SERVICES =================

            services.AddScoped<IAuthService, AuthServiceImpl>();

            services.AddScoped<IAccountService>(p =>
            {
                var accountRepo = p.GetRequiredService<IAccountRepository>();
                var userRepo = p.GetRequiredService<IUserProfileRepository>();
                var empRepo = p.GetRequiredService<IEmployeeProfileRepository>();
                var cusRepo = p.GetRequiredService<ICustomerProfileRepository>();
                var db = p.GetRequiredService<DBConfig>();

                return new AccountServiceImpl(
                    accountRepo,
                    userRepo,
                    empRepo,
                    cusRepo,
                    db.ConnectionString
                );
            });

            services.AddScoped<IAuthService, AuthServiceImpl>();
            services.AddScoped<IEmployeeService, EmployeeServiceImpl>();
            services.AddScoped<IInventoryService, InventoryServiceImpl>();
            services.AddScoped<IWarehousesService, WarehousesServiceImpl>();
            services.AddScoped<IStockMovementService, StockMovementServiceImpl>();
            services.AddScoped<IProductService, ProductServiceImpl>();
            
            services.AddScoped<IBatchService>(p =>
            {
                var repo = p.GetRequiredService<IBatchRepository>();
                return new BatchServiceImpl(repo);
            });

            services.AddScoped<ICategoryService>(p =>
            {
                var repo = p.GetRequiredService<ICategoryRepository>();
                return new CategoryServiceImpl(repo);
            });

            services.AddScoped<IOrderService>(p =>
            {
                var db = p.GetRequiredService<DBConfig>();
                return new OrderServiceImpl(db.ConnectionString);
            });

            // ================= IMPORT SERVICES =================
            
            services.AddScoped<ProductImportService>();
            services.AddScoped<utils.importer.service.StockMovementImportService>();

            // ================= CONTROLLERS =================

            services.AddScoped<AccountController>();
            services.AddScoped<EmployeeController>();
            services.AddScoped<InventoryController>();
            services.AddScoped<WarehousesController>();
            services.AddScoped<ProductController>();
            services.AddScoped<CategoryController>(); 
            services.AddScoped<BatchController>();
            services.AddScoped<StockMovementController>();
            services.AddScoped<OrderController>();
            

            // ================= VIEWS =================

            services.AddTransient<LoginForm>();
            services.AddTransient<ProductManagementPanel>(); 
            services.AddTransient<BatchManagementPanel>();
            services.AddTransient<CreateOrderForm>();

            return services;
        }
    }
}
