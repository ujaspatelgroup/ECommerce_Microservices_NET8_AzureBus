using ECommerce.Services.ShoppingCartAPI.DTOs.Cart;
using ECommerce.Services.ShoppingCartAPI.DTOs.Shared;

namespace ECommerce.Services.ShoppingCartAPI.Services.ShoppingCart
{
    public interface IShoppingCartService
    {
        public Task<ServiceResponse> AddShoppingCartAsync(CartDto cartDto);

        public Task<ServiceResponse> DeleteShoppingCartAsync(int CartDetailsId);
    }
}
