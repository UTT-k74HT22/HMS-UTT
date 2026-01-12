using HospitalManagement.configuration;
using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.service.impl
{
    /// <summary>
    /// Service implementation cho quản lý xuất nhập kho
    /// </summary>
    public class StockMovementServiceImpl : IStockMovementService
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly string _connectionString;

        public StockMovementServiceImpl(IStockMovementRepository stockMovementRepository, IInventoryRepository inventoryRepository, DBConfig config)
        {
            _stockMovementRepository = stockMovementRepository;
            _inventoryRepository = inventoryRepository;
            _connectionString = config.ConnectionString;
        }

        /// <summary>
        /// Xuất nhập kho với việc theo dõi số lượng tồn kho trước và sau
        /// Sử dụng transaction để đảm bảo tính nhất quán dữ liệu
        /// 1. Lấy số lượng tồn kho hiện tại
        /// 2. Tính toán số lượng tồn kho mới dựa trên loại giao dịch
        /// 3. Cập nhật số lượng tồn kho
        /// 4. Ghi nhận giao dịch xuất nhập kho với số lượng trước và sau
        /// 5. Commit transaction nếu thành công, rollback nếu có lỗi
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception"></exception>
        public void CreateMovement(CreateStockMovementRequest request)
        {
            ValidateCreateMovementRequest(request);
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //Step 1: Get current quantity
                        var inventoryInfo = _inventoryRepository.GetOrCreateInventoryItem(request.ProductId,
                            request.BatchId ?? 0, request.WarehouseId);
                        Console.WriteLine("Info retrieved: " + inventoryInfo);
                        int currentQuantityBefore = inventoryInfo.CurrentQuantity;
                        int quantityAfter;

                        //Step 2: Calculate new quantity
                        switch (request.MovementType)
                        {
                            case StockMovementType.IMPORT:
                                quantityAfter = currentQuantityBefore + request.Quantity;
                                break;
                            case StockMovementType.EXPORT:
                                //check if quantity > current quantity
                                if (request.Quantity > currentQuantityBefore)
                                {
                                    throw new Exception(
                                        $"Không đủ hàng để xuất. Tồn kho hiện tại: {currentQuantityBefore}, yêu cầu xuất: {request.Quantity}");
                                }

                                quantityAfter = currentQuantityBefore - request.Quantity;
                                break;
                            case StockMovementType.ADJUST:
                                quantityAfter = currentQuantityBefore + request.Quantity;
                                break;
                            case StockMovementType.TRANSFER:
                                // Validate destination warehouse
                                if (!request.DestinationWarehouseId.HasValue || request.DestinationWarehouseId <= 0)
                                {
                                    throw new Exception("Phải chỉ định kho đích để chuyển kho");
                                }

                                if (request.WarehouseId == request.DestinationWarehouseId)
                                {
                                    throw new Exception("Kho nguồn và kho đích không được trùng nhau");
                                }

                                // Check source warehouse has enough stock
                                if (currentQuantityBefore < request.Quantity)
                                {
                                    throw new Exception(
                                        $"Kho nguồn không đủ hàng để chuyển. Tồn kho hiện tại: {currentQuantityBefore}, yêu cầu chuyển: {request.Quantity}");
                                }

                                // Step 1: Decrease from source warehouse
                                quantityAfter = currentQuantityBefore - request.Quantity;
                                _inventoryRepository.UpdateQuantity(inventoryInfo.InventoryItemId, quantityAfter);

                                // Step 2: Get destination warehouse inventory
                                var destInventoryInfo = _inventoryRepository.GetOrCreateInventoryItem(
                                    request.ProductId,
                                    request.BatchId ?? 0,
                                    request.DestinationWarehouseId.Value);
                                int destQuantityBefore = destInventoryInfo.CurrentQuantity;
                                int destQuantityAfter = destQuantityBefore + request.Quantity;

                                // Step 3: Increase in destination warehouse
                                _inventoryRepository.UpdateQuantity(destInventoryInfo.InventoryItemId, destQuantityAfter);

                                // Step 4: Record source warehouse transaction (EXPORT)
                                request.QuantityBefore = currentQuantityBefore;
                                request.QuantityAfter = quantityAfter;
                                _stockMovementRepository.InsertWithQuantityTracking(request);

                                // Step 5: Record destination warehouse transaction (IMPORT)
                                var destRequest = new CreateStockMovementRequest
                                {
                                    MovementType = StockMovementType.IMPORT,
                                    ProductId = request.ProductId,
                                    BatchId = request.BatchId,
                                    WarehouseId = request.DestinationWarehouseId.Value,
                                    Quantity = request.Quantity,
                                    ReferenceType = "TRANSFER",
                                    ReferenceId = request.ReferenceId,
                                    PerformedByUserId = request.PerformedByUserId,
                                    Note = $"Nhận chuyển kho từ WH-{request.WarehouseId}. {request.Note}",
                                    QuantityBefore = destQuantityBefore,
                                    QuantityAfter = destQuantityAfter
                                };
                                _stockMovementRepository.InsertWithQuantityTracking(destRequest);

                                // Commit and return early for TRANSFER
                                transaction.Commit();
                                return;
                            default:
                                throw new Exception($"Loại giao dịch không hợp lệ: {request.MovementType}");
                        }

                        //Step 3: Update inventory
                        _inventoryRepository.UpdateQuantity(inventoryInfo.InventoryItemId, quantityAfter);

                        //Step 4: Insert stock movement
                        request.QuantityBefore = currentQuantityBefore;
                        request.QuantityAfter = quantityAfter;
                        _stockMovementRepository.InsertWithQuantityTracking(request);
                        
                        // Commit transaction
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        
        private void ValidateCreateMovementRequest(CreateStockMovementRequest request)
        {
            if (request.ProductId <= 0)
                throw new ArgumentException("Product ID không hợp lệ");

            if (request.WarehouseId <= 0)
                throw new ArgumentException("Warehouse ID không hợp lệ");

            if (request.Quantity <= 0)
                throw new ArgumentException("Số lượng phải lớn hơn 0");

            if (request.PerformedByUserId <= 0)
                throw new ArgumentException("User ID không hợp lệ");

            // Validate TRANSFER specific requirements
            if (request.MovementType == StockMovementType.TRANSFER)
            {
                if (!request.DestinationWarehouseId.HasValue || request.DestinationWarehouseId <= 0)
                    throw new ArgumentException("Chuyển kho phải chỉ định kho đích");

                if (request.WarehouseId == request.DestinationWarehouseId)
                    throw new ArgumentException("Kho nguồn và kho đích không được trùng nhau");
            }
        }

        public List<StockMovementResponse> GetAll()
        {
            Console.WriteLine("Fetching all stock movements");
            return _stockMovementRepository.GetAll();
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
