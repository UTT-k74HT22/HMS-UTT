using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl;

/// <summary>
/// Service implementation cho quản lý tài khoản
/// </summary>
public class AccountServiceImpl : AccountService, IAccountService
{
    private readonly IAccountRepository _accountRepository;
    
    public AccountServiceImpl(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public List<Account> GetAll()
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public List<AccountResponse> GetAllAccount()
    {
        // TODO: Implement
        throw new NotImplementedException();
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

    public void Update(long accountId, Role role, bool active)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public void DeleteById(long id)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public bool ExistsByUsername(string username)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
    
    public List<Account> GetAccounts()
    {
        Console.WriteLine("Fetching all accounts");
        return new List<Account>(_accountRepository.FindAll());
    }
}