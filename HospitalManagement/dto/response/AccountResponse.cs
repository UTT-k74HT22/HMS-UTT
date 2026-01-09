using HospitalManagement.entity;

namespace HospitalManagement.dto.response
{
    /// <summary>
    /// Response DTO cho danh sách tài khoản
    /// </summary>
    public class AccountResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
        public bool Active { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public AccountResponse(long id, string username, Role role, bool active, DateTime? lastLoginAt)
        {
            Id = id;
            Username = username;
            Role = role;
            Active = active;
            LastLoginAt = lastLoginAt;
        }

        public AccountResponse() { }
    }
}
