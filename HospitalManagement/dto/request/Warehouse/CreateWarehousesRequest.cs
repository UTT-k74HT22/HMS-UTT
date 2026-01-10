namespace HospitalManagement.dto.request
{
    /// <summary>
    /// DTO for creating or updating a warehouse
    /// </summary>
    public class WarehouseRequest
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
        /// Warehouse address (optional)
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Contact phone number (optional)
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Warehouse manager name (optional)
        /// </summary>
        public string? ManagerName { get; set; }

        /// <summary>
        /// Is warehouse active/operational
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}