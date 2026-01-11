using HospitalManagement.dto.request.Order;
using HospitalManagement.dto.response.Order;

namespace HospitalManagement.service;

public interface IOrderService
{
    long CreateOrder(CreateOrderWithItemsRequest req, long employeeId);

    void ConfirmOrder(long orderId);

    void CancelOrder(long orderId);

    List<OrderResponse> GetAll();

    OrderResponse GetById(long orderId);

    List<OrderItemResponse> GetItems(long orderId);
}