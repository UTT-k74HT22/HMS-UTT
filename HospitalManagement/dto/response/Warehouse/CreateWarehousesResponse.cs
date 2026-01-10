namespace HospitalManagement.dto.response
{
    /// <summary>
    /// DTO for returning warehouse information to client
    /// </summary>
    public class WarehouseResponse
    {
        /// <summary>
        /// Warehouse ID
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// Created timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last updated timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}