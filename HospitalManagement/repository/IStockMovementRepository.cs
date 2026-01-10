using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;

namespace HospitalManagement.repository
{
    /// <summary>
    /// Repository interface cho quản lý xuất nhập kho
    /// </summary>
    public interface IStockMovementRepository
    {
        /// <summary>
        /// Tạo giao dịch xuất/nhập kho
        /// </summary>
        long Create(CreateStockMovementRequest request);

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
        /// Lấy giao dịch theo loại (IMPORT/EXPORT/ADJUST/TRANSFER)
        /// </summary>
        List<StockMovementResponse> GetByMovementType(StockMovementType movementType);

        /// <summary>
        /// Lấy giao dịch trong khoảng thời gian
        /// </summary>
        List<StockMovementResponse> GetByDateRange(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Lấy giao dịch theo ID
        /// </summary>
        StockMovementResponse FindById(long id);

        /// <summary>
        /// Lấy lịch sử xuất nhập của một sản phẩm tại một kho
        /// </summary>
        List<StockMovementResponse> GetHistoryByProductAndWarehouse(long productId, long warehouseId);

        /// <summary>
        /// Tạo giao dịch xuất/nhập kho với tracking quantity before/after (dùng trong transaction)
        /// </summary>
        long InsertWithQuantityTracking(CreateStockMovementRequest request);
    }
}
