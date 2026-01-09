using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace HospitalManagement.utils.excel.core
{
    /// <summary>
    /// Quản lý các kiểu định dạng Excel
    /// Tương đương với ExcelStyles.java trong Java version
    /// </summary>
    public class ExcelStyles
    {
        private readonly ExcelPackage _package;
        private readonly Dictionary<StyleKey, ExcelStyle> _cache;

        public ExcelStyles(ExcelPackage package)
        {
            _package = package;
            _cache = new Dictionary<StyleKey, ExcelStyle>();
        }

        /// <summary>
        /// Lấy style theo key, tự động cache lại
        /// </summary>
        public ExcelStyle Get(StyleKey key)
        {
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = Build(key);
            }
            return _cache[key];
        }

        /// <summary>
        /// Dựng style theo key
        /// </summary>
        private ExcelStyle Build(StyleKey key)
        {
            return key switch
            {
                StyleKey.TITLE => BuildTitle(),
                StyleKey.HEADER => BuildHeader(),
                StyleKey.DATA => BuildData(false),
                StyleKey.DATA_CENTER => BuildData(true),
                StyleKey.BADGE_ACTIVE => BuildBadge(true),
                StyleKey.BADGE_INACTIVE => BuildBadge(false),
                _ => throw new ArgumentException($"Unknown style key: {key}")
            };
        }

        /// <summary>
        /// Style cho tiêu đề sheet
        /// </summary>
        private ExcelStyle BuildTitle()
        {
            var style = _package.Workbook.Styles.CreateNamedStyle($"Title_{Guid.NewGuid()}");
            
            // Font
            style.Style.Font.Bold = true;
            style.Style.Font.Size = 16;
            style.Style.Font.Color.SetColor(Color.White);
            
            // Background
            style.Style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 0, 139)); // Dark Blue
            
            // Alignment
            style.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            style.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            
            // Border
            SetThinBorder(style.Style);
            
            return style.Style;
        }

        /// <summary>
        /// Style cho header cột
        /// </summary>
        private ExcelStyle BuildHeader()
        {
            var style = _package.Workbook.Styles.CreateNamedStyle($"Header_{Guid.NewGuid()}");
            
            // Font
            style.Style.Font.Bold = true;
            style.Style.Font.Size = 12;
            style.Style.Font.Color.SetColor(Color.White);
            
            // Background
            style.Style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(65, 105, 225)); // Royal Blue
            
            // Alignment
            style.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            style.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            
            // Border
            SetThinBorder(style.Style);
            
            return style.Style;
        }

        /// <summary>
        /// Style cho dữ liệu
        /// </summary>
        private ExcelStyle BuildData(bool center)
        {
            var style = _package.Workbook.Styles.CreateNamedStyle($"Data_{center}_{Guid.NewGuid()}");
            
            // Font
            style.Style.Font.Size = 11;
            style.Style.Font.Color.SetColor(Color.Black);
            
            // Background
            style.Style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Style.Fill.BackgroundColor.SetColor(Color.White);
            
            // Alignment
            style.Style.HorizontalAlignment = center 
                ? ExcelHorizontalAlignment.Center 
                : ExcelHorizontalAlignment.Left;
            style.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            style.Style.WrapText = true;
            
            // Border
            SetThinBorder(style.Style);
            
            return style.Style;
        }

        /// <summary>
        /// Style cho badge (Active/Inactive)
        /// </summary>
        private ExcelStyle BuildBadge(bool isActive)
        {
            var style = _package.Workbook.Styles.CreateNamedStyle($"Badge_{isActive}_{Guid.NewGuid()}");
            
            // Font
            style.Style.Font.Bold = true;
            style.Style.Font.Size = 11;
            style.Style.Font.Color.SetColor(Color.White);
            
            // Background
            style.Style.Fill.PatternType = ExcelFillStyle.Solid;
            if (isActive)
            {
                style.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(40, 167, 69)); // Green
            }
            else
            {
                style.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(220, 53, 69)); // Red
            }
            
            // Alignment
            style.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            style.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            
            // Border
            SetThinBorder(style.Style);
            
            return style.Style;
        }

        /// <summary>
        /// Thêm border mỏng cho style
        /// </summary>
        private void SetThinBorder(ExcelStyle style)
        {
            style.Border.Top.Style = ExcelBorderStyle.Thin;
            style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            style.Border.Left.Style = ExcelBorderStyle.Thin;
            style.Border.Right.Style = ExcelBorderStyle.Thin;
            
            style.Border.Top.Color.SetColor(Color.LightGray);
            style.Border.Bottom.Color.SetColor(Color.LightGray);
            style.Border.Left.Color.SetColor(Color.LightGray);
            style.Border.Right.Color.SetColor(Color.LightGray);
        }
    }
}
