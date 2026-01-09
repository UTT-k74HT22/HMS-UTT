using HospitalManagement.dto.request.Batch;
using HospitalManagement.dto.response;

namespace HospitalManagement.service;

public interface IBatchService
{
    /// <summary>
    /// Lấy toàn bộ lô hàng
    /// </summary>
    List<BatchResponse> GetAll();

    /// <summary>
    /// Lấy danh sách lô theo sản phẩm
    /// </summary>
    List<BatchResponse> GetByProduct(long productId);

    /// <summary>
    /// Lấy chi tiết một lô
    /// </summary>
    BatchResponse? GetDetail(long batchId);

    /// <summary>
    /// Tạo mới một lô hàng
    /// </summary>
    long Create(CreateBatchRequest request);

    /// <summary>
    /// Cập nhật lô hàng (expiryDate, status)
    /// </summary>
    long Update(long batchId, UpdateBatchRequest updateBatchRequest);

    /// <summary>
    /// Ngưng sử dụng một lô hàng
    /// </summary>
    void Disable(long batchId);
    // tim kiem
    List<BatchResponse> FindByBatchCode(string keyword);
}
