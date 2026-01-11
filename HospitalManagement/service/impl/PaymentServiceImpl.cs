using HospitalManagement.configuration;
using HospitalManagement.entity;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;

namespace HospitalManagement.service.impl;

public class PaymentServiceImpl : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly InvoiceRepositoryImpl _invoiceRepo;

    public PaymentServiceImpl(DBConfig dbConfig)
    {
        _repository = new PaymentRepositoryImpl(dbConfig);
        _invoiceRepo = new InvoiceRepositoryImpl(dbConfig);
    }

    public List<Payment> GetAll()
    {
        return _repository.FindAll();
    }

    public Payment GetById(int id)
    {
        return _repository.FindById(id);
    }

    public int CreateByInvoice(int invoiceId, string paymentNumber, string method)
    {
        // 1. Tạo payment
        int rows = _repository.InsertByInvoiceId(invoiceId, paymentNumber, method);

        if (rows > 0)
        {
            // 2. Lấy invoice
            var invoice = _invoiceRepo.FindById(invoiceId);
            if (invoice != null)
            {
                // 3. Vì payment amount = total_amount nên set luôn PaidAmount = TotalAmount và status = PAID
                invoice.PaidAmount = invoice.TotalAmount;
                invoice.Status = "PAID";

                // 4. Cập nhật invoice
                _invoiceRepo.Update(invoice);
            }
        }

        return rows;
    }

    public void UpdatePayment(Payment payment)
    {
        _repository.Update(payment);
    }

    public void CancelPayment(int id)
    {
        // 1. Lấy payment trước khi hủy
        var payment = _repository.FindById(id);
        if (payment == null || payment.Status != "SUCCESS")
            return;

        // 2. Hủy payment
        _repository.Delete(id); // set status = CANCELED

        // 3. Lấy invoice liên quan
        var invoice = _invoiceRepo.FindById(payment.InvoiceId);
        if (invoice != null)
        {
            // 4. Tính lại PaidAmount = tổng các payment SUCCESS còn lại
            var allPayments = _repository.FindByInvoiceId((int)invoice.Id);
            decimal totalPaid = allPayments
                .Where(p => p.Status == "SUCCESS")
                .Sum(p => p.Amount);

            invoice.PaidAmount = totalPaid;

            // 5. Cập nhật status
            if (totalPaid == 0)
                invoice.Status = "NEW";
            else if (totalPaid < invoice.TotalAmount)
                invoice.Status = "PARTIAL";
            else
                invoice.Status = "PAID";

            // 6. Cập nhật invoice
            _invoiceRepo.Update(invoice);
        }
    }


    public bool ExistsPaymentNumber(string paymentNumber)
    {
        return _repository.ExistsByPaymentNumber(paymentNumber);
    }
    
    public List<int> GetAvailableInvoiceIds()
    {
        return _repository.FindAvailableInvoiceIds();
    }

}