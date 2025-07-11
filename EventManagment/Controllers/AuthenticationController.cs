using EventManagmentBackend.Models.DTO;
using EventManagmentBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EventManagmentBackend.Controllers
{
		[ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
		{
			private readonly IAuthService _authService;

			public AuthController(IAuthService authService)
			{
				_authService = authService;
			}

            [HttpPost("Register")]
			public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return BadRequest(new { success = false, message = "Validation failed!", errors });
				}

				var result = await _authService.RegisterAsync(registerDto);

				if (result.Success)
				{
					return Ok(result);
				}

				return BadRequest(result);
			}

			[HttpPost("Login")]
			public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return BadRequest(new { success = false, message = "Validation failed!", errors });
				}

				var result = await _authService.LoginAsync(loginDto);

				if (result.Success)
				{
					return Ok(result);
				}

				return Unauthorized(result);
			}

			[HttpPost("Logout")]
			public IActionResult Logout()
			{
				// In a JWT-based system, logout is typically handled client-side
				// by removing the token from storage. Server-side logout would require
				// token blacklisting, which is beyond the scope of this basic implementation.
				return Ok(new { success = true, message = "Logout successful" });
			}
		}
	}