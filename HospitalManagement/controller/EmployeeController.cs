using HospitalManagement.service;

namespace HospitalManagement.controller;

public class EmployeeController
{
    
    private readonly EmployeeService employeeService;
    
    public EmployeeController(EmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    /// <summary>
    /// Lấy danh sách tất cả nhân viên
    /// </summary>
    public List<dto.response.EmployeeResponse> GetEmployees()
    {
        return employeeService.GetEmployees();
    }
}