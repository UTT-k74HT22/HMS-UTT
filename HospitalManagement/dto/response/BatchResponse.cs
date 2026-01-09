namespace HospitalManagement.dto.response;

public class BatchResponse
{
    
    public long? Id { get; set; }
    public string BatchCode { get; set; }

    public long? ProductId { get; set; }
    public string ProductName { get; set; }

    public DateTime? ManufactureDate { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public decimal? ImportPrice { get; set; }
    public string SupplierName { get; set; }
    public string Status { get; set; }

    public override string ToString()
    {
        return BatchCode;
    }
}