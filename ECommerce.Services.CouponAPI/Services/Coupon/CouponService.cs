using Dapper;
using ECommerce.Services.CouponAPI.Data;
using ECommerce.Services.CouponAPI.DTOs.Coupon;
using ECommerce.Services.CouponAPI.DTOs.Shared;
using System.Data;

namespace ECommerce.Services.CouponAPI.Services.Coupon
{
    public class CouponService : ICouponService
    {
        private readonly IApplicationContextDapper _context;
        private ServiceResponse _serviceResponse;

        public CouponService(IApplicationContextDapper context)
        {
            _context = context;
            _serviceResponse = new ServiceResponse();
        }

        public async Task<ServiceResponse> GetAllCouponsAsync()
        {
            var query = "Select [CouponId], [CouponCode], [DiscountAmount], [MinAmount] From Coupons";
            var coupons = await _context.GetDataAsync<GetCouponDto>(query);
            _serviceResponse.data = coupons.ToList();
            return _serviceResponse;
        }

        public async Task<ServiceResponse> GetCouponAsync(int id)
        {
            var findcoupon = await findCoupon(id);
            if (findcoupon is not null)
            {
                _serviceResponse.data = findcoupon;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Coupon not found";
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse> AddCouponAsync(AddCouponDto _coupon)
        {
            var query = "INSERT INTO Coupons ([CouponCode], [DiscountAmount], [MinAmount]) VALUES (@CouponCode, @DiscountAmount, @MinAmount)";

            var parameters = new DynamicParameters();
            parameters.Add("CouponCode", _coupon.CouponCode, DbType.String);
            parameters.Add("DiscountAmount", _coupon.DiscountAmount, DbType.Double);
            parameters.Add("MinAmount", _coupon.MinAmount, DbType.Int64);
            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);

            if (result)
            {
                var coupons = await GetAllCouponsAsync();
                _serviceResponse.data = coupons.data;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Coupon not added";
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse> UpdateCouponAsync(UpdateCouponDto _coupon)
        {
            var findcoupon = await findCoupon(_coupon.CouponId);
            if (findcoupon is null)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Coupon not found";
                return _serviceResponse;
            }

            var query = "UPDATE Coupons SET [CouponCode] = @CouponCode, [DiscountAmount] = @DiscountAmount, [MinAmount] = @MinAmount WHERE [CouponId] = @CouponId";

            var parameters = new DynamicParameters();
            parameters.Add("CouponId", _coupon.CouponId, DbType.Int64);
            parameters.Add("CouponCode", _coupon.CouponCode, DbType.String);
            parameters.Add("DiscountAmount", _coupon.DiscountAmount, DbType.Double);
            parameters.Add("MinAmount", _coupon.MinAmount, DbType.Int64);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            if (result)
            {
                var couponResult = await findCoupon(_coupon.CouponId);
                _serviceResponse.data = couponResult;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Coupon not updated";
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse> DeleteCouponAsync(int id)
        {
            var findcoupon = await findCoupon(id);
            if (findcoupon is null)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Coupon not found";
                return _serviceResponse;
            }

            var query = "DELETE FROM Coupons WHERE [CouponId] = @CouponId";

            var parameters = new DynamicParameters();
            parameters.Add("CouponId", id, DbType.Int64);

            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);
            if (result)
            {
                var coupons = await GetAllCouponsAsync();
                _serviceResponse.data = coupons.data;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Coupon not deleted";
                return _serviceResponse;
            }
        }

        private async Task<GetCouponDto> findCoupon(int id)
        {
            var query = "Select [CouponId], [CouponCode], [DiscountAmount], [MinAmount] From Coupons Where [CouponId] = @CouponId";

            var parameters = new DynamicParameters();
            parameters.Add("CouponId", id, DbType.Int64);

            var coupon = await _context.GetDataSingleAsync<GetCouponDto>(query, parameters);
            return coupon;
        }
    }
}
