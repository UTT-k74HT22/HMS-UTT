namespace HospitalManagement.utils.importer.dto
{
    /// <summary>
    /// DTO cho việc import Employee từ Excel
    /// </summary>
    public class EmployeeImportDto
    {
        public int ProfileId { get; set; }
        public string Position { get; set; } = null!;
        public string Department { get; set; } = null!;
        public DateTime? HiredDate { get; set; }
        public decimal BaseSalary { get; set; }
    }
}
