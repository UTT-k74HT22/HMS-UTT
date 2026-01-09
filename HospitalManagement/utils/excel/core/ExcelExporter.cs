// using System.ComponentModel;
// using OfficeOpenXml;
// using System.Diagnostics;
// using System.IO;
// using System.Windows.Forms;
// using OfficeOpenXml.Core.ExcelPackage;
//
// namespace HospitalManagement.utils.excel.core
// {
//     /// <summary>
//     /// Utility class để xuất dữ liệu ra file Excel
//     /// Tương đương với ExcelExporter.java
//     /// </summary>
//     public static class ExcelExporter
//     {
//         /// <summary>
//         /// Hiển thị hộp thoại lưu file và xuất dữ liệu ra Excel
//         /// </summary>
//         public static void ExportWithDialog<T>(List<T> data, IExcelSheetWriter<T> writer, Form? parent = null)
//         {
//             using var saveDialog = new SaveFileDialog
//             {
//                 Title = "Chọn nơi lưu file Excel",
//                 Filter = "Excel Files (*.xlsx)|*.xlsx",
//                 FileName = writer.SheetName + ".xlsx",
//                 DefaultExt = "xlsx"
//             };
//
//             if (saveDialog.ShowDialog(parent) != DialogResult.OK)
//                 return;
//
//             string filePath = saveDialog.FileName;
//
//             // Kiểm tra file tồn tại
//             if (File.Exists(filePath))
//             {
//                 var confirmResult = MessageBox.Show(
//                     $"File đã tồn tại. Bạn có muốn ghi đè không?\n{filePath}",
//                     "Xác nhận ghi đè",
//                     MessageBoxButtons.YesNo,
//                     MessageBoxIcon.Warning);
//
//                 if (confirmResult != DialogResult.Yes)
//                     return;
//             }
//
//             try
//             {
//                 // Xuất file
//                 ExportToFile(data, writer, filePath);
//
//                 // Hiển thị thông báo thành công với options
//                 var result = MessageBox.Show(
//                     $"Xuất file Excel thành công!\n{filePath}\n\nBạn có muốn mở file không?",
//                     "Thành công",
//                     MessageBoxButtons.YesNo,
//                     MessageBoxIcon.Information);
//
//                 if (result == DialogResult.Yes)
//                 {
//                     OpenFile(filePath, parent);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show(
//                     $"Lỗi khi xuất file Excel:\n{GetRootMessage(ex)}",
//                     "Lỗi",
//                     MessageBoxButtons.OK,
//                     MessageBoxIcon.Error);
//             }
//         }
//
//         /// <summary>
//         /// Xuất dữ liệu ra file Excel (không phụ thuộc UI)
//         /// </summary>
//         public static void ExportToFile<T>(List<T> data, IExcelSheetWriter<T> writer, string filePath)
//         {
//             // Đảm bảo thư mục tồn tại
//             string? directory = Path.GetDirectoryName(filePath);
//             if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
//             {
//                 Directory.CreateDirectories(directory);
//             }
//
//             // Set license context (NonCommercial cho phiên bản miễn phí)
//             ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//
//             using var package = new ExcelPackage();
//             var styles = new ExcelStyles(package);
//             var worksheet = package.Workbook.Worksheets.Add(writer.SheetName);
//
//             // Gọi writer để tạo nội dung
//             writer.Create(worksheet, styles, data);
//
//             // Freeze title + header (2 rows đầu)
//             worksheet.View.FreezePanes(3, 1);
//
//             // Auto-fit columns với padding
//             int columnCount = writer.Headers.Length;
//             for (int i = 1; i <= columnCount; i++)
//             {
//                 worksheet.Column(i).AutoFit();
//                 
//                 // Thêm padding để không bị sát chữ (tối đa 255 characters width)
//                 double currentWidth = worksheet.Column(i).Width;
//                 double newWidth = Math.Min(currentWidth + 2, 100);
//                 worksheet.Column(i).Width = newWidth;
//             }
//
//             // Lưu file
//             var fileInfo = new FileInfo(filePath);
//             package.SaveAs(fileInfo);
//         }
//
//         /// <summary>
//         /// Mở file Excel bằng ứng dụng mặc định
//         /// </summary>
//         private static void OpenFile(string filePath, Form? parent)
//         {
//             try
//             {
//                 Process.Start(new ProcessStartInfo
//                 {
//                     FileName = filePath,
//                     UseShellExecute = true
//                 });
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show(
//                     $"Không thể mở file:\n{ex.Message}",
//                     "Lỗi",
//                     MessageBoxButtons.OK,
//                     MessageBoxIcon.Warning);
//             }
//         }
//
//         /// <summary>
//         /// Lấy thông điệp lỗi gốc từ exception
//         /// </summary>
//         private static string GetRootMessage(Exception ex)
//         {
//             var innerMost = ex;
//             while (innerMost.InnerException != null)
//             {
//                 innerMost = innerMost.InnerException;
//             }
//             return innerMost.Message;
//         }
//     }
// }
