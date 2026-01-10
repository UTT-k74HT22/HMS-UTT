using HospitalManagement.entity;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository
{
    /// <summary>
    /// Repository interface cho quản lý hồ sơ người dùng
    /// </summary>
    public interface IUserProfileRepository
    {
        /// <summary>
        /// Chèn hồ sơ người dùng mới (không có transaction)
        /// </summary>
        long Insert(SqlConnection conn, UserProfile profile);
        
        /// <summary>
        /// Chèn hồ sơ người dùng mới (có transaction)
        /// </summary>
        long Insert(SqlConnection conn, SqlTransaction transaction, UserProfile profile);

        /// <summary>
        /// Tìm hồ sơ theo Account ID
        /// </summary>
        UserProfile? FindByAccountId(long accountId);

        /// <summary>
        /// Tìm hồ sơ theo ID
        /// </summary>
        UserProfile? FindById(long id);

        /// <summary>
        /// Cập nhật hồ sơ người dùng
        /// </summary>
        void Update(SqlConnection conn, UserProfile profile);

        /// <summary>
        /// Kiểm tra email đã tồn tại
        /// </summary>
        bool ExistsByEmail(string email);

        /// <summary>
        /// Kiểm tra phone đã tồn tại
        /// </summary>
        bool ExistsByPhone(string phone);

        /// <summary>
        /// Generate mã code tự động
        /// </summary>
        string GenerateCode(string prefix);
    }
}
