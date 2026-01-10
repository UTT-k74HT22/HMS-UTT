namespace HospitalManagement.utils.importer.dto
{
    /// <summary>
    /// DTO cho việc import Account từ Excel
    /// </summary>
    public class AccountImportDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
