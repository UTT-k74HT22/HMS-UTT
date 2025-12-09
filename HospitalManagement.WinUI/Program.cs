using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Repositories;
using HospitalManagement.Application.Services;
using HospitalManagement.WinUI.Views;
using HospitalManagement.WinUI.Controllers;

namespace HospitalManagement.WinUI;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();
        ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();

        var dbContext = serviceProvider.GetRequiredService<HospitalDbContext>();
        dbContext.Database.EnsureCreated();

        var authController = serviceProvider.GetRequiredService<IAuthController>();
        authController.ShowLogin();

        Application.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<HospitalDbContext>(options =>
            options.UseSqlite("Data Source=hospital.db"));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddTransient<LoginForm>();
        services.AddTransient<RegisterForm>();
        services.AddTransient<Func<string, string, MainForm>>(sp => 
            (username, role) => new MainForm(username, role));

        services.AddSingleton<IAuthController, AuthController>();
    }    
}