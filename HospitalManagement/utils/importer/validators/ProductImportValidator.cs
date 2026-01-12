using HospitalManagement.dto.request;
using HospitalManagement.repository;
using HospitalManagement.utils.importer.core;

namespace HospitalManagement.utils.importer.validators
{
    /// <summary>
    /// Validator để kiểm tra dữ liệu ProductImportDto
    /// Strategy Pattern - implement cách validate cụ thể cho Product
    /// </summary>
    public class ProductImportValidator : IImportValidator<ProductImportDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        public ProductImportValidator(IProductRepository productRepository, ICategoryRepository categoryRepository, IManufacturerRepository manufacturerRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _manufacturerRepository = manufacturerRepository;
        }

        public List<ImportError> Validate(ProductImportDto data, int rowIndex)
        {
            var errors = new List<ImportError>();

            // Validate Code
            if (string.IsNullOrWhiteSpace(data.Code))
            {
                errors.Add(new ImportError(rowIndex, "Code", "Code is required"));
            }
            else if (_productRepository.ExistsByCode(data.Code))
            {
                errors.Add(new ImportError(rowIndex, "Code", $"Code already exists: {data.Code}"));
            }

            // Validate Name
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                errors.Add(new ImportError(rowIndex, "Name", "Name is required"));
            }

            // Validate Category Code
            if (string.IsNullOrWhiteSpace(data.CategoryCode))
            {
                errors.Add(new ImportError(rowIndex, "Category Code", "Category Code is required"));
            }
            else
            {
                var category = _categoryRepository.FindByCode(data.CategoryCode);
                if (category == null)
                {
                    errors.Add(new ImportError(rowIndex, "Category Code",
                        $"Category not found: {data.CategoryCode}"));
                }
            }

            // Validate Manufacturer Code (optional nhưng nếu có thì phải tồn tại)
            if (!string.IsNullOrWhiteSpace(data.ManufacturerCode))
            {
                if (!_manufacturerRepository.ExistsByCode(data.ManufacturerCode))
                {
                    errors.Add(new ImportError(rowIndex, "Manufacturer Code",
                        $"Manufacturer not found: {data.ManufacturerCode}"));
                }
            }

            // Validate Price
            if (data.StandardPrice.HasValue && data.StandardPrice.Value < 0)
            {
                errors.Add(new ImportError(rowIndex, "Standard Price", "Price must be positive"));
            }

            return errors;
        }
    }
}
