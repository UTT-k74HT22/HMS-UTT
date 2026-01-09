using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.service;

namespace HospitalManagement.controller;

/// <summary>
/// Controller cho quản lý tài khoản
/// </summary>
public class AccountController
{
    private readonly AccountService _accountService;
    private readonly IAccountService _newAccountService;
    
    public AccountController(AccountService accountService, IAccountService newAccountService)
    {
        _accountService = accountService;
        _newAccountService = newAccountService;
    }

    /// <summary>
    /// Lấy danh sách tất cả tài khoản
    /// </summary>
    public List<Account> GetAccounts()
    {
        return _accountService.GetAccounts();
    }

    /// <summary>
    /// Lấy tất cả tài khoản (mới)
    /// </summary>
    public List<AccountResponse> GetAllAccounts()
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Tìm tài khoản theo ID
    /// </summary>
    public Account GetAccountById(long id)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Tạo tài khoản mới
    /// </summary>
    public void CreateAccount(CreateAccountRequest request)
    {
        // TODO: Implement
        // 1. Validate request
        // 2. Check duplicate username/email
        // 3. Hash password
        // 4. Create account
        // 5. Create user profile
        throw new NotImplementedException();
    }

    /// <summary>
    /// Cập nhật tài khoản
    /// </summary>
    public void UpdateAccount(long id, Role role, bool active)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Xóa tài khoản
    /// </summary>
    public void DeleteAccount(long id)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}