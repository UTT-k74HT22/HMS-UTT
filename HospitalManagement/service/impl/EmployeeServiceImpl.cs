using HospitalManagement.dto.response;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl;

public class EmployeeServiceImpl : EmployeeService
{
    
    private readonly EmployeeRepository employeeRepository;
    
    public EmployeeServiceImpl(EmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }
    
    public List<EmployeeResponse> GetEmployees()
    {
        // throw new NotImplementedException();
        return employeeRepository.GetEmployees();
    }
}