using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;

namespace HospitalManagement.service.impl
{
    /// <summary>
    /// Service implementation cho quản lý xuất nhập kho
    /// </summary>
    public class StockMovementServiceImpl : IStockMovementService
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public StockMovementServiceImpl(
            IStockMovementRepository stockMovementRepository,
            IInventoryRepository inventoryRepository)
        {
            _stockMovementRepository = stockMovementRepository;
            _inventoryRepository = inventoryRepository;
        }

        public void CreateMovement(CreateStockMovementRequest request)
        {
            // TODO: Implement with transaction
            // 1. Get current quantity
            // 2. Calculate new quantity
            // 3. Update inventory
            // 4. Insert stock movement
            throw new NotImplementedException();
        }

        public List<StockMovementResponse> GetAll()
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        public List<StockMovementResponse> GetByWarehouse(long warehouseId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        public List<StockMovementResponse> GetByProduct(long productId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        public List<StockMovementResponse> GetByMovementType(StockMovementType movementType)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        public List<StockMovementResponse> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        public List<StockMovementResponse> GetHistoryByProductAndWarehouse(long productId, long warehouseId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
