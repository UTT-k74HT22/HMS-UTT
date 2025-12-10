using HospitalManagement.entity.dto;
using HospitalManagement.repository;
using HospitalManagement.entity;

namespace HospitalManagement.service.impl
{
    public class AuthServiceImpl : IAuthService
    {

        private readonly IAccountRepository _accountRepository;

        public AuthServiceImpl(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /**
         * Method to handle user login.
         *
         * @param request The login request containing username and password.
         */
        public bool login(LoginRequest request)
        {
            Console.WriteLine($"Login method call with username = {request.Username}");
            validAccount(request.Username, request.Password);

            var user = _accountRepository.FindByUsername(request.Username);

            if (user == null || user.Password != request.Password)
            {
                throw new Exception("Invalid username or password");
            }

            if (!user.IsActive)
            {
                throw new Exception("Account is inactive");
            }

            Console.WriteLine("User logged in successfully");
            return true;
        }

        /**
         * Method to handle user registration.
         *
         * @param request The registration request containing user details.
         */
        public void register(RegisterRequest request)
        {
            Console.WriteLine($"Register username = {request.Username}");

            if (_accountRepository.FindByUsername(request.Username) != null)
            {
                throw new Exception("Username already exists");
            }

            validAccount(request.Username, request.Password);

            if (string.IsNullOrWhiteSpace(request.Fullname))
            {
                throw new Exception("Fullname cannot be empty");
            }

            var user = new Account
            {
                Username = request.Username,
                Password = request.Password,
                Fullname = request.Fullname,
                Role = "ADMIN",
                IsActive = true
            };

            _accountRepository.Save(user);
            Console.WriteLine("User registered successfully");
        }

        private void validAccount(String username, String password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Username cannot be null or empty");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Password cannot be null or empty");
            }
        }
    }
}