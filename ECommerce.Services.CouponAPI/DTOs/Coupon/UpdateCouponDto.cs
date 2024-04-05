using System.ComponentModel.DataAnnotations;

namespace ECommerce.Services.CouponAPI.DTOs.Coupon
{
    public class UpdateCouponDto
    {
        [Required(ErrorMessage = "Coupon id is required")]
        public required int CouponId { get; set; }

        [Required(ErrorMessage = "Coupon code is required")]
        public required string CouponCode { get; set; }

        public double? DiscountAmount { get; set; }

        public int? MinAmount { get; set; }
    }
}