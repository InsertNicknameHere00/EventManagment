using System.ComponentModel.DataAnnotations;

namespace EventManagmentBackend.Models.DTO
{
	public class EventCreateDTO
	{
		[Required(ErrorMessage = "Event name required!")]
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		[Required(ErrorMessage = "Event date and time required!")]
		public DateTime DateTime { get; set; }

		[Required(ErrorMessage = "Location required!")]
		public string Location { get; set; } = string.Empty;

		[Required(ErrorMessage ="Creator Email is requried!")]
        public string CreatorEmail { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Maximum attendees Can't be below 1!")]
		public int? MaxAttendees { get; set; }
	}
}
