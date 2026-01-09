using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl;

/// <summary>
/// Service implementation cho quản lý tài khoản
/// </summary>
public class AccountServiceImpl : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    
    public AccountServiceImpl(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public List<AccountResponse> GetAll()
    {
        Console.WriteLine("Fetching all accounts");
        var accounts = _accountRepository.FindAll();
        return accounts.Select(a => new AccountResponse
        {
            Id = a.Id,
            Username = a.Username,
            Role = a.Role,
            Active = a.IsActive,
            LastLoginAt = a.LastLoginAt
        }).ToList();
    }

    public Account FindByUsername(string username)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public Account FindById(long id)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public void Update(long accountId, RoleType role, bool active)
    {
        Console.WriteLine("Updating account");
        var account = _accountRepository.FindById(accountId);
        if (account == null)
        {
            throw new Exception("Account not found");
        }

        if (role == RoleType.ADMIN && !active)
        {
            // Kiểm tra nếu có ít nhất một tài khoản ADMIN khác đang hoạt động
            var adminAccount = _accountRepository.FindAll()
                .Where(a => a.Role == RoleType.ADMIN && a.IsActive && a.Id != accountId)
                .ToList();

            if (adminAccount.Count == 0)
            {
                throw new Exception("Cannot deactivate the last active ADMIN account");
            }
        }
        
        _accountRepository.UpdateRoleAndStatus(accountId, role, active);
    }

    public void DeleteById(long id)
    {
        Console.WriteLine("Deleting account");
        // Validate account exists
        var account = FindById(id);
            
        // Business rule: Không thể xóa tài khoản ADMIN cuối cùng
        if (account.Role == RoleType.ADMIN)
        {
            var adminAccounts = _accountRepository.FindAll()
                .Where(a => a.Role == RoleType.ADMIN && a.Id != id)
                .ToList();
                
            if (adminAccounts.Count == 0)
            {
                throw new Exception("Không thể xóa tài khoản ADMIN cuối cùng");
            }
        }
            
        _accountRepository.DeleteById(id);
    }

    public bool ExistsByUsername(string username)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}