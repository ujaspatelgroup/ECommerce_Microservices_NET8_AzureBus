using ECommerce.Services.EmailAPI.DTOs.Cart;
using ECommerce.Services.EmailAPI.DTOs.Shared;

namespace ECommerce.Services.EmailAPI.Services.Email
{
    public interface IEmailService
    {
        Task<ServiceResponse> EmailCartAndLog(CartDto cartDto);
    }
}
