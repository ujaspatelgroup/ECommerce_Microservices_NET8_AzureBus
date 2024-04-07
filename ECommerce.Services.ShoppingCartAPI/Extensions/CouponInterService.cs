namespace ECommerce.Services.ShoppingCartAPI.Extensions
{
    public static class CouponInterService
    {
        public static WebApplicationBuilder CouponInterServiceCall(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"]));
            return builder;
        }
    }
}
