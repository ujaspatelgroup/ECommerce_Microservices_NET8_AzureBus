using System.ComponentModel.DataAnnotations;

namespace ECommerce.Services.AuthAPI.DTOs.UserAccount
{
    public class UserLoginRequestDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
