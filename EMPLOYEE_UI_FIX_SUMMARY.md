# Employee Management UI Fix - Summary

## Các vấn đề được phát hiện và sửa

### 1. **DTO thiếu các trường dữ liệu** ❌
**File:** `HospitalManagement/dto/response/EmployeeResponse.cs`

**Vấn đề:** DTO chỉ có các trường cơ bản (Id, AccountId, Username, Code, FullName, Email, Phone) nhưng thiếu các trường quan trọng từ bảng `employee_profiles`:
- Position (Chức vụ)
- Department (Phòng ban)
- HiredDate (Ngày vào làm)
- BaseSalary (Lương cơ bản)
- ProfileId (Khóa ngoại)

**Sửa:** Đã thêm đầy đủ các trường thiếu vào DTO.

---

### 2. **Query SQL sai hoàn toàn** ❌❌❌
**File:** `HospitalManagement/repository/impl/EmployeeRepositoryImpl.cs`

**Vấn đề nghiêm trọng:**
```sql
-- Query CŨ (SAI):
SELECT ep.id, ep.account_id, a.username, ep.code, up.full_name, up.email, up.phone_number
FROM employee_profiles ep
JOIN accounts a ON ep.account_id = a.id  -- ❌ Sai! Không có cột account_id trong employee_profiles
JOIN user_profiles up ON a.id = up.account_id
```

**Lý do sai:**
- Bảng `employee_profiles` KHÔNG có cột `account_id`
- Cấu trúc đúng: `accounts` → `user_profiles` → `employee_profiles`
- Join phải thông qua `profile_id`, không phải `account_id`

**Query MỚI (ĐÚNG):**
```sql
SELECT
    ep.id,
    ep.profile_id,
    up.account_id,
    a.username,
    up.code,
    up.full_name,
    up.email,
    up.phone,
    ep.position,        -- ✅ Thêm
    ep.department,      -- ✅ Thêm
    ep.hired_date,      -- ✅ Thêm
    ep.base_salary      -- ✅ Thêm
FROM employee_profiles ep
INNER JOIN user_profiles up ON ep.profile_id = up.id      -- ✅ Đúng quan hệ
INNER JOIN accounts a ON up.account_id = a.id
WHERE a.role = 'EMPLOYEE'
ORDER BY ep.id DESC
```

---

### 3. **MainFrame không nhận EmployeeController** ❌
**File:** `HospitalManagement/view/MainFrame.cs`

**Vấn đề:** Constructor chỉ nhận `AccountController`, không có `EmployeeController`
```csharp
// CŨ:
public MainFrame(string username, string role, AccountController accountController)

// MỚI:
public MainFrame(string username, string role, AccountController accountController, EmployeeController employeeController)
```

---

### 4. **LoginForm không inject EmployeeController** ❌
**File:** `HospitalManagement/view/LoginForm.cs`

**Vấn đề:** LoginForm chỉ inject `AccountController`, không có `EmployeeController`
```csharp
// CŨ:
private AccountController? _accountController;

public LoginForm(IAuthService authService, AccountController accountController) : this()
{
    _authService = authService;
    _accountController = accountController;
}

var mainFrame = new MainFrame(account.Username, account.Role, _accountController);

// MỚI:
private AccountController? _accountController;
private EmployeeController? _employeeController;  // ✅ Thêm

public LoginForm(IAuthService authService, AccountController accountController, EmployeeController employeeController) : this()
{
    _authService = authService;
    _accountController = accountController;
    _employeeController = employeeController;  // ✅ Gán
}

var mainFrame = new MainFrame(account.Username, account.Role, _accountController, _employeeController);  // ✅ Pass
```

---

## Cấu trúc Database (3 bảng liên quan)

```
accounts (id, username, password, role)
    ↓ (FK: account_id)
user_profiles (id, account_id, code, full_name, phone, email, address, status)
    ↓ (FK: profile_id)
employee_profiles (id, profile_id, position, department, hired_date, base_salary)
```

**Quan hệ:**
- 1 Account → 1 User Profile (1-1)
- 1 User Profile → 1 Employee Profile (1-1 nếu role = EMPLOYEE)

---

## Các file đã sửa

1. ✅ `dto/response/EmployeeResponse.cs` - Thêm Position, Department, HiredDate, BaseSalary, ProfileId
2. ✅ `repository/impl/EmployeeRepositoryImpl.cs` - Sửa query SQL và mapper
3. ✅ `view/MainFrame.cs` - Thêm parameter EmployeeController
4. ✅ `view/LoginForm.cs` - Inject và truyền EmployeeController
5. ✅ `sample_employee_data.sql` - Script tạo dữ liệu mẫu để test

---

## Cách test

### Bước 1: Chạy script tạo dữ liệu mẫu
```sql
-- Chạy file: sample_employee_data.sql
-- Script này tạo 5 nhân viên mẫu với đầy đủ thông tin
```

### Bước 2: Chạy ứng dụng
```bash
dotnet run --project HospitalManagement/HospitalManagement.csproj
```

### Bước 3: Đăng nhập và kiểm tra
1. Đăng nhập với `admin / 123456789`
2. Click menu "Quản lý nhân viên"
3. Xem danh sách 5 nhân viên hiển thị đầy đủ:
   - ID
   - Profile ID
   - Chức vụ (Position)
   - Phòng ban (Department)
   - Ngày vào làm (HiredDate)
   - Lương cơ bản (BaseSalary)

---

## Kết quả

✅ **Query SQL hoạt động chính xác** - Join đúng quan hệ giữa 3 bảng  
✅ **DTO đầy đủ fields** - Có tất cả thông tin cần thiết  
✅ **UI hiển thị data** - EmployeeManagementPanel nhận được dữ liệu  
✅ **Dependency Injection hoàn chỉnh** - Controller được truyền qua toàn bộ chain  

---

## Note quan trọng

⚠️ **Lỗi phổ biến khi làm việc với multi-table joins:**
- Không hiểu rõ cấu trúc bảng → join sai
- Giả định tồn tại cột không có trong schema → query fail
- DTO không khớp với query result → runtime error

💡 **Best practice:**
1. Luôn kiểm tra schema trước khi viết query
2. DTO phải match với SELECT columns
3. Test query trực tiếp trong SQL Management Studio trước khi code
4. Sử dụng `INNER JOIN` thay vì `JOIN` để rõ ràng hơn

