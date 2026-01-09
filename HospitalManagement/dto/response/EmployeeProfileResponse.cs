using HospitalManagement.entity;

namespace HospitalManagement.dto.response
{
    /// <summary>
    /// Response DTO cho danh sách nhân viên
    /// </summary>
    public class EmployeeProfileResponse
    {
        public long? Id { get; set; }
        public long? AccountId { get; set; }
        public string AccountUsername { get; set; }
        public long? ProfileId { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public ProfileStatus Status { get; set; }

        public EmployeeProfileResponse() { }

        public EmployeeProfileResponse(long accountId, string accountUsername, long profileId, 
            string code, string fullName, string phone, string position, ProfileStatus status)
        {
            AccountId = accountId;
            AccountUsername = accountUsername;
            ProfileId = profileId;
            Code = code;
            FullName = fullName;
            Phone = phone;
            Position = position;
            Status = status;
        }
    }
}
