using HospitalManagement.dto.request;
using HospitalManagement.dto.response;

namespace HospitalManagement.repository
{
    /// <summary>
    /// Repository interface cho quản lý tồn kho
    /// </summary>
    public interface IInventoryRepository
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
        /// Lấy tồn kho cụ thể theo product + warehouse (+ batch optional)
        /// </summary>
        InventoryResponse FindByProductAndWarehouse(long productId, long warehouseId, long? batchId);

        /// <summary>
        /// Lấy danh sách sắp hết hàng (low stock)
        /// </summary>
        List<InventoryResponse> GetLowStockItems();

        /// <summary>
        /// Lấy danh sách sắp hết hạn (near expiry)
        /// </summary>
        List<InventoryResponse> GetNearExpiryItems();

        /// <summary>
        /// Cập nhật ngưỡng min/max cho inventory item
        /// </summary>
        void UpdateThresholds(long inventoryItemId, UpdateInventoryThresholdRequest request);

        /// <summary>
        /// Lấy tổng số lượng tồn của product (tất cả kho)
        /// </summary>
        int GetTotalQuantityByProduct(long productId);

        /// <summary>
        /// Kiểm tra còn hàng hay không
        /// </summary>
        bool HasStock(long productId, long warehouseId, int requiredQuantity);

        /// <summary>
        /// Lấy hoặc tạo mới inventory item (dùng trong transaction)
        /// Trả về ID và quantity hiện tại
        /// </summary>
        InventoryItemInfo GetOrCreateInventoryItem(long productId, long batchId, long warehouseId);

        /// <summary>
        /// Cập nhật số lượng tồn kho (dùng trong transaction)
        /// </summary>
        void UpdateQuantity(long inventoryItemId, int newQuantity);

        /// <summary>
        /// Lấy số lượng hiện tại của inventory item
        /// </summary>
        int GetCurrentQuantity(long productId, long batchId, long warehouseId);

        /// <summary>
        /// Cập nhật số lượng tồn kho
        /// </summary>
        void UpdateStock(long productId, long batchId, long warehouseId, int newQuantity);

        /// <summary>
        /// Chèn lịch sử xuất nhập kho
        /// </summary>
        void InsertStockMovement(long productId, long batchId, long warehouseId,
                                 int quantity, int before, int after,
                                 long userId, string note, string movementType);
    }
}
