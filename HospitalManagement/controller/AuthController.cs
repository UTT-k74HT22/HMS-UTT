using HospitalManagement.entity.dto;
using HospitalManagement.service;

namespace HospitalManagement.controller
{
    public class AuthController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public bool Login(LoginRequest req)
        {
            return _authService.login(req);
        }

        public void Register(RegisterRequest req)
        {
            _authService.register(req);
        }
    }
}