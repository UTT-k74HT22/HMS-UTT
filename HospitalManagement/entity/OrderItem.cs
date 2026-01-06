namespace HospitalManagement.entity
{
    /// <summary>
    /// OrderItem entity - represents individual items in an order
    /// </summary>
    public class OrderItem : BaseEntity
    {
        /// <summary>
        /// Reference to Order
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Reference to Product
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Reference to Batch (optional)
        /// </summary>
        public int? BatchId { get; set; }

        /// <summary>
        /// Quantity ordered
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price at time of order
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Discount applied to this line item
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Total for this line (Quantity * UnitPrice - Discount)
        /// </summary>
        public decimal LineTotal { get; set; }
    }
}

