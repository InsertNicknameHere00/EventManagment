using System.ComponentModel.DataAnnotations;

namespace EventManagmentBackend.Models.Entities
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string PasswordHash { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public ICollection<Event> Events { get; set; } = new List<Event>();
	}
}
