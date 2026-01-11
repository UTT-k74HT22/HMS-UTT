namespace HospitalManagement.utils.importer.core
{
    /// <summary>
    /// Model chứa dữ liệu của một dòng import
    /// </summary>
    public class ImportRowData<T>
    {
        public int RowIndex { get; set; }
        public T? Data { get; set; }
        public List<ImportError> Errors { get; set; } = new();
        public bool IsValid { get; set; }

        public string GetErrorSummary()
        {
            if (IsValid) return "OK";
            return string.Join("; ", Errors.Select(e => $"{e.FieldName}: {e.ErrorMessage}"));
        }
    }

    /// <summary>
    /// Response chứa kết quả preview import
    /// </summary>
    public class ImportPreviewResponse<T>
    {
        public List<ImportRowData<T>> ValidRows { get; set; } = new();
        public List<ImportRowData<T>> InvalidRows { get; set; } = new();
        public int TotalRows { get; set; }
        public bool HasErrors { get; set; }
        
        public int ValidCount => ValidRows.Count;
        public int InvalidCount => InvalidRows.Count;

        public string GetSummary()
        {
            return $"Tổng: {TotalRows} | Hợp lệ: {ValidCount} | Lỗi: {InvalidCount}";
        }
    }
}
