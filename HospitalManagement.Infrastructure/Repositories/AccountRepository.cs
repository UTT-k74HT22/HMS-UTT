using Microsoft.EntityFrameworkCore;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;

namespace HospitalManagement.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly HospitalDbContext _context;

    public AccountRepository(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByUsernameAsync(string username)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.Username == username);
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }
}
