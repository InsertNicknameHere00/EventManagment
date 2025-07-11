using System.ComponentModel.DataAnnotations;

namespace EventManagmentBackend.Models.DTO
{
	public class RegisterDTO
	{
		[Required(ErrorMessage = "Email is required!")]
		[EmailAddress(ErrorMessage = "Invalid email")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required!")]
		[MinLength(6, ErrorMessage = "Password must be at least 6 characters long!")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password confirmation is required!")]
		[Compare("Password", ErrorMessage = "Invalid! Passwords do not match!")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
