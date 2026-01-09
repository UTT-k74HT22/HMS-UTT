namespace HospitalManagement.dto.response
{
    /// <summary>
    /// Response DTO cho thông tin inventory item (dùng trong transaction)
    /// </summary>
    public class InventoryItemInfo
    {
        public long InventoryItemId { get; set; }
        public int CurrentQuantity { get; set; }
    }
}
