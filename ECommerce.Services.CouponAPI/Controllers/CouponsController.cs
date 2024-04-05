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
        public async Task<ServiceResponse> AddStudent(AddCouponDto student)
        {
            return await _couponService.AddCouponAsync(student);
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateStudent(UpdateCouponDto student)
        {
            return await _couponService.UpdateCouponAsync(student);
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteStudent(int id)
        {
            return await _couponService.DeleteCouponAsync(id);
        }
    }
}
