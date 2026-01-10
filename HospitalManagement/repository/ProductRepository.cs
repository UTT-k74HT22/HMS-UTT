using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Product;

namespace HospitalManagement.repository;

    public interface IProductRepository
    {
        /// Thêm mới một sản phẩm vào cơ sở dữ liệu.
        long Insert(CreateProductRequest request);
        
        /// Lấy tất cả sản phẩm từ cơ sở dữ liệu.
        List<ProductResponse> GetAll();
        
        /// Tìm kiếm sản phẩm theo mã.
        ProductResponse FindByCode(string code);


        /// Lấy thông tin sản phẩm theo ID (phục vụ chỉnh sửa).
        ProductResponse FindById(long id);

        /// Tìm kiếm chi tiết sản phẩm theo mã.
        ProductDetailResponse FindDetailByCode(string code);

 
        /// Kiểm tra sự tồn tại của sản phẩm theo mã.
        bool ExistsByCode(string code);
        
        /// Cập nhật thông tin sản phẩm theo mã.
        void UpdateByCode(string code, UpdateProductRequest request);


        /// Xóa mềm sản phẩm theo mã.
        void SoftDeleteByCode(string code);
        
        /// Tìm kiếm các lô hàng theo ID sản phẩm.
        List<BatchResponse> FindBatchesByProduct(long productId);

        /// Tìm kiếm sản phẩm theo ID danh mục.
        List<ProductResponse> FindByCategory(long categoryId);

    }
