namespace HospitalManagement.dto.response.ReportDetailResponse;

public class BestSellingProductResponse
{
    public string Product { get; set; }
    public string ProductCode { get; set; }
    public int TotalSold { get; set; }
    public decimal TotalRevenue { get; set; }
    public int Month { get; set; }      // tháng bán
    public int Year { get; set; }
}