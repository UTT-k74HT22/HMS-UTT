using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.service;

namespace HospitalManagement.controller;

/// <summary>
/// Controller cho quản lý nhân viên
/// </summary>
public class EmployeeController
{
    private readonly EmployeeService employeeService;
    private readonly IEmployeeService _newEmployeeService;
    
    public EmployeeController(EmployeeService employeeService, IEmployeeService newEmployeeService)
    {
        this.employeeService = employeeService;
        _newEmployeeService = newEmployeeService;
    }

    /// <summary>
    /// Lấy danh sách tất cả nhân viên
    /// </summary>
    public List<EmployeeResponse> GetEmployees()
    {
        return employeeService.GetEmployees();
    }

    /// <summary>
    /// Lấy tất cả nhân viên (mới)
    /// </summary>
    public List<EmployeeProfileResponse> GetAllEmployees()
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Lấy tất cả chi tiết hồ sơ nhân viên
    /// </summary>
    public List<EmployeeProfileDetailResponse> GetAllProfileDetails()
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Lấy chi tiết nhân viên theo mã
    /// </summary>
    public EmployeeProfileDetailResponse GetEmployeeByCode(string code)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Cập nhật thông tin nhân viên
    /// </summary>
    public void UpdateEmployee(string code, UpdateProfileEmployeeRequest request)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Cập nhật chi tiết hồ sơ nhân viên
    /// </summary>
    public void UpdateEmployeeDetail(string code, UpdateEmployeeProfileDetailRequest request)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Xóa nhân viên (cập nhật trạng thái)
    /// </summary>
    public void DeleteEmployee(string code)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}