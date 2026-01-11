using HospitalManagement.repository.impl;
using HospitalManagement.service.impl;
using HospitalManagement.dto.response.ReportDetailResponse;

namespace HospitalManagement.controller;

public class ReportDetailController
{
    private readonly ReportDetailServiceImpl _service;

    public ReportDetailController(string connectionString)
    {
        var repo = new ReportDetailRepositoryImpl(connectionString);
        _service = new ReportDetailServiceImpl(repo);
    }

    public List<InventoryItemResponse> GetInventory(int lowStockThreshold = 10) => _service.GetInventory(lowStockThreshold);
    public List<BestSellingProductResponse> GetBestSellingProducts(int top = 10, int? month = null, int? year = null)
        => _service.GetBestSellingProducts(top, month, year);
    public List<CustomerResponse> GetCustomers(int? month = null, int? year = null) => _service.GetCustomers(month, year);
    public List<OrderStatusResponse> GetOrdersByStatus(int? month = null, int? year = null) => _service.GetOrdersByStatus(month, year);
}
