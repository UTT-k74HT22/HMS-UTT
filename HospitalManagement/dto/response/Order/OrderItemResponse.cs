namespace HospitalManagement.dto.response.Order;

public class OrderItemResponse
{
    public long? Id { get; set; }
    public long? OrderId { get; set; }

    public long? ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }

    public long? BatchId { get; set; }
    public string BatchCode { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal LineTotal { get; set; }

    public string Note { get; set; }
    public string ShippingAddress { get; set; }

    public long? WarehouseId { get; set; }
    public string WarehouseName { get; set; }
}