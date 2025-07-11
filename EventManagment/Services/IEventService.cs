using EventManagmentBackend.Models.DTO;
using EventManagmentBackend.Models.Response;

namespace EventManagmentBackend.Services
{
	public interface IEventService
	{
		Task<ApiResponse<List<EventDTO>>> GetAllEventsAsync(string? searchTerm = null, string? location = null, string? sortBy = null);
		Task<ApiResponse<EventDTO>> GetEventByIdAsync(int id);
		Task<ApiResponse<EventDTO>> CreateEventAsync(EventCreateDTO eventCreateDto, int userId);
		Task<ApiResponse<EventDTO>> UpdateEventAsync(int id, EventCreateDTO eventCreateDto, int userId);
		Task<ApiResponse> DeleteEventAsync(int id, int userId);
	}
}