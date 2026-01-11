using System;
using System.Collections.Generic;
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.service;

namespace HospitalManagement.controller
{
    /// <summary>
    /// Controller cho quản lý tồn kho
    /// </summary>
    public class InventoryController
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
            => _inventoryService = inventoryService;

        /// <summary>
        /// Lấy tất cả tồn kho
        /// </summary>
        public List<InventoryResponse> GetAllInventory()
            => _inventoryService.GetAll();

        /// <summary>
        /// Lấy tồn kho theo warehouse
        /// </summary>
        public List<InventoryResponse> GetInventoryByWarehouse(long warehouseId)
            => _inventoryService.GetByWarehouse(warehouseId);

        /// <summary>
        /// Lấy tồn kho theo product
        /// </summary>
        public List<InventoryResponse> GetInventoryByProduct(long productId)
            => _inventoryService.GetByProduct(productId);

        /// <summary>
        /// Lấy danh sách sắp hết hàng
        /// </summary>
        public List<InventoryResponse> GetLowStockItems()
            => _inventoryService.GetLowStockItems();

        /// <summary>
        /// Lấy danh sách sắp hết hạn
        /// </summary>
        public List<InventoryResponse> GetNearExpiryItems()
            => _inventoryService.GetNearExpiryItems();

        /// <summary>
        /// Cập nhật ngưỡng min/max
        /// </summary>
        public void UpdateThresholds(long inventoryItemId, UpdateInventoryThresholdRequest request)
            => _inventoryService.UpdateThresholds(inventoryItemId, request);

        /// <summary>
        /// Lấy tổng tồn kho của product (tất cả kho)
        /// </summary>
        public int GetTotalQuantityByProduct(long productId)
            => _inventoryService.GetTotalQuantityByProduct(productId);

        /// <summary>
        /// Kiểm tra còn hàng theo product + warehouse
        /// </summary>
        public bool CheckStock(long productId, long warehouseId, int requiredQuantity)
            => _inventoryService.HasStock(productId, warehouseId, requiredQuantity);

        /// <summary>
        /// Cập nhật số lượng tồn kho theo product + batch + warehouse
        /// </summary>
        public void UpdateStock(long productId, long batchId, long warehouseId, int newQuantity)
            => _inventoryService.UpdateStock(productId, batchId, warehouseId, newQuantity);

        /// <summary>
        /// Ghi log lịch sử nhập/xuất kho
        /// </summary>
        public void InsertStockMovement(
            long productId,
            long batchId,
            long warehouseId,
            int quantity,
            int before,
            int after,
            string note,
            string movementType)
            => _inventoryService.InsertStockMovement(
                productId, batchId, warehouseId,
                quantity, before, after,
              note, movementType);

        /// <summary>
        /// Lấy số lượng tồn kho hiện tại
        /// </summary>
        public int GetCurrentQuantity(long productId, long batchId, long warehouseId)
            => _inventoryService.GetCurrentQuantity(productId, batchId, warehouseId);
    }
}
