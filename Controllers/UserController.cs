using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WawAPI.DTOs;
using WawAPI.Services;

namespace WawAPI.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
	{
		var result = await _userService.RegisterAsync(registerDto);

		if (result.IsSuccess)
		{
			return Ok(result);
		}

		return BadRequest(result);
	}
}
