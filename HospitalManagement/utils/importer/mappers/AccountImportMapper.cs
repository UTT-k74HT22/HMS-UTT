using HospitalManagement.utils.importer.core;
using HospitalManagement.utils.importer.dto;
using OfficeOpenXml;

namespace HospitalManagement.utils.importer.mappers
{
    /// <summary>
    /// Mapper để map dữ liệu từ Excel row sang AccountImportDto
    /// </summary>
    public class AccountImportMapper : IImportMapper<AccountImportDto>
    {
        public string[] RequiredHeaders => new[]
        {
            "Username",
            "Password",
            "Role",
            "Is Active"
        };

        public AccountImportDto MapRow(ExcelRange row, int rowIndex)
        {
            return new AccountImportDto
            {
                Username = GetCellValue(row, 1),
                Password = GetCellValue(row, 2),
                Role = GetCellValue(row, 3),
                IsActive = ParseBool(GetCellValue(row, 4))
            };
        }

        // ===== Helper Methods =====

        private string GetCellValue(ExcelRange row, int colIndex)
        {
            var cell = row.Worksheet.Cells[row.Start.Row, colIndex];
            return cell.Value?.ToString()?.Trim() ?? string.Empty;
        }

        private bool ParseBool(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.ToLower().Trim();
            return value == "yes" || value == "true" || value == "1" || 
                   value == "có" || value == "active" || value == "hoạt động";
        }
    }
}
