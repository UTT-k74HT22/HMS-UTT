using HospitalManagement.entity;
using HospitalManagement.repository;
using HospitalManagement.utils.importer.core;
using System;
using System.Linq;

namespace HospitalManagement.utils.importer.validator
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

        public StockMovementImportValidator(
            IProductRepository productRepository,
            IWarehousesRepository warehouseRepository,
            IBatchRepository batchRepository)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _batchRepository = batchRepository;
        }

        /// <summary>
        /// Validate 1 dòng dữ liệu đã map
        /// </summary>
        public List<ImportError> Validate(dto.StockMovementImportDto dto, int rowIndex)
        {
            var errors = new List<ImportError>();

            // 1. MovementType: bắt buộc, phải là IMPORT/EXPORT/ADJUST
            if (string.IsNullOrWhiteSpace(dto.MovementType))
            {
                errors.Add(new ImportError(rowIndex, "Loại", "Loại không được để trống"));
            }
            else
            {
                var validTypes = new[] { "IMPORT", "EXPORT", "ADJUST" };
                if (!validTypes.Contains(dto.MovementType.ToUpper()))
                {
                    errors.Add(new ImportError(rowIndex, "Loại", $"Loại không hợp lệ: {dto.MovementType}. Chỉ chấp nhận: IMPORT, EXPORT, ADJUST"));
                }
            }

            // 2. WarehouseCode: bắt buộc, phải tồn tại trong DB (tìm theo Code hoặc Name)
            if (string.IsNullOrWhiteSpace(dto.WarehouseCode))
            {
                errors.Add(new ImportError(rowIndex, "Kho hàng", "Kho hàng không được để trống"));
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
                    errors.Add(new ImportError(rowIndex, "Kho hàng", 
                        $"Kho hàng không tồn tại: '{dto.WarehouseCode}'. Vui lòng nhập đúng tên hoặc mã kho."));
                }
            }

            // 3. ProductCode: bắt buộc, phải tồn tại trong DB
            if (string.IsNullOrWhiteSpace(dto.ProductCode))
            {
                errors.Add(new ImportError(rowIndex, "Mã sản phẩm", "Mã sản phẩm không được để trống"));
            }
            else
            {
                var product = _productRepository.FindByCode(dto.ProductCode);
                if (product == null)
                {
                    errors.Add(new ImportError(rowIndex, "Mã sản phẩm", $"Sản phẩm không tồn tại: {dto.ProductCode}"));
                }
            }

            // 4. BatchCode: optional, nhưng nếu có thì phải tồn tại
            if (!string.IsNullOrWhiteSpace(dto.BatchCode))
            {
                var batches = _batchRepository.FindByBatchCode(dto.BatchCode);
                if (batches == null || batches.Count == 0)
                {
                    errors.Add(new ImportError(rowIndex, "Mã lô", $"Lô hàng không tồn tại: {dto.BatchCode}"));
                }
            }

            // 5. Quantity: phải > 0
            if (dto.Quantity <= 0)
            {
                errors.Add(new ImportError(rowIndex, "Số lượng", $"Số lượng phải lớn hơn 0 (hiện tại: {dto.Quantity})"));
            }

            return errors;
        }
    }
}
