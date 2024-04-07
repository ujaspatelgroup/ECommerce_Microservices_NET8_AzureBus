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

        [HttpGet("GetCart/{userId}")]
        public async Task<ServiceResponse> GetCart(string userId)
        {
            return await _shoppingCartService.GetCartAsync(userId);
        }

        [HttpPost("Cartupsert")]
        public async Task<ServiceResponse> CartUpsert(CartDto cartDto)
        {
            return await _shoppingCartService.AddShoppingCartAsync(cartDto);
        }

        [HttpPost("AppyCoupon")]
        public async Task<ServiceResponse> ApplyCoupon(CartDto cartDto)
        {
            return await _shoppingCartService.ApplyCouponAsync(cartDto);
        }

        [HttpPost("EmailCartRequest")]
        public async Task<ServiceResponse> EmailCartRequest(CartDto cartDto)
        {
            return await _shoppingCartService.EmailCartRequestAsync(cartDto);
        }

        [HttpDelete("RemoveCoupon")]
        public async Task<ServiceResponse> RemoveCoupon(string UserId)
        {
            return await _shoppingCartService.RemoveCouponAsync(UserId);
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteCart(int CartDetailsId)
        {
            return await _shoppingCartService.DeleteShoppingCartAsync(CartDetailsId);
        }
    }
}
