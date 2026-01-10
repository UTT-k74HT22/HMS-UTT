using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
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
        {
            _stockMovementService = stockMovementService;
        }

        /// <summary>
        /// Tạo giao dịch xuất/nhập kho mới
        /// </summary>
        public void CreateStockMovement(CreateStockMovementRequest request)
        {
            // TODO: Implement
            // 1. Validate request
            // 2. Check product/warehouse exists
            // 3. For EXPORT, check stock availability
            // 4. Create movement (service will handle inventory update)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy tất cả giao dịch xuất nhập kho
        /// </summary>
        public List<StockMovementResponse> GetAllMovements()
        {
            Console.WriteLine("Fetching all stock movements");
            return _stockMovementService.GetAll();
        }

        /// <summary>
        /// Lấy giao dịch theo warehouse
        /// </summary>
        public List<StockMovementResponse> GetMovementsByWarehouse(long warehouseId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy giao dịch theo product
        /// </summary>
        public List<StockMovementResponse> GetMovementsByProduct(long productId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy giao dịch theo loại
        /// </summary>
        public List<StockMovementResponse> GetMovementsByType(StockMovementType movementType)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy giao dịch trong khoảng thời gian
        /// </summary>
        public List<StockMovementResponse> GetMovementsByDateRange(DateTime fromDate, DateTime toDate)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy lịch sử xuất nhập của sản phẩm tại kho
        /// </summary>
        public List<StockMovementResponse> GetProductWarehouseHistory(long productId, long warehouseId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}
