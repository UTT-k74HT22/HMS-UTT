using System;
using System.Collections.Generic;
using HospitalManagement.configuration;
using HospitalManagement.entity;
using HospitalManagement.repository;
using HospitalManagement.repository.impl;

namespace HospitalManagement.service.impl
{
    public class InvoiceServiceImpl : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;

        public InvoiceServiceImpl(DBConfig dbConfig)
        {
            _repository = new InvoiceRepositoryImpl(dbConfig);
        }

        // =================== GET ===================
        public List<Invoice> GetAll()
        {
            return _repository.FindAll();
        }

        public Invoice GetById(long id)
        {
            return _repository.FindById(id);
        }

        public List<int> GetAvailableOrderIds()
        {
            return _repository.FindAvailableOrderIds();
        }

        // =================== CREATE ===================
        public long CreateInvoice(Invoice invoice)
        {
            _repository.InsertByOrderId(invoice.OrderId, invoice.InvoiceNumber);
            // Lấy ID vừa insert → Sql Server ko hỗ trợ RETURNING trực tiếp, cần Query lại
            var list = _repository.FindAll();
            return list.Count > 0 ? list[0].Id : 0; // trả ID mới nhất
        }

        // =================== UPDATE ===================
        public void UpdateInvoice(Invoice invoice)
        {
            var existing = _repository.FindById(invoice.Id);
            if (existing == null) return;

            // Cập nhật ngày đến hạn nếu có
            existing.DueDate = invoice.DueDate;

            // Cập nhật theo status
            switch (invoice.Status)
            {
                case "PAID":
                    existing.PaidAmount = existing.TotalAmount;  // PaidAmount = TotalAmount cũ
                    existing.TotalAmount = 0;                   // TotalAmount = 0
                    break;
                case "PARTIAL":
                    // ví dụ: giữ nguyên PaidAmount, TotalAmount giảm đi PaidAmount
                    existing.PaidAmount = invoice.PaidAmount;
                    existing.TotalAmount = existing.TotalAmount - invoice.PaidAmount;
                    break;
                case "NEW":
                case "CANCELED":
                    existing.PaidAmount = 0;
                    // TotalAmount giữ nguyên
                    break;
                default:
                    existing.PaidAmount = invoice.PaidAmount; // giữ nguyên nếu status lạ
                    break;
            }

            existing.Status = invoice.Status;

            _repository.Update(existing);
        }


        public void UpdatePaidAmount(long invoiceId, decimal newPaidAmount)
        {
            var invoice = _repository.FindById(invoiceId);
            if (invoice == null) return;

            invoice.PaidAmount = newPaidAmount;

            // Cập nhật trạng thái tự động
            if (invoice.PaidAmount >= invoice.TotalAmount)
                invoice.Status = "PAID";
            else if (invoice.PaidAmount > 0)
                invoice.Status = "PARTIAL";
            else
                invoice.Status = "NEW";

            _repository.Update(invoice);
        }

        // =================== CANCEL ===================
        public void CancelInvoice(long invoiceId)
        {
            _repository.Cancel(invoiceId);
        }

        // =================== PAYMENT DELETED ===================
        public void OnPaymentDeleted(long invoiceId)
        {
            var invoice = _repository.FindById(invoiceId);
            if (invoice == null) return;

            // Reset lại PaidAmount
            invoice.PaidAmount = 0;
            invoice.Status = "NEW";

            _repository.Update(invoice);
        }
    }
}
