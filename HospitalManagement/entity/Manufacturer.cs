namespace HospitalManagement.entity
{
    /// <summary>
    /// Manufacturer entity - represents product manufacturers
    /// </summary>
    public class Manufacturer : BaseEntity
    {
        /// <summary>
        /// Unique manufacturer code
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// Manufacturer name
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Country of origin
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Manufacturer address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Contact phone number
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Contact email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Contact person name
        /// </summary>
        public string? ContactPerson { get; set; }
    }
}

