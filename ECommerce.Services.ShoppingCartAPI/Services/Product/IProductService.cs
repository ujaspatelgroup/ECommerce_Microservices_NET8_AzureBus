using ECommerce.Services.ShoppingCartAPI.DTOs.Product;

namespace ECommerce.Services.ShoppingCartAPI.Services.Product
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();
    }
}
