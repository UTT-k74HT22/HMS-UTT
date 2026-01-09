// using OfficeOpenXml;
// using System.IO;
//
// namespace HospitalManagement.utils.importer.core
// {
//     /// <summary>
//     /// Abstract class cung cấp logic chung cho việc import
//     /// Tương đương với AbstractImportService.java
//     /// </summary>
//     public abstract class AbstractImportService<T> where T : class
//     {
//         // ========== Abstract Methods (phải implement) ==========
//
//         /// <summary>
//         /// Lấy mapper để chuyển đổi dòng Excel thành DTO
//         /// </summary>
//         protected abstract IImportMapper<T> GetMapper();
//
//         /// <summary>
//         /// Lấy validator để validate dữ liệu
//         /// </summary>
//         protected abstract IImportValidator<T> GetValidator();
//
//         /// <summary>
//         /// Lưu dữ liệu hợp lệ vào database
//         /// </summary>
//         protected abstract void SaveData(List<T> validData);
//
//         // ========== Public Methods ==========
//
//         /// <summary>
//         /// Preview dữ liệu từ file Excel (đọc và validate, không lưu)
//         /// </summary>
//         public ImportPreviewResponse<T> PreviewFromFile(string filePath)
//         {
//             var response = new ImportPreviewResponse<T>();
//
//             if (!File.Exists(filePath))
//             {
//                 throw new FileNotFoundException($"File không tồn tại: {filePath}");
//             }
//
//             ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//
//             using var package = new ExcelPackage(new FileInfo(filePath));
//             var worksheet = package.Workbook.Worksheets.FirstOrDefault();
//
//             if (worksheet == null)
//             {
//                 throw new InvalidOperationException("File Excel không có worksheet nào.");
//             }
//
//             var mapper = GetMapper();
//             var validator = GetValidator();
//
//             // Validate headers
//             ValidateHeaders(worksheet, mapper.RequiredHeaders);
//
//             // Process data rows (skip header - row 1)
//             int rowCount = worksheet.Dimension?.Rows ?? 0;
//             for (int i = 2; i <= rowCount; i++) // Start from row 2 (skip header)
//             {
//                 var row = worksheet.Cells[i, 1, i, worksheet.Dimension.Columns];
//                 
//                 if (IsEmptyRow(row))
//                     continue;
//
//                 try
//                 {
//                     // Map dữ liệu
//                     T data = mapper.MapRow(row, i);
//
//                     // Validate dữ liệu
//                     List<ImportError> errors = validator.Validate(data, i);
//
//                     var rowData = new ImportRowData<T>
//                     {
//                         RowIndex = i,
//                         Data = data,
//                         Errors = errors,
//                         IsValid = errors.Count == 0
//                     };
//
//                     if (rowData.IsValid)
//                     {
//                         response.ValidRows.Add(rowData);
//                     }
//                     else
//                     {
//                         response.InvalidRows.Add(rowData);
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     // Nếu có exception khi map, coi như dòng lỗi
//                     var rowData = new ImportRowData<T>
//                     {
//                         RowIndex = i,
//                         Data = null,
//                         IsValid = false,
//                         Errors = new List<ImportError>
//                         {
//                             new ImportError(i, "General", $"Lỗi khi xử lý dòng: {ex.Message}")
//                         }
//                     };
//                     response.InvalidRows.Add(rowData);
//                 }
//             }
//
//             return response;
//         }
//
//         /// <summary>
//         /// Import dữ liệu từ file Excel (đọc, validate và lưu vào DB)
//         /// </summary>
//         public ImportResult ImportFromFile(string filePath)
//         {
//             var result = new ImportResult();
//
//             try
//             {
//                 // Preview trước
//                 var preview = PreviewFromFile(filePath);
//
//                 if (preview.HasErrors)
//                 {
//                     result.FailCount = preview.InvalidCount;
//                     result.Errors.Add($"File có {preview.InvalidCount} dòng lỗi. Vui lòng kiểm tra lại.");
//                     return result;
//                 }
//
//                 // Lưu dữ liệu hợp lệ
//                 var validData = preview.ValidRows.Select(r => r.Data!).ToList();
//                 SaveData(validData);
//
//                 result.SuccessCount = validData.Count;
//             }
//             catch (Exception ex)
//             {
//                 result.Errors.Add($"Lỗi khi import: {ex.Message}");
//             }
//
//             return result;
//         }
//
//         // ========== Helper Methods ==========
//
//         /// <summary>
//         /// Validate headers của file Excel
//         /// </summary>
//         private void ValidateHeaders(ExcelWorksheet worksheet, string[] requiredHeaders)
//         {
//             var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension?.Columns ?? 0];
//             
//             for (int i = 0; i < requiredHeaders.Length; i++)
//             {
//                 var expectedHeader = requiredHeaders[i];
//                 var actualHeader = GetCellValue(worksheet, 1, i + 1);
//
//                 if (!string.Equals(expectedHeader, actualHeader, StringComparison.OrdinalIgnoreCase))
//                 {
//                     throw new InvalidOperationException(
//                         $"Header không khớp tại cột {i + 1}. " +
//                         $"Expected: '{expectedHeader}', Actual: '{actualHeader}'");
//                 }
//             }
//         }
//
//         /// <summary>
//         /// Kiểm tra xem dòng có rỗng không
//         /// </summary>
//         protected bool IsEmptyRow(ExcelRange row)
//         {
//             for (int col = row.Start.Column; col <= row.End.Column; col++)
//             {
//                 var value = GetCellValue(row.Worksheet, row.Start.Row, col);
//                 if (!string.IsNullOrWhiteSpace(value))
//                     return false;
//             }
//             return true;
//         }
//
//         /// <summary>
//         /// Lấy giá trị cell dạng string
//         /// </summary>
//         protected string GetCellValue(ExcelWorksheet worksheet, int row, int col)
//         {
//             var cell = worksheet.Cells[row, col];
//             return cell.Value?.ToString()?.Trim() ?? string.Empty;
//         }
//
//         /// <summary>
//         /// Lấy giá trị cell từ ExcelRange
//         /// </summary>
//         protected string GetCellValue(ExcelRange row, int colIndex)
//         {
//             return GetCellValue(row.Worksheet, row.Start.Row, colIndex);
//         }
//
//         /// <summary>
//         /// Parse int từ cell
//         /// </summary>
//         protected int? ParseInt(string value)
//         {
//             if (string.IsNullOrWhiteSpace(value))
//                 return null;
//             return int.TryParse(value, out int result) ? result : null;
//         }
//
//         /// <summary>
//         /// Parse decimal từ cell
//         /// </summary>
//         protected decimal? ParseDecimal(string value)
//         {
//             if (string.IsNullOrWhiteSpace(value))
//                 return null;
//             return decimal.TryParse(value, out decimal result) ? result : null;
//         }
//
//         /// <summary>
//         /// Parse DateTime từ cell
//         /// </summary>
//         protected DateTime? ParseDateTime(string value)
//         {
//             if (string.IsNullOrWhiteSpace(value))
//                 return null;
//             return DateTime.TryParse(value, out DateTime result) ? result : null;
//         }
//
//         /// <summary>
//         /// Parse bool từ cell (Yes/No, True/False, 1/0)
//         /// </summary>
//         protected bool ParseBool(string value)
//         {
//             if (string.IsNullOrWhiteSpace(value))
//                 return false;
//
//             value = value.ToLower().Trim();
//             return value == "yes" || value == "true" || value == "1" || value == "có";
//         }
//     }
// }
