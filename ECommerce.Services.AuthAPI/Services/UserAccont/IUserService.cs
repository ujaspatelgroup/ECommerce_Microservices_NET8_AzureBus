using ECommerce.Services.AuthAPI.DTOs.Shared;
using ECommerce.Services.AuthAPI.DTOs.UserAccount;

namespace ECommerce.Services.AuthAPI.Services.UserAccont
{
    public interface IUserService
    {
        public Task<ServiceResponse> CreateAccountAsync(UserRegisterRequestDto registerDto);

        public Task<UserLoginResponseDto> LoginAccountAsync(UserLoginRequestDto loginDto);

        public Task<ServiceResponse> AssignRole(string email, string roleName);
    }
}
