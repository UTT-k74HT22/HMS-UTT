using HospitalManagement.utils.importer.core;
using OfficeOpenXml;

namespace HospitalManagement.utils.importer.mapper
{
    /// <summary>
    /// Strategy implementation: Map từ Excel row sang StockMovementImportDto
    /// </summary>
    public class StockMovementImportMapper : IImportMapper<dto.StockMovementImportDto>
    {
        /// <summary>
        /// Định nghĩa headers cần có trong Excel (theo thứ tự từ trái sang phải)
        /// </summary>
        public string[] RequiredHeaders => new string[]
        {
            "Loại",
            "Kho hàng",
            "Mã sản phẩm",
            "Mã lô",
            "Số lượng",
            "Ghi chú"
        };

        /// <summary>
        /// Map 1 row Excel thành StockMovementImportDto object
        /// </summary>
        public dto.StockMovementImportDto MapRow(ExcelRange row, int rowIndex)
        {
            return new dto.StockMovementImportDto
            {
                MovementType = GetCellValue(row, 1),
                WarehouseCode = GetCellValue(row, 2),
                ProductCode = GetCellValue(row, 3),
                BatchCode = GetCellValue(row, 4),
                Quantity = int.TryParse(GetCellValue(row, 5), out int qty) ? qty : 0,
                Note = GetCellValue(row, 6)
            };
        }

        private string GetCellValue(ExcelRange row, int column)
        {
            var cell = row[1, column];
            return cell.Text?.Trim() ?? string.Empty;
        }
    }
}
