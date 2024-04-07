using ECommerce.Services.ShoppingCartAPI.DTOs.Product;
using ECommerce.Services.ShoppingCartAPI.DTOs.Shared;
using Newtonsoft.Json;

namespace ECommerce.Services.ShoppingCartAPI.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/products");
            var apiContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ServiceResponse>(apiContent);
            if (result.Success)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(result.data));
            }
            return new List<ProductDto>();
        }
    }
}
