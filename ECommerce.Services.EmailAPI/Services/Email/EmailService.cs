using Dapper;
using ECommerce.Services.EmailAPI.Data;
using ECommerce.Services.EmailAPI.DTOs.Cart;
using ECommerce.Services.EmailAPI.DTOs.Shared;
using Newtonsoft.Json;
using System.Data;

namespace ECommerce.Services.EmailAPI.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IApplicationContextDapper _context;
        private ServiceResponse _serviceResponse;

        public EmailService(IApplicationContextDapper context)
        {
            _context = context;
            _serviceResponse = new ServiceResponse();
        }

        public async Task<ServiceResponse> EmailCartAndLog(CartDto cartDto)
        {
            string subject = $"Cart Header id {cartDto.CartHeader.CartHeaderId}, Cart from User id {cartDto.CartHeader.UserId}";
            string cartDetails = JsonConvert.SerializeObject(cartDto);
            string body = $"Cart Details is {cartDetails}";
            var result = await AddEmailLog(subject, body);

            if (result)
            {
                _serviceResponse.Message = "Email log added";
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = "Email log not added";
                return _serviceResponse;
            }
        }

        private async Task<bool> AddEmailLog(string subject, string body)
        {
            var query = "INSERT INTO EmailLogs ([EmailSubject], [EmailBody]) VALUES (@EmailSubject, @EmailBody)";

            var parameters = new DynamicParameters();
            parameters.Add("EmailSubject", subject, DbType.String);
            parameters.Add("EmailBody", body, DbType.String);
            bool result = await _context.ExecuteSqlAsync<bool>(query, parameters);

            return result;
        }
    }
}
