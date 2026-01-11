using System.Collections.Generic;
using HospitalManagement.dto.response.ReportDetailResponse;
namespace HospitalManagement.repository;

public interface IReportDetailRepository
{
    List<InventoryItemResponse> GetInventory(int lowStockThreshold = 10);
    List<BestSellingProductResponse> GetBestSellingProducts(int top = 10, int? month = null, int? year = null);
    List<CustomerResponse> GetCustomers(int? month = null, int? year = null);
    List<OrderStatusResponse> GetOrdersByStatus(int? month = null, int? year = null);
}