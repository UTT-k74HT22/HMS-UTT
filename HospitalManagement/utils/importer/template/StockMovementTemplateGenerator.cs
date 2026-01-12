using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace HospitalManagement.utils.importer.template
{
    /// <summary>
    /// Tạo file Excel template cho Stock Movement import (Xuất/Nhập/Điều chỉnh kho)
    /// </summary>
    public class StockMovementTemplateGenerator
    {
        public byte[] Generate()
        {
            // Set EPPlus license context
            OfficeOpenXml.ExcelPackage.License.SetNonCommercialOrganization("HospitalManagement");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Dữ liệu");

                // ===== Header Row (Row 1) =====
                var headerRow = 1;
                var headers = new[] { "Loại", "Kho hàng", "Mã sản phẩm", "Mã lô", "Số lượng", "Ghi chú" };

                for (int col = 1; col <= headers.Length; col++)
                {
                    var cell = worksheet.Cells[headerRow, col];
                    cell.Value = headers[col - 1];

                    // Header style: Bold, Blue background, White text
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Color.SetColor(Color.White);
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192)); // Dark Blue
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // ===== Example Rows (Row 2, 3, 4) =====
                var exampleData = new object[,]
                {
                    { "IMPORT", "1", "PRD001", "BATCH001", 100, "Nhập hàng từ NCC ABC" },
                    { "EXPORT", "1", "PRD002", "BATCH002", 50, "Xuất bán" },
                    { "ADJUST", "2", "PRD003", "BATCH003", 95, "Kiểm kê" }
                };

                for (int row = 0; row < exampleData.GetLength(0); row++)
                {
                    for (int col = 0; col < exampleData.GetLength(1); col++)
                    {
                        var cell = worksheet.Cells[row + 2, col + 1];
                        cell.Value = exampleData[row, col];

                        // Text style with border
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        // Number column (Số lượng) - right align
                        if (col == 4)
                        {
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0";
                        }
                        else
                        {
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }

                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.WrapText = true;
                    }
                }

                // ===== Column Widths =====
                worksheet.Column(1).Width = 15;  // Loại
                worksheet.Column(2).Width = 15;  // Kho hàng
                worksheet.Column(3).Width = 18;  // Mã sản phẩm
                worksheet.Column(4).Width = 15;  // Mã lô
                worksheet.Column(5).Width = 12;  // Số lượng
                worksheet.Column(6).Width = 30;  // Ghi chú

                // ===== Data Validation for "Loại" column (A5:A1000) =====
                var validationRange = worksheet.DataValidations.AddListValidation("A5:A1000");
                validationRange.Formula.Values.Add("IMPORT");
                validationRange.Formula.Values.Add("EXPORT");
                validationRange.Formula.Values.Add("ADJUST");
                validationRange.ShowErrorMessage = true;
                validationRange.ErrorTitle = "Giá trị không hợp lệ";
                validationRange.Error = "Loại chỉ được chọn: IMPORT, EXPORT, ADJUST.";

                // ===== Freeze Header Row =====
                worksheet.View.FreezePanes(2, 1);

                // ===== Return Excel file as byte array =====
                return package.GetAsByteArray();
            }
        }
    }
}
