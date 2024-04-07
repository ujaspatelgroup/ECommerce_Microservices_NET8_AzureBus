using ECommerce.Services.ShoppingCartAPI.DTOs.Coupon;
using ECommerce.Services.ShoppingCartAPI.DTOs.Shared;
using Newtonsoft.Json;

namespace ECommerce.Services.ShoppingCartAPI.Services.Coupon
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }

        public async Task<GetCouponDto> GetCouponAsync(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/Coupons/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            if (!String.IsNullOrEmpty(apiContent))
            {
                var result = JsonConvert.DeserializeObject<ServiceResponse>(apiContent);
                if (result.Success)
                {
                    return JsonConvert.DeserializeObject<GetCouponDto>(Convert.ToString(result.data));
                }
            }
            
            return default;
        }

    }
}
