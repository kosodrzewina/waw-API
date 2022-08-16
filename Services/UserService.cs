using Microsoft.AspNetCore.Identity;
using WawAPI.DTOs;

namespace WawAPI.Services;

public interface IUserService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
}

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (registerDto.Password != registerDto.RetypedPassword)
        {
            return new AuthResponseDto
            {
                Message = "Typed passwords do not match!",
                IsSuccess = false
            };
        }

        var identityUser = new IdentityUser 
        { 
            Email = registerDto.Email,
            UserName = registerDto.Email
        };
        var result = await _userManager.CreateAsync(identityUser, registerDto.Password);

        if (result.Succeeded)
        {
            return new AuthResponseDto
            {
                Message = "Successful registration",
                IsSuccess = true
            };
        }

        return new AuthResponseDto
        {
            Message = "Registration failed",
            IsSuccess = false,
            Errors = result.Errors.Select(e => e.Description)
        };
    }
}
