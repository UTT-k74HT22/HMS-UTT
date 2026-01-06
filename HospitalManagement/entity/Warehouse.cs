namespace HospitalManagement.entity
{
    /// <summary>
    /// Warehouse entity - represents storage locations for inventory
    /// </summary>
    public class Warehouse : BaseEntity
    {
        /// <summary>
        /// Unique warehouse code
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// Warehouse name/location
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Warehouse address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Contact phone number
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Warehouse manager name
        /// </summary>
        public string? ManagerName { get; set; }

        /// <summary>
        /// Is warehouse active/operational
        /// </summary>
        public bool IsActive { get; set; }
    }
}

