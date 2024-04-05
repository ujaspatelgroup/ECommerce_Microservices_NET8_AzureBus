using ECommerce.Services.AuthAPI.DTOs.Shared;

namespace ECommerce.Services.AuthAPI.DTOs.UserAccount
{
    public class UserLoginResponseDto
    {
        public ServiceResponse serviceResponse { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
