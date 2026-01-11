namespace HospitalManagement.dto.response.Order;

public class OrderResponse
{
    /// <summary>
    /// ID đơn hàng
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// Số đơn hàng
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Tổng tiền
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Ngày tạo
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// Thông tin khách hàng
    /// </summary>
    public long? CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public string CustomerEmail { get; set; }

    /// <summary>
    /// Địa chỉ giao hàng
    /// </summary>
    public string ShippingAddress { get; set; }

    /// <summary>
    /// Thông tin người tạo
    /// </summary>
    public string CreatorName { get; set; }
    public string CreatorEmail { get; set; }
    public string CreatorPhone { get; set; }
}