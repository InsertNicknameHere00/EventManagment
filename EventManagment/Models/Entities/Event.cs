using System.ComponentModel.DataAnnotations;

namespace EventManagmentBackend.Models.Entities
{
	public class Event
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		[Required]
		public DateTime DateTime { get; set; }

		[Required]
		public string Location { get; set; } = string.Empty;

		public int? MaxAttendees { get; set; }

		public string CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public User Creator { get; set; } = null!;
	}
}
