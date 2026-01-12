namespace HospitalManagement.utils.importer.dto
{
    /// <summary>
    /// Data Transfer Object cho Stock Movement Excel import
    /// Map từ Excel headers sang properties này
    /// </summary>
    public class StockMovementImportDto
    {
        /// <summary>
        /// Loại: IMPORT, EXPORT, ADJUST
        /// </summary>
        public string? MovementType { get; set; }

        /// <summary>
        /// Mã kho hàng (WarehouseId hoặc Code)
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string? ProductCode { get; set; }

        /// <summary>
        /// Mã lô (optional)
        /// </summary>
        public string? BatchCode { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Ghi chú (optional)
        /// </summary>
        public string? Note { get; set; }
    }
}
