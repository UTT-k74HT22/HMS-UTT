using HospitalManagement.dto.request;
using HospitalManagement.entity;
using HospitalManagement.repository;
using HospitalManagement.service;
using HospitalManagement.utils.importer.core;
using HospitalManagement.utils.importer.validators;

namespace HospitalManagement.utils.importer.services
{
    /// <summary>
    /// Concrete service: Stock Movement Import
    /// Extends AbstractImportService để thực hiện Template Method Pattern
    /// </summary>
    public class StockMovementImportService : AbstractImportService<dto.StockMovementImportDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IWarehousesRepository _warehouseRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IStockMovementService _stockMovementService;
        private readonly mapper.StockMovementImportMapper _mapper;
        private readonly StockMovementImportValidator _validator;
        public StockMovementImportService(
            IProductRepository productRepository, IWarehousesRepository warehouseRepository, IBatchRepository batchRepository, IStockMovementService stockMovementService)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _batchRepository = batchRepository;
            _stockMovementService = stockMovementService;

            // Khởi tạo mapper và validator
            _mapper = new mapper.StockMovementImportMapper();
            _validator = new StockMovementImportValidator(productRepository, warehouseRepository, batchRepository);
        }

        /// <summary>
        /// Hook method: Trả về mapper instance
        /// </summary>
        protected override IImportMapper<dto.StockMovementImportDto> GetMapper()
        {
            return _mapper;
        }

        /// <summary>
        /// Hook method: Trả về validator instance
        /// </summary>
        protected override IImportValidator<dto.StockMovementImportDto> GetValidator()
        {
            return _validator;
        }

        /// <summary>
        /// Hook method: Lưu dữ liệu vào DB
        /// Convert từ StockMovementImportDto → CreateStockMovementRequest
        /// </summary>
        protected override void SaveData(List<dto.StockMovementImportDto> validData)
        {
            foreach (var dto in validData)
            {
                // Resolve Product
                var product = _productRepository.FindByCode(dto.ProductCode!);
                if (product == null)
                {
                    throw new Exception($"Product not found: {dto.ProductCode}");
                }

                // Resolve Warehouse by Code or Name
                var warehouse = _warehouseRepository.GetByCode(dto.WarehouseCode!);
                
                // Nếu không tìm thấy theo Code, thử tìm theo Name
                if (warehouse == null)
                {
                    var allWarehouses = _warehouseRepository.GetAll();
                    warehouse = allWarehouses?.FirstOrDefault(w => 
                        w.Name?.Equals(dto.WarehouseCode, StringComparison.OrdinalIgnoreCase) == true);
                }
                
                if (warehouse == null)
                {
                    throw new Exception($"Warehouse not found: {dto.WarehouseCode}");
                }
                
                Console.WriteLine($"[IMPORT] Resolved warehouse: {warehouse.Name} (ID: {warehouse.Id})");
                int warehouseId = (int)warehouse.Id;

                // Resolve Batch (optional)
                int? batchId = null;
                if (!string.IsNullOrWhiteSpace(dto.BatchCode))
                {
                    var batches = _batchRepository.FindByBatchCode(dto.BatchCode);
                    if (batches != null && batches.Count > 0 && batches[0].Id.HasValue)
                    {
                        batchId = (int)batches[0].Id.Value;
                    }
                }

                // Parse MovementType
                StockMovementType movementType;
                if (!Enum.TryParse(dto.MovementType, true, out movementType))
                {
                    movementType = StockMovementType.IMPORT; // Default
                }

                // Tạo request
                var request = new CreateStockMovementRequest
                {
                    MovementType = movementType,
                    ProductId = product.Id,
                    BatchId = batchId,
                    WarehouseId = warehouseId,
                    Quantity = dto.Quantity,
                    ReferenceType = "IMPORT_EXCEL",
                    ReferenceId = null,
                    PerformedByUserId = 1, // TODO: Get from current user session
                    Note = dto.Note ?? $"Import từ Excel - {DateTime.Now:yyyy-MM-dd HH:mm}"
                };

                // Gọi service để tạo movement (sẽ tự động cập nhật inventory)
                _stockMovementService.CreateMovement(request);
            }
        }
    }
}
