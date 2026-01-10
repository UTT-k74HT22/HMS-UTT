using HospitalManagement.entity;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository
{
    /// <summary>
    /// Repository interface cho quản lý hồ sơ khách hàng
    /// </summary>
    public interface ICustomerProfileRepository
    {
        /// <summary>
        /// Chèn hồ sơ khách hàng mới (không có transaction)
        /// </summary>
        void Insert(SqlConnection conn, CustomerProfile profile);
        
        /// <summary>
        /// Chèn hồ sơ khách hàng mới (có transaction)
        /// </summary>
        void Insert(SqlConnection conn, SqlTransaction transaction, CustomerProfile profile);

        /// <summary>
        /// Tìm hồ sơ khách hàng theo Profile ID
        /// </summary>
        CustomerProfile? FindByProfileId(long profileId);

        /// <summary>
        /// Cập nhật hồ sơ khách hàng
        /// </summary>
        void Update(SqlConnection conn, CustomerProfile profile);
    }
}
