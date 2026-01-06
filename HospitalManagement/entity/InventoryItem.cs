namespace HospitalManagement.entity
{
    /// <summary>
    /// InventoryItem entity - tracks stock levels by product, batch, and warehouse
    /// </summary>
    public class InventoryItem : BaseEntity
    {
        /// <summary>
        /// Reference to Product
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Reference to Batch (optional, for batch-specific tracking)
        /// </summary>
        public int? BatchId { get; set; }

        /// <summary>
        /// Reference to Warehouse
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        /// Current quantity available in warehouse
        /// </summary>
        public int QuantityOnHand { get; set; }

        /// <summary>
        /// Quantity reserved for pending orders
        /// </summary>
        public int QuantityReserved { get; set; }

        /// <summary>
        /// Minimum stock level threshold (reorder point)
        /// </summary>
        public int MinThreshold { get; set; }

        /// <summary>
        /// Maximum stock level threshold
        /// </summary>
        public int MaxThreshold { get; set; }

        /// <summary>
        /// Last date of physical stock verification
        /// </summary>
        public DateTime? LastStockCheck { get; set; }
    }
}

