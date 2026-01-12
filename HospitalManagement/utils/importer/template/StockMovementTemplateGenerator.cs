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
                    { "IMPORT", "Kho chính", "PRD001", "BATCH001", 100, "Nhập hàng từ NCC ABC" },
                    { "EXPORT", "Kho chính", "PRD002", "BATCH002", 50, "Xuất bán" },
                    { "ADJUST", "Kho phụ", "PRD003", "BATCH003", 95, "Kiểm kê điều chỉnh" }
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

                // ===== Add Instruction Sheet =====
                CreateInstructionSheet(package);

                // ===== Return Excel file as byte array =====
                return package.GetAsByteArray();
            }
        }

        private void CreateInstructionSheet(ExcelPackage package)
        {
            var sheet = package.Workbook.Worksheets.Add("Hướng dẫn");

            // Title
            var titleCell = sheet.Cells["A1"];
            titleCell.Value = "HƯỚNG DẪN IMPORT GIAO DỊCH KHO";
            titleCell.Style.Font.Size = 16;
            titleCell.Style.Font.Bold = true;
            titleCell.Style.Font.Color.SetColor(Color.DarkBlue);
            sheet.Cells["A1:E1"].Merge = true;

            int row = 3;

            // Instructions
            var instructions = new[]
            {
                ("📋 CÁC CỘT BẮT BUỘC", ""),
                ("Loại", "IMPORT (Nhập kho) | EXPORT (Xuất kho) | ADJUST (Điều chỉnh)"),
                ("Kho hàng", "Nhập TÊN KHO hoặc MÃ KHO (vd: 'Kho chính', 'WH001')"),
                ("Mã sản phẩm", "Mã sản phẩm phải tồn tại trong hệ thống"),
                ("Số lượng", "Số nguyên dương (> 0)"),
                ("", ""),
                ("📝 CÁC CỘT TÙY CHỌN", ""),
                ("Mã lô", "Để trống nếu không quản lý theo lô"),
                ("Ghi chú", "Thông tin bổ sung về giao dịch"),
                ("", ""),
                ("⚠️ LƯU Ý QUAN TRỌNG", ""),
                ("1.", "Kho hàng: Bạn có thể nhập TÊN KHO (vd: 'Kho chính') hoặc MÃ KHO (vd: 'WH001')"),
                ("2.", "File phải có header ở dòng 1 (không được xóa)"),
                ("3.", "Dữ liệu bắt đầu từ dòng 2 trở đi"),
                ("4.", "Các dòng trống sẽ bị bỏ qua"),
                ("5.", "Hệ thống sẽ kiểm tra dữ liệu trước khi import"),
                ("", ""),
                ("✅ VÍ DỤ", ""),
                ("IMPORT | Kho chính | PRD001 | BATCH001 | 100 | Nhập từ NCC", ""),
                ("EXPORT | Kho phụ   | PRD002 |          | 50  | Xuất bán", ""),
                ("ADJUST | Kho chính | PRD003 | BATCH003 | 95  | Kiểm kê", "")
            };

            foreach (var (col1, col2) in instructions)
            {
                sheet.Cells[row, 1].Value = col1;
                sheet.Cells[row, 2].Value = col2;

                if (col1.Contains("📋") || col1.Contains("📝") || col1.Contains("⚠️") || col1.Contains("✅"))
                {
                    sheet.Cells[row, 1].Style.Font.Bold = true;
                    sheet.Cells[row, 1].Style.Font.Size = 12;
                    sheet.Cells[row, 1].Style.Font.Color.SetColor(Color.DarkBlue);
                }

                if (col1.Contains("|"))
                {
                    sheet.Cells[row, 1].Style.Font.Name = "Consolas";
                    sheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(240, 240, 240));
                }

                row++;
            }

            sheet.Column(1).Width = 40;
            sheet.Column(2).Width = 60;
        }
    }
}
