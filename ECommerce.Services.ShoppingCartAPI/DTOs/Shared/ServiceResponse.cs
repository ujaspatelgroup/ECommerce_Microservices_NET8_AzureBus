namespace ECommerce.Services.ShoppingCartAPI.DTOs.Shared
{
    public class ServiceResponse
    {
        public object? data { get; set; }

        public bool Success { get; set; } = true;

        public string? Message { get; set; }
    }
}
