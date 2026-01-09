using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using HospitalManagement.service;

namespace HospitalManagement.controller;

/// <summary>
/// Controller cho quản lý tài khoản
/// </summary>
public class AccountController
{
    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService _accountService)
    {
        this._accountService = _accountService;
    }

    /// <summary>
    /// Lấy danh sách tất cả tài khoản
    /// </summary>
    public List<AccountResponse> GetAccounts()
    {
        return _accountService.GetAll();
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
    public void UpdateAccount(long id, RoleType role, bool active)
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