using HospitalManagement.dto.request.Order;
using HospitalManagement.dto.response.Order;
using HospitalManagement.service;
using HospitalManagement.service.impl;

namespace HospitalManagement.controller;

public class OrderController
{
    private readonly IOrderService _service;

    public OrderController(string connectionString)
    {
        _service = new OrderServiceImpl(connectionString);
    }

    public OrderController(IOrderService service)
    {
        _service = service;
    }
    
    // Tạo đơn hàng
    public long CreateOrder(CreateOrderWithItemsRequest req, long employeeId)
        => _service.CreateOrder(req, employeeId);

    // Xác nhận đơn hàng
    public void Confirm(long orderId)
        => _service.ConfirmOrder(orderId);

    // Lấy danh sách đơn hàng
    public List<OrderResponse> GetAll()
        => _service.GetAll();

    // Lấy đơn hàng theo id
    public OrderResponse GetById(long orderId)
        => _service.GetById(orderId);

    // Hủy đơn hàng
    public void Cancel(long orderId)
        => _service.CancelOrder(orderId);

    // Lấy danh sách thuốc trong đơn
    public List<OrderItemResponse> GetItems(long orderId)
        => _service.GetItems(orderId);
}