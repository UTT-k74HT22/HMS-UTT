namespace HospitalManagement.dto.request.Batch;

public class UpdateBatchRequest
{
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// ACTIVE | EXPIRED | BLOCKED | DEPLETED
    /// </summary>
    public string Status { get; set; } = "ACTIVE";
}