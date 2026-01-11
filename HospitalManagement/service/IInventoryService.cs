using HospitalManagement.dto.request;
using HospitalManagement.dto.response;

namespace HospitalManagement.service
{
    /// <summary>
    /// Service interface cho quản lý tồn kho
    /// </summary>
    public interface IInventoryService
    {
        /// <summary>
        /// Lấy tất cả tồn kho
        /// </summary>
        List<InventoryResponse> GetAll();

        /// <summary>
        /// Lấy tồn kho theo warehouse
        /// </summary>
        List<InventoryResponse> GetByWarehouse(long warehouseId);

        /// <summary>
        /// Lấy tồn kho theo product
        /// </summary>
        List<InventoryResponse> GetByProduct(long productId);

        /// <summary>
        /// Lấy danh sách sắp hết hàng
        /// </summary>
        List<InventoryResponse> GetLowStockItems();

        /// <summary>
        /// Lấy danh sách sắp hết hạn
        /// </summary>
        List<InventoryResponse> GetNearExpiryItems();

        /// <summary>
        /// Cập nhật ngưỡng min/max
        /// </summary>
        void UpdateThresholds(long inventoryItemId, UpdateInventoryThresholdRequest request);

        /// <summary>
        /// Lấy tổng tồn kho của product
        /// </summary>
        int GetTotalQuantityByProduct(long productId);

        /// <summary>
        /// Kiểm tra còn hàng
        /// </summary>
        bool HasStock(long productId, long warehouseId, int requiredQuantity);

        /// <summary>
        /// Cập nhật số lượng tồn kho theo product + batch + warehouse
        /// </summary>
        void UpdateStock(long productId, long batchId, long warehouseId, int newQuantity);

        /// <summary>
        /// Ghi log lịch sử nhập/xuất kho
        /// </summary>
        void InsertStockMovement(long productId, long batchId, long warehouseId,
                                 int quantity, int before, int after,
                                     string note, string movementType);

        /// <summary>
        /// Lấy số lượng tồn kho hiện tại
        /// </summary>
        int GetCurrentQuantity(long productId, long batchId, long warehouseId);
    }
}
