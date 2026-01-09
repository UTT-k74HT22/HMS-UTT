using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository
{
    /// <summary>
    /// Repository interface cho quản lý tài khoản
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Lấy tất cả tài khoản
        /// </summary>
        List<Account> FindAll();

        /// <summary>
        /// Tìm tài khoản theo tên đăng nhập
        /// </summary>
        Account FindByUsername(string username);

        /// <summary>
        /// Tìm tài khoản theo ID
        /// </summary>
        Account FindById(long id);

        /// <summary>
        /// Chèn tài khoản mới và trả về ID đã chèn
        /// </summary>
        long Insert(SqlConnection conn, Account account);

        /// <summary>
        /// Cập nhật vai trò và trạng thái kích hoạt của tài khoản
        /// </summary>
        void UpdateRoleAndStatus(long id, RoleType role, bool active);

        /// <summary>
        /// Xóa tài khoản theo ID
        /// </summary>
        void DeleteById(long id);

        /// <summary>
        /// Kiểm tra tài khoản tồn tại theo tên đăng nhập
        /// </summary>
        bool ExistsByUsername(string username);

        /// <summary>
        /// Kiểm tra tài khoản tồn tại theo email
        /// </summary>
        bool ExistsByEmail(string email);

        /// <summary>
        /// Cập nhật mật khẩu
        /// </summary>
        void UpdatePassword(long accountId, string hashedPassword);

        /// <summary>
        /// Cập nhật thời gian đăng nhập cuối
        /// </summary>
        void UpdateLastLogin(long accountId);

        /// <summary>
        /// Tìm User ID theo Account ID
        /// </summary>
        long? FindUserIdByAccountId(long accountId);
    }
}
