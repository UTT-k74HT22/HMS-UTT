// using OfficeOpenXml;
// using System.Globalization;
// using OfficeOpenXml.Core.ExcelPackage;
//
// namespace HospitalManagement.utils.excel.core
// {
//     /// <summary>
//     /// Base class cho các Excel Writer
//     /// Cung cấp các helper methods để ghi dữ liệu vào cell
//     /// Tương đương với AbstractExcelWriter.java
//     /// </summary>
//     /// <typeparam name="T">Kiểu dữ liệu cần export</typeparam>
//     public abstract class AbstractExcelWriter<T> : IExcelSheetWriter<T>
//     {
//         protected static readonly string DateFormat = "dd/MM/yyyy";
//         protected static readonly string DateTimeFormat = "dd/MM/yyyy HH:mm";
//
//         // ========== Abstract Members (phải implement) ==========
//         
//         public abstract string SheetName { get; }
//         public abstract string Title { get; }
//         public abstract string[] Headers { get; }
//         public abstract void Create(ExcelWorksheet worksheet, ExcelStyles styles, List<T> data);
//
//         // ========== Helper Methods ==========
//
//         /// <summary>
//         /// Trả về chuỗi an toàn (không null)
//         /// </summary>
//         protected string Safe(string? s)
//         {
//             return s ?? string.Empty;
//         }
//
//         /// <summary>
//         /// Ghi giá trị vào cell với style
//         /// </summary>
//         protected void SetCell(ExcelWorksheet worksheet, int row, int col, object? value, ExcelStyles style)
//         {
//             var cell = worksheet.Cells[row, col];
//             
//             if (value == null)
//             {
//                 cell.Value = string.Empty;
//             }
//             else if (value is string str)
//             {
//                 cell.Value = str;
//             }
//             else if (value is bool b)
//             {
//                 cell.Value = b;
//             }
//             else if (value is DateTime dt)
//             {
//                 cell.Value = dt.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
//             }
//             else if (value is DateOnly d)
//             {
//                 cell.Value = d.ToString(DateFormat, CultureInfo.InvariantCulture);
//             }
//             else if (value is decimal dec)
//             {
//                 cell.Value = dec;
//             }
//             else if (value is int || value is long || value is double || value is float)
//             {
//                 cell.Value = Convert.ToDouble(value);
//             }
//             else
//             {
//                 cell.Value = value.ToString();
//             }
//
//             // Apply style
//             ApplyStyle(cell, style);
//         }
//
//         /// <summary>
//         /// Apply style vào cell
//         /// </summary>
//         protected void ApplyStyle(ExcelRange cell, ExcelStyles style)
//         {
//             cell.Style.Font.Bold = style.Font.Bold;
//             cell.Style.Font.Size = style.Font.Size;
//             cell.Style.Font.Color.SetColor(style.Font.Color.Rgb);
//             
//             if (style.Fill.PatternType != OfficeOpenXml.Style.ExcelFillStyle.None)
//             {
//                 cell.Style.Fill.PatternType = style.Fill.PatternType;
//                 cell.Style.Fill.BackgroundColor.SetColor(style.Fill.BackgroundColor.Rgb);
//             }
//             
//             cell.Style.HorizontalAlignment = style.HorizontalAlignment;
//             cell.Style.VerticalAlignment = style.VerticalAlignment;
//             cell.Style.WrapText = style.WrapText;
//             
//             // Borders
//             cell.Style.Border.Top.Style = style.Border.Top.Style;
//             cell.Style.Border.Bottom.Style = style.Border.Bottom.Style;
//             cell.Style.Border.Left.Style = style.Border.Left.Style;
//             cell.Style.Border.Right.Style = style.Border.Right.Style;
//             
//             if (style.Border.Top.Style != OfficeOpenXml.Style.ExcelBorderStyle.None)
//             {
//                 cell.Style.Border.Top.Color.SetColor(style.Border.Top.Color.Rgb);
//                 cell.Style.Border.Bottom.Color.SetColor(style.Border.Bottom.Color.Rgb);
//                 cell.Style.Border.Left.Color.SetColor(style.Border.Left.Color.Rgb);
//                 cell.Style.Border.Right.Color.SetColor(style.Border.Right.Color.Rgb);
//             }
//         }
//
//         /// <summary>
//         /// Format số thành chuỗi với ngăn cách hàng nghìn
//         /// </summary>
//         protected string FormatNumber(decimal? number)
//         {
//             return number?.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) ?? string.Empty;
//         }
//
//         /// <summary>
//         /// Format DateTime
//         /// </summary>
//         protected string FormatDateTime(DateTime? dateTime)
//         {
//             return dateTime?.ToString(DateTimeFormat) ?? string.Empty;
//         }
//
//         /// <summary>
//         /// Format Date
//         /// </summary>
//         protected string FormatDate(DateTime? date)
//         {
//             return date?.ToString(DateFormat) ?? string.Empty;
//         }
//     }
// }
