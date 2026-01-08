using HospitalManagement.dto.response;

namespace HospitalManagement.service;

public interface EmployeeService
{
    List<EmployeeResponse> GetEmployees();
}