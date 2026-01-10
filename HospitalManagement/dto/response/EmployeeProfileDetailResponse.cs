using HospitalManagement.entity;

namespace HospitalManagement.dto.response
{
    /// <summary>
    /// Response DTO cho chi tiết hồ sơ nhân viên
    /// </summary>
    public class EmployeeProfileDetailResponse : EmployeeProfileResponse
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public DateTime HiredDate { get; set; }
        public decimal Salary { get; set; }

        public EmployeeProfileDetailResponse() { }

        public EmployeeProfileDetailResponse(
            long accountId,
            string accountUsername,
            long profileId,
            string code,
            string fullName,
            string phone,
            string email,
            string address,
            string position,
            string department,
            DateTime? hiredDate,
            decimal? salary,
            ProfileStatus status
        ) : base(accountId, accountUsername, profileId, code, fullName, phone, position, status)
        {
            Email = email;
            Address = address;
            Department = department;
            HiredDate = (DateTime)hiredDate;
            Salary = (decimal)salary;
        }
    }
}
