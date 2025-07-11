using BCrypt.Net;
using EventManagmentBackend.Data;
using EventManagmentBackend.Models.DTO;
using EventManagmentBackend.Models.Entities;
using EventManagmentBackend.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagmentBackend.Services
{
	public class AuthService : IAuthService
	{
		private readonly EventDBContext _context;
		private readonly IConfiguration _configuration;

		public AuthService(EventDBContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		public async Task<ApiResponse<string>> RegisterAsync(RegisterDTO registerDto)
		{
			try
			{
				var existingUser = await _context.Users
					.FirstOrDefaultAsync(u => u.Email == registerDto.Email);

				if (existingUser != null)
				{
					return ApiResponse<string>.ErrorResponse("User already exists!");
				}

				var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

				var newUser = new User
				{
					Email = registerDto.Email,
					PasswordHash = passwordHash,
					CreatedAt = DateTime.UtcNow
				};

				_context.Users.Add(newUser);
				await _context.SaveChangesAsync();
				var token = GenerateJwtToken(newUser.Email, newUser.Id);

				return ApiResponse<string>.SuccessResponse(token, "User registered successfully!");
			}
			catch (Exception ex)
			{
				return ApiResponse<string>.ErrorResponse("Registration failed", new List<string> { ex.Message });
			}
		}

		public async Task<ApiResponse<string>> LoginAsync(LoginDTO loginDto)
		{
			try
			{
				var user = await _context.Users
					.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

				if (user == null)
				{
					return ApiResponse<string>.ErrorResponse("Invalid email or password");
				}

				if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
				{
					return ApiResponse<string>.ErrorResponse("Invalid email or password");
				}

				var token = GenerateJwtToken(user.Email, user.Id);

				return ApiResponse<string>.SuccessResponse(token, "Login successful");
			}
			catch (Exception ex)
			{
				return ApiResponse<string>.ErrorResponse("Login failed", new List<string> { ex.Message });
			}
		}

		public string GenerateJwtToken(string email, int userId)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

			var claims = new[]
			{
				new Claim(ClaimTypes.Email, email),
				new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
			};

			var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
				Issuer = jwtSettings["Issuer"],
				Audience = jwtSettings["Audience"],
				SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}

