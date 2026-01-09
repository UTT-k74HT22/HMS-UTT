using OfficeOpenXml;

namespace HospitalManagement.utils.importer.core
{
    /// <summary>
    /// Interface để map dữ liệu từ Excel row sang DTO
    /// Tương đương với ImportMapper.java
    /// </summary>
    /// <typeparam name="T">Kiểu DTO</typeparam>
    public interface IImportMapper<T>
    {
        /// <summary>
        /// Map một dòng Excel thành đối tượng DTO
        /// </summary>
        /// <param name="row">Dòng Excel (ExcelRange)</param>
        /// <param name="rowIndex">Chỉ số dòng (1-based, không tính header)</param>
        /// <returns>Đối tượng DTO</returns>
        T MapRow(ExcelRange row, int rowIndex);

        /// <summary>
        /// Lấy danh sách các cột bắt buộc trong file Excel
        /// </summary>
        string[] RequiredHeaders { get; }
    }
}
