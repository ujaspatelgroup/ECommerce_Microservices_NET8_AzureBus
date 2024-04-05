using ECommerce.Services.ShoppingCartAPI.DTOs.Cart;
using ECommerce.Services.ShoppingCartAPI.DTOs.Product;

namespace ECommerce.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
