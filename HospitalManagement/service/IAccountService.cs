using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;

namespace HospitalManagement.service
{
    /// <summary>
    /// Service interface cho quản lý tài khoản
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Lấy tất cả tài khoản
        /// </summary>
        List<Account> GetAll();

        /// <summary>
        /// Lấy tất cả tài khoản với response DTO
        /// </summary>
        List<AccountResponse> GetAllAccount();

        /// <summary>
        /// Tìm tài khoản theo tên đăng nhập
        /// </summary>
        Account FindByUsername(string username);

        /// <summary>
        /// Tìm tài khoản theo ID
        /// </summary>
        Account FindById(long id);

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        void Update(long accountId, RoleType role, bool active);

        /// <summary>
        /// Xóa tài khoản theo ID
        /// </summary>
        void DeleteById(long id);

        /// <summary>
        /// Kiểm tra tài khoản tồn tại theo tên đăng nhập
        /// </summary>
        bool ExistsByUsername(string username);
    }
}
