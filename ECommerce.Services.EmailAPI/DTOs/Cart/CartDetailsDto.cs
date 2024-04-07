using ECommerce.Services.EmailAPI.DTOs.Product;

namespace ECommerce.Services.EmailAPI.DTOs.Cart
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        //public int CartHeaderId { get; set; }
        //public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
    }
}
