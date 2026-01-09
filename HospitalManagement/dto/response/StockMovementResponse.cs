using HospitalManagement.entity;

namespace HospitalManagement.dto.response
{
    /// <summary>
    /// Response DTO cho lịch sử xuất nhập kho
    /// </summary>
    public class StockMovementResponse
    {
        public long? Id { get; set; }
        
        // Movement info
        public StockMovementType MovementType { get; set; }
        public DateTime? MovementDate { get; set; }
        
        // Product info
        public long? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        
        // Batch info (optional)
        public long? BatchId { get; set; }
        public string BatchCode { get; set; }
        
        // Warehouse info
        public long? WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        
        // Quantity changes
        public int? Quantity { get; set; }
        public int? QuantityBefore { get; set; }
        public int? QuantityAfter { get; set; }
        
        // Reference
        public string ReferenceType { get; set; }
        public long? ReferenceId { get; set; }
        
        // Who performed
        public long? PerformedByUserId { get; set; }
        public string PerformedByUsername { get; set; }
        public string PerformedByFullName { get; set; }
        
        public string Note { get; set; }

        public StockMovementResponse() { }
    }
}
