using HospitalManagement.entity.enums;

namespace HospitalManagement.dto.request.Product;

public class UpdateProductRequest
{
   
    /// ID danh mục (categories.id)
    public long CategoryId { get; set; }

    public long? ManufacturerId { get; set; }
    
    public string Barcode { get; set; }
    
    public string Name { get; set; }
    public string DosageForm { get; set; }

    public string Unit { get; set; }

    public string Description { get; set; }
    
    public decimal StandardPrice { get; set; }
    
    public bool RequiresPrescription { get; set; }

    public ProductStatus Status { get; set; }
}