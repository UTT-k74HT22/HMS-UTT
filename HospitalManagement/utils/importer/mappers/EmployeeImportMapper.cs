using HospitalManagement.utils.importer.core;
using HospitalManagement.utils.importer.dto;
using OfficeOpenXml;

namespace HospitalManagement.utils.importer.mappers
{
    /// <summary>
    /// Mapper để map dữ liệu từ Excel row sang EmployeeImportDto
    /// </summary>
    public class EmployeeImportMapper : IImportMapper<EmployeeImportDto>
    {
        public string[] RequiredHeaders => new[]
        {
            "Profile ID",
            "Chức vụ",
            "Phòng ban",
            "Ngày vào làm",
            "Lương cơ bản"
        };

        public EmployeeImportDto MapRow(ExcelRange row, int rowIndex)
        {
            return new EmployeeImportDto
            {
                ProfileId = ParseInt(GetCellValue(row, 1)) ?? 0,
                Position = GetCellValue(row, 2),
                Department = GetCellValue(row, 3),
                HiredDate = ParseDateTime(GetCellValue(row, 4)),
                BaseSalary = ParseDecimal(GetCellValue(row, 5)) ?? 0
            };
        }

        // ===== Helper Methods =====

        private string GetCellValue(ExcelRange row, int colIndex)
        {
            var cell = row.Worksheet.Cells[row.Start.Row, colIndex];
            return cell.Value?.ToString()?.Trim() ?? string.Empty;
        }

        private int? ParseInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return int.TryParse(value, out int result) ? result : null;
        }

        private decimal? ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            
            // Remove currency symbols and thousands separators
            value = value.Replace("₫", "").Replace(",", "").Replace(".", "").Trim();
            
            return decimal.TryParse(value, out decimal result) ? result : null;
        }

        private DateTime? ParseDateTime(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return DateTime.TryParse(value, out DateTime result) ? result : null;
        }
    }
}
