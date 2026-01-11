using HospitalManagement.dto.request;
using HospitalManagement.utils.importer.core;
using OfficeOpenXml;

namespace HospitalManagement.utils.importer.mappers
{
    /// <summary>
    /// Mapper để chuyển đổi từ Excel row sang ProductImportDto
    /// Strategy Pattern - implement cách map cụ thể cho Product
    /// </summary>
    public class ProductImportMapper : IImportMapper<ProductImportDto>
    {
        public string[] RequiredHeaders => new[]
        {
            "Code", "Name", "Category Code", "Manufacturer Code",
            "Barcode", "Dosage Form", "Unit", "Description",
            "Standard Price", "Requires Prescription"
        };

        public ProductImportDto MapRow(ExcelRange row, int rowIndex)
        {
            var dto = new ProductImportDto
            {
                Code = GetCellValue(row, 1),
                Name = GetCellValue(row, 2),
                CategoryCode = GetCellValue(row, 3),
                ManufacturerCode = GetCellValue(row, 4),
                Barcode = GetCellValue(row, 5),
                DosageForm = GetCellValue(row, 6),
                Unit = GetCellValue(row, 7),
                Description = GetCellValue(row, 8)
            };

            // Parse price
            string priceStr = GetCellValue(row, 9);
            if (!string.IsNullOrEmpty(priceStr))
            {
                if (decimal.TryParse(priceStr, out decimal price))
                {
                    dto.StandardPrice = price;
                }
                else
                {
                    dto.StandardPrice = 0;
                }
            }

            // Parse boolean
            string requiresPrescriptionStr = GetCellValue(row, 10);
            dto.RequiresPrescription =
                requiresPrescriptionStr.Equals("Yes", StringComparison.OrdinalIgnoreCase) ||
                requiresPrescriptionStr.Equals("True", StringComparison.OrdinalIgnoreCase) ||
                requiresPrescriptionStr == "1";

            return dto;
        }

        /// <summary>
        /// Helper method to get cell value
        /// </summary>
        private string GetCellValue(ExcelRange row, int colIndex)
        {
            var cell = row.Worksheet.Cells[row.Start.Row, colIndex];
            if (cell.Value == null) return string.Empty;

            // Xử lý các kiểu dữ liệu
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
