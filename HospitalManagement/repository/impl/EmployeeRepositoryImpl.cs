using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl;

public class EmployeeRepositoryImpl : IEmployeeProfileRepository
{
    private readonly string _connectionString;
    
    public EmployeeRepositoryImpl(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    private EmployeeProfileDetailResponse MapToEmployeeDetailResponse(SqlDataReader reader)
    {
        return new EmployeeProfileDetailResponse
        {
            AccountId = Convert.ToInt64(reader["account_id"]),
            AccountUsername = reader.GetString(reader.GetOrdinal("account_username")),
            ProfileId = Convert.ToInt64(reader["profile_id"]),
            Code = reader.GetString(reader.GetOrdinal("code")),
            FullName = reader.GetString(reader.GetOrdinal("full_name")),
            Phone = reader.IsDBNull(reader.GetOrdinal("phone")) 
                ? null 
                : reader.GetString(reader.GetOrdinal("phone")),
            Email = reader.IsDBNull(reader.GetOrdinal("email")) 
                ? null 
                : reader.GetString(reader.GetOrdinal("email")),
            Address = reader.IsDBNull(reader.GetOrdinal("address")) 
                ? null 
                : reader.GetString(reader.GetOrdinal("address")),
            Position = reader.GetString(reader.GetOrdinal("position")),
            Department = reader.GetString(reader.GetOrdinal("department")),
            HiredDate = reader.GetDateTime(reader.GetOrdinal("hired_date")),
            Salary = reader.GetDecimal(reader.GetOrdinal("base_salary")),
            Status = Enum.Parse<ProfileStatus>(reader.GetString(reader.GetOrdinal("status")))
        };
    }

    public void Insert(SqlConnection conn, long profileId, string position, 
        string department, DateTime hiredDate, decimal baseSalary)
    {
        Console.WriteLine($"[EmployeeProfileRepo] Insert: Inserting employee profile_id={profileId}");
        string query = @"
                INSERT INTO employee_profiles 
                    (profile_id, position, department, hired_date, base_salary, created_at)
                VALUES 
                    (@profileId, @position, @department, @hiredDate, @baseSalary, GETDATE())";

        using (var command = new SqlCommand(query, conn))
        {
            command.Parameters.AddWithValue("@profileId", profileId);
            command.Parameters.AddWithValue("@position", position);
            command.Parameters.AddWithValue("@department", department);
            command.Parameters.AddWithValue("@hiredDate", hiredDate);
            command.Parameters.AddWithValue("@baseSalary", baseSalary);

            command.ExecuteNonQuery();
            Console.WriteLine($"[EmployeeProfileRepo] Insert: Employee profile inserted");
        }
    }
    
    public void Insert(SqlConnection conn, SqlTransaction transaction, long profileId, string position, 
        string department, DateTime hiredDate, decimal baseSalary)
    {
        Console.WriteLine($"[EmployeeProfileRepo] Insert (with transaction): Inserting employee profile_id={profileId}");
        string query = @"
                INSERT INTO employee_profiles 
                    (profile_id, position, department, hired_date, base_salary, created_at)
                VALUES 
                    (@profileId, @position, @department, @hiredDate, @baseSalary, GETDATE())";

        using (var command = new SqlCommand(query, conn, transaction))
        {
            command.Parameters.AddWithValue("@profileId", profileId);
            command.Parameters.AddWithValue("@position", position);
            command.Parameters.AddWithValue("@department", department);
            command.Parameters.AddWithValue("@hiredDate", hiredDate);
            command.Parameters.AddWithValue("@baseSalary", baseSalary);

            command.ExecuteNonQuery();
            Console.WriteLine($"[EmployeeProfileRepo] Insert: Employee profile inserted");
        }
    }


    public List<EmployeeProfileResponse> GetAllProfiles()
    {
        var employees = new List<EmployeeProfileResponse>();

        string query = @"
                SELECT 
                    a.id AS account_id,
                    a.username AS account_username,
                    up.id AS profile_id,
                    up.code,
                    up.full_name,
                    up.phone,
                    ep.position,
                    up.status
                FROM accounts a
                INNER JOIN user_profiles up ON a.id = up.account_id
                INNER JOIN employee_profiles ep ON up.id = ep.profile_id
                WHERE a.role = 'EMPLOYEE'
                ORDER BY up.created_at DESC";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    employees.Add(new EmployeeProfileResponse
                    {
                        AccountId = Convert.ToInt64(reader["account_id"]),
                        AccountUsername = reader.GetString(reader.GetOrdinal("account_username")),
                        ProfileId = Convert.ToInt64(reader["profile_id"]),
                        Code = reader.GetString(reader.GetOrdinal("code")),
                        FullName = reader.GetString(reader.GetOrdinal("full_name")),
                        Phone = reader.IsDBNull(reader.GetOrdinal("phone"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("phone")),
                        Position = reader.GetString(reader.GetOrdinal("position")),
                        Status = Enum.Parse<ProfileStatus>(reader.GetString(reader.GetOrdinal("status")))
                    });
                }
            }
        }

        return employees;
    }

    public EmployeeProfileDetailResponse GetProfileDetailByCode(string code)
    {
        string query = @"
                    SELECT 
                        a.id AS account_id,
                        a.username AS account_username,
                        up.id AS profile_id,
                        up.code,
                        up.full_name,
                        up.phone,
                        up.email,
                        up.address,
                        ep.position,
                        ep.department,
                        ep.hired_date,
                        ep.base_salary,
                        up.status
                    FROM accounts a
                    INNER JOIN user_profiles up ON a.id = up.account_id
                    INNER JOIN employee_profiles ep ON up.id = ep.profile_id
                    WHERE up.code = @code";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@code", code);
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new EmployeeProfileDetailResponse
                    {
                        AccountId = Convert.ToInt64(reader["account_id"]),
                        AccountUsername = reader.GetString(reader.GetOrdinal("account_username")),
                        ProfileId = Convert.ToInt64(reader["profile_id"]),
                        Code = reader.GetString(reader.GetOrdinal("code")),
                        FullName = reader.GetString(reader.GetOrdinal("full_name")),
                        Phone = reader.IsDBNull(reader.GetOrdinal("phone"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("phone")),
                        Email = reader.IsDBNull(reader.GetOrdinal("email"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("email")),
                        Address = reader.IsDBNull(reader.GetOrdinal("address"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("address")),
                        Position = reader.GetString(reader.GetOrdinal("position")),
                        Department = reader.GetString(reader.GetOrdinal("department")),
                        HiredDate = reader.GetDateTime(reader.GetOrdinal("hired_date")),
                        Salary = reader.GetDecimal(reader.GetOrdinal("base_salary")),
                        Status = Enum.Parse<ProfileStatus>(reader.GetString(reader.GetOrdinal("status")))
                    };
                }
            }
        }
        return null;
    }

    public List<EmployeeProfileDetailResponse> GetAllProfileDetails()
    {
        var employees = new List<EmployeeProfileDetailResponse>();
            
        string query = @"
                SELECT 
                    a.id AS account_id,
                    a.username AS account_username,
                    up.id AS profile_id,
                    up.code,
                    up.full_name,
                    up.phone,
                    up.email,
                    up.address,
                    ep.position,
                    ep.department,
                    ep.hired_date,
                    ep.base_salary,
                    up.status
                FROM accounts a
                INNER JOIN user_profiles up ON a.id = up.account_id
                INNER JOIN employee_profiles ep ON up.id = ep.profile_id
                WHERE a.role = 'EMPLOYEE'
                ORDER BY up.created_at DESC";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    employees.Add(MapToEmployeeDetailResponse(reader));
                }
            }
        }
        return employees;
    }

    public void UpdateProfile(string code, UpdateProfileEmployeeRequest request)
    {
        string query = @"
                UPDATE up
                SET up.full_name = @fullName,
                    up.phone = @phone,
                    up.status = @status,
                    up.updated_at = GETDATE()
                FROM user_profiles up
                WHERE up.code = @code;

                UPDATE ep
                SET ep.position = @position,
                    ep.updated_at = GETDATE()
                FROM employee_profiles ep
                INNER JOIN user_profiles up ON ep.profile_id = up.id
                WHERE up.code = @code";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@code", code);
            command.Parameters.AddWithValue("@fullName", request.FullName);
            command.Parameters.AddWithValue("@phone", request.Phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@position", request.Position);
            command.Parameters.AddWithValue("@status", request.Status.ToString());

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void UpdateDetailByProfileId(long profileId, UpdateEmployeeProfileDetailRequest request)
    {
        string query = @"
                UPDATE up
                SET up.full_name = @fullName,
                    up.phone = @phone,
                    up.email = @email,
                    up.address = @address,
                    up.status = @status,
                    up.updated_at = GETDATE()
                FROM user_profiles up
                WHERE up.id = @profileId;

                UPDATE ep
                SET ep.position = @position,
                    ep.department = @department,
                    ep.hired_date = @hiredDate,
                    ep.base_salary = @salary,
                    ep.updated_at = GETDATE()
                FROM employee_profiles ep
                WHERE ep.profile_id = @profileId";

        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@profileId", profileId);
            command.Parameters.AddWithValue("@fullName", request.FullName);
            command.Parameters.AddWithValue("@phone", request.Phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@email", request.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@address", request.Address ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@position", request.Position);
            command.Parameters.AddWithValue("@department", request.Department);
            command.Parameters.AddWithValue("@hiredDate", request.HiredDate);
            command.Parameters.AddWithValue("@salary", request.Salary);
            command.Parameters.AddWithValue("@status", request.Status.ToString());

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void UpdateStatus(string code, ProfileStatus status)
    {
        throw new NotImplementedException();
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