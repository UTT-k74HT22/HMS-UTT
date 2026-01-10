using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.service.impl;

/// <summary>
/// Service implementation cho quản lý tài khoản
/// </summary>
public class AccountServiceImpl : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IEmployeeProfileRepository _employeeProfileRepository;
    private readonly ICustomerProfileRepository _customerProfileRepository;
    private readonly string _connectionString;
    
    public AccountServiceImpl(
        IAccountRepository accountRepository,
        IUserProfileRepository userProfileRepository,
        IEmployeeProfileRepository employeeProfileRepository,
        ICustomerProfileRepository customerProfileRepository,
        string connectionString)
    {
        _accountRepository = accountRepository;
        _userProfileRepository = userProfileRepository;
        _employeeProfileRepository = employeeProfileRepository;
        _customerProfileRepository = customerProfileRepository;
        _connectionString = connectionString;
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
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username không được để trống");

        var account = _accountRepository.FindByUsername(username);
        if (account == null)
            throw new Exception($"Không tìm thấy tài khoản với username: {username}");

        return account;
    }

    public Account FindById(long id)
    {
        var account = _accountRepository.FindById(id);
        if (account == null)
            throw new Exception($"Không tìm thấy tài khoản với ID: {id}");

        return account;
    }

    /// <summary>
    /// Tạo tài khoản mới với cascade: Account -> UserProfile -> EmployeeProfile/CustomerProfile
    /// Logic giống Java AccountServiceImpl
    /// </summary>
    public void CreateAccount(CreateAccountRequest request)
    {
        // 1. Validate request
        ValidateCreateRequest(request);

        // 2. Check duplicate
        if (_accountRepository.ExistsByUsername(request.Username))
            throw new Exception($"Username [{request.Username}] đã tồn tại");

        if (!string.IsNullOrEmpty(request.Email) && _userProfileRepository.ExistsByEmail(request.Email))
            throw new Exception($"Email [{request.Email}] đã tồn tại");

        if (!string.IsNullOrEmpty(request.Phone) && _userProfileRepository.ExistsByPhone(request.Phone))
            throw new Exception($"SĐT [{request.Phone}] đã tồn tại");

        // 3. Hash password (simplified, nên dùng BCrypt trong production)
        string hashedPassword = HashPassword(request.Password);

        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            // 4. Create Account
            var account = new Account
            {
                Username = request.Username,
                Password = hashedPassword,
                Role = request.Role,
                IsActive = request.Active
            };

            long accountId = _accountRepository.Insert(connection, account);

            // 5. Create UserProfile
            string codePrefix = request.Role == RoleType.EMPLOYEE ? "EMP" : "CUS";
            string code = _userProfileRepository.GenerateCode(codePrefix);

            var userProfile = new UserProfile
            {
                AccountId = (int)accountId,
                Code = code,
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                Status = ProfileStatus.ACTIVE.ToString()
            };

            long profileId = _userProfileRepository.Insert(connection, userProfile);

            // 6. Create EmployeeProfile or CustomerProfile based on role
            if (request.Role == RoleType.EMPLOYEE)
            {
                // Create EmployeeProfile with default values
                _employeeProfileRepository.Insert(
                    connection,
                    profileId,
                    position: "Nhân viên",
                    department: "Chưa phân bộ phận",
                    hiredDate: DateTime.Now,
                    baseSalary: 0
                );
            }
            else if (request.Role == RoleType.CUSTOMER)
            {
                // Create CustomerProfile with default values
                var customerProfile = new CustomerProfile
                {
                    ProfileId = (int)profileId,
                    CustomerType = "RETAIL",
                    TaxCode = null
                };
                _customerProfileRepository.Insert(connection, customerProfile);
            }

            transaction.Commit();
            Console.WriteLine($"✓ Tạo tài khoản thành công: {request.Username} ({request.Role})");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Lỗi khi tạo tài khoản: {ex.Message}", ex);
        }
    }

    private void ValidateCreateRequest(CreateAccountRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username không được để trống");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password không được để trống");

        if (request.Password != request.ConfirmPassword)
            throw new ArgumentException("Password và Confirm Password không khớp");

        if (request.Password.Length < 6)
            throw new ArgumentException("Password phải có ít nhất 6 ký tự");

        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new ArgumentException("Họ tên không được để trống");

        if (request.Role == RoleType.ADMIN)
            throw new ArgumentException("Không thể tạo tài khoản ADMIN qua form này");
    }

    private string HashPassword(string password)
    {
        // TODO: Implement BCrypt hoặc password hashing thật
        // Tạm thời return plain text (KHÔNG AN TOÀN - chỉ demo)
        return password;
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