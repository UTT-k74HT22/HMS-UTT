using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Category;
using HospitalManagement.dto.response.Product;

namespace HospitalManagement.service;

public interface IProductService
{
    void Create(CreateProductRequest request);
    
    void Update(string code, UpdateProductRequest request);
    
    void Delete(string code);

    List<ProductResponse> GetAll();
    
    ProductResponse GetByCode(string code);
    
    ProductDetailResponse GetDetailByCode(string code);

    List<CategoryResponse> getAllCategories();
    
    List<ProductResponse> GetByCategory(long categoryId);

    List<ManufacturerResponse> GetAllManufacturers();
    
    List<BatchResponse> GetBatchesByProduct(long productId);
}