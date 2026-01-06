namespace HospitalManagement.entity
{
    /// <summary>
    /// Invoice entity - represents an invoice for an order
    /// </summary>
    public class Invoice : BaseEntity
    {
        /// <summary>
        /// Reference to Order
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Unique invoice number
        /// </summary>
        public string InvoiceNumber { get; set; } = "";

        /// <summary>
        /// Date invoice was issued
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Date payment is due
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Total invoice amount
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Amount already paid
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// Invoice status: NEW, PAID, PARTIAL, CANCELED
        /// </summary>
        public string Status { get; set; } = InvoiceStatus.NEW.ToString();
    }
}
