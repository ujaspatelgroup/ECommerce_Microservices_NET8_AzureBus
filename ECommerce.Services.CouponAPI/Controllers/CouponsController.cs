using ECommerce.Services.CouponAPI.DTOs.Coupon;
using ECommerce.Services.CouponAPI.DTOs.Shared;
using ECommerce.Services.CouponAPI.Services.Coupon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;
        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        public async Task<ServiceResponse> Get()
        {
            return await _couponService.GetAllCouponsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ServiceResponse> Get(int id)
        {
            return await _couponService.GetCouponAsync(id);
        }

        [HttpPost]
        public async Task<ServiceResponse> AddCoupon(AddCouponDto coupon)
        {
            return await _couponService.AddCouponAsync(coupon);
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateCoupon(UpdateCouponDto coupon)
        {
            return await _couponService.UpdateCouponAsync(coupon);
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteCoupon(int id)
        {
            return await _couponService.DeleteCouponAsync(id);
        }
    }
}
