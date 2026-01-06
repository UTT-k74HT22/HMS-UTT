namespace HospitalManagement.entity
{
    /// <summary>
    /// Product entity - represents medicinal products/medicines
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Reference to Category
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Reference to Manufacturer
        /// </summary>
        public int? ManufacturerId { get; set; }

        /// <summary>
        /// Unique product code
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// Product barcode for scanning
        /// </summary>
        public string? Barcode { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Dosage form (e.g., tablet, capsule, liquid)
        /// </summary>
        public string? DosageForm { get; set; }

        /// <summary>
        /// Unit of measurement (e.g., mg, ml, box)
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Product image URL
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Standard selling price
        /// </summary>
        public decimal StandardPrice { get; set; }

        /// <summary>
        /// Whether this product requires a prescription
        /// </summary>
        public bool RequiresPrescription { get; set; }

        /// <summary>
        /// Product status: ACTIVE, INACTIVE, DISCONTINUED
        /// </summary>
        public string Status { get; set; } = CategoryStatus.ACTIVE.ToString();
    }
}

