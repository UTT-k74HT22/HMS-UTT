using HospitalManagement.entity.dto;

namespace HospitalManagement.service
{
    public interface IAuthService
    {
        /**
         * Method to handle user login.
         *
         * @param request The login request containing username and password.
         */
        bool login(LoginRequest request);

        /**
         * Method to handle user registration.
         *
         * @param request The registration request containing user details.
         */
        void register(RegisterRequest request);
    }
}