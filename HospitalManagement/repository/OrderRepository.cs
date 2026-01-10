using HospitalManagement.dto.request.Order;
using HospitalManagement.dto.response.Order;

namespace HospitalManagement.repository;
    public interface IOrderRepository
    {
        long InsertOrder(
            long? customerId,
            string shippingAddress,
            string note,
            decimal discount,
            long employeeId
        );

        void InsertItem(long orderId, OrderItemRequest item);

        void UpdateOrderTotal(long orderId);

        void UpdateStatus(long orderId, string status);

        void CancelOrder(long orderId);

        List<OrderResponse> FindAll();

        List<OrderItemResponse> GetItems(long orderId);

        OrderResponse FindById(long orderId);
}