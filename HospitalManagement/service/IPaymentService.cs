using HospitalManagement.entity;

namespace HospitalManagement.service;

public interface IPaymentService
{
    List<Payment> GetAll();
    Payment GetById(int id);
    int CreateByInvoice(int invoiceId, string paymentNumber, string method);
    void UpdatePayment(Payment payment);
    void CancelPayment(int id);
    bool ExistsPaymentNumber(string paymentNumber);
}