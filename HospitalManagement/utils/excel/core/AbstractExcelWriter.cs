using System.Globalization;
using ClosedXML.Excel;

namespace HospitalManagement.utils.excel.core
{
    public abstract class AbstractExcelWriter<T> : IExcelSheetWriter<T>
    {
        protected static readonly string DateFormat = "dd/MM/yyyy";
        protected static readonly string DateTimeFormat = "dd/MM/yyyy HH:mm";

        public abstract string SheetName { get; }
        public abstract string Title { get; }
        public abstract string[] Headers { get; }
        
        public abstract void Create(IXLWorksheet worksheet, List<T> data);

        protected string Safe(string? s) => s ?? string.Empty;

        // Helper để ghi giá trị với style
        protected void SetCell(IXLWorksheet worksheet, int row, int col, object? value, Action<IXLCell> styleAction)
        {
            var cell = worksheet.Cell(row, col);
            
            // Set value - ClosedXML cần explicit type
            switch (value)
            {
                case null:
                    cell.SetValue(string.Empty);
                    break;
                case DateTime dt:
                    cell.SetValue(dt.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
                    break;
                case DateOnly d:
                    cell.SetValue(d.ToString(DateFormat, CultureInfo.InvariantCulture));
                    break;
                case int i:
                    cell.SetValue(i);
                    break;
                case long l:
                    cell.SetValue(l);
                    break;
                case decimal dec:
                    cell.SetValue((double)dec);
                    break;
                case double dbl:
                    cell.SetValue(dbl);
                    break;
                case float f:
                    cell.SetValue(f);
                    break;
                case string s:
                    cell.SetValue(s);
                    break;
                default:
                    cell.SetValue(value.ToString() ?? string.Empty);
                    break;
            }

            // Apply style
            styleAction(cell);
        }

        // Style presets
        protected void ApplyTitleStyle(IXLCell cell)
        {
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontSize = 16;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        protected void ApplyHeaderStyle(IXLCell cell)
        {
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontSize = 12;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = XLColor.RoyalBlue;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        protected void ApplyDataStyle(IXLCell cell, bool center = false)
        {
            cell.Style.Font.FontSize = 11;
            cell.Style.Font.FontColor = XLColor.Black;
            cell.Style.Fill.BackgroundColor = XLColor.White;
            cell.Style.Alignment.Horizontal = center 
                ? XLAlignmentHorizontalValues.Center 
                : XLAlignmentHorizontalValues.Left;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Alignment.WrapText = true;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        protected void ApplyBadgeStyle(IXLCell cell, bool isActive)
        {
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontSize = 11;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = isActive ? XLColor.Green : XLColor.Red;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }
    }
}
