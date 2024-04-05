namespace ECommerce.Services.AuthAPI.DTOs.Shared
{
    public record class UserSession(string? Id, string? Name, string? Email, string Role);
}
