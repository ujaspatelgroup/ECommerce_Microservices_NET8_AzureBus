using System.ComponentModel.DataAnnotations;

namespace ECommerce.Services.CouponAPI.DTOs.Coupon
{
    public class AddCouponDto
    {
        [Required(ErrorMessage = "Coupon code is required")]
        public required string CouponCode { get; set; }

        public double? DiscountAmount { get; set; }

        public int? MinAmount { get; set; }
    }
}
