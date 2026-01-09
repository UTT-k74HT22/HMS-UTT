# Hướng Dẫn Implementation - Employee Management

## 📋 Tổng Quan
Module quản lý nhân viên với thông tin cơ bản và chi tiết (phòng ban, lương, ngày vào làm).

---

## 🗄️ 1. REPOSITORY IMPLEMENTATION

### File: `repository/impl/EmployeeProfileRepositoryImpl.cs`

```csharp
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class EmployeeProfileRepositoryImpl : IEmployeeProfileRepository
    {
        private readonly string _connectionString;

        public EmployeeProfileRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(SqlConnection conn, long profileId, string position, 
                          string department, DateTime hiredDate, decimal baseSalary)
        {
            string query = @"
                INSERT INTO employee_profile 
                    (profile_id, position, department, hired_date, salary, created_at)
                VALUES 
                    (@profileId, @position, @department, @hiredDate, @salary, GETDATE())";

            using (var command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@profileId", profileId);
                command.Parameters.AddWithValue("@position", position);
                command.Parameters.AddWithValue("@department", department);
                command.Parameters.AddWithValue("@hiredDate", hiredDate);
                command.Parameters.AddWithValue("@salary", baseSalary);

                command.ExecuteNonQuery();
            }
        }

        public List<EmployeeProfileResponse> GetAllProfiles()
        {
            var employees = new List<EmployeeProfileResponse>();
            
            string query = @"
                SELECT 
                    a.id AS account_id,
                    a.username AS account_username,
                    up.id AS profile_id,
                    up.code,
                    up.full_name,
                    up.phone,
                    ep.position,
                    up.status
                FROM account a
                INNER JOIN user_profile up ON a.id = up.account_id
                INNER JOIN employee_profile ep ON up.id = ep.profile_id
                WHERE a.deleted_at IS NULL 
                  AND up.deleted_at IS NULL
                  AND a.role = 'EMPLOYEE'
                ORDER BY up.created_at DESC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new EmployeeProfileResponse
                        {
                            AccountId = reader.GetInt64(reader.GetOrdinal("account_id")),
                            AccountUsername = reader.GetString(reader.GetOrdinal("account_username")),
                            ProfileId = reader.GetInt64(reader.GetOrdinal("profile_id")),
                            Code = reader.GetString(reader.GetOrdinal("code")),
                            FullName = reader.GetString(reader.GetOrdinal("full_name")),
                            Phone = reader.IsDBNull(reader.GetOrdinal("phone")) 
                                ? null 
                                : reader.GetString(reader.GetOrdinal("phone")),
                            Position = reader.GetString(reader.GetOrdinal("position")),
                            Status = Enum.Parse<ProfileStatus>(reader.GetString(reader.GetOrdinal("status")))
                        });
                    }
                }
            }
            return employees;
        }

        public EmployeeProfileDetailResponse GetProfileDetailByCode(string code)
        {
            string query = @"
                SELECT 
                    a.id AS account_id,
                    a.username AS account_username,
                    up.id AS profile_id,
                    up.code,
                    up.full_name,
                    up.phone,
                    up.email,
                    up.address,
                    ep.position,
                    ep.department,
                    ep.hired_date,
                    ep.salary,
                    up.status
                FROM account a
                INNER JOIN user_profile up ON a.id = up.account_id
                INNER JOIN employee_profile ep ON up.id = ep.profile_id
                WHERE up.code = @code 
                  AND a.deleted_at IS NULL 
                  AND up.deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@code", code);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new EmployeeProfileDetailResponse
                        {
                            AccountId = reader.GetInt64(reader.GetOrdinal("account_id")),
                            AccountUsername = reader.GetString(reader.GetOrdinal("account_username")),
                            ProfileId = reader.GetInt64(reader.GetOrdinal("profile_id")),
                            Code = reader.GetString(reader.GetOrdinal("code")),
                            FullName = reader.GetString(reader.GetOrdinal("full_name")),
                            Phone = reader.IsDBNull(reader.GetOrdinal("phone")) 
                                ? null 
                                : reader.GetString(reader.GetOrdinal("phone")),
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) 
                                ? null 
                                : reader.GetString(reader.GetOrdinal("email")),
                            Address = reader.IsDBNull(reader.GetOrdinal("address")) 
                                ? null 
                                : reader.GetString(reader.GetOrdinal("address")),
                            Position = reader.GetString(reader.GetOrdinal("position")),
                            Department = reader.GetString(reader.GetOrdinal("department")),
                            HiredDate = reader.GetDateTime(reader.GetOrdinal("hired_date")),
                            Salary = reader.GetDecimal(reader.GetOrdinal("salary")),
                            Status = Enum.Parse<ProfileStatus>(reader.GetString(reader.GetOrdinal("status")))
                        };
                    }
                }
            }
            return null;
        }

        public List<EmployeeProfileDetailResponse> GetAllProfileDetails()
        {
            var employees = new List<EmployeeProfileDetailResponse>();
            
            string query = @"
                SELECT 
                    a.id AS account_id,
                    a.username AS account_username,
                    up.id AS profile_id,
                    up.code,
                    up.full_name,
                    up.phone,
                    up.email,
                    up.address,
                    ep.position,
                    ep.department,
                    ep.hired_date,
                    ep.salary,
                    up.status
                FROM account a
                INNER JOIN user_profile up ON a.id = up.account_id
                INNER JOIN employee_profile ep ON up.id = ep.profile_id
                WHERE a.deleted_at IS NULL 
                  AND up.deleted_at IS NULL
                  AND a.role = 'EMPLOYEE'
                ORDER BY up.created_at DESC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(MapToEmployeeDetailResponse(reader));
                    }
                }
            }
            return employees;
        }

        public void UpdateProfile(string code, UpdateProfileEmployeeRequest request)
        {
            string query = @"
                UPDATE up
                SET up.full_name = @fullName,
                    up.phone = @phone,
                    up.status = @status,
                    up.updated_at = GETDATE()
                FROM user_profile up
                WHERE up.code = @code AND up.deleted_at IS NULL;

                UPDATE ep
                SET ep.position = @position,
                    ep.updated_at = GETDATE()
                FROM employee_profile ep
                INNER JOIN user_profile up ON ep.profile_id = up.id
                WHERE up.code = @code AND up.deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@fullName", request.FullName);
                command.Parameters.AddWithValue("@phone", request.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@position", request.Position);
                command.Parameters.AddWithValue("@status", request.Status.ToString());

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateDetailByProfileId(long profileId, UpdateEmployeeProfileDetailRequest request)
        {
            string query = @"
                UPDATE up
                SET up.full_name = @fullName,
                    up.phone = @phone,
                    up.email = @email,
                    up.address = @address,
                    up.status = @status,
                    up.updated_at = GETDATE()
                FROM user_profile up
                WHERE up.id = @profileId AND up.deleted_at IS NULL;

                UPDATE ep
                SET ep.position = @position,
                    ep.department = @department,
                    ep.hired_date = @hiredDate,
                    ep.salary = @salary,
                    ep.updated_at = GETDATE()
                FROM employee_profile ep
                WHERE ep.profile_id = @profileId";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@profileId", profileId);
                command.Parameters.AddWithValue("@fullName", request.FullName);
                command.Parameters.AddWithValue("@phone", request.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@email", request.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@address", request.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@position", request.Position);
                command.Parameters.AddWithValue("@department", request.Department);
                command.Parameters.AddWithValue("@hiredDate", request.HiredDate);
                command.Parameters.AddWithValue("@salary", request.Salary);
                command.Parameters.AddWithValue("@status", request.Status.ToString());

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateStatus(string code, ProfileStatus status)
        {
            string query = @"
                UPDATE user_profile 
                SET status = @status, updated_at = GETDATE()
                WHERE code = @code AND deleted_at IS NULL";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@status", status.ToString());

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private EmployeeProfileDetailResponse MapToEmployeeDetailResponse(SqlDataReader reader)
        {
            return new EmployeeProfileDetailResponse
            {
                AccountId = reader.GetInt64(reader.GetOrdinal("account_id")),
                AccountUsername = reader.GetString(reader.GetOrdinal("account_username")),
                ProfileId = reader.GetInt64(reader.GetOrdinal("profile_id")),
                Code = reader.GetString(reader.GetOrdinal("code")),
                FullName = reader.GetString(reader.GetOrdinal("full_name")),
                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("phone")),
                Email = reader.IsDBNull(reader.GetOrdinal("email")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("email")),
                Address = reader.IsDBNull(reader.GetOrdinal("address")) 
                    ? null 
                    : reader.GetString(reader.GetOrdinal("address")),
                Position = reader.GetString(reader.GetOrdinal("position")),
                Department = reader.GetString(reader.GetOrdinal("department")),
                HiredDate = reader.GetDateTime(reader.GetOrdinal("hired_date")),
                Salary = reader.GetDecimal(reader.GetOrdinal("salary")),
                Status = Enum.Parse<ProfileStatus>(reader.GetString(reader.GetOrdinal("status")))
            };
        }
    }
}
```

---

## 💼 2. SERVICE IMPLEMENTATION

### File: `service/impl/EmployeeServiceImpl.cs`

```csharp
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl
{
    public class EmployeeServiceImpl : IEmployeeService
    {
        private readonly IEmployeeProfileRepository _employeeProfileRepository;

        public EmployeeServiceImpl(IEmployeeProfileRepository employeeProfileRepository)
        {
            _employeeProfileRepository = employeeProfileRepository;
        }

        public List<EmployeeProfileResponse> GetAllEmployees()
        {
            return _employeeProfileRepository.GetAllProfiles();
        }

        public List<EmployeeProfileDetailResponse> GetAllProfileDetails()
        {
            return _employeeProfileRepository.GetAllProfileDetails();
        }

        public EmployeeProfileDetailResponse GetEmployeeDetailByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Mã nhân viên không được để trống");
            }

            var employee = _employeeProfileRepository.GetProfileDetailByCode(code);
            if (employee == null)
            {
                throw new Exception($"Không tìm thấy nhân viên với mã: {code}");
            }

            return employee;
        }

        public void UpdateProfile(string code, UpdateProfileEmployeeRequest request)
        {
            // Validate
            ValidateUpdateProfileRequest(request);

            // Check employee exists
            var employee = GetEmployeeDetailByCode(code);

            // Update
            _employeeProfileRepository.UpdateProfile(code, request);
        }

        public void UpdateProfileDetail(string code, UpdateEmployeeProfileDetailRequest request)
        {
            // Validate
            ValidateUpdateDetailRequest(request);

            // Check employee exists
            var employee = GetEmployeeDetailByCode(code);

            // Update
            _employeeProfileRepository.UpdateDetailByProfileId(employee.ProfileId.Value, request);
        }

        public void Delete(string code, ProfileStatus status)
        {
            // Check employee exists
            var employee = GetEmployeeDetailByCode(code);

            // Soft delete bằng cách cập nhật status
            _employeeProfileRepository.UpdateStatus(code, status);
        }

        private void ValidateUpdateProfileRequest(UpdateProfileEmployeeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                throw new ArgumentException("Họ tên không được để trống");

            if (string.IsNullOrWhiteSpace(request.Position))
                throw new ArgumentException("Chức vụ không được để trống");
        }

        private void ValidateUpdateDetailRequest(UpdateEmployeeProfileDetailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                throw new ArgumentException("Họ tên không được để trống");

            if (string.IsNullOrWhiteSpace(request.Position))
                throw new ArgumentException("Chức vụ không được để trống");

            if (string.IsNullOrWhiteSpace(request.Department))
                throw new ArgumentException("Phòng ban không được để trống");

            if (request.HiredDate == null)
                throw new ArgumentException("Ngày vào làm không được để trống");

            if (request.HiredDate > DateTime.Now)
                throw new ArgumentException("Ngày vào làm không thể trong tương lai");

            if (request.Salary == null || request.Salary <= 0)
                throw new ArgumentException("Lương cơ bản phải lớn hơn 0");
        }
    }
}
```

---

## 🎮 3. CONTROLLER IMPLEMENTATION

### File: `controller/EmployeeController.cs`

```csharp
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.service;

namespace HospitalManagement.controller
{
    public class EmployeeController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// [CHỨC NĂNG 1] Lấy danh sách tất cả nhân viên (thông tin cơ bản)
        /// Dùng cho: DataGridView hiển thị danh sách
        /// </summary>
        public List<EmployeeProfileResponse> GetAllEmployees()
        {
            try
            {
                return _employeeService.GetAllEmployees();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 2] Lấy danh sách chi tiết tất cả nhân viên
        /// Dùng cho: Export Excel, báo cáo
        /// </summary>
        public List<EmployeeProfileDetailResponse> GetAllProfileDetails()
        {
            try
            {
                return _employeeService.GetAllProfileDetails();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chi tiết nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 3] Xem chi tiết một nhân viên
        /// FLOW:
        /// 1. Validate mã nhân viên
        /// 2. Lấy thông tin từ database
        /// 3. Trả về DTO
        /// </summary>
        public EmployeeProfileDetailResponse GetEmployeeByCode(string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    throw new ArgumentException("Vui lòng nhập mã nhân viên");

                return _employeeService.GetEmployeeDetailByCode(code);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 4] Cập nhật thông tin cơ bản nhân viên
        /// FLOW:
        /// 1. Validate request
        /// 2. Check nhân viên tồn tại
        /// 3. Update user_profile + employee_profile
        /// </summary>
        public void UpdateEmployee(string code, UpdateProfileEmployeeRequest request)
        {
            try
            {
                _employeeService.UpdateProfile(code, request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 5] Cập nhật chi tiết đầy đủ nhân viên
        /// FLOW:
        /// 1. Validate request (họ tên, phòng ban, lương, ngày vào làm)
        /// 2. Check nhân viên tồn tại
        /// 3. Validate business rules (ngày vào làm không trong tương lai, lương > 0)
        /// 4. Update cả user_profile và employee_profile
        /// </summary>
        public void UpdateEmployeeDetail(string code, UpdateEmployeeProfileDetailRequest request)
        {
            try
            {
                _employeeService.UpdateProfileDetail(code, request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật chi tiết nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 6] Vô hiệu hóa nhân viên (soft delete)
        /// FLOW:
        /// 1. Check nhân viên tồn tại
        /// 2. Cập nhật status = INACTIVE
        /// 3. Không xóa dữ liệu
        /// </summary>
        public void DeleteEmployee(string code)
        {
            try
            {
                _employeeService.Delete(code, ProfileStatus.INACTIVE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// [CHỨC NĂNG 7] Kích hoạt lại nhân viên
        /// </summary>
        public void ActivateEmployee(string code)
        {
            try
            {
                _employeeService.Delete(code, ProfileStatus.ACTIVE);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kích hoạt nhân viên: {ex.Message}", ex);
            }
        }
    }
}
```

---

## 📊 4. SQL SCHEMA (SQL Server)

```sql
-- Table: employee_profile
CREATE TABLE employee_profile (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    profile_id BIGINT NOT NULL,
    position NVARCHAR(100) NOT NULL,
    department NVARCHAR(100) NOT NULL,
    hired_date DATE NOT NULL,
    salary DECIMAL(15,2) NOT NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2,
    FOREIGN KEY (profile_id) REFERENCES user_profile(id)
);

CREATE INDEX idx_employee_profile_id ON employee_profile(profile_id);
CREATE INDEX idx_employee_department ON employee_profile(department);
```

---

## 🔄 5. FLOW DIAGRAMS

### Get Employee Detail Flow:
```
[UI - Nhấn xem chi tiết] 
    → [Controller.GetEmployeeByCode(code)]
        → [Service.GetEmployeeDetailByCode]
            → [Repository query JOIN 3 tables]
                → [Map to EmployeeProfileDetailResponse]
                    → [Return DTO]
```

### Update Employee Flow:
```
[UI - Form chỉnh sửa]
    → [Controller.UpdateEmployeeDetail(code, request)]
        → [Validate Request]
            → [Service.UpdateProfileDetail]
                → [Check employee exists]
                    → [Repository.UpdateDetailByProfileId]
                        → [UPDATE user_profile]
                        → [UPDATE employee_profile]
                            → [Success]
```

---

## ✅ 6. TESTING CHECKLIST

- [ ] Hiển thị danh sách nhân viên
- [ ] Xem chi tiết một nhân viên
- [ ] Cập nhật thông tin cơ bản (họ tên, SĐT, chức vụ)
- [ ] Cập nhật đầy đủ (thêm phòng ban, lương, ngày vào làm)
- [ ] Validate ngày vào làm không được trong tương lai
- [ ] Validate lương phải > 0
- [ ] Vô hiệu hóa nhân viên (status = INACTIVE)
- [ ] Kích hoạt lại nhân viên (status = ACTIVE)
- [ ] Search nhân viên theo mã/tên
- [ ] Export danh sách ra Excel

---

## 🎯 7. NOTES CHO DEVELOPER

1. **Mã nhân viên (code)**: Auto-generate theo format "EMP{YYYY}{MM}{DDHHMMSS}" hoặc "NV{số thứ tự}"
2. **Lương**: Dùng DECIMAL(15,2) để tránh mất chính xác
3. **Status**: 
   - ACTIVE: Đang làm việc
   - INACTIVE: Đã nghỉ/tạm ngừng
   - TERMINATED: Đã sa thải
4. **Join 3 tables**: account → user_profile → employee_profile
5. **Update**: Cập nhật cả user_profile VÀ employee_profile trong cùng transaction
