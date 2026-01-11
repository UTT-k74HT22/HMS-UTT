namespace HospitalManagement.dto.request
{
    /// <summary>
    /// DTO đơn giản để import Product từ Excel
    /// Tương đương với ImportProductRequest.java
    /// </summary>
    public class ProductImportDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CategoryCode { get; set; } = string.Empty;
        public string? ManufacturerCode { get; set; }
        public string? Barcode { get; set; }
        public string? DosageForm { get; set; }
        public string? Unit { get; set; }
        public string? Description { get; set; }
        public decimal? StandardPrice { get; set; }
        public bool RequiresPrescription { get; set; }
    }
}
