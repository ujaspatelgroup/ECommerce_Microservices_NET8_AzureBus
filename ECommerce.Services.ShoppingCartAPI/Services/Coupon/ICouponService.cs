using ECommerce.Services.ShoppingCartAPI.DTOs.Coupon;

namespace ECommerce.Services.ShoppingCartAPI.Services.Coupon
{
    public interface ICouponService
    {
        public Task<GetCouponDto> GetCouponAsync(string couponCode);
    }
}
