using System.Collections.Generic;
using HospitalManagement.entity;

namespace HospitalManagement.service
{
    public interface IInvoiceService
    {
        List<Invoice> GetAll();
        Invoice GetById(long id);
        List<int> GetAvailableOrderIds();
        long CreateInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        void UpdatePaidAmount(long invoiceId, decimal newPaidAmount);
        void CancelInvoice(long invoiceId);
        void OnPaymentDeleted(long invoiceId);
    }
}