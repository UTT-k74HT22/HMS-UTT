using HospitalManagement.dto.request.Batch;
using HospitalManagement.dto.response;

namespace HospitalManagement.repository;

public interface IBatchRepository
{
    /// <summary>
    /// Lấy toàn bộ lô hàng
    /// </summary>
    List<BatchResponse> FindAll();

    BatchResponse FindById(long id);

    /// <summary>
    /// Lấy danh sách lô hàng theo sản phẩm
    /// </summary>
    List<BatchResponse> FindByProduct(long productId);

    /// <summary>
    /// Lấy chi tiết 1 lô hàng
    /// </summary>
    BatchResponse? FindDetail(long batchId);

    /// <summary>
    /// Tạo mới lô hàng
    /// </summary>
    long Insert(CreateBatchRequest request);

    /// <summary>
    /// Cập nhật thông tin lô hàng
    /// </summary>
    void Update(long batchId, UpdateBatchRequest request);

    /// <summary>
    /// Ngưng sử dụng lô hàng
    /// </summary>
    void Disable(long batchId);
    // kiem tra trung lap
    bool ExistsBatchCode(string batchCode);
    // tìm kiem theo mã lô
    List<BatchResponse> FindByBatchCode(string keyword);
}
