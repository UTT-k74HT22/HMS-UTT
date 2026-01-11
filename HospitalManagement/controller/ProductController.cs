using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Category;
using HospitalManagement.dto.response.Product;
using HospitalManagement.repository.impl;
using HospitalManagement.service;
using HospitalManagement.Service.Impl;

namespace HospitalManagement.controller
{
    public class ProductController
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public List<ProductResponse> GetAll()
            => _productService.GetAll();

        public ProductResponse GetByCode(string code)
            => _productService.GetByCode(code);

        public ProductDetailResponse GetDetail(string code)
            => _productService.GetDetailByCode(code);

        public void Create(CreateProductRequest request)
            => _productService.Create(request);

        public void Update(string code, UpdateProductRequest request)
            => _productService.Update(code, request);

        public void Delete(string code)
            => _productService.Delete(code);

        public List<CategoryResponse> GetAllCategories()
            => _productService.getAllCategories(); 

        public List<ManufacturerResponse> GetAllManufacturers()
            => _productService.GetAllManufacturers();
        
        public List<BatchResponse> GetBatchesByProduct(long productId)
            => _productService.GetBatchesByProduct(productId);
        public List<ProductResponse> GetByCategory(long categoryId)
            => _productService.GetByCategory(categoryId);

        // public List<ProductResponse> SearchByName(string name)
        //     => _productService.SearchByName(name);

    }
}