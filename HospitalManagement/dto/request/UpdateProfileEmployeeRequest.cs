using HospitalManagement.entity;

namespace HospitalManagement.dto.request
{
    /// <summary>
    /// Request DTO để cập nhật thông tin hồ sơ nhân viên
    /// </summary>
    public class UpdateProfileEmployeeRequest
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public ProfileStatus Status { get; set; }
    }
}
