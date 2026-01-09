
using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response;
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
        private readonly IProductRepository productRepository = new ProductRepositoryImpl();
        // private readonly ICategoryRepository categoryRepository = new CategoryRepositoryImpl();
        // private readonly IManufacturerRepository manufacturerRepository = new ManufacturerRepositoryImpl();

        /// <summary>
        /// Tạo mới sản phẩm.
        /// </summary>
        public void Create(CreateProductRequest request)
        {
            if (productRepository.ExistsByCode(request.Code))
            {
                throw new ArgumentException(
                    "Product code already exists: " + request.Code
                );
            }

            productRepository.Insert(request);
        }

        /// <summary>
        /// Cập nhật sản phẩm theo code.
        /// </summary>
        public void Update(string code, UpdateProductRequest request)
        {
            ProductResponse existing =
                productRepository.FindByCode(code)
                ?? throw new ArgumentException(
                    "Product not found with code: " + code
                );

            if (existing.Status == ProductStatus.DISCONTINUED)
            {
                throw new InvalidOperationException(
                    "Cannot update discontinued product"
                );
            }

            productRepository.UpdateByCode(code, request);
        }

        /// <summary>
        /// Xóa mềm sản phẩm.
        /// </summary>
        public void Delete(string code)
        {
            productRepository.SoftDeleteByCode(code);
        }

        /// <summary>
        /// Lấy danh sách tất cả sản phẩm.
        /// </summary>
        public List<ProductResponse> GetAll()
        {
            return productRepository.GetAll();
        }

        /// <summary>
        /// Lấy sản phẩm theo code.
        /// </summary>
        public ProductResponse GetByCode(string code)
        {
            return productRepository.FindByCode(code)
                   ?? throw new ArgumentException(
                       "Product not found with code: " + code
                   );
        }

        public ProductDetailResponse GetDetailByCode(string code)
        {
            return productRepository.FindDetailByCode(code)
                   ?? throw new ArgumentException(
                       "Product not found with code: " + code
                   );
        }

        // public List<CategoryResponse> GetAllCategories()
        // {
        //     return categoryRepository.FindAllActive();
        // }

        public List<BatchResponse> GetBatchesByProduct(long productId)
        {
            return productRepository.FindBatchesByProduct(productId);
        }

        /// <summary>
        /// Lấy toàn bộ nhà sản xuất.
        /// </summary>
        // public List<ManufacturerResponse> GetAllManufacturers()
        // {
        //     return manufacturerRepository.FindAllActive();
        // }
    }
}
