using System.Collections.Generic;
using HospitalManagement.entity;

namespace HospitalManagement.repository
{
    public interface IInvoiceRepository
    {
        List<Invoice> FindAll();
        Invoice FindById(long id);
        int InsertByOrderId(int orderId, string invoiceNumber);
        void Update(Invoice i);
        void Cancel(long id);
        List<int> FindAvailableOrderIds();
    }
}