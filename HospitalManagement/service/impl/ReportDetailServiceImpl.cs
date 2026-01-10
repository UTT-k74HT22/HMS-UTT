using System;
using System.Collections.Generic;
using HospitalManagement.dto.response.ReportDetailResponse;
using HospitalManagement.repository;
using HospitalManagement.service;

namespace HospitalManagement.service.impl
{
    public class ReportDetailServiceImpl : IReportDetailService
    {
        private readonly IReportDetailRepository _repository;

        public ReportDetailServiceImpl(IReportDetailRepository repository)
        {
            _repository = repository;
        }

        public List<InventoryItemResponse> GetInventory(int lowStockThreshold = 10) => _repository.GetInventory(lowStockThreshold);
        public List<BestSellingProductResponse> GetBestSellingProducts(int top = 10, int? month = null, int? year = null)
            => _repository.GetBestSellingProducts(top, month, year);
        public List<CustomerResponse> GetCustomers(int? month = null, int? year = null) => _repository.GetCustomers(month, year);
        public List<OrderStatusResponse> GetOrdersByStatus(int? month = null, int? year = null) => _repository.GetOrdersByStatus(month, year);
    }
}