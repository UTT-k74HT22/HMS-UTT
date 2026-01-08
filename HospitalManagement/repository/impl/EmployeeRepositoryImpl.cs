using HospitalManagement.dto.response;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl;

public class EmployeeRepositoryImpl : EmployeeRepository
{
    private readonly string _connectionString;
    
    public EmployeeRepositoryImpl(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public List<EmployeeResponse> GetEmployees()
    {
        var list = new List<EmployeeResponse>();
        const string sql = """
                           SELECT
                               ep.id,
                               ep.profile_id,
                               up.account_id,
                               a.username,
                               up.code,
                               up.full_name,
                               up.email,
                               up.phone,
                               ep.position,
                               ep.department,
                               ep.hired_date,
                               ep.base_salary
                           FROM employee_profiles ep
                           INNER JOIN user_profiles up ON ep.profile_id = up.id
                           INNER JOIN accounts a ON up.account_id = a.id
                           WHERE a.role = 'EMPLOYEE'
                           ORDER BY ep.id DESC
                           """;
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(MapToData(reader));
        }
        return list;
    }

    private EmployeeResponse MapToData(SqlDataReader rs)
    {
        return new EmployeeResponse
        {
            Id = rs.GetInt32(rs.GetOrdinal("id")),
            ProfileId = rs.GetInt32(rs.GetOrdinal("profile_id")),
            AccountId = rs.GetInt32(rs.GetOrdinal("account_id")),
            Username = rs.GetString(rs.GetOrdinal("username")),
            Code = rs.GetString(rs.GetOrdinal("code")),
            FullName = rs.GetString(rs.GetOrdinal("full_name")),
            Email = rs.IsDBNull(rs.GetOrdinal("email")) ? string.Empty : rs.GetString(rs.GetOrdinal("email")),
            Phone = rs.IsDBNull(rs.GetOrdinal("phone")) ? string.Empty : rs.GetString(rs.GetOrdinal("phone")),
            Position = rs.IsDBNull(rs.GetOrdinal("position")) ? string.Empty : rs.GetString(rs.GetOrdinal("position")),
            Department = rs.IsDBNull(rs.GetOrdinal("department")) ? string.Empty : rs.GetString(rs.GetOrdinal("department")),
            HiredDate = rs.IsDBNull(rs.GetOrdinal("hired_date")) ? null : rs.GetDateTime(rs.GetOrdinal("hired_date")),
            BaseSalary = rs.GetDecimal(rs.GetOrdinal("base_salary"))
        };
    }
}