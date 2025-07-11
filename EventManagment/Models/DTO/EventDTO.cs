using System.ComponentModel.DataAnnotations;

namespace EventManagmentBackend.Models.DTO
{
	public class EventDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime DateTime { get; set; }
		public string Location { get; set; } = string.Empty;
		public int? MaxAttendees { get; set; }
		public string CreatorEmail { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
