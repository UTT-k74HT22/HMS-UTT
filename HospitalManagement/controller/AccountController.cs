using HospitalManagement.entity;
using HospitalManagement.repository.impl;
using HospitalManagement.service;
using HospitalManagement.service.impl;

namespace HospitalManagement.controller;

public class AccountController
{
    private readonly AccountService _accountService;
    
    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Lấy danh sách tất cả tài khoản
    /// </summary>
    public List<Account> GetAccounts()
    {
        return _accountService.GetAccounts();
    }
}