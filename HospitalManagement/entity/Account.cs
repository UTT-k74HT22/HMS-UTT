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
        public string Role { get; set; } = "";

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
