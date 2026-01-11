namespace HospitalManagement.dto.response.ReportDetailResponse;

public class InventoryItemResponse
{
    public string Warehouse { get; set; }
    public string Product { get; set; }
    public string Batch { get; set; }
    public int Quantity { get; set; }
    public bool IsLowStock { get; set; }
}