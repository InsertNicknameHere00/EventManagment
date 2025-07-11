using EventManagmentBackend.Models.DTO;
using EventManagmentBackend.Models.Response;

namespace EventManagmentBackend.Services
{
	public interface IAuthService
	{
		Task<ApiResponse<string>> RegisterAsync(RegisterDTO registerDto);
		Task<ApiResponse<string>> LoginAsync(LoginDTO loginDto);
		string GenerateJwtToken(string email, int userId);
	}
}