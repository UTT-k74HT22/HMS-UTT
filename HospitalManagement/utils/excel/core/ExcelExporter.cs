using ClosedXML.Excel;
using System.Diagnostics;

namespace HospitalManagement.utils.excel.core
{
    /// <summary>
    /// Utility class để xuất dữ liệu ra file Excel
    /// Tương đương với ExcelExporter.java
    /// </summary>
    public static class ExcelExporter
    {
        /// <summary>
        /// Hiển thị hộp thoại lưu file và xuất dữ liệu ra Excel
        /// </summary>
        public static void ExportWithDialog<T>(List<T> data, IExcelSheetWriter<T> writer, Form? parent = null)
        {
            try
            {
                Console.WriteLine($"[ExcelExporter] Bắt đầu export {data.Count} items");
            
                using var saveDialog = new SaveFileDialog
                {
                    Title = "Chọn nơi lưu file Excel",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = writer.SheetName + ".xlsx",
                    DefaultExt = "xlsx"
                };

                if (saveDialog.ShowDialog(parent) != DialogResult.OK)
                {
                    Console.WriteLine("[ExcelExporter] User cancelled");
                    return;
                }

                string filePath = saveDialog.FileName;
                Console.WriteLine($"[ExcelExporter] Saving to: {filePath}");

                // Kiểm tra file tồn tại
                if (File.Exists(filePath))
                {
                    var confirmResult = MessageBox.Show(
                        $"File đã tồn tại. Bạn có muốn ghi đè không?\n{filePath}",
                        "Xác nhận ghi đè",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirmResult != DialogResult.Yes)
                        return;
                }

                Console.WriteLine("[ExcelExporter] Calling ExportToFile...");
                // Xuất file
                ExportToFile(data, writer, filePath);
                Console.WriteLine("[ExcelExporter] ExportToFile completed successfully");

                // Hiển thị thông báo thành công với options
                var result = MessageBox.Show(
                    $"Xuất file Excel thành công!\n{filePath}\n\nBạn có muốn mở file không?",
                    "Thành công",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    OpenFile(filePath, parent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ExcelExporter] ERROR: {ex.Message}");
                Console.WriteLine($"[ExcelExporter] Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[ExcelExporter] Inner: {ex.InnerException.Message}");
                }
                
                MessageBox.Show(
                    $"Lỗi khi xuất file Excel:\n{GetRootMessage(ex)}\n\nChi tiết: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file Excel (không phụ thuộc UI)
        /// </summary>
        public static void ExportToFile<T>(List<T> data, IExcelSheetWriter<T> writer, string filePath)
        {
            Console.WriteLine($"[ExportToFile] Starting export to {filePath}");
            
            // Đảm bảo thư mục tồn tại
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Console.WriteLine($"[ExportToFile] Creating directory: {directory}");
                Directory.CreateDirectory(directory);
            }

            Console.WriteLine("[ExportToFile] Creating workbook...");
            using var workbook = new XLWorkbook();
            
            Console.WriteLine($"[ExportToFile] Adding worksheet: {writer.SheetName}");
            var worksheet = workbook.Worksheets.Add(writer.SheetName);

            Console.WriteLine("[ExportToFile] Calling writer.Create...");
            // Gọi writer để tạo nội dung
            writer.Create(worksheet, data);

            Console.WriteLine("[ExportToFile] Freezing panes...");
            // Freeze title + header (2 rows đầu)
            worksheet.SheetView.FreezeRows(2);

            Console.WriteLine("[ExportToFile] Auto-fitting columns...");
            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            Console.WriteLine($"[ExportToFile] Saving to file: {filePath}");
            // Lưu file
            workbook.SaveAs(filePath);
            
            Console.WriteLine("[ExportToFile] Export completed successfully!");
        }

        /// <summary>
        /// Mở file Excel bằng ứng dụng mặc định
        /// </summary>
        private static void OpenFile(string filePath, Form? parent)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể mở file:\n{ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Lấy thông điệp lỗi gốc từ exception
        /// </summary>
        private static string GetRootMessage(Exception ex)
        {
            var innerMost = ex;
            while (innerMost.InnerException != null)
            {
                innerMost = innerMost.InnerException;
            }
            return innerMost.Message;
        }
    }
}
