using System;
using System.Collections.Generic;
using HospitalManagement.entity;
using HospitalManagement.service;

namespace HospitalManagement.Controller
{
    public class InvoiceController
    {
        private readonly IInvoiceService _service;

        public InvoiceController(IInvoiceService service)
        {
            _service = service;
        }

        // =================== GET ===================
        public List<Invoice> GetAll()
        {
            return _service.GetAll();
        }

        public Invoice GetById(long invoiceId)
        {
            return _service.GetById(invoiceId);
        }

        public List<int> GetAvailableOrderIds()
        {
            return _service.GetAvailableOrderIds();
        }

        // =================== CREATE ===================
        public long CreateInvoice(int orderId, string invoiceNumber)
        {
            var invoice = new Invoice
            {
                OrderId = orderId,
                InvoiceNumber = invoiceNumber,
                IssueDate = DateTime.Now,
                PaidAmount = 0,
                Status = "NEW"
            };

            // Gọi đúng phương thức trong service
            return _service.CreateInvoice(invoice);
        }

        // =================== UPDATE ===================
        public void UpdateInvoice(long invoiceId, DateTime? dueDate, decimal paidAmount, string status)
        {
            // Gọi service.UpdateInvoice đúng chuẩn
            _service.UpdateInvoice(new Invoice
            {
                Id = invoiceId,
                DueDate = dueDate,
                PaidAmount = paidAmount,
                Status = status
            });
        }

        public void UpdatePaidAmount(long invoiceId, decimal newPaidAmount)
        {
            _service.UpdatePaidAmount(invoiceId, newPaidAmount);
        }

        // =================== CANCEL ===================
        public void CancelInvoice(long invoiceId)
        {
            _service.CancelInvoice(invoiceId);
        }

        // =================== PAYMENT DELETED ===================
        public void OnPaymentDeleted(long invoiceId)
        {
            _service.OnPaymentDeleted(invoiceId);
        }
    }
}
