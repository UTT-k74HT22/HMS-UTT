namespace HospitalManagement.entity
{
    /// <summary>
    /// EmployeeProfile entity - extends UserProfile with employee-specific info
    /// </summary>
    public class EmployeeProfile : BaseEntity
    {
        /// <summary>
        /// Reference to UserProfile
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        /// Employee's job position
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// Department name
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// Date hired
        /// </summary>
        public DateTime? HiredDate { get; set; }

        /// <summary>
        /// Base salary
        /// </summary>
        public decimal BaseSalary { get; set; }
    }
}

