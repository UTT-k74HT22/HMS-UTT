using HospitalManagement.entity.enums;

namespace HospitalManagement.dto.response.Product;

public class ProductResponse
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public string CategoryName { get; set; }
    public string ManufacturerName { get; set; }

    public decimal StandardPrice { get; set; }
    public bool RequiresPrescription { get; set; }

    public ProductStatus Status { get; set; }
}