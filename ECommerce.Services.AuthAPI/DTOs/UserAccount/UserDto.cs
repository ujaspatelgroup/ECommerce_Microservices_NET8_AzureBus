using System.ComponentModel.DataAnnotations;

namespace ECommerce.Services.AuthAPI.DTOs.UserAccount
{
    public class UserDto
    {
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
