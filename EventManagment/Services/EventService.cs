using EventManagmentBackend.Data;
using EventManagmentBackend.Models.DTO;
using EventManagmentBackend.Models.Entities;
using EventManagmentBackend.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventManagmentBackend.Services
{
	public class EventService : IEventService
	{
		private readonly EventDBContext _context;
		public EventService(EventDBContext context)
		{
			_context = context;
		}

		public async Task<ApiResponse<List<EventDTO>>> GetAllEventsAsync(string? searchTerm = null, string? location = null, string? sortBy = null)
		{
			try
			{
				var query = _context.Events
					.Include(e => e.Creator)
					.AsQueryable();

				if (!string.IsNullOrEmpty(searchTerm))
				{
					query = query.Where(e => e.Name.Contains(searchTerm) || e.Description.Contains(searchTerm));
				}

				if (!string.IsNullOrEmpty(location))
				{
					query = query.Where(e => e.Location.Contains(location));
				}

				query = sortBy?.ToLower() switch
				{
					"date" => query.OrderBy(e => e.DateTime),
					"date_desc" => query.OrderByDescending(e => e.DateTime),
					"name" => query.OrderBy(e => e.Name),
					"name_desc" => query.OrderByDescending(e => e.Name),
					_ => query.OrderBy(e => e.DateTime)
				};

				var events = await query.ToListAsync();

				var EventDTOs = events.Select(e => new EventDTO
				{
					Id = e.Id,
					Name = e.Name,
					Description = e.Description,
					DateTime = e.DateTime,
					Location = e.Location,
					MaxAttendees = e.MaxAttendees,
					CreatorEmail = e.Creator.Email,
					CreatedAt = e.CreatedAt,
					UpdatedAt = e.UpdatedAt
				}).ToList();

				return ApiResponse<List<EventDTO>>.SuccessResponse(EventDTOs);
			}
			catch (Exception ex)
			{
				return ApiResponse<List<EventDTO>>.ErrorResponse("Failed to retrieve the events", new List<string> { ex.Message });
			}
		}

		public async Task<ApiResponse<EventDTO>> GetEventByIdAsync(int id)
		{
			try
			{
				var eventEntity = await _context.Events
				.Include(e => e.Creator)
					.FirstOrDefaultAsync(e => e.Id == id);

				if (eventEntity == null)
				{
					return ApiResponse<EventDTO>.ErrorResponse("Event not found");
				}

				var tempEvent = new EventDTO
				{
					Id = eventEntity.Id,
					Name = eventEntity.Name,
					Description = eventEntity.Description,
					DateTime = eventEntity.DateTime,
					Location = eventEntity.Location,
					MaxAttendees = eventEntity.MaxAttendees,
					CreatorEmail = eventEntity.Creator.Email,
					CreatedAt = eventEntity.CreatedAt,
					UpdatedAt = eventEntity.UpdatedAt
				};

				return ApiResponse<EventDTO>.SuccessResponse(tempEvent);
			}
			catch (Exception ex)
			{
				return ApiResponse<EventDTO>.ErrorResponse("Failed to retrieve event", new List<string> { ex.Message });
			}
		}

		public async Task<ApiResponse<EventDTO>> CreateEventAsync(EventCreateDTO eventCreateDto, int userId)
		{
			try
			{
				if (eventCreateDto.DateTime <= DateTime.UtcNow)
				{
					return ApiResponse<EventDTO>.ErrorResponse("Event date must be in the future");
				}

				var eventEntity = new Event
				{
					Name = eventCreateDto.Name,
					Description = eventCreateDto.Description,
					DateTime = eventCreateDto.DateTime,
					Location = eventCreateDto.Location,
					MaxAttendees = eventCreateDto.MaxAttendees,
					CreatedBy = eventCreateDto.CreatorEmail,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};

				_context.Events.Add(eventEntity);
				await _context.SaveChangesAsync();

				await _context.Entry(eventEntity)
					.Reference(e => e.Creator)
					.LoadAsync();

				var newEvent = new EventDTO
				{
					Id = eventEntity.Id,
					Name = eventEntity.Name,
					Description = eventEntity.Description,
					DateTime = eventEntity.DateTime,
					Location = eventEntity.Location,
					MaxAttendees = eventEntity.MaxAttendees,
					CreatorEmail = eventEntity.Creator.Email,
					CreatedAt = eventEntity.CreatedAt,
					UpdatedAt = eventEntity.UpdatedAt
				};

				return ApiResponse<EventDTO>.SuccessResponse(newEvent, "Event created successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<EventDTO>.ErrorResponse("Failed to create event", new List<string> { ex.Message });
			}
		}

		public async Task<ApiResponse<EventDTO>> UpdateEventAsync(int id, EventCreateDTO eventCreateDto, int userId)
		{
			try
			{
				var eventEntity = await _context.Events
				.Include(e => e.Creator)
					.FirstOrDefaultAsync(e => e.Id == id);

			var tempUser= await _context.Users.FindAsync(userId);

				if (eventEntity == null)
				{
					return ApiResponse<EventDTO>.ErrorResponse("Event not found");
				}

				if (eventEntity.CreatedBy != tempUser.Email)
				{
					return ApiResponse<EventDTO>.ErrorResponse("You can only update events you created");
				}

				if (eventCreateDto.DateTime <= DateTime.UtcNow)
				{
					return ApiResponse<EventDTO>.ErrorResponse("Event date must be in the future");
				}

				eventEntity.Name = eventCreateDto.Name;
				eventEntity.Description = eventCreateDto.Description;
				eventEntity.DateTime = eventCreateDto.DateTime;
				eventEntity.Location = eventCreateDto.Location;
				eventEntity.MaxAttendees = eventCreateDto.MaxAttendees;
				eventEntity.UpdatedAt = DateTime.UtcNow;

				await _context.SaveChangesAsync();

				var updatedEvent = new EventDTO
				{
					Id = eventEntity.Id,
					Name = eventEntity.Name,
					Description = eventEntity.Description,
					DateTime = eventEntity.DateTime,
					Location = eventEntity.Location,
					MaxAttendees = eventEntity.MaxAttendees,
					CreatorEmail = eventEntity.Creator.Email,
					CreatedAt = eventEntity.CreatedAt,
					UpdatedAt = eventEntity.UpdatedAt
				};

				return ApiResponse<EventDTO>.SuccessResponse(updatedEvent, "Event updated successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse<EventDTO>.ErrorResponse("Failed to update event", new List<string> { ex.Message });
			}
		}

		public async Task<ApiResponse> DeleteEventAsync(int id, int userId)
		{
			try
			{
				var eventEntity = await _context.Events
					.FirstOrDefaultAsync(e => e.Id == id);

                var tempUser = await _context.Users.FindAsync(userId);

                if (eventEntity == null)
				{
					return ApiResponse.ErrorResponse("Event not found");
				}

				if (eventEntity.CreatedBy != tempUser.Email)
				{
					return ApiResponse.ErrorResponse("You can only delete events you created");
				}

				_context.Events.Remove(eventEntity);
				await _context.SaveChangesAsync();

				return ApiResponse.SuccessResponse("Event deleted successfully");
			}
			catch (Exception ex)
			{
				return ApiResponse.ErrorResponse("Failed to delete event", new List<string> { ex.Message });
			}
		}
	}
}