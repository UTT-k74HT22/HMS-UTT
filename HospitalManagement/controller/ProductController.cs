using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Product;
using HospitalManagement.repository.impl;
using HospitalManagement.service;
using HospitalManagement.Service.Impl;

namespace HospitalManagement.controller;

public class ProductController
{
        private readonly IProductService productService;
        private readonly ProductRepositoryImpl repo = new ProductRepositoryImpl();

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public ProductController()
        {
            productService = new ProductServiceImpl();

        }


        /// <summary>
        /// Lấy danh sách tất cả sản phẩm.
        /// </summary>
        public List<ProductResponse> GetAll()
        {
            return productService.GetAll();
        }

        /// <summary>
        /// Lấy sản phẩm theo mã.
        /// </summary>
        public ProductResponse GetByCode(string code)
        {
            return productService.GetByCode(code);
        }

        /// <summary>
        /// Lấy sản phẩm chi tiết theo mã.
        /// </summary>
        public ProductDetailResponse GetDetail(string code)
        {
            return productService.GetDetailByCode(code);
        }

        /// <summary>
        /// Thêm mới sản phẩm.
        /// </summary>
        public void Create(CreateProductRequest request)
        {
            productService.Create(request);
        }

        /// <summary>
        /// Cập nhật sản phẩm theo mã.
        /// </summary>
        public void Update(string code, UpdateProductRequest request)
        {
            productService.Update(code, request);
        }

        /// <summary>
        /// Xóa mềm sản phẩm.
        /// </summary>
        public void Delete(string code)
        {
            productService.Delete(code);
        }

        /// <summary>
        /// Lấy danh sách tất cả danh mục.
        /// </summary>
        // public List<CategoryResponse> GetAllCategories()
        // {
        //     return productService.GetAllCategories();
        // }

        /// <summary>
        /// Lấy danh sách tất cả nhà sản xuất.
        /// </summary>
        // public List<ManufacturerResponse> GetAllManufacturers()
        // {
        //     return productService.GetAllManufacturers();
        // }

        /// <summary>
        /// Lấy danh sách sản phẩm theo category (phục vụ tạo đơn hàng)
        /// </summary>
        public List<ProductResponse> GetByCategory(long categoryId)
        {
            return repo.FindByCategory(categoryId);
        }

        /// <summary>
        /// Lấy sản phẩm đúng lô hàng
        /// </summary>
        public List<BatchResponse> GetBatchesByProduct(long productId)
        {
            return repo.FindBatchesByProduct(productId);
        }
     
}