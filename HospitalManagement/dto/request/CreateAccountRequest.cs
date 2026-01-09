using HospitalManagement.entity;
using HospitalManagement.entity.enums;

namespace HospitalManagement.dto.request
{
    /// <summary>
    /// Request DTO để tạo tài khoản mới
    /// </summary>
    public class CreateAccountRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public RoleType Role { get; set; }
        public bool Active { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
