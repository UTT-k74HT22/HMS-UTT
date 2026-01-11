using ClosedXML.Excel;

namespace HospitalManagement.utils.excel.core
{
    /// <summary>
    /// Interface định nghĩa contract cho việc ghi dữ liệu vào Excel sheet (ClosedXML version)
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu cần export</typeparam>
    public interface IExcelSheetWriter<T>
    {
        /// <summary>
        /// Tên sheet trong file Excel
        /// </summary>
        string SheetName { get; }

        /// <summary>
        /// Tiêu đề của sheet (hiển thị ở dòng đầu tiên)
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Các tiêu đề cột trong sheet
        /// </summary>
        string[] Headers { get; }

        /// <summary>
        /// Ghi dữ liệu vào sheet
        /// </summary>
        /// <param name="worksheet">Worksheet cần ghi</param>
        /// <param name="data">Danh sách dữ liệu cần ghi</param>
        void Create(IXLWorksheet worksheet, List<T> data);
    }
}
