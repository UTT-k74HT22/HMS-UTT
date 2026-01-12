using ClosedXML.Excel;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.utils.excel.core;
using OfficeOpenXml;

namespace HospitalManagement.utils.excel.writers
{
    /// <summary>
    /// Excel writer cho StockMovement
    /// Export lịch sử nhập/xuất kho ra file Excel
    /// </summary>
    public class StockMovementExcelWriter : AbstractExcelWriter<StockMovementResponse>
    {
        public override string SheetName { get; } = "Lịch sử xuất nhập kho";
        public override string Title { get; } = "LỊCH SỬ XUẤT NHẬP KHO";

        public override string[] Headers { get; } = new string[]
        {
            "STT",
            "ID Giao dịch",
            "Loại giao dịch",
            "Ngày giao dịch",
            "ID Sản phẩm",
            "Mã sản phẩm",
            "Tên sản phẩm",
            "Đơn vị tính",
            "ID Lô hàng",
            "Mã Lô hàng",
            "ID Kho",
            "Mã Kho",
            "Tên Kho",
            "Số lượng",
            "Số lượng trước",
            "Số lượng sau",
            "Loại tham chiếu",
            "ID Tham chiếu",
            "ID Người thực hiện",
            "Tên đăng nhập Người thực hiện",
            "Họ tên Người thực hiện",
            "Ghi chú"
        };
        
        public override void Create(IXLWorksheet worksheet, List<StockMovementResponse> data)
        {
            Console.WriteLine("[StockMovementExcelWriter] Creating sheet for {data.Count} stock movements");
            var titleRange = worksheet.Range("A1:V1");
            titleRange.Merge();
            SetCell(worksheet, 1, 1, Title, ApplyTitleStyle);
            
            // Header row
            for (int i = 0; i < Headers.Length; i++)
            {
                SetCell(worksheet, 2, i + 1, Headers[i], ApplyHeaderStyle);
            }
            
            // Data rows
            int row = 3;
            int stt = 1;
            foreach (var item in data)
            {
                SetCell(worksheet, row, 1, stt, cell => ApplyDataStyle(cell, true));
                SetCell(worksheet, row, 2, item.Id, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 3, item.MovementType.ToString(), cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 4, item.MovementDate?.ToString("yyyy-MM-dd HH:mm:ss"), cell => ApplyDataStyle(cell));   
                SetCell(worksheet, row, 5, item.ProductId, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 6, item.ProductCode, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 7, item.ProductName, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 8, item.Unit, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 9, item.BatchId, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 10, item.BatchCode, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 11, item.WarehouseId, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 12, item.WarehouseCode, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 13, item.WarehouseName, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 14, item.Quantity, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 15, item.QuantityBefore, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 16, item.QuantityAfter, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 17, item.ReferenceType, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 18, item.ReferenceId, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 19, item.PerformedByUserId, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 20, item.PerformedByUsername, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 21, item.PerformedByFullName, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 22, item.Note, cell => ApplyDataStyle(cell));
                row++;
                stt++;
            }
        }
    }
}
