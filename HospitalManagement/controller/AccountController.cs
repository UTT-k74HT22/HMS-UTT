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
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// [CHỨC NĂNG 1] Lấy danh sách tất cả tài khoản
    /// </summary>
    public List<AccountResponse> GetAllAccounts()
    {
        try
        {
            return _accountService.GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy danh sách tài khoản: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// [CHỨC NĂNG 2] Tìm tài khoản theo ID
    /// </summary>
    public Account GetAccountById(long id)
    {
        try
        {
            return _accountService.FindById(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi lấy tài khoản: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// [CHỨC NĂNG 3] Tạo tài khoản mới
    /// FLOW (giống Java):
    /// 1. Validate request (username, password, email, phone)
    /// 2. Check duplicate username/email/phone
    /// 3. Hash password
    /// 4. Create Account
    /// 5. Create UserProfile
    /// 6. Create EmployeeProfile (nếu role = STAFF) hoặc CustomerProfile (nếu role = CUSTOMER)
    /// </summary>
    public void CreateAccount(CreateAccountRequest request)
    {
        try
        {
            Console.WriteLine($"[Controller] CreateAccount: Starting for user={request.Username}");
            _accountService.CreateAccount(request);
            Console.WriteLine($"[Controller] CreateAccount: Completed for user={request.Username}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Controller] CreateAccount: ERROR - {ex}");
            throw new Exception($"Lỗi khi tạo tài khoản: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// [CHỨC NĂNG 4] Cập nhật tài khoản
    /// </summary>
    public void UpdateAccount(long id, RoleType role, bool active)
    {
        try
        {
            _accountService.Update(id, role, active);
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi cập nhật tài khoản: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// [CHỨC NĂNG 5] Xóa tài khoản
    /// </summary>
    public void DeleteAccount(long id)
    {
        try
        {
            _accountService.DeleteById(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi xóa tài khoản: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// [CHỨC NĂNG 6] Kiểm tra username đã tồn tại
    /// </summary>
    public bool ExistsByUsername(string username)
    {
        try
        {
            return _accountService.ExistsByUsername(username);
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi khi kiểm tra username: {ex.Message}", ex);
        }
    }
}