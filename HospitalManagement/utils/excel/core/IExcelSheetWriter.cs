// using OfficeOpenXml.Core.ExcelPackage;
//
// namespace HospitalManagement.utils.excel.core
// {
//     /// <summary>
//     /// Interface định nghĩa contract cho việc ghi dữ liệu vào Excel sheet
//     /// Tương đương với ExcelSheetWriter.java trong Java version
//     /// </summary>
//     /// <typeparam name="T">Kiểu dữ liệu cần export</typeparam>
//     public interface IExcelSheetWriter<T>
//     {
//         /// <summary>
//         /// Tên sheet trong file Excel
//         /// </summary>
//         string SheetName { get; }
//
//         /// <summary>
//         /// Tiêu đề của sheet (hiển thị ở dòng đầu tiên)
//         /// </summary>
//         string Title { get; }
//
//         /// <summary>
//         /// Các tiêu đề cột trong sheet
//         /// </summary>
//         string[] Headers { get; }
//
//         /// <summary>
//         /// Ghi dữ liệu vào sheet
//         /// </summary>
//         /// <param name="worksheet">Worksheet cần ghi</param>
//         /// <param name="styles">Đối tượng quản lý styles</param>
//         /// <param name="data">Danh sách dữ liệu cần ghi</param>
//         void Create(ExcelWorksheet worksheet, ExcelStyles styles, List<T> data);
//     }
// }
