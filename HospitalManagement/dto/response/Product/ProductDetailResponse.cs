using HospitalManagement.entity.enums;

namespace HospitalManagement.dto.response.Product;

public class ProductDetailResponse
{
   
    public long Id { get; set; }

    public string Code { get; set; }
    public string Barcode { get; set; }
    public string Name { get; set; }

    public long CategoryId { get; set; }
    public string CategoryName { get; set; }

    public long? ManufacturerId { get; set; }
    public string ManufacturerCode { get; set; }
    public string ManufacturerName { get; set; }
    public string ManufacturerCountry { get; set; }

    public string DosageForm { get; set; }
    public string Unit { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public decimal StandardPrice { get; set; }
    public bool RequiresPrescription { get; set; }
    public ProductStatus Status { get; set; } 
}