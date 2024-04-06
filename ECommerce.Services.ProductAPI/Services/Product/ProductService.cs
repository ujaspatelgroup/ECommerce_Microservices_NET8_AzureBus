using Dapper;
using ECommerce.Services.ProductAPI.Data;
using ECommerce.Services.ProductAPI.DTOs.Product;
using ECommerce.Services.ProductAPI.DTOs.Shared;
using System.Data;

namespace ECommerce.Services.ProductAPI.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IApplicationContextDapper _context;
        private ServiceResponse _serviceResponse;

        public ProductService(IApplicationContextDapper context)
        {
            _context = context;
            _serviceResponse = new ServiceResponse();
        }
        public async Task<ServiceResponse> AddProductAsync(AddProductDto _product)
        {
            var query = "INSERT INTO Products ([Name], [Price], [Description], [CategoryName], [ImageUrl]) VALUES (@Name, @Price, @Description, @CategoryName, @ImageUrl)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", _product.Name, DbType.String);
            parameters.Add("Price", _product.Price, DbType.Double);
            parameters.Add("Description", _product.Description, DbType.String);
            parameters.Add("CategoryName", _product.CategoryName, DbType.String);
            parameters.Add("ImageUrl", _product.ImageUrl, DbType.String);
            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);

            if (result)
            {
                _serviceResponse.Message = "Product added";
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Product not added";
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse> DeleteProductAsync(int id)
        {
            var findproduct = await findProduct(id);
            if (findproduct is null)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Product not found";
                return _serviceResponse;
            }

            var query = "DELETE FROM Products WHERE [ProductId] = @ProductId";

            var parameters = new DynamicParameters();
            parameters.Add("ProductId", id, DbType.Int64);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            if (result)
            {
                _serviceResponse.Message = "Product deleted";
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Product not deleted";
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse> GetAllProductsAsync()
        {
            var query = "Select [ProductId], [Name], [Price], [Description], [CategoryName], [ImageUrl] From Products";
            var products = await _context.GetDataAsync<GetProductDto>(query);
            _serviceResponse.data = products.ToList();
            return _serviceResponse;
        }

        public async Task<ServiceResponse> GetProductAsync(int id)
        {
            var findproduct = await findProduct(id);
            if (findproduct is not null)
            {
                _serviceResponse.data = findproduct;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Product not found";
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse> UpdateProdctAsync(UpdateProductDto _product)
        {
            var findproduct = await findProduct(_product.ProductId);
            if (findproduct is null)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Product not found";
                return _serviceResponse;
            }

            var query = "UPDATE Products SET [Name] = @Name, [Price] = @Price, [Description] = @Description, [CategoryName] = @CategoryName, [ImageUrl] = @ImageUrl WHERE [ProductId] = @ProductId";

            var parameters = new DynamicParameters();
            parameters.Add("ProductId", _product.ProductId, DbType.Int64);
            parameters.Add("Name", _product.Name, DbType.String);
            parameters.Add("Price", _product.Price, DbType.Double);
            parameters.Add("Description", _product.Description, DbType.String);
            parameters.Add("CategoryName", _product.Description, DbType.String);
            parameters.Add("ImageUrl", _product.ImageUrl, DbType.String);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            if (result)
            {
                var productResult = await findProduct(_product.ProductId);
                _serviceResponse.data = productResult;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Product not updated";
                return _serviceResponse;
            }
        }

        private async Task<GetProductDto> findProduct(int id)
        {
            var query = "Select [ProductId], [Name], [Price], [Description], [CategoryName], [ImageUrl] From Products Where [ProductId] = @ProductId";

            var parameters = new DynamicParameters();
            parameters.Add("ProductId", id, DbType.Int64);

            var product = await _context.GetDataSingleAsync<GetProductDto>(query, parameters);
            return product;
        }
    }
}
