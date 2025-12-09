using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;

    public AuthService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Account?> LoginAsync(string username, string passwordPlain)
    {
        var account = await _accountRepository.GetByUsernameAsync(username);
        
        if (account == null || !account.IsActive)
        {
            return null;
        }

        if (!VerifyPassword(passwordPlain, account.PasswordHash))
        {
            return null;
        }

        return account;
    }

    public async Task<Account> RegisterAsync(string username, string passwordPlain, string role)
    {
        var existingAccount = await _accountRepository.GetByUsernameAsync(username);
        if (existingAccount != null)
        {
            throw new InvalidOperationException("Username ?ã t?n t?i");
        }

        var passwordHash = HashPassword(passwordPlain);
        
        var newAccount = new Account
        {
            Username = username,
            PasswordHash = passwordHash,
            Role = role,
            IsActive = true
        };

        await _accountRepository.AddAsync(newAccount);
        return newAccount;
    }

    private bool VerifyPassword(string passwordPlain, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(passwordPlain, passwordHash);
    }

    private string HashPassword(string passwordPlain)
    {
        return BCrypt.Net.BCrypt.HashPassword(passwordPlain);
    }
}
