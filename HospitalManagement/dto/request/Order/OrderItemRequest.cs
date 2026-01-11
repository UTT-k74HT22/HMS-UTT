namespace HospitalManagement.dto.request.Order;

public class OrderItemRequest
{
    /// <summary>
    /// ID sản phẩm
    /// </summary>
    public long? ProductId { get; set; }

    /// <summary>
    /// ID lô hàng
    /// </summary>
    public long? BatchId { get; set; }

    /// <summary>
    /// Số lượng
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Giá
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// ID kho
    /// </summary>
    public long? WarehouseId { get; set; }
}