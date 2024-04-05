using ECommerce.Services.AuthAPI.DTOs.UserAccount;
using ECommerce.Services.AuthAPI.Services.UserAccont;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequestDto addUserRegisterDto)
        {
            var response = await _userService.CreateAccountAsync(addUserRegisterDto);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequestDto userLoginDto)
        {
            var response = await _userService.LoginAccountAsync(userLoginDto);
            return Ok(response);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole(UserRoleRequestDto userRoleRequestDto)
        {
            var response = await _userService.AssignRole(userRoleRequestDto.Email, userRoleRequestDto.Role);
            return Ok(response);
        }
    }
}
