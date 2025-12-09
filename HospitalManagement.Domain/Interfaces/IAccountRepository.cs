using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByUsernameAsync(string username);
    Task AddAsync(Account account);
}
