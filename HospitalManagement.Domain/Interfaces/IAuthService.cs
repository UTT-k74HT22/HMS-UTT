using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces;

public interface IAuthService
{
    Task<Account?> LoginAsync(string username, string passwordPlain);
    Task<Account> RegisterAsync(string username, string passwordPlain, string role);
}
