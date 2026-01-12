using HospitalManagement.dto.request.Order;
using HospitalManagement.dto.response.Order;
using HospitalManagement.entity.enums;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;

namespace HospitalManagement.service.impl;

public class OrderServiceImpl : IOrderService
{
    private readonly IOrderRepository _repo;

    public OrderServiceImpl(string connectionString)
    {
        _repo = new OrderRepositoryImpl(connectionString);
    }

    // ===== Tạo đơn hàng =====
    public long CreateOrder(CreateOrderWithItemsRequest req, long employeeId)
    {
        var discount = req.Discount;

        var orderId = _repo.InsertOrder(
            
            req.CustomerId,
            req.ShippingAddress,
            req.Note,
            discount,
            employeeId
        );

        var items = req.Items;
        if (items != null)
        {
            foreach (var item in items)
            {
                _repo.InsertItem(orderId, item);
            }
        }

        _repo.UpdateOrderTotal(orderId);

        return orderId;
    }

    // ===== Xác nhận đơn hàng =====
    public void ConfirmOrder(long orderId)
    {
        _repo.UpdateStatus(orderId, OrderStatus.CONFIRMED.ToString());
    }

    // ===== Hủy đơn hàng =====
    public void CancelOrder(long orderId)
    {
        _repo.CancelOrder(orderId);
    }

    // ===== Lấy tất cả đơn =====
    public List<OrderResponse> GetAll()
    {
        return _repo.FindAll();
    }

    // ===== Lấy đơn theo ID =====
    public OrderResponse GetById(long orderId)
    {
        return _repo.FindById(orderId);
    }

    // ===== Lấy danh sách sản phẩm =====
    public List<OrderItemResponse> GetItems(long orderId)
    {
        return _repo.GetItems(orderId);
    }
}
//string orderNumber = "ORD-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
