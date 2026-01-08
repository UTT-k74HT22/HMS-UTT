using System.Drawing;

namespace HospitalManagement.view.@base
{
    /// <summary>
    /// Theme và color constants cho toàn bộ ứng dụng
    /// Định nghĩa màu sắc và font chữ chuẩn theo phong cách ERP hiện đại
    /// </summary>
    public static class UiTheme
    {
        // ===== Base colors =====
        public static readonly Color PRIMARY = Color.FromArgb(113, 99, 248);
        public static readonly Color SECONDARY = Color.FromArgb(108, 117, 125);
        public static readonly Color BORDER = Color.FromArgb(235, 237, 242);
        public static readonly Color ROW_ALT = Color.FromArgb(250, 251, 252);
        public static readonly Color SELECT = Color.FromArgb(232, 236, 255);
        public static readonly Color TEXT = Color.FromArgb(52, 58, 70);
        public static readonly Color BG = Color.FromArgb(248, 249, 252);

        // ===== Semantic colors (ERP standard) =====
        public static readonly Color SUCCESS = Color.FromArgb(40, 167, 69);    // green
        public static readonly Color WARNING = Color.FromArgb(255, 193, 7);    // yellow
        public static readonly Color DANGER = Color.FromArgb(220, 53, 69);     // red
        public static readonly Color INFO = Color.FromArgb(23, 162, 184);      // cyan
        public static readonly Color ORANGE = Color.FromArgb(253, 126, 20);    // orange
        public static readonly Color PURPLE = Color.FromArgb(111, 66, 193);    // purple
        public static readonly Color DARK = Color.FromArgb(52, 58, 64);        // dark gray

        // ===== Fonts =====
        public static readonly Font FONT_BASE = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font FONT_BOLD = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font FONT_LARGE = new Font("Segoe UI", 12F, FontStyle.Regular);
        public static readonly Font FONT_LARGE_BOLD = new Font("Segoe UI", 12F, FontStyle.Bold);
        public static readonly Font FONT_SMALL = new Font("Segoe UI", 9F, FontStyle.Regular);
    }
}
