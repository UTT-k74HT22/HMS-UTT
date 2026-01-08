using HospitalManagement.entity;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl;

public class AccountServiceImpl : AccountService
{
    
    private readonly IAccountRepository _accountRepository;
    
    public AccountServiceImpl(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public List<Account> GetAccounts()
    {
        Console.WriteLine("Fetching all accounts");
        return new List<Account>(_accountRepository.FindAll());
    }
}