# Hướng Dẫn Implementation - Account Management

## 📋 Tổng Quan
Module quản lý tài khoản người dùng với các chức năng CRUD cơ bản.

---

## 🗄️ 1. REPOSITORY IMPLEMENTATION

### File: `repository/impl/AccountRepositoryImpl.cs`

```csharp
using HospitalManagement.entity;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HospitalManagement.repository.impl
{
    public class AccountRepositoryImpl : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Account> FindAll()
        {
            var accounts = new List<Account>();
            string query = @"
                SELECT id, username, password_hash, role, active, 
                       email, created_at, last_login_at
                FROM account
                WHERE deleted_at IS NULL
                ORDER BY created_at DESC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        accounts.Add(MapToAccount(reader));
                    }
                }
            }
            return accounts;
        }

        public Account FindByUsername(string username)
        {
            string query = @"
                SELECT id, username, password_hash, role, active, 
                       email, created_at, last_login_at
                FROM account
                WHERE username = @username AND deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToAccount(reader);
                    }
                }
            }
            return null;
        }

        public Account FindById(long id)
        {
            string query = @"
                SELECT id, username, password_hash, role, active, 
                       email, created_at, last_login_at
                FROM account
                WHERE id = @id AND deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToAccount(reader);
                    }
                }
            }
            return null;
        }

        public long Insert(SqlConnection conn, Account account)
        {
            string query = @"
                INSERT INTO account (username, password_hash, role, active, email, created_at)
                OUTPUT INSERTED.id
                VALUES (@username, @passwordHash, @role, @active, @email, GETDATE())";

            using (var command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@username", account.Username);
                command.Parameters.AddWithValue("@passwordHash", account.PasswordHash);
                command.Parameters.AddWithValue("@role", account.Role.ToString());
                command.Parameters.AddWithValue("@active", account.Active);
                command.Parameters.AddWithValue("@email", account.Email ?? (object)DBNull.Value);

                return (long)(int)command.ExecuteScalar();
            }
        }

        public void UpdateRoleAndStatus(long id, Role role, bool active)
        {
            string query = @"
                UPDATE account 
                SET role = @role, active = @active, updated_at = GETDATE()
                WHERE id = @id AND deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@role", role.ToString());
                command.Parameters.AddWithValue("@active", active);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteById(long id)
        {
            // Soft delete
            string query = @"
                UPDATE account 
                SET deleted_at = GETDATE(), active = 0
                WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool ExistsByUsername(string username)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM account 
                WHERE username = @username AND deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                connection.Open();
                return (int)command.ExecuteScalar() > 0;
            }
        }

        public bool ExistsByEmail(string email)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM account 
                WHERE email = @email AND deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                return (int)command.ExecuteScalar() > 0;
            }
        }

        public void UpdatePassword(long accountId, string hashedPassword)
        {
            string query = @"
                UPDATE account 
                SET password_hash = @passwordHash, updated_at = GETDATE()
                WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", accountId);
                command.Parameters.AddWithValue("@passwordHash", hashedPassword);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateLastLogin(long accountId)
        {
            string query = @"
                UPDATE account 
                SET last_login_at = GETDATE()
                WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", accountId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public long? FindUserIdByAccountId(long accountId)
        {
            string query = @"
                SELECT id 
                FROM user_profile 
                WHERE account_id = @accountId AND deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@accountId", accountId);
                connection.Open();
                
                var result = command.ExecuteScalar();
                return result != null ? (long?)(int)result : null;
            }
        }

        private Account MapToAccount(SqlDataReader reader)
        {
            return new Account
            {
                Id = reader.GetInt64(reader.GetOrdinal("id")),
                Username = reader.GetString(reader.GetOrdinal("username")),
                PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                Role = Enum.Parse<Role>(reader.GetString(reader.GetOrdinal("role"))),
                Active = reader.GetBoolean(reader.GetOrdinal("active")),
                Email = reader.IsDBNull(reader.GetOrdinal("email")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("email")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                LastLoginAt = reader.IsDBNull(reader.GetOrdinal("last_login_at")) 
                    ? null 
                    : reader.GetDateTime(reader.GetOrdinal("last_login_at"))
            };
        }
    }
}
```

---

## 💼 2. SERVICE IMPLEMENTATION

### File: `service/impl/AccountServiceImpl.cs`

```csharp
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.service.impl
{
    public class AccountServiceImpl : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly string _connectionString;

        public AccountServiceImpl(IAccountRepository accountRepository, string connectionString)
        {
            _accountRepository = accountRepository;
            _connectionString = connectionString;
        }

        public List<Account> GetAll()
        {
            return _accountRepository.FindAll();
        }

        public List<AccountResponse> GetAllAccount()
        {
            var accounts = _accountRepository.FindAll();
            return accounts.Select(a => new AccountResponse
            {
                Id = a.Id,
                Username = a.Username,
                Role = a.Role,
                Active = a.Active,
                LastLoginAt = a.LastLoginAt
            }).ToList();
        }

        public Account FindByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username không được để trống");
            }
            return _accountRepository.FindByUsername(username);
        }

        public Account FindById(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID không hợp lệ");
            }
            
            var account = _accountRepository.FindById(id);
            if (account == null)
            {
                throw new Exception($"Không tìm thấy tài khoản với ID: {id}");
            }
            return account;
        }

        public void Update(long accountId, Role role, bool active)
        {
            // Validate account exists
            var account = FindById(accountId);
            
            // Business rule: Không thể deactivate tài khoản ADMIN cuối cùng
            if (role == Role.ADMIN && !active)
            {
                var adminAccounts = _accountRepository.FindAll()
                    .Where(a => a.Role == Role.ADMIN && a.Active && a.Id != accountId)
                    .ToList();
                
                if (adminAccounts.Count == 0)
                {
                    throw new Exception("Không thể vô hiệu hóa tài khoản ADMIN cuối cùng");
                }
            }
            
            _accountRepository.UpdateRoleAndStatus(accountId, role, active);
        }

        public void DeleteById(long id)
        {
            // Validate account exists
            var account = FindById(id);
            
            // Business rule: Không thể xóa tài khoản ADMIN cuối cùng
            if (account.Role == Role.ADMIN)
            {
                var adminAccounts = _accountRepository.FindAll()
                    .Where(a => a.Role == Role.ADMIN && a.Id != id)
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
            return _accountRepository.ExistsByUsername(username);
        }
    }
}
```

---

## 🎮 3. CONTROLLER IMPLEMENTATION

### File: `controller/AccountController.cs`

```csharp
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.service;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagement.controller
{
    public class AccountController
    {
        private readonly IAccountService _accountService;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly string _connectionString;

        public AccountController(
            IAccountService accountService,
            IUserProfileRepository userProfileRepository,
            string connectionString)
        {
            _accountService = accountService;
            _userProfileRepository = userProfileRepository;
            _connectionString = connectionString;
        }

        /// <summary>
        /// [CHỨC NĂNG 1] Lấy danh sách tất cả tài khoản
        /// </summary>
        public List<AccountResponse> GetAllAccounts()
        {
            try
            {
                return _accountService.GetAllAccount();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách tài khoản: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 2] Tạo tài khoản mới
        /// FLOW:
        /// 1. Validate dữ liệu đầu vào
        /// 2. Kiểm tra username/email trùng
        /// 3. Hash password
        /// 4. Tạo Account trong transaction
        /// 5. Tạo UserProfile tương ứng
        /// 6. Commit hoặc Rollback
        /// </summary>
        public void CreateAccount(CreateAccountRequest request)
        {
            // STEP 1: Validation
            ValidateCreateAccountRequest(request);

            // STEP 2: Check duplicate
            if (_accountService.ExistsByUsername(request.Username))
            {
                throw new Exception($"Username '{request.Username}' đã tồn tại");
            }

            // STEP 3: Hash password
            string hashedPassword = HashPassword(request.Password);

            // STEP 4 & 5: Create account and profile in transaction
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Create Account
                        var account = new Account
                        {
                            Username = request.Username,
                            PasswordHash = hashedPassword,
                            Role = request.Role,
                            Active = request.Active,
                            Email = request.Email
                        };

                        long accountId = _accountRepository.Insert(connection, account);

                        // Create UserProfile
                        var userProfile = new UserProfile
                        {
                            AccountId = accountId,
                            FullName = request.FullName,
                            Phone = request.Phone,
                            Email = request.Email,
                            Address = request.Address,
                            Status = ProfileStatus.ACTIVE
                        };

                        _userProfileRepository.Insert(connection, userProfile);

                        // STEP 6: Commit
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 3] Cập nhật tài khoản
        /// FLOW:
        /// 1. Validate input
        /// 2. Check business rules (không deactivate ADMIN cuối)
        /// 3. Update
        /// </summary>
        public void UpdateAccount(long id, Role role, bool active)
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
        /// [CHỨC NĂNG 4] Xóa tài khoản (soft delete)
        /// FLOW:
        /// 1. Validate account tồn tại
        /// 2. Check business rules (không xóa ADMIN cuối)
        /// 3. Soft delete
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

        // ========== HELPER METHODS ==========

        private void ValidateCreateAccountRequest(CreateAccountRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username không được để trống");

            if (request.Username.Length < 3 || request.Username.Length > 50)
                throw new ArgumentException("Username phải từ 3-50 ký tự");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password không được để trống");

            if (request.Password.Length < 6)
                throw new ArgumentException("Password phải ít nhất 6 ký tự");

            if (request.Password != request.ConfirmPassword)
                throw new ArgumentException("Password và Confirm Password không khớp");

            if (string.IsNullOrWhiteSpace(request.FullName))
                throw new ArgumentException("Họ tên không được để trống");

            if (!string.IsNullOrEmpty(request.Email) && !IsValidEmail(request.Email))
                throw new ArgumentException("Email không hợp lệ");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
```

---

## 📊 4. SQL SCHEMA (SQL Server)

```sql
-- Table: account
CREATE TABLE account (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) NOT NULL UNIQUE,
    password_hash NVARCHAR(255) NOT NULL,
    role NVARCHAR(20) NOT NULL CHECK (role IN ('ADMIN', 'MANAGER', 'EMPLOYEE', 'CUSTOMER')),
    active BIT NOT NULL DEFAULT 1,
    email NVARCHAR(100),
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2,
    deleted_at DATETIME2,
    last_login_at DATETIME2
);

CREATE INDEX idx_account_username ON account(username);
CREATE INDEX idx_account_email ON account(email);
```

---

## 🔄 5. FLOW DIAGRAM

### Create Account Flow:
```
[UI] → [Controller.CreateAccount]
         ↓
    [Validation]
         ↓
    [Check Duplicate]
         ↓
    [Hash Password]
         ↓
    [BEGIN TRANSACTION]
         ↓
    [Insert Account] → [Get AccountId]
         ↓
    [Insert UserProfile]
         ↓
    [COMMIT]
         ↓
    [Return Success]
```

### Update Account Flow:
```
[UI] → [Controller.UpdateAccount]
         ↓
    [Service.Update]
         ↓
    [Check Business Rules]
         ↓
    [Repository.UpdateRoleAndStatus]
         ↓
    [Return Success]
```

---

## ✅ 6. TESTING CHECKLIST

- [ ] Tạo tài khoản với đầy đủ thông tin
- [ ] Tạo tài khoản với username trùng (phải lỗi)
- [ ] Tạo tài khoản với password không khớp (phải lỗi)
- [ ] Cập nhật role và status
- [ ] Deactivate tài khoản ADMIN cuối (phải lỗi)
- [ ] Xóa tài khoản ADMIN cuối (phải lỗi)
- [ ] Soft delete tài khoản thường

---

## 🎯 7. NOTES CHO DEVELOPER

1. **Password Security**: Hiện tại dùng SHA256, nên nâng cấp lên BCrypt hoặc PBKDF2
2. **Transaction**: Luôn dùng transaction khi tạo Account + UserProfile
3. **Validation**: Validate ở cả Controller và Service layer
4. **Error Handling**: Throw exception rõ ràng, có message tiếng Việt
5. **Soft Delete**: Dùng deleted_at thay vì xóa thật
