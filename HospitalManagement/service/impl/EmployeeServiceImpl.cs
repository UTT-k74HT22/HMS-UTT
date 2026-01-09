using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl;

/// <summary>
/// Service implementation cho quản lý nhân viên
/// </summary>
public class EmployeeServiceImpl : EmployeeService, IEmployeeService
{
    private readonly EmployeeRepository employeeRepository;
    private readonly IEmployeeProfileRepository _employeeProfileRepository;
    
    public EmployeeServiceImpl(
        EmployeeRepository employeeRepository,
        IEmployeeProfileRepository employeeProfileRepository)
    {
        this.employeeRepository = employeeRepository;
        _employeeProfileRepository = employeeProfileRepository;
    }

    public List<EmployeeProfileResponse> GetAllEmployees()
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public List<EmployeeProfileDetailResponse> GetAllProfileDetails()
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public EmployeeProfileDetailResponse GetEmployeeDetailByCode(string code)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public void UpdateProfile(string code, UpdateProfileEmployeeRequest request)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public void UpdateProfileDetail(string code, UpdateEmployeeProfileDetailRequest request)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public void Delete(string code, ProfileStatus status)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
    
    public List<EmployeeResponse> GetEmployees()
    {
        // throw new NotImplementedException();
        return employeeRepository.GetEmployees();
    }
}