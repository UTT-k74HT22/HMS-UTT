namespace HospitalManagement.dto.request.Batch;

public class CreateBatchRequest
{
    public string BatchCode { get; set; } = string.Empty;

    public long ProductId { get; set; }

    public decimal ImportPrice { get; set; }

    public DateTime? ManufactureDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? SupplierName { get; set; }

    /// <summary>
    /// ACTIVE | EXPIRED | BLOCKED | DEPLETED
    /// </summary>
    public string Status { get; set; } = "ACTIVE";
}