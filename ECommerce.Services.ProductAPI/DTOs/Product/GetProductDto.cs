using System.ComponentModel.DataAnnotations;

namespace ECommerce.Services.ProductAPI.DTOs.Product
{
    public class GetProductDto
    {
       
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
