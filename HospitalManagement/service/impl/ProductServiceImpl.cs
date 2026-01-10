using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Category; 
using HospitalManagement.dto.response.Product;
using HospitalManagement.entity.enums;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;
using HospitalManagement.service;

namespace HospitalManagement.Service.Impl
{
    /// <summary>
    /// Service xử lý nghiệp vụ sản phẩm.
    /// </summary>
    public class ProductServiceImpl : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        public ProductServiceImpl(string connectionString)
        {
            _productRepository = new ProductRepositoryImpl(connectionString);
            _categoryRepository = new CategoryRepositoryImpl(connectionString);
            _manufacturerRepository = new ManufacturerRepositoryImpl(connectionString);
        }

        // ================= CRUD =================

        public void Create(CreateProductRequest request)
        {
            if (_productRepository.ExistsByCode(request.Code))
                throw new ArgumentException(
                    "Product code already exists: " + request.Code
                );

            _productRepository.Insert(request);
        }

        public void Update(string code, UpdateProductRequest request)
        {
            ProductResponse existing =
                _productRepository.FindByCode(code)
                ?? throw new ArgumentException(
                    "Product not found with code: " + code
                );

            if (existing.Status == ProductStatus.DISCONTINUED)
                throw new InvalidOperationException(
                    "Cannot update discontinued product"
                );

            _productRepository.UpdateByCode(code, request);
        }

        public void Delete(string code)
        {
            _productRepository.SoftDeleteByCode(code);
        }

        // ================= QUERY =================

        public List<ProductResponse> GetAll()
            => _productRepository.GetAll();

        public ProductResponse GetByCode(string code)
            => _productRepository.FindByCode(code)
               ?? throw new ArgumentException(
                   "Product not found with code: " + code
               );

        public ProductDetailResponse GetDetailByCode(string code)
            => _productRepository.FindDetailByCode(code)
               ?? throw new ArgumentException(
                   "Product not found with code: " + code
               );

        public List<CategoryResponse> GetAllCategories()
            => _categoryRepository.FindAllActive();

        public List<ManufacturerResponse> GetAllManufacturers()
            => _manufacturerRepository.FindAllActive();

        public List<BatchResponse> GetBatchesByProduct(long productId)
            => _productRepository.FindBatchesByProduct(productId);

    }

}
