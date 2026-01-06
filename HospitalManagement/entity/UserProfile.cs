namespace HospitalManagement.entity
{
    /// <summary>
    /// UserProfile entity - represents user profile information
    /// </summary>
    public class UserProfile : BaseEntity
    {
        /// <summary>
        /// Reference to Account
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Unique user code/ID
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string FullName { get; set; } = "";

        /// <summary>
        /// Phone number
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Home/office address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// User status: ACTIVE, INACTIVE, SUSPENDED
        /// </summary>
        public string Status { get; set; } = ProfileStatus.ACTIVE.ToString();
    }
}

