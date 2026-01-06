namespace HospitalManagement.entity
{
    /// <summary>
    /// Order entity - represents customer orders
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Reference to Customer (UserProfile)
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Unique order number
        /// </summary>
        public string OrderNumber { get; set; } = "";

        /// <summary>
        /// Date the order was placed
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Order status: NEW, CONFIRMED, PROCESSING, SHIPPED, COMPLETED, CANCELED
        /// </summary>
        public string Status { get; set; } = OrderStatus.NEW.ToString();

        /// <summary>
        /// Order subtotal before tax and discount
        /// </summary>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Discount amount applied
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Tax amount
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Final total amount to pay
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Shipping address
        /// </summary>
        public string? ShippingAddress { get; set; }

        /// <summary>
        /// User who created the order (employee)
        /// </summary>
        public int? CreatedByUserId { get; set; }

        /// <summary>
        /// Additional order notes
        /// </summary>
        public string? Note { get; set; }
    }
}

