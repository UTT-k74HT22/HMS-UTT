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
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Lấy tất cả tồn kho
        /// </summary>
        public List<InventoryResponse> GetAllInventory()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy tồn kho theo warehouse
        /// </summary>
        public List<InventoryResponse> GetInventoryByWarehouse(long warehouseId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy tồn kho theo product
        /// </summary>
        public List<InventoryResponse> GetInventoryByProduct(long productId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy danh sách sắp hết hàng
        /// </summary>
        public List<InventoryResponse> GetLowStockItems()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy danh sách sắp hết hạn
        /// </summary>
        public List<InventoryResponse> GetNearExpiryItems()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cập nhật ngưỡng tồn kho
        /// </summary>
        public void UpdateThresholds(long inventoryItemId, UpdateInventoryThresholdRequest request)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Kiểm tra còn hàng
        /// </summary>
        public bool CheckStock(long productId, long warehouseId, int requiredQuantity)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
