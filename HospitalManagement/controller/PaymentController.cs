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

        // ================= GET ALL =================
        public List<Payment> GetAll()
        {
            return _service.GetAll();
        }

        // ================= GET BY ID =================
        public Payment GetById(int id)
        {
            return _service.GetById(id);
        }

        // ================= CREATE PAYMENT BY INVOICE =================
        public int CreatePaymentByInvoice(int invoiceId, string paymentNumber, string method)
        {
            if (_service.ExistsPaymentNumber(paymentNumber))
            {
                throw new Exception("Payment number đã tồn tại!");
            }

            return _service.CreateByInvoice(invoiceId, paymentNumber, method);
        }

        // ================= UPDATE PAYMENT =================
        public void Update(Payment payment)
        {
            _service.UpdatePayment(payment);
        }

        // ================= CANCEL PAYMENT =================
        public void Cancel(int id)
        {
            _service.CancelPayment(id);
        }

        // ================= CHECK EXISTS =================
        public bool ExistsPaymentNumber(string paymentNumber)
        {
            return _service.ExistsPaymentNumber(paymentNumber);
        }

        // ================= SEARCH BY PAYMENT NUMBER =================
        public List<Payment> SearchByPaymentNumber(string paymentNumber)
        {
            var all = _service.GetAll();
            return all.FindAll(p => p.PaymentNumber.Contains(paymentNumber, StringComparison.OrdinalIgnoreCase));
        }
    }
}