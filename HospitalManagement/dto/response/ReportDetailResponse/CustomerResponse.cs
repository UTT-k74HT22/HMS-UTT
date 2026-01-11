namespace HospitalManagement.dto.response.ReportDetailResponse;

public class CustomerResponse
{
    public string Customer { get; set; }
    public string Type { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
    public int Month { get; set; }      // thống kê theo tháng
    public int Year { get; set; }
}