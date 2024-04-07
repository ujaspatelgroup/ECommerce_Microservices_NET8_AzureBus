using AutoMapper;
using Dapper;
using ECommerce.ServiceBus;
using ECommerce.Services.ShoppingCartAPI.Data;
using ECommerce.Services.ShoppingCartAPI.DTOs.Cart;
using ECommerce.Services.ShoppingCartAPI.DTOs.Product;
using ECommerce.Services.ShoppingCartAPI.DTOs.Shared;
using ECommerce.Services.ShoppingCartAPI.Models;
using ECommerce.Services.ShoppingCartAPI.Services.Coupon;
using ECommerce.Services.ShoppingCartAPI.Services.Product;
using System.Collections.Generic;
using System.Data;

namespace ECommerce.Services.ShoppingCartAPI.Services.ShoppingCart
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IApplicationContextDapper _context;
        private ServiceResponse _serviceResponse;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;

        public ShoppingCartService(IApplicationContextDapper context, IMapper mapper, IProductService productService, ICouponService couponService, IMessageBus messageBus, IConfiguration configuration)
        {
            _context = context;
            _serviceResponse = new ServiceResponse();
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
            _messageBus = messageBus;
            _configuration = configuration;
        }

        public async Task<ServiceResponse> GetCartAsync(string UserId)
        {
            var cartHeader = await GetCartHeader(UserId);
            if (cartHeader is not null)
            {
                var cartDetails = await GetCartDetails(cartHeader.CartHeaderId);

                CartDto cart = new CartDto
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(cartHeader),
                    CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(cartDetails)
                };

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCouponAsync(cart.CartHeader.CouponCode);
                    if (coupon is not null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount > 0 ? (double)coupon.DiscountAmount : 0;

                        cart.CartHeader.Discount = coupon.DiscountAmount > 0 ? (double)coupon.DiscountAmount : 0;
                    }
                }

                _serviceResponse.data = cart;
            }
            else
            {
                _serviceResponse.Message = "No product in Cart";
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse> AddShoppingCartAsync(CartDto cartDto)
        {
            var cartHeader = await GetCartHeader(cartDto.CartHeader.UserId);

            if (cartHeader is null)
            {
                int cartHeaderId = await AddCartHeader(cartDto);
                AddCartDetails(cartDto, cartHeaderId);
            }
            else
            {
                var cartDetails = await GetCartDetails(cartDto.CartDetails.FirstOrDefault().ProductId, cartHeader.CartHeaderId);
                if (cartDetails is null)
                {
                    AddCartDetails(cartDto, cartHeader.CartHeaderId);
                }
                else
                {
                    cartDetails.Count += cartDto.CartDetails.FirstOrDefault().Count;
                    UpdateCartDetails(cartDetails.CartDetailsId, cartDetails.Count);
                }
            }
            _serviceResponse.data = "Cart Inserted/Updated";
            return _serviceResponse;
        }

        public async Task<ServiceResponse> DeleteShoppingCartAsync(int CartDetailsId)
        {
            bool result = await DeleteCart(CartDetailsId);
            if (!result)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Something went wrong";
                return _serviceResponse;
            }
            _serviceResponse.Message = "Cart Deleted";
            return _serviceResponse;
        }

        public async Task<ServiceResponse> ApplyCouponAsync(CartDto cartDto)
        {
            var cart = await UpdateCoupon(cartDto);
            if (cart)
            {
                _serviceResponse.Message = "Coupon Updated";
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Something went wrong";
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse> RemoveCouponAsync(string UserId)
        {
            var cart = await DeleteCoupon(UserId);
            if (cart)
            {
                _serviceResponse.Message = "Coupon Deleted";
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Something went wrong";
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse> EmailCartRequestAsync(CartDto cartDto)
        {
            try
            {
                _messageBus.SetConnetionString(_configuration.GetConnectionString("AzureBusConnection"));
                await _messageBus.PublishMessage(cartDto, _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));
                _serviceResponse.Message = "Topic/Queue Added";
                return _serviceResponse;
            }
            catch (Exception ex)
            {
                _serviceResponse.Message = ex.Message;
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        private async Task<CartHeader> GetCartHeader(string UserId)
        {
            var query = "Select [CartHeaderId], [UserId], [CouponCode] From CartHeader Where [UserId] = @UserId";

            var parameters = new DynamicParameters();
            parameters.Add("UserId", UserId, DbType.String);

            var cartHeader = await _context.GetDataSingleAsync<CartHeader>(query, parameters);

            return cartHeader;
        }

        private async Task<CartDetails> GetCartDetails(int ProductId, int CartHeaderId)
        {
            var query = "Select [CartDetailsId], [CartHeaderId], [ProductId], [Count] From CartDetails Where [ProductId] = @ProductId And [CartHeaderId] = @CartHeaderId";

            var parameters = new DynamicParameters();
            parameters.Add("ProductId", ProductId, DbType.Int64);
            parameters.Add("CartHeaderId", CartHeaderId, DbType.Int64);
            var cartDetail = await _context.GetDataSingleAsync<CartDetails>(query, parameters);

            return cartDetail;
        }

        private async Task<IEnumerable<CartDetails>> GetCartDetails(int CartHeaderId)
        {
            var query = "Select [CartDetailsId], [CartHeaderId], [ProductId], [Count] From CartDetails Where [CartHeaderId] = @CartHeaderId";

            var parameters = new DynamicParameters();
            parameters.Add("CartHeaderId", CartHeaderId, DbType.Int64);
            var cartDetail = await _context.GetDataAsync<CartDetails>(query, parameters);
            return cartDetail;
        }

        private async Task<int> AddCartHeader(CartDto cartDto)
        {
            var query = "Insert Into CartHeader([UserId], [CouponCode]) OUTPUT inserted.CartHeaderId Values(@UserId, @CouponCode)";

            var parameters = new DynamicParameters();
            parameters.Add("UserId", cartDto.CartHeader.UserId, DbType.String);
            parameters.Add("CouponCode", cartDto.CartHeader.CouponCode, DbType.String);
            int Key = await _context.GetDataSingleAsync<int>(query, parameters);
            return Key;
        }

        private async void AddCartDetails(CartDto cartDto, int CartHeaderId)
        {
            var query = "Insert Into CartDetails([ProductId], [Count], [CartHeaderId]) Values(@ProductId, @Count, @CartHeaderId)";

            var parameters = new DynamicParameters();
            parameters.Add("ProductId", cartDto.CartDetails.FirstOrDefault().ProductId, DbType.Int64);
            parameters.Add("Count", cartDto.CartDetails.FirstOrDefault().Count, DbType.Int64);
            parameters.Add("CartHeaderId", CartHeaderId, DbType.Int64);
            await _context.ExecuteSqlAsync<bool>(query, parameters);
        }

        private async void UpdateCartDetails(int CartDetailsId, int Count)
        {
            var query = "UPDATE CartDetails SET [Count] = @Count WHERE [CartDetailsId] = @CartDetailsId";

            var parameters = new DynamicParameters();
            parameters.Add("CartDetailsId", CartDetailsId, DbType.Int64);
            parameters.Add("Count", Count, DbType.Int64);

            await _context.ExecuteSqlAsync<bool>(query, parameters);
        }

        private async Task<bool> UpdateCoupon(CartDto cartDto)
        {
            var query = "UPDATE CartHeader SET [CouponCode] = @CouponCode WHERE [UserId] = @UserId";

            var parameters = new DynamicParameters();
            parameters.Add("CouponCode", cartDto.CartHeader.CouponCode, DbType.String);
            parameters.Add("UserId", cartDto.CartHeader.UserId, DbType.String);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            return result;
        }

        private async Task<bool> DeleteCoupon(string UserId)
        {
            var query = "UPDATE CartHeader SET [CouponCode] = null WHERE [UserId] = @UserId";

            var parameters = new DynamicParameters();
            parameters.Add("UserId", UserId, DbType.String);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            return result;
        }

        private async Task<bool> DeleteCart(int CartDetailsId)
        {
            var query = "Select [CartDetailsId], [CartHeaderId], [ProductId], [Count] From CartDetails Where [CartDetailsId] = @CartDetailsId";

            var parameters = new DynamicParameters();
            parameters.Add("CartDetailsId", CartDetailsId, DbType.Int64);
            var cartDetail = await _context.GetDataSingleAsync<CartDetails>(query, parameters);

            bool isDetailsDelete = await DeleteCartDetails(cartDetail.CartDetailsId);
            bool isHeaderDelete = true;
            if (cartDetail.Count == 1)
            {
                isHeaderDelete = await DeleteCartHeader(cartDetail.CartHeaderId);
            }

            if (isDetailsDelete && isHeaderDelete)
            {
                return true;
            }
            return false;
        }

        private async Task<bool> DeleteCartHeader(int CartHeaderId)
        {
            var query = "DELETE FROM CartHeader WHERE [CartHeaderId] = @CartHeaderId";

            var parameters = new DynamicParameters();
            parameters.Add("CartHeaderId", CartHeaderId, DbType.Int64);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            return result;
        }

        private async Task<bool> DeleteCartDetails(int CartDetailsId)
        {
            var query = "DELETE FROM CartDetails WHERE [CartDetailsId] = @CartDetailsId";

            var parameters = new DynamicParameters();
            parameters.Add("CartDetailsId", CartDetailsId, DbType.Int64);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            return result;
        }
    }
}
