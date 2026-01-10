using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.service;

namespace HospitalManagement.controller;

/// <summary>
/// Controller cho quản lý nhân viên
/// </summary>
public class EmployeeController
{
    private readonly IEmployeeService _employeeService;
    
    public EmployeeController(IEmployeeService _employeeService)
    {
        this._employeeService = _employeeService;
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