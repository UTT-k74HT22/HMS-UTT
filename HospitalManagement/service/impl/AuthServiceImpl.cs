using HospitalManagement.entity;
using HospitalManagement.repository;
using Microsoft.IdentityModel.Tokens;

namespace HospitalManagement.service.impl
{
    public class AuthServiceImpl : IAuthService
    {
        private static Account? _currentAccount;
        private IAccountRepository repository;
        private static long? _currentUserProfileId;
        
        public AuthServiceImpl(IAccountRepository repository)
        {
            this.repository = repository;
        }
        public static long? GetCurrentUserProfileId()
        {
            //FIND BY user_profile qua account => find tiếp employee (DI repo)
            return _currentUserProfileId;
        }
        
        public static Account? GetCurrentAccount()
        {
            return _currentAccount;
        }
        public Account authenticate(string username, string password)
        {
            Console.WriteLine($"[AUTH] Authenticating user: {username}");
            
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new SecurityTokenException("Username hoặc password không được để trống");
            }
            var account = repository.FindByUsername(username);
            if (account == null)
            {
                throw new Exception("Sai tên đăng nhập hoặc mật khẩu");
            }
            if (account.Password != password)
            {
                throw new Exception("Sai tên đăng nhập hoặc mật khẩu");
            }
            if (!account.IsActive)
            {
                throw new Exception("Tài khoản đã bị vô hiệu hóa");
            }
            try
            {
                repository.UpdateLastLogin(account.Id);
            }
            catch (NotImplementedException)
            {
            }
            _currentAccount = account;
            var userProfileId = repository.FindUserIdByAccountId(account.Id);
            if (userProfileId == null)
            {
                Console.WriteLine("[AUTH] User profile not found → creating new profile");
                userProfileId = repository.CreateUserProfile(
                    accountId: account.Id,
                    username: account.Username,
                    role: account.Role.ToString()
                );
            }
            _currentUserProfileId = userProfileId;
            return account;
        }
    }
}