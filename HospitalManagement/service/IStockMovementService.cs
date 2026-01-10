using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;

namespace HospitalManagement.service
{
    /// <summary>
    /// Service interface cho quản lý xuất nhập kho
    /// </summary>
    public interface IStockMovementService
    {
        /// <summary>
        /// Tạo giao dịch xuất/nhập kho
        /// Tự động cập nhật inventory và ghi lại quantity_before/after
        /// </summary>
        void CreateMovement(CreateStockMovementRequest request);

        /// <summary>
        /// Lấy tất cả giao dịch
        /// </summary>
        List<StockMovementResponse> GetAll();

        /// <summary>
        /// Lấy giao dịch theo warehouse
        /// </summary>
        List<StockMovementResponse> GetByWarehouse(long warehouseId);

        /// <summary>
        /// Lấy giao dịch theo product
        /// </summary>
        List<StockMovementResponse> GetByProduct(long productId);

        /// <summary>
        /// Lấy giao dịch theo loại
        /// </summary>
        List<StockMovementResponse> GetByMovementType(StockMovementType movementType);

        /// <summary>
        /// Lấy giao dịch trong khoảng thời gian
        /// </summary>
        List<StockMovementResponse> GetByDateRange(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Lấy lịch sử xuất nhập của sản phẩm tại kho
        /// </summary>
        List<StockMovementResponse> GetHistoryByProductAndWarehouse(long productId, long warehouseId);
    }
}
