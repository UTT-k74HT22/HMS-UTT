namespace HospitalManagement.utils.importer.core
{
    /// <summary>
    /// Model đại diện cho một lỗi trong quá trình import
    /// </summary>
    public class ImportError
    {
        public int RowIndex { get; set; }
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }

        public ImportError(int rowIndex, string fieldName, string errorMessage)
        {
            RowIndex = rowIndex;
            FieldName = fieldName;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return $"[Row {RowIndex}] {FieldName}: {ErrorMessage}";
        }
    }
}
