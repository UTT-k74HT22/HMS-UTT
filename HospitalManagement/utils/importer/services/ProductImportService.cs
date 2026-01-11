using HospitalManagement.dto.request;
using HospitalManagement.dto.request.Product;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using HospitalManagement.repository;
using HospitalManagement.utils.importer.core;
using HospitalManagement.utils.importer.mappers;
using HospitalManagement.utils.importer.validators;

namespace HospitalManagement.utils.importer.services
{
    /// <summary>
    /// Service để import Product từ file Excel
    /// Template Method Pattern - kế thừa AbstractImportService và implement các bước cụ thể
    /// 
    /// Workflow:
    /// 1. User chọn file Excel
    /// 2. Gọi PreviewFromFile() để xem trước và validate
    /// 3. Nếu OK, gọi ApplyImport() để lưu vào DB
    /// </summary>
    public class ProductImportService : AbstractImportService<ProductImportDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        public ProductImportService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IManufacturerRepository manufacturerRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _manufacturerRepository = manufacturerRepository;
        }

        /// <summary>
        /// Định nghĩa cách map từ Excel row sang DTO
        /// Strategy Pattern - sử dụng ProductImportMapper
        /// </summary>
        protected override IImportMapper<ProductImportDto> GetMapper()
        {
            return new ProductImportMapper();
        }

        /// <summary>
        /// Định nghĩa các rule validate
        /// Strategy Pattern - sử dụng ProductImportValidator
        /// </summary>
        protected override IImportValidator<ProductImportDto> GetValidator()
        {
            return new ProductImportValidator(
                _productRepository,
                _categoryRepository,
                _manufacturerRepository
            );
        }

        /// <summary>
        /// Lưu dữ liệu hợp lệ vào database
        /// </summary>
        protected override void SaveData(List<ProductImportDto> validData)
        {
            foreach (var dto in validData)
            {
                var createRequest = new CreateProductRequest
                {
                    Code = dto.Code,
                    Name = dto.Name,
                    Barcode = dto.Barcode,
                    DosageForm = dto.DosageForm,
                    Unit = dto.Unit,
                    Description = dto.Description,
                    StandardPrice = dto.StandardPrice ?? 0,
                    RequiresPrescription = dto.RequiresPrescription,
                    Status = ProductStatus.ACTIVE
                };

                // Get Category ID by code
                var category = _categoryRepository.FindByCode(dto.CategoryCode);
                if (category != null)
                {
                    createRequest.CategoryId = category.Id;
                }

                // Get Manufacturer ID by code (optional)
                if (!string.IsNullOrEmpty(dto.ManufacturerCode))
                {
                    var manufacturers = _manufacturerRepository.FindAll();
                    var manufacturer = manufacturers.FirstOrDefault(m => m.Code == dto.ManufacturerCode);
                    if (manufacturer != null)
                    {
                        createRequest.ManufacturerId = manufacturer.Id;
                    }
                }

                _productRepository.Insert(createRequest);
            }
        }
    }
}
