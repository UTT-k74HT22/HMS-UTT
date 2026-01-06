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
        public static readonly Color BORDER = Color.FromArgb(230, 232, 245);
        public static readonly Color ROW_ALT = Color.FromArgb(248, 249, 255);
        public static readonly Color SELECT = Color.FromArgb(232, 236, 255);
        public static readonly Color TEXT = Color.FromArgb(45, 45, 70);
        public static readonly Color BG = Color.FromArgb(245, 246, 248);

        // ===== Semantic colors (ERP standard) =====
        public static readonly Color SUCCESS = Color.FromArgb(39, 174, 96);   // green
        public static readonly Color WARNING = Color.FromArgb(241, 196, 15);  // yellow
        public static readonly Color DANGER = Color.FromArgb(231, 76, 60);    // red
        public static readonly Color INFO = Color.FromArgb(52, 152, 219);     // blue
        public static readonly Color ORANGE = Color.FromArgb(230, 126, 34);   // orange
        public static readonly Color PURPLE = Color.FromArgb(155, 89, 182);   // purple
        public static readonly Color DARK = Color.FromArgb(52, 73, 94);       // dark gray

        // ===== Fonts =====
        public static readonly Font FONT_BASE = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font FONT_BOLD = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font FONT_LARGE = new Font("Segoe UI", 12F, FontStyle.Regular);
        public static readonly Font FONT_LARGE_BOLD = new Font("Segoe UI", 12F, FontStyle.Bold);
        public static readonly Font FONT_SMALL = new Font("Segoe UI", 9F, FontStyle.Regular);
    }
}
