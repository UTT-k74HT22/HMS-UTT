using Microsoft.EntityFrameworkCore;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data;

public class HospitalDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }

    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Username).IsUnique();
        });

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
        
        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                Id = 1,
                Username = "admin",
                PasswordHash = adminPasswordHash,
                Role = "ADMIN",
                IsActive = true
            }
        );
    }
}
