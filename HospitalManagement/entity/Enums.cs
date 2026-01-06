namespace HospitalManagement.entity
{
    public enum AccountRole { ADMIN, EMPLOYEE, CUSTOMER }
    public enum ProfileStatus { ACTIVE, INACTIVE, SUSPENDED }
    public enum CustomerType { RETAIL, WHOLESALE }
    public enum CategoryStatus { ACTIVE, INACTIVE, DISCONTINUED }
    public enum BatchStatus { ACTIVE, EXPIRED, BLOCKED, DEPLETED }
    public enum StockMovementType { IMPORT, EXPORT, ADJUST, TRANSFER }
    public enum OrderStatus { NEW, CONFIRMED, PROCESSING, SHIPPED, COMPLETED, CANCELED }
    public enum InvoiceStatus { NEW, PAID, PARTIAL, CANCELED }
    public enum PaymentStatus { SUCCESS, FAILED, PENDING, CANCELED }
}

