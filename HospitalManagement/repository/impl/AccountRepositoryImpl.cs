using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl
{
    public class AccountRepositoryImpl : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Account> FindAll()
        {
            throw new NotImplementedException();
        }

        public Account FindByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Account FindById(long id)
        {
            throw new NotImplementedException();
        }

        public long Insert(SqlConnection conn, Account account)
        {
            throw new NotImplementedException();
        }

        public void UpdateRoleAndStatus(long id, RoleType role, bool active)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(long id)
        {
            throw new NotImplementedException();
        }

        public bool ExistsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public bool ExistsByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public void UpdatePassword(long accountId, string hashedPassword)
        {
            throw new NotImplementedException();
        }

        public void UpdateLastLogin(long accountId)
        {
            throw new NotImplementedException();
        }

        public long? FindUserIdByAccountId(long accountId)
        {
            throw new NotImplementedException();
        }
        
        
        private Account Map(SqlDataReader rs)
        {
            return new Account
            {
                Id = rs.GetInt32(rs.GetOrdinal("id")),
                Username = rs.GetString(rs.GetOrdinal("username")),
                Password = rs.GetString(rs.GetOrdinal("password")),
                Role = rs.GetString(rs.GetOrdinal("role")),
                IsActive = rs.GetBoolean(rs.GetOrdinal("is_active"))
            };
        }
    }
}
