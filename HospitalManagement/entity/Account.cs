using HospitalManagement.entity.enums;

namespace HospitalManagement.entity
{
    /// <summary>
    /// Account entity - represents user login account
    /// </summary>
    public class Account : BaseEntity
    {
        /// <summary>
        /// Unique username for login
        /// </summary>
        public string Username { get; set; } = "";

        /// <summary>
        /// Password hash/encrypted
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// User role: ADMIN, EMPLOYEE, CUSTOMER
        /// </summary>
        public RoleType Role { get; set; } = RoleType.CUSTOMER;

        /// <summary>
        /// Account status
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Last login timestamp
        /// </summary>
        public DateTime? LastLoginAt { get; set; }
    }
}
