using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;

namespace HospitalManagement.service
{
    /// <summary>
    /// Service interface cho quản lý nhân viên
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Lấy tất cả nhân viên
        /// </summary>
        List<EmployeeProfileResponse> GetAllEmployees();

        /// <summary>
        /// Lấy tất cả chi tiết hồ sơ nhân viên
        /// </summary>
        List<EmployeeProfileDetailResponse> GetAllProfileDetails();

        /// <summary>
        /// Lấy chi tiết nhân viên theo mã nhân viên
        /// </summary>
        EmployeeProfileDetailResponse GetEmployeeDetailByCode(string code);

        /// <summary>
        /// Cập nhật thông tin hồ sơ nhân viên
        /// </summary>
        void UpdateProfile(string code, UpdateProfileEmployeeRequest request);

        /// <summary>
        /// Cập nhật chi tiết hồ sơ nhân viên theo mã nhân viên
        /// </summary>
        void UpdateProfileDetail(string code, UpdateEmployeeProfileDetailRequest request);

        /// <summary>
        /// Xoá hồ sơ nhân viên (cập nhật trạng thái)
        /// </summary>
        void Delete(string code, ProfileStatus status);
    }
}
