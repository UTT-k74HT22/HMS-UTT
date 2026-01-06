using HospitalManagement.entity;

namespace HospitalManagement.service
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate user by username and password
        /// </summary>
        Account authenticate(string username, string password);
    }
}