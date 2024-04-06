using System.ComponentModel.DataAnnotations;

namespace ECommerce.Services.ProductAPI.DTOs.Product
{
    public class AddProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Product price is required")]
        public required double Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
