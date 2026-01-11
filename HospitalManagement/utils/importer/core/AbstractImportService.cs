using OfficeOpenXml;

namespace HospitalManagement.utils.importer.core
{
    /// <summary>
    /// Abstract class cung cấp logic chung cho việc import
    /// Sử dụng Template Method Pattern - định nghĩa khung sườn thuật toán
    /// Các class con chỉ cần implement các bước cụ thể
    /// Tương đương với AbstractImportService.java
    /// </summary>
    /// <typeparam name="T">Kiểu DTO cần import</typeparam>
    public abstract class AbstractImportService<T> where T : class
    {
        // ========== Template Method Pattern - Abstract Methods ==========

        /// <summary>
        /// Lấy mapper để chuyển đổi dòng Excel thành DTO
        /// </summary>
        protected abstract IImportMapper<T> GetMapper();

        /// <summary>
        /// Lấy validator để validate dữ liệu
        /// </summary>
        protected abstract IImportValidator<T> GetValidator();

        /// <summary>
        /// Lưu dữ liệu hợp lệ vào database
        /// </summary>
        protected abstract void SaveData(List<T> validData);

        // ========== Public Methods ==========

        /// <summary>
        /// Preview dữ liệu từ file Excel
        /// Đọc file, map và validate từng dòng
        /// </summary>
        public ImportPreviewResponse<T> PreviewFromFile(string filePath)
        {
            var validRows = new List<ImportRowData<T>>();
            var invalidRows = new List<ImportRowData<T>>();

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File không tồn tại: {filePath}");
            }

            // EPPlus 8+ requires setting license using static property
            ExcelPackage.License.SetNonCommercialOrganization("HospitalManagement");
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                throw new InvalidOperationException("File rỗng hoặc không có worksheet.");
            }

            var mapper = GetMapper();
            var validator = GetValidator();

            // Validate headers
            if (worksheet.Dimension == null || worksheet.Dimension.Rows == 0)
            {
                throw new ArgumentException("File rỗng hoặc không có header row.");
            }

            string[] requiredHeaders = mapper.RequiredHeaders;
            if (!ValidateHeaders(worksheet, requiredHeaders))
            {
                var error = BuildHeaderErrorMessage(worksheet, requiredHeaders);
                throw new ArgumentException(error);
            }

            // Process data rows (skip header)
            int rowCount = worksheet.Dimension.Rows;
            for (int i = 2; i <= rowCount; i++) // Start from row 2 (skip header)
            {
                var row = worksheet.Cells[i, 1, i, worksheet.Dimension.Columns];

                if (IsEmptyRow(row))
                    continue;

                try
                {
                    // Map dữ liệu
                    T data = mapper.MapRow(row, i);

                    // Validate dữ liệu
                    List<ImportError> errors = validator.Validate(data, i);

                    var rowData = new ImportRowData<T>
                    {
                        RowIndex = i,
                        Data = data,
                        Errors = errors,
                        IsValid = errors.Count == 0
                    };

                    if (errors.Count == 0)
                    {
                        validRows.Add(rowData);
                    }
                    else
                    {
                        invalidRows.Add(rowData);
                    }
                }
                catch (Exception ex)
                {
                    // Nếu có exception khi map, coi như dòng lỗi
                    var rowData = new ImportRowData<T>
                    {
                        RowIndex = i,
                        Data = null,
                        IsValid = false,
                        Errors = new List<ImportError>
                        {
                            new ImportError(i, "Parse Error", $"Không thể đọc dữ liệu: {ex.Message}")
                        }
                    };
                    invalidRows.Add(rowData);
                }
            }

            return new ImportPreviewResponse<T>
            {
                ValidRows = validRows,
                InvalidRows = invalidRows,
                TotalRows = validRows.Count + invalidRows.Count,
                HasErrors = invalidRows.Any()
            };
        }

        /// <summary>
        /// Apply import - lưu dữ liệu hợp lệ vào database
        /// </summary>
        public int ApplyImport(List<T> validData)
        {
            SaveData(validData);
            return validData.Count;
        }

        // ========== Protected Helper Methods ==========

        /// <summary>
        /// Lấy giá trị cell dưới dạng String
        /// Xử lý các kiểu dữ liệu khác nhau
        /// </summary>
        protected string GetCellValue(ExcelWorksheet worksheet, int row, int col)
        {
            var cell = worksheet.Cells[row, col];
            if (cell.Value == null) return string.Empty;

            // Xử lý các kiểu dữ liệu
            if (cell.Value is string strValue)
            {
                return strValue.Trim();
            }
            else if (cell.Value is double || cell.Value is decimal || 
                     cell.Value is int || cell.Value is long)
            {
                // Xử lý số
                var numericValue = Convert.ToDouble(cell.Value);
                // Nếu là số nguyên
                if (numericValue == (long)numericValue)
                {
                    return ((long)numericValue).ToString();
                }
                return numericValue.ToString();
            }
            else if (cell.Value is DateTime dateValue)
            {
                return dateValue.ToString();
            }
            else if (cell.Value is bool boolValue)
            {
                return boolValue.ToString();
            }

            return cell.Value.ToString()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Lấy giá trị cell từ ExcelRange
        /// </summary>
        protected string GetCellValue(ExcelRange row, int colIndex)
        {
            return GetCellValue(row.Worksheet, row.Start.Row, colIndex);
        }

        // ========== Private Helper Methods ==========

        /// <summary>
        /// Validate headers của file Excel
        /// </summary>
        private bool ValidateHeaders(ExcelWorksheet worksheet, string[] requiredHeaders)
        {
            if (worksheet.Dimension == null) return false;

            // Check if we have enough columns
            int actualColumnCount = worksheet.Dimension.Columns;
            if (actualColumnCount < requiredHeaders.Length)
            {
                Console.Error.WriteLine($"Not enough columns. Expected: {requiredHeaders.Length}, Found: {actualColumnCount}");
                return false;
            }

            // Validate each header
            for (int i = 0; i < requiredHeaders.Length; i++)
            {
                string actualHeader = GetCellValue(worksheet, 1, i + 1).Trim();
                string expectedHeader = requiredHeaders[i].Trim();

                if (!actualHeader.Equals(expectedHeader, StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine($"Header mismatch at column {i + 1}:");
                    Console.Error.WriteLine($"  Expected: '{expectedHeader}'");
                    Console.Error.WriteLine($"  Found: '{actualHeader}'");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Tạo thông báo lỗi chi tiết khi header không khớp
        /// </summary>
        private string BuildHeaderErrorMessage(ExcelWorksheet worksheet, string[] requiredHeaders)
        {
            var error = new System.Text.StringBuilder();
            error.AppendLine("File không đúng định dạng. Header không khớp với template.");
            error.Append("Expected headers: ");
            
            for (int i = 0; i < requiredHeaders.Length; i++)
            {
                if (i > 0) error.Append(", ");
                error.Append($"\"{requiredHeaders[i]}\"");
            }
            
            error.AppendLine();
            error.Append("Actual headers: ");
            
            int actualColCount = Math.Min(worksheet.Dimension?.Columns ?? 0, requiredHeaders.Length + 5);
            for (int i = 0; i < actualColCount; i++)
            {
                if (i > 0) error.Append(", ");
                error.Append($"\"{GetCellValue(worksheet, 1, i + 1)}\"");
            }
            
            return error.ToString();
        }

        /// <summary>
        /// Kiểm tra dòng có rỗng không
        /// </summary>
        private bool IsEmptyRow(ExcelRange row)
        {
            for (int col = row.Start.Column; col <= row.End.Column; col++)
            {
                if (!string.IsNullOrWhiteSpace(GetCellValue(row.Worksheet, row.Start.Row, col)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
