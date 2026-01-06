namespace HospitalManagement.entity
{
    /// <summary>
    /// StockMovement entity - audit trail for all inventory movements
    /// </summary>
    public class StockMovement : BaseEntity
    {
        /// <summary>
        /// Type of movement: IMPORT, EXPORT, ADJUST, TRANSFER
        /// </summary>
        public string MovementType { get; set; } = StockMovementType.IMPORT.ToString();

        /// <summary>
        /// Reference to Product
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Reference to Batch (optional)
        /// </summary>
        public int? BatchId { get; set; }

        /// <summary>
        /// Reference to Warehouse
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        /// Quantity moved
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Quantity before movement
        /// </summary>
        public int? QuantityBefore { get; set; }

        /// <summary>
        /// Quantity after movement
        /// </summary>
        public int? QuantityAfter { get; set; }

        /// <summary>
        /// Date/time of the movement
        /// </summary>
        public DateTime MovementDate { get; set; }

        /// <summary>
        /// Reference document type (e.g., ORDER, PURCHASE_ORDER)
        /// </summary>
        public string? ReferenceType { get; set; }

        /// <summary>
        /// Reference document ID
        /// </summary>
        public int? ReferenceId { get; set; }

        /// <summary>
        /// User who performed the movement
        /// </summary>
        public int? PerformedByUserId { get; set; }

        /// <summary>
        /// Additional notes about the movement
        /// </summary>
        public string? Note { get; set; }
    }
}

