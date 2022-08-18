using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WawAPI.DTOs;

namespace WawAPI.Services;

public interface IUserService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LogInAsync(LogInDto logInDto);
}

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
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

        var user = new IdentityUser 
        { 
            Email = registerDto.Email,
            UserName = registerDto.Email
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);

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

    public async Task<AuthResponseDto> LogInAsync(LogInDto logInDto)
    {
        var user = await _userManager.FindByEmailAsync(logInDto.Email);

        if (user is null)
        {
            return new AuthResponseDto
            {
                Message = "User related to that e-mail address does not exist",
                IsSuccess = false
            };
        }

        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, logInDto.Password);

        if (!checkPasswordResult)
        {
            return new AuthResponseDto
            {
                Message = "Invalid password",
                IsSuccess = false
            };
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, logInDto.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new AuthResponseDto
        {
            Message = new JwtSecurityTokenHandler().WriteToken(token),
            IsSuccess = true,
            ExpireDate = token.ValidTo
        };
    }
}
