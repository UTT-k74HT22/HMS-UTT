using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace HospitalManagement.utils.importer.template
{
    /// <summary>
    /// Tạo file Excel template cho Product import
    /// </summary>
    public class ProductTemplateGenerator
    {
        public static void GenerateTemplate(string outputPath)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");

            // Create header row
            string[] headers = {
                "Code", "Name", "Category Code", "Manufacturer Code",
                "Barcode", "Dosage Form", "Unit", "Description",
                "Standard Price", "Requires Prescription"
            };

            // Set headers with styling
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(Color.White);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 0, 139)); // Dark Blue
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                
                // Set column width
                worksheet.Column(i + 1).Width = 20;
            }

            // Add example rows
            AddExampleRow(worksheet, 2, new object[]
            {
                "PRD001", "Paracetamol 500mg", "CAT001", "MFG001",
                "8934567890123", "Tablet", "Box", "Pain relief medication",
                15000, "No"
            });

            AddExampleRow(worksheet, 3, new object[]
            {
                "PRD002", "Amoxicillin 250mg", "CAT002", "MFG002",
                "8934567890456", "Capsule", "Box", "Antibiotic medication",
                45000, "Yes"
            });

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Save to file
            var fileInfo = new FileInfo(outputPath);
            package.SaveAs(fileInfo);
        }

        private static void AddExampleRow(ExcelWorksheet worksheet, int rowIndex, object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                worksheet.Cells[rowIndex, i + 1].Value = values[i];
            }
        }
    }
}
