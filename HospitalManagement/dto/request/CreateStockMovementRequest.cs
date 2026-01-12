using HospitalManagement.entity;

namespace HospitalManagement.dto.request
{
    /// <summary>
    /// Request DTO để tạo giao dịch xuất/nhập kho
    /// </summary>
    public class CreateStockMovementRequest
    {
        public StockMovementType MovementType { get; set; }
        public long ProductId { get; set; }
        public long? BatchId { get; set; } // Optional
        public long WarehouseId { get; set; } // For TRANSFER: source warehouse (FROM)
        public long? DestinationWarehouseId { get; set; } // For TRANSFER: destination warehouse (TO)
        public int Quantity { get; set; }
        public string ReferenceType { get; set; } // Optional: "IMPORT_ORDER", "SALE_ORDER", "STOCK_CHECK"
        public long? ReferenceId { get; set; } // Optional
        public long PerformedByUserId { get; set; }
        public string Note { get; set; }
        public int? QuantityBefore { get; set; }
        public int? QuantityAfter { get; set; }
    }
}
