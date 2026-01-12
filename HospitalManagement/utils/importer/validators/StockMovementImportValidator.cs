using HospitalManagement.repository;
using HospitalManagement.utils.importer.core;
using HospitalManagement.utils.importer.dto;

namespace HospitalManagement.utils.importer.validators
{
    /// <summary>
    /// Strategy implementation: Validation logic cho Stock Movement import
    /// Kiểm tra business rules trước khi lưu DB
    /// </summary>
    public class StockMovementImportValidator : IImportValidator<dto.StockMovementImportDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IWarehousesRepository _warehouseRepository;
        private readonly IBatchRepository _batchRepository;

        public StockMovementImportValidator(IProductRepository productRepository, IWarehousesRepository warehouseRepository, IBatchRepository batchRepository)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _batchRepository = batchRepository;
        }

        /// <summary>
        /// Validate 1 dòng dữ liệu đã map
        /// </summary>
        public List<ImportError> Validate(StockMovementImportDto dto, int rowIndex)
        {
            var errors = new List<ImportError>();

            // 1. MovementType: bắt buộc, phải là IMPORT/EXPORT/ADJUST
            if (string.IsNullOrWhiteSpace(dto.MovementType))
            {
                errors.Add(new ImportError(rowIndex, "MovementType", "MovementType is required"));
            }
            else
            {
                var validTypes = new[] { "IMPORT", "EXPORT", "ADJUST" };
                if (!validTypes.Contains(dto.MovementType.ToUpper()))
                {
                    errors.Add(new ImportError(rowIndex, "MovementType", $"MovementType invalid: {dto.MovementType}. Accept only: IMPORT, EXPORT, ADJUST"));
                }
            }

            // 2. WarehouseCode: bắt buộc, phải tồn tại trong DB (tìm theo Code hoặc Name)
            if (string.IsNullOrWhiteSpace(dto.WarehouseCode))
            {
                errors.Add(new ImportError(rowIndex, "Warehouse", "Ware house is required"));
            }
            else
            {
                // Tìm warehouse theo Code trước
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
                    errors.Add(new ImportError(rowIndex, "Warehouse", 
                        $"Warehouse invalid : '{dto.WarehouseCode}'. Please import correct Warehouse name."));
                }
            }

            // 3. ProductCode: bắt buộc, phải tồn tại trong DB
            if (string.IsNullOrWhiteSpace(dto.ProductCode))
            {
                errors.Add(new ImportError(rowIndex, "Product code", "Product code is required"));
            }
            else
            {
                var product = _productRepository.FindByCode(dto.ProductCode);
                if (product == null)
                {
                    errors.Add(new ImportError(rowIndex, "Product code", $"Product not found: {dto.ProductCode}"));
                }
            }

            // 4. BatchCode: optional, nhưng nếu có thì phải tồn tại
            if (!string.IsNullOrWhiteSpace(dto.BatchCode))
            {
                var batches = _batchRepository.FindByBatchCode(dto.BatchCode);
                if (batches == null || batches.Count == 0)
                {
                    errors.Add(new ImportError(rowIndex, "Batch code", $"Batch code not found: {dto.BatchCode}"));
                }
            }

            // 5. Quantity: phải > 0
            if (dto.Quantity <= 0)
            {
                errors.Add(new ImportError(rowIndex, "Quantity", $"Quantity must be large 0 (Now: {dto.Quantity})"));
            }

            return errors;
        }
    }
}
