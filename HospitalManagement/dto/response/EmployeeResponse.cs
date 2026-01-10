﻿namespace HospitalManagement.dto.response;

public class EmployeeResponse
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public int AccountId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime? HiredDate { get; set; }
    public decimal BaseSalary { get; set; }
}