namespace HospitalManagement.entity
{
    /// <summary>
    /// Payment entity - represents a payment for an invoice
    /// </summary>
    public class Payment : BaseEntity
    {
        /// <summary>
        /// Reference to Invoice
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Unique payment number
        /// </summary>
        public string PaymentNumber { get; set; } = "";

        /// <summary>
        /// Payment date
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Payment amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Payment method (e.g., CASH, CARD, BANK_TRANSFER)
        /// </summary>
        public string Method { get; set; } = "";

        /// <summary>
        /// Payment status: SUCCESS, FAILED, PENDING, CANCELED
        /// </summary>
        public string Status { get; set; } = PaymentStatus.SUCCESS.ToString();
    }
}
