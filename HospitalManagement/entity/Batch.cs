namespace HospitalManagement.entity
{
    /// <summary>
    /// Batch entity - represents product batches with expiry tracking
    /// </summary>
    public class Batch : BaseEntity
    {
        /// <summary>
        /// Reference to Product
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Unique batch code/lot number
        /// </summary>
        public string BatchCode { get; set; } = "";

        /// <summary>
        /// Cost price when imported
        /// </summary>
        public decimal ImportPrice { get; set; }

        /// <summary>
        /// Date when the batch was manufactured
        /// </summary>
        public DateTime? ManufactureDate { get; set; }

        /// <summary>
        /// Expiry date of the batch
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Supplier name
        /// </summary>
        public string? SupplierName { get; set; }

        /// <summary>
        /// Batch status: ACTIVE, EXPIRED, BLOCKED, DEPLETED
        /// </summary>
        public string Status { get; set; } = BatchStatus.ACTIVE.ToString();
    }
}

