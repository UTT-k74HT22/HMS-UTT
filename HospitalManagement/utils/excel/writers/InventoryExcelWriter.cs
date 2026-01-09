using HospitalManagement.entity;
using HospitalManagement.utils.excel.core;
using OfficeOpenXml;

namespace HospitalManagement.utils.excel.writers
{
    /// <summary>
    /// Excel writer cho Inventory/Product
    /// Export danh sách hàng tồn kho ra file Excel
    /// TODO: Implement based on InventoryItem entity
    /// </summary>
    public class InventoryExcelWriter : AbstractExcelWriter<InventoryItem>
    {
        public override string SheetName => "Inventory";
        public override string Title => "DANH SÁCH TỒN KHO";
        public override string[] Headers => new[]
        {
            "STT",
            "Product ID",
            "Warehouse ID",
            "Batch Number",
            "Quantity",
            "Reserved Quantity",
            "Available Quantity",
            "Last Updated"
        };

        public override void Create(ExcelWorksheet worksheet, ExcelStyles styles, List<InventoryItem> data)
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
            foreach (var item in data)
            {
                worksheet.Row(currentRow).Height = 16;

                SetCell(worksheet, currentRow, 1, stt++, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 2, item.ProductId, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 3, item.WarehouseId, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 4, Safe(item.BatchNumber), styles.Get(StyleKey.DATA));
                SetCell(worksheet, currentRow, 5, item.Quantity, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 6, item.ReservedQuantity, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 7, item.AvailableQuantity, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 8, FormatDateTime(item.UpdatedAt), styles.Get(StyleKey.DATA_CENTER));

                currentRow++;
            }
        }
    }
}
