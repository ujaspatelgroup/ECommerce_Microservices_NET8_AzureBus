namespace ECommerce.Services.ShoppingCartAPI.Extensions
{
    public static class ProductInterService
    {
        public static WebApplicationBuilder ProductInterServiceCall(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));
            return builder;
        }

    }
}
