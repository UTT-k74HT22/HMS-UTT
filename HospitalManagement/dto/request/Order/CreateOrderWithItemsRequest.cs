namespace HospitalManagement.dto.request.Order;

public class CreateOrderWithItemsRequest
{
    /// <summary>
    /// ID khách hàng
    /// </summary>
    public long? CustomerId { get; set; }

    /// <summary>
    /// Địa chỉ giao hàng
    /// </summary>
    public string ShippingAddress { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// Giảm giá
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Danh sách sản phẩm
    /// </summary>
    public List<OrderItemRequest> Items { get; set; }
}