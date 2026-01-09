using HospitalManagement.entity.enums;

namespace HospitalManagement.dto.request.Product;

public class CreateProductRequest
{
    /// <summary>
    /// ID danh mục (categories.id)
    /// </summary>
    public long CategoryId { get; set; }

    public long? ManufacturerId { get; set; }

    /// <summary>
    /// Mã sản phẩm (unique)
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Mã vạch
    /// </summary>
    public string Barcode { get; set; }

    /// <summary>
    /// Tên sản phẩm
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Dạng bào chế
    /// </summary>
    public string DosageForm { get; set; }

    /// <summary>
    /// Đơn vị tính
    /// </summary>
    public string Unit { get; set; }

    /// <summary>
    /// Mô tả
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Giá chuẩn
    /// </summary>
    public decimal StandardPrice { get; set; }

    /// <summary>
    /// Có cần toa thuốc không
    /// </summary>
    public bool RequiresPrescription { get; set; }

    /// <summary>
    /// Trạng thái sản phẩm
    /// </summary>
    public ProductStatus Status { get; set; } = ProductStatus.ACTIVE;
}