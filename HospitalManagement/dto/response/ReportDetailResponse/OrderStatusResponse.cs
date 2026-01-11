namespace HospitalManagement.dto.response.ReportDetailResponse;

public class OrderStatusResponse
{
    public string Status { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalValue { get; set; }
    public int Month { get; set; }      // thống kê theo tháng
    public int Year { get; set; }
}