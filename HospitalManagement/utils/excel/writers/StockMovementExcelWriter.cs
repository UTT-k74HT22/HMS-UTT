using HospitalManagement.entity;
using HospitalManagement.utils.excel.core;
using OfficeOpenXml;

namespace HospitalManagement.utils.excel.writers
{
    /// <summary>
    /// Excel writer cho StockMovement
    /// Export lịch sử nhập/xuất kho ra file Excel
    /// TODO: Implement based on StockMovement entity
    /// </summary>
    public class StockMovementExcelWriter : AbstractExcelWriter<StockMovement>
    {
        public override string SheetName => "StockMovements";
        public override string Title => "LỊCH SỬ NHẬP XUẤT KHO";
        public override string[] Headers => new[]
        {
            "STT",
            "Movement ID",
            "Product ID",
            "Warehouse ID",
            "Movement Type",
            "Quantity",
            "Batch Number",
            "Movement Date",
            "Created By"
        };

        public override void Create(ExcelWorksheet worksheet, ExcelStyles styles, List<StockMovement> data)
        {
            int currentRow = 1;

            // ===== Title Row =====
            worksheet.Row(currentRow).Height = 25;
            worksheet.Cells[currentRow, 1, currentRow, Headers.Length].Merge = true;
            SetCell(worksheet, currentRow, 1, Title, styles.Get(StyleKey.TITLE));
            currentRow++;

            // ===== Header Row =====
            worksheet.Row(currentRow).Height = 18;
            for (int i = 0; i < Headers.Length; i++)
            {
                SetCell(worksheet, currentRow, i + 1, Headers[i], styles.Get(StyleKey.HEADER));
            }
            currentRow++;

            // ===== Data Rows =====
            int stt = 1;
            foreach (var movement in data)
            {
                worksheet.Row(currentRow).Height = 16;

                SetCell(worksheet, currentRow, 1, stt++, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 2, movement.Id, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 3, movement.ProductId, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 4, movement.WarehouseId, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 5, movement.MovementType, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 6, movement.Quantity, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 7, Safe(movement.BatchNumber), styles.Get(StyleKey.DATA));
                SetCell(worksheet, currentRow, 8, FormatDateTime(movement.MovementDate), styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 9, movement.CreatedBy, styles.Get(StyleKey.DATA_CENTER));

                currentRow++;
            }
        }
    }
}
