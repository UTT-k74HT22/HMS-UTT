using HospitalManagement.dto.response;

namespace HospitalManagement.repository;

public interface EmployeeRepository
{
    List<EmployeeResponse> GetEmployees();
}