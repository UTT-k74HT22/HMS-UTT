using System;
using System.Collections.Generic;
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using HospitalManagement.service;

namespace HospitalManagement.controller
{
    /// <summary>
    /// Controller cho quản lý xuất nhập kho
    /// </summary>
    public class StockMovementController
    {
        private readonly IStockMovementService _stockMovementService;

        public StockMovementController(IStockMovementService stockMovementService)
            => _stockMovementService = stockMovementService;

        /// <summary>
        /// Tạo giao dịch xuất/nhập kho mới
        /// </summary>
        public void CreateStockMovement(CreateStockMovementRequest request)
            => _stockMovementService.CreateMovement(request);

        /// <summary>
        /// Lấy tất cả giao dịch xuất nhập kho
        /// </summary>
        public List<StockMovementResponse> GetAllMovements()
            => _stockMovementService.GetAll();

        /// <summary>
        /// Lấy giao dịch theo warehouse
        /// </summary>
        public List<StockMovementResponse> GetMovementsByWarehouse(long warehouseId)
            => _stockMovementService.GetByWarehouse(warehouseId);

        /// <summary>
        /// Lấy giao dịch theo product
        /// </summary>
        public List<StockMovementResponse> GetMovementsByProduct(long productId)
            => _stockMovementService.GetByProduct(productId);

        /// <summary>
        /// Lấy giao dịch theo loại
        /// </summary>
        public List<StockMovementResponse> GetMovementsByType(StockMovementType movementType)
            => _stockMovementService.GetByMovementType(movementType);

        /// <summary>
        /// Lấy giao dịch trong khoảng thời gian
        /// </summary>
        public List<StockMovementResponse> GetMovementsByDateRange(DateTime fromDate, DateTime toDate)
            => _stockMovementService.GetByDateRange(fromDate, toDate);

        /// <summary>
        /// Lấy lịch sử xuất nhập của sản phẩm tại kho
        /// </summary>
        public List<StockMovementResponse> GetProductWarehouseHistory(long productId, long warehouseId)
            => _stockMovementService.GetHistoryByProductAndWarehouse(productId, warehouseId);
    }
}
