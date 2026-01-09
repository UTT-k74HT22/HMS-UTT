namespace HospitalManagement.dto.response
{
    /// <summary>
    /// Response DTO cho danh sách tồn kho
    /// </summary>
    public class InventoryResponse
    {
        public long? Id { get; set; }
        
        // Product info
        public long? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        
        // Batch info (optional)
        public long? BatchId { get; set; }
        public string BatchCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        
        // Warehouse info
        public long? WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        
        // Inventory quantities
        public int? QuantityOnHand { get; set; }
        public int? QuantityReserved { get; set; }
        public int? QuantityAvailable { get; set; } // = QuantityOnHand - QuantityReserved
        
        // Thresholds
        public int? MinThreshold { get; set; }
        public int? MaxThreshold { get; set; }
        
        // Status indicators
        public bool IsLowStock { get; set; }    // QuantityOnHand <= MinThreshold
        public bool IsOverStock { get; set; }   // QuantityOnHand >= MaxThreshold
        public bool IsNearExpiry { get; set; }  // ExpiryDate within 3 months
        
        public DateTime? LastStockCheck { get; set; }

        public InventoryResponse() { }

        public override string ToString()
        {
            return WarehouseName; // ComboBox sẽ hiện tên kho
        }
    }
}
