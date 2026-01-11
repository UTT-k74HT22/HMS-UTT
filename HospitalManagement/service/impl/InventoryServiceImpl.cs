using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl
{
    /// <summary>
    /// Service implementation cho quản lý tồn kho
    /// </summary>
    public class InventoryServiceImpl : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryServiceImpl(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public List<InventoryResponse> GetAll()
        {
            return _inventoryRepository.GetAll();
        }

        public List<InventoryResponse> GetByWarehouse(long warehouseId)
        {
            if (warehouseId <= 0)
            {
                throw new ArgumentException("Warehouse ID must be greater than zero.");
            }
            return  _inventoryRepository.GetByWarehouse(warehouseId);
        }

        public List<InventoryResponse> GetByProduct(long productId)
        {
            if (productId <= 0)
                throw new ArgumentException("Product ID không hợp lệ");

            return _inventoryRepository.GetByProduct(productId);
        }

        public List<InventoryResponse> GetLowStockItems()
        {
            return _inventoryRepository.GetLowStockItems();
        }

        public List<InventoryResponse> GetNearExpiryItems()
        {
            return _inventoryRepository.GetNearExpiryItems();
        }

        public void UpdateThresholds(long inventoryItemId, UpdateInventoryThresholdRequest request)
        {
            // Validate
            if (request.MinThreshold.HasValue && request.MinThreshold.Value < 0)
                throw new ArgumentException("Ngưỡng tối thiểu không thể âm");

            if (request.MaxThreshold.HasValue && request.MaxThreshold.Value < 0)
                throw new ArgumentException("Ngưỡng tối đa không thể âm");

            if (request.MinThreshold.HasValue && request.MaxThreshold.HasValue 
                                              && request.MinThreshold.Value > request.MaxThreshold.Value)
                throw new ArgumentException("Ngưỡng tối thiểu không thể lớn hơn ngưỡng tối đa");

            _inventoryRepository.UpdateThresholds(inventoryItemId, request);
        }

        public int GetTotalQuantityByProduct(long productId)
        {
            return _inventoryRepository.GetTotalQuantityByProduct(productId);
        }

        public bool HasStock(long productId, long warehouseId, int requiredQuantity)
        {
            return _inventoryRepository.HasStock(productId, warehouseId, requiredQuantity);
        }

        public void UpdateStock(long productId, long batchId, long warehouseId, int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Số lượng không thể âm");

            _inventoryRepository.UpdateStock(productId, batchId, warehouseId, newQuantity);
        }

        public void InsertStockMovement(
            long productId, long batchId, long warehouseId,
            int quantity, int before, int after,
            string note, string movementType)
        {
            long userProfileId = AuthServiceImpl.GetCurrentUserProfileId()
                                 ?? throw new Exception("Chưa đăng nhập");

            _inventoryRepository.InsertStockMovement(
                productId,
                batchId,
                warehouseId,
                quantity,
                before,
                after,
                userProfileId,  
                note,
                movementType
            );
        }


        public int GetCurrentQuantity(long productId, long batchId, long warehouseId)
        {
            return _inventoryRepository.GetCurrentQuantity(productId, batchId, warehouseId);
        }
    }
}
