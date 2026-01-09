using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Product;

namespace HospitalManagement.repository;

    public interface IProductRepository
    {
        /// <summary>
        /// Thêm mới một sản phẩm vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="request">Đối tượng chứa thông tin sản phẩm cần thêm</param>
        /// <returns>ID của sản phẩm vừa được thêm</returns>
        long Insert(CreateProductRequest request);

        /// <summary>
        /// Lấy tất cả sản phẩm từ cơ sở dữ liệu.
        /// </summary>
        /// <returns>Danh sách tất cả sản phẩm</returns>
        List<ProductResponse> GetAll();

        /// <summary>
        /// Tìm kiếm sản phẩm theo mã.
        /// </summary>
        /// <param name="code">Mã sản phẩm</param>
        /// <returns>ProductResponse nếu tìm thấy, null nếu không</returns>
        ProductResponse FindByCode(string code);

        /// <summary>
        /// Lấy thông tin sản phẩm theo ID (phục vụ chỉnh sửa).
        /// </summary>
        /// <param name="id">ID sản phẩm</param>
        /// <returns>ProductResponse nếu tìm thấy, null nếu không</returns>
        ProductResponse FindById(long id);

        /// <summary>
        /// Tìm kiếm chi tiết sản phẩm theo mã.
        /// </summary>
        /// <param name="code">Mã sản phẩm</param>
        /// <returns>ProductDetailResponse nếu tìm thấy, null nếu không</returns>
        ProductDetailResponse FindDetailByCode(string code);

        /// <summary>
        /// Kiểm tra sự tồn tại của sản phẩm theo mã.
        /// </summary>
        /// <param name="code">Mã sản phẩm</param>
        /// <returns>true nếu tồn tại, false nếu không</returns>
        bool ExistsByCode(string code);

        /// <summary>
        /// Cập nhật thông tin sản phẩm theo mã.
        /// </summary>
        /// <param name="code">Mã sản phẩm</param>
        /// <param name="request">Thông tin cập nhật</param>
        void UpdateByCode(string code, UpdateProductRequest request);

        /// <summary>
        /// Xóa mềm sản phẩm theo mã.
        /// </summary>
        /// <param name="code">Mã sản phẩm</param>
        void SoftDeleteByCode(string code);

        /// <summary>
        /// Tìm kiếm các lô hàng theo ID sản phẩm.
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>Danh sách lô hàng</returns>
        List<BatchResponse> FindBatchesByProduct(long productId);

        /// <summary>
        /// Tìm kiếm sản phẩm theo ID danh mục.
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <returns>Danh sách sản phẩm</returns>
        List<ProductResponse> FindByCategory(long categoryId);
    }
