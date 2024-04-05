namespace ECommerce.Services.CouponAPI.DTOs.Coupon
{
    public class GetCouponDto
    {
        public int CouponId { get; set; }

        public required string CouponCode { get; set; }

        public double? DiscountAmount { get; set; }

        public int? MinAmount { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}