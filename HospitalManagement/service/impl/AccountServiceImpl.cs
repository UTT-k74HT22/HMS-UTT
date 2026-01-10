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
    /// </summary>
    public void CreateAccount(CreateAccountRequest request)
    {
        Console.WriteLine($"[Service] CreateAccount: Starting for user={request.Username}");
        
        // 1. Validate request
        Console.WriteLine("[Service] Step 1: Validating request...");
        ValidateCreateRequest(request);
        Console.WriteLine("[Service] Step 1: Validation passed");

        // 2. Check duplicate
        Console.WriteLine("[Service] Step 2: Checking duplicates...");
        if (_accountRepository.ExistsByUsername(request.Username))
        {
            Console.WriteLine($"[Service] ERROR: Username [{request.Username}] already exists");
            throw new Exception($"Username [{request.Username}] đã tồn tại");
        }

        if (!string.IsNullOrEmpty(request.Email) && _userProfileRepository.ExistsByEmail(request.Email))
        {
            Console.WriteLine($"[Service] ERROR: Email [{request.Email}] already exists");
            throw new Exception($"Email [{request.Email}] đã tồn tại");
        }

        if (!string.IsNullOrEmpty(request.Phone) && _userProfileRepository.ExistsByPhone(request.Phone))
        {
            Console.WriteLine($"[Service] ERROR: Phone [{request.Phone}] already exists");
            throw new Exception($"SĐT [{request.Phone}] đã tồn tại");
        }
        Console.WriteLine("[Service] Step 2: No duplicates found");

        // 3. Hash password (simplified, nên dùng BCrypt trong production)
        Console.WriteLine("[Service] Step 3: Hashing password...");
        string hashedPassword = HashPassword(request.Password);
        Console.WriteLine("[Service] Step 3: Password hashed");

        Console.WriteLine("[Service] Step 4: Opening DB connection and transaction...");
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            // 4. Create Account
            Console.WriteLine("[Service] Step 4: Creating Account entity...");
            var account = new Account
            {
                Username = request.Username,
                Password = hashedPassword,
                Role = request.Role,
                IsActive = request.Active
            };

            Console.WriteLine($"[Service] Step 4: Inserting account into DB (username={account.Username}, role={account.Role})...");
            long accountId = _accountRepository.Insert(connection, transaction, account);
            Console.WriteLine($"[Service] Step 4: Account created with ID={accountId}");

            // 5. Create UserProfile
            Console.WriteLine("[Service] Step 5: Creating UserProfile...");
            string codePrefix = request.Role == RoleType.EMPLOYEE ? "EMP" : "CUS";
            string code = _userProfileRepository.GenerateCode(codePrefix);
            Console.WriteLine($"[Service] Step 5: Generated code={code}");

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

            Console.WriteLine($"[Service] Step 5: Inserting UserProfile (accountId={accountId}, code={code})...");
            long profileId = _userProfileRepository.Insert(connection, transaction, userProfile);
            Console.WriteLine($"[Service] Step 5: UserProfile created with ID={profileId}");

            // 6. Create EmployeeProfile or CustomerProfile based on role
            Console.WriteLine($"[Service] Step 6: Creating role-specific profile (role={request.Role})...");
            if (request.Role == RoleType.EMPLOYEE)
            {
                // Create EmployeeProfile with default values
                Console.WriteLine("[Service] Step 6: Creating EmployeeProfile...");
                _employeeProfileRepository.Insert(
                    connection,
                    transaction,
                    profileId,
                    position: "Nhân viên",
                    department: "Chưa phân bộ phận",
                    hiredDate: DateTime.Now,
                    baseSalary: 0
                );
                Console.WriteLine("[Service] Step 6: EmployeeProfile created");
            }
            else if (request.Role == RoleType.CUSTOMER)
            {
                // Create CustomerProfile with default values
                Console.WriteLine("[Service] Step 6: Creating CustomerProfile...");
                var customerProfile = new CustomerProfile
                {
                    ProfileId = (int)profileId,
                    CustomerType = "RETAIL",
                    TaxCode = null
                };
                _customerProfileRepository.Insert(connection, transaction, customerProfile);
                Console.WriteLine("[Service] Step 6: CustomerProfile created");
            }

            Console.WriteLine("[Service] Committing transaction...");
            transaction.Commit();
            Console.WriteLine($"[Service] ✓ Tạo tài khoản thành công: {request.Username} ({request.Role})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Service] ✗ ERROR during account creation: {ex}");
            Console.WriteLine("[Service] Rolling back transaction...");
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
        Console.WriteLine($"[Service] Update: Updating account ID={accountId}, newRole={role}, newActive={active}");
        var account = _accountRepository.FindById(accountId);
        if (account == null)
        {
            Console.WriteLine($"[Service] Update: ERROR - Account ID={accountId} not found");
            throw new Exception("Account not found");
        }

        Console.WriteLine($"[Service] Update: Current state - role={account.Role}, active={account.IsActive}");

        // RULE 1: Không cho deactivate ADMIN cuối cùng
        if (account.Role == RoleType.ADMIN && account.IsActive && !active)
        {
            Console.WriteLine("[Service] Update: Checking if can deactivate ADMIN...");
            var otherActiveAdmins = _accountRepository.FindAll()
                .Where(a => a.Role == RoleType.ADMIN && a.IsActive && a.Id != accountId)
                .ToList();

            if (otherActiveAdmins.Count == 0)
            {
                Console.WriteLine("[Service] Update: ERROR - Cannot deactivate the last active ADMIN");
                throw new Exception("Không thể vô hiệu hóa tài khoản ADMIN cuối cùng");
            }
            Console.WriteLine($"[Service] Update: OK - Found {otherActiveAdmins.Count} other active ADMINs");
        }

        // RULE 2: Không cho đổi role của ADMIN cuối cùng sang role khác
        if (account.Role == RoleType.ADMIN && role != RoleType.ADMIN)
        {
            Console.WriteLine("[Service] Update: Checking if can change ADMIN role...");
            var otherAdmins = _accountRepository.FindAll()
                .Where(a => a.Role == RoleType.ADMIN && a.Id != accountId)
                .ToList();

            if (otherAdmins.Count == 0)
            {
                Console.WriteLine("[Service] Update: ERROR - Cannot change role of the last ADMIN");
                throw new Exception("Không thể thay đổi vai trò của tài khoản ADMIN cuối cùng");
            }
            Console.WriteLine($"[Service] Update: OK - Found {otherAdmins.Count} other ADMINs");
        }

        Console.WriteLine("[Service] Update: Executing update...");
        _accountRepository.UpdateRoleAndStatus(accountId, role, active);
        Console.WriteLine("[Service] Update: Success!");
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