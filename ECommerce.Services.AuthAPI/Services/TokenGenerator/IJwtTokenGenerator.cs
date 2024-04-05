using ECommerce.Services.AuthAPI.DTOs.Shared;
using ECommerce.Services.AuthAPI.DTOs.UserAccount;

namespace ECommerce.Services.AuthAPI.Services.TokenGenerator
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(UserSession user);
    }
}
