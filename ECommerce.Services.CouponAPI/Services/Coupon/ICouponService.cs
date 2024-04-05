using ECommerce.Services.CouponAPI.DTOs.Coupon;
using ECommerce.Services.CouponAPI.DTOs.Shared;

namespace ECommerce.Services.CouponAPI.Services.Coupon
{
    public interface ICouponService
    {
        public Task<ServiceResponse> GetAllCouponsAsync();

        public Task<ServiceResponse> GetCouponAsync(int id);

        public Task<ServiceResponse> AddCouponAsync(AddCouponDto coupon);

        public Task<ServiceResponse> UpdateCouponAsync(UpdateCouponDto coupon);

        public Task<ServiceResponse> DeleteCouponAsync(int id);
    }
}
