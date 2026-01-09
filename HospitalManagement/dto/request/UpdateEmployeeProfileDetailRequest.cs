using HospitalManagement.entity;

namespace HospitalManagement.dto.request
{
    /// <summary>
    /// Request DTO để cập nhật chi tiết hồ sơ nhân viên
    /// </summary>
    public class UpdateEmployeeProfileDetailRequest
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime? HiredDate { get; set; }
        public decimal? Salary { get; set; }
        public ProfileStatus Status { get; set; }
    }
}
