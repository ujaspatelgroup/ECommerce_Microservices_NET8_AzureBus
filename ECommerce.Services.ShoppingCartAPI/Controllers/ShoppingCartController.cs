using ECommerce.Services.ShoppingCartAPI.DTOs.Cart;
using ECommerce.Services.ShoppingCartAPI.DTOs.Shared;
using ECommerce.Services.ShoppingCartAPI.Services.ShoppingCart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost("Cartupsert")]
        public async Task<ServiceResponse> CartUpsert(CartDto cartDto)
        {
            return await _shoppingCartService.AddShoppingCartAsync(cartDto);
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteCart(int id)
        {
            return await _shoppingCartService.DeleteShoppingCartAsync(id);
        }
    }
}
