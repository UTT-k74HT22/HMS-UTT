namespace HospitalManagement.utils.importer.core
{
    /// <summary>
    /// Interface để validate dữ liệu import
    /// Tương đương với ImportValidator.java
    /// </summary>
    /// <typeparam name="T">Kiểu DTO cần validate</typeparam>
    public interface IImportValidator<T>
    {
        /// <summary>
        /// Validate dữ liệu import
        /// </summary>
        /// <param name="data">Đối tượng cần validate</param>
        /// <param name="rowIndex">Chỉ số dòng (1-based)</param>
        /// <returns>Danh sách lỗi, rỗng nếu hợp lệ</returns>
        List<ImportError> Validate(T data, int rowIndex);
    }
}
