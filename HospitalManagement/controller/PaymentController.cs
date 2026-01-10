using System;
using System.Collections.Generic;
using HospitalManagement.entity;
using HospitalManagement.service;

namespace HospitalManagement.Controller
{
    public class PaymentController
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        public List<Payment> GetAll()
        {
            return _service.GetAll();
        }

        public Payment GetById(int id)
        {
            return _service.GetById(id);
        }

        public int CreatePaymentByInvoice(int invoiceId, string paymentNumber, string method)
        {
            if (_service.ExistsPaymentNumber(paymentNumber))
            {
                throw new Exception("Payment number đã tồn tại!");
            }

            return _service.CreateByInvoice(invoiceId, paymentNumber, method);
        }

        public void Update(Payment payment)
        {
            _service.UpdatePayment(payment);
        }

        public void Cancel(int id)
        {
            _service.CancelPayment(id);
        }

        public bool ExistsPaymentNumber(string paymentNumber)
        {
            return _service.ExistsPaymentNumber(paymentNumber);
        }
        
        public List<Payment> SearchByPaymentNumber(string paymentNumber)
        {
            var all = _service.GetAll();
            return all.FindAll(p => p.PaymentNumber.Contains(paymentNumber, StringComparison.OrdinalIgnoreCase));
        }
        
        public List<int> GetAvailableInvoiceIds()
        {
            return _service.GetAvailableInvoiceIds();
        }

    }
}