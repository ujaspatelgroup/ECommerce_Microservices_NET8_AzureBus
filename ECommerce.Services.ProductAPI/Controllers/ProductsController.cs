using ECommerce.Services.ProductAPI.DTOs.Product;
using ECommerce.Services.ProductAPI.DTOs.Shared;
using ECommerce.Services.ProductAPI.Services.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ServiceResponse> Get()
        {
            return await _productService.GetAllProductsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ServiceResponse> Get(int id)
        {
            return await _productService.GetProductAsync(id);
        }

        [HttpPost]
        public async Task<ServiceResponse> AddProduct(AddProductDto product)
        {
            return await _productService.AddProductAsync(product);
        }

        [HttpPut]
        public async Task<ServiceResponse> UpdateProduct(UpdateProductDto product)
        {
            return await _productService.UpdateProdctAsync(product);
        }

        [HttpDelete]
        public async Task<ServiceResponse> DeleteProduct(int id)
        {
            return await _productService.DeleteProductAsync(id);
        }
    }
}
