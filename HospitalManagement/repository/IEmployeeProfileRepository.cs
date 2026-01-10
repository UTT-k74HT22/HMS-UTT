using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository
{
    /// <summary>
    /// Repository interface cho quản lý hồ sơ nhân viên
    /// </summary>
    public interface IEmployeeProfileRepository
    {
        /// <summary>
        /// Chèn hồ sơ nhân viên mới (không có transaction)
        /// </summary>
        void Insert(SqlConnection conn, long profileId, string position, string department,
                    DateTime hiredDate, decimal baseSalary);
        
        /// <summary>
        /// Chèn hồ sơ nhân viên mới (có transaction)
        /// </summary>
        void Insert(SqlConnection conn, SqlTransaction transaction, long profileId, string position, string department,
                    DateTime hiredDate, decimal baseSalary);

        /// <summary>
        /// Lấy tất cả hồ sơ nhân viên
        /// </summary>
        List<EmployeeProfileResponse> GetAllProfiles();

        /// <summary>
        /// Lấy chi tiết hồ sơ nhân viên theo mã nhân viên
        /// </summary>
        EmployeeProfileDetailResponse GetProfileDetailByCode(string code);

        /// <summary>
        /// Lấy tất cả chi tiết hồ sơ nhân viên
        /// </summary>
        List<EmployeeProfileDetailResponse> GetAllProfileDetails();

        /// <summary>
        /// Cập nhật thông tin hồ sơ nhân viên
        /// </summary>
        void UpdateProfile(string code, UpdateProfileEmployeeRequest request);

        /// <summary>
        /// Cập nhật chi tiết hồ sơ nhân viên theo ID hồ sơ
        /// </summary>
        void UpdateDetailByProfileId(long profileId, UpdateEmployeeProfileDetailRequest request);

        /// <summary>
        /// Cập nhật trạng thái hồ sơ nhân viên
        /// </summary>
        void UpdateStatus(string code, ProfileStatus status);
    }
}
