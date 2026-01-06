using HospitalManagement.entity;
using HospitalManagement.repository;
using Microsoft.IdentityModel.Tokens;

namespace HospitalManagement.service.impl
{
    public class AuthServiceImpl : IAuthService
    {
        
        private IAccountRepository repository;
        
        public AuthServiceImpl(IAccountRepository repository)
        {
            this.repository = repository;
        }
        
        public Account authenticate(string username, string password)
        {
            Console.WriteLine("Authenticating user: " + username);
            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                throw new SecurityTokenException("Username or password is required");
            }
            var account = repository.FindByUsername(username);
            if (account == null)
            {
                throw new Exception("Invalid username or password");
            }
            
            if (account.Password != password)
            {
                throw new Exception("Invalid username or password");
            }
            
            if (account.IsActive.Equals(false))
            {
                throw new Exception("Account is inactive");
            }
            return account;
        }
    }
}