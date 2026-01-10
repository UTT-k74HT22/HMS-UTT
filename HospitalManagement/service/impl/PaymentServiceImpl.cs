using HospitalManagement.configuration;
using HospitalManagement.entity;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;

namespace HospitalManagement.service.impl;

public class PaymentServiceImpl : IPaymentService
{
    private readonly IPaymentRepository _repository;

    public PaymentServiceImpl(DBConfig dbConfig)
    {
        _repository = new PaymentRepositoryImpl(dbConfig);
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
        return _repository.InsertByInvoiceId(invoiceId, paymentNumber, method);
    }

    public void UpdatePayment(Payment payment)
    {
        _repository.Update(payment);
    }

    public void CancelPayment(int id)
    {
        _repository.Delete(id);
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