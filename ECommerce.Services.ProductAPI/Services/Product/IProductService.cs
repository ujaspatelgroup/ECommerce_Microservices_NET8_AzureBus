using ECommerce.Services.ProductAPI.DTOs.Product;
using ECommerce.Services.ProductAPI.DTOs.Shared;

namespace ECommerce.Services.ProductAPI.Services.Product
{
    public interface IProductService
    {
        public Task<ServiceResponse> GetAllProductsAsync();

        public Task<ServiceResponse> GetProductAsync(int id);

        public Task<ServiceResponse> AddProductAsync(AddProductDto _product);

        public Task<ServiceResponse> UpdateProdctAsync(UpdateProductDto _product);

        public Task<ServiceResponse> DeleteProductAsync(int id);
    }
}
