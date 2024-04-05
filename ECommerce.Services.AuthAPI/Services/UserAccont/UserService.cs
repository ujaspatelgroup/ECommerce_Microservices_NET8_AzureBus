using ECommerce.Services.AuthAPI.DTOs.Shared;
using ECommerce.Services.AuthAPI.DTOs.UserAccount;
using ECommerce.Services.AuthAPI.Models;
using ECommerce.Services.AuthAPI.Services.TokenGenerator;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Services.AuthAPI.Services.UserAccont
{
    public class UserService : IUserService
    {
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        IJwtTokenGenerator _jwtTokenGenerator;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ServiceResponse> AssignRole(string email, string roleName)
        {
            var _serviceResponse = new ServiceResponse();
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                var role = await _userManager.AddToRoleAsync(user, roleName);
                if (role.Succeeded)
                {
                    _serviceResponse.Message = "Role Added";
                    return _serviceResponse;
                }
            }
            _serviceResponse.Message = "User not found";
            _serviceResponse.Success = false;
            return _serviceResponse;
        }

        public async Task<ServiceResponse> CreateAccountAsync(UserRegisterRequestDto registerDto)
        {
            var _serviceResponse = new ServiceResponse();
            var newUser = new ApplicationUser()
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = registerDto.Password,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var user = await _userManager.FindByEmailAsync(newUser.Email);
            if (user is not null)
            {
                _serviceResponse.Message = "User registered already";
                _serviceResponse.Success = false;
                return _serviceResponse;
            }

            var createUser = await _userManager.CreateAsync(newUser!, registerDto.Password);

            if (!createUser.Succeeded)
            {
                _serviceResponse.Message = "Something went wrong. please try again";
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Message = "Account Created";
                return _serviceResponse;
            }
        }

        public async Task<UserLoginResponseDto> LoginAccountAsync(UserLoginRequestDto loginDto)
        {
            var _userLoginResponse = new UserLoginResponseDto();

            var getUser = await _userManager.FindByEmailAsync(loginDto.UserName);
            if (getUser is null)
            {
                _userLoginResponse.serviceResponse.Message = "User not found";
                _userLoginResponse.serviceResponse.Success = false;
                return _userLoginResponse;
            }

            bool checkUserPasswords = await _userManager.CheckPasswordAsync(getUser, loginDto.Password);
            if (!checkUserPasswords)
            {
                _userLoginResponse.serviceResponse.Message = "Invalid email/password";
                _userLoginResponse.serviceResponse.Success = false;
                return _userLoginResponse;
            }

            var getUserRole = await _userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());

            _userLoginResponse.Token = _jwtTokenGenerator.GenerateToken(userSession);
            UserDto userDto = new UserDto { UserId = getUser.Id, Name = getUser.Name, Email = getUser.Email, PhoneNumber = getUser.PhoneNumber };
            ServiceResponse serviceResponse = new ServiceResponse { data = userDto, Message = "Login completed" };
            _userLoginResponse.serviceResponse = serviceResponse;
            return _userLoginResponse;
        }
    }
}
