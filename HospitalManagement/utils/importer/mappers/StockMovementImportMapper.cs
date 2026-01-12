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
            var cell = row.Worksheet.Cells[row.Start.Row, column];
            if (cell.Value == null) return String.Empty;
            
            //Xử lí các kiểu dữ liệu khác nhau
            if (cell.Value is string strValue)
            {
                return strValue.Trim();
            }
            else if (cell.Value is double || cell.Value is decimal ||
                     cell.Value is int || cell.Value is long)
            {
                // Xử lý số
                var numericValue = Convert.ToDouble(cell.Value);
                // Nếu là số nguyên
                if (numericValue == (long)numericValue)
                {
                    return ((long)numericValue).ToString();
                }
                return numericValue.ToString();
            }
            else if (cell.Value is DateTime dateValue)
            {
                return dateValue.ToString();
            }
            else if (cell.Value is bool boolValue)
            {
                return boolValue.ToString();
            }

            return cell.Value.ToString()?.Trim() ?? string.Empty;
        }
    }
}
