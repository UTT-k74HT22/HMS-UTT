using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl;

/// <summary>
/// Service implementation cho quản lý nhân viên
/// </summary>
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
    
    // PRIVATE METHODS
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