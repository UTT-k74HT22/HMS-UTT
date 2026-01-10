using HospitalManagement.entity;
using HospitalManagement.dto.response;

namespace HospitalManagement.repository;

public interface IPaymentRepository
{
    // Lấy tất cả payment
    List<Payment> FindAll();

    // Lấy payment theo ID
    Payment FindById(int id);

    // Thêm payment dựa vào invoice
    int InsertByInvoiceId(int invoiceId, string paymentNumber, string method);

    // Cập nhật payment
    void Update(Payment payment);

    // Hủy payment
    void Delete(int id);

    // Kiểm tra tồn tại theo payment number
    bool ExistsByPaymentNumber(string paymentNumber);
}