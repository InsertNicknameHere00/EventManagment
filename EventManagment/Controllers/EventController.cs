using EventManagmentBackend.Models.DTO;
using EventManagmentBackend.Models.Entities;
using EventManagmentBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagmentBackend.Controllers
{
		[ApiController]
		[Route("api/[controller]")]
		public class EventsController : ControllerBase
		{
			private readonly IEventService _eventService;

			public EventsController(IEventService eventService)
			{
				_eventService = eventService;
			}

			[HttpGet]
			public async Task<IActionResult> GetEvents([FromQuery] string? search, [FromQuery] string? location, [FromQuery] string? sortBy)
			{
				var result = await _eventService.GetAllEventsAsync(search, location, sortBy);

				if (result.Success)
				{
					return Ok(result);
				}

				return BadRequest(result);
			}

			[HttpGet("{id}")]
			public async Task<IActionResult> GetEvent(int id)
			{
				var result = await _eventService.GetEventByIdAsync(id);
				if (result.Success)
				{
					return Ok(result);
				}

				if (result.Message == "Event not found")
				{
					return NotFound(result);
				}

				return BadRequest(result);
			}

			[HttpPost]
			[Authorize]
			public async Task<IActionResult> CreateEvent([FromBody] EventCreateDTO eventCreateDto)
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return BadRequest(new { success = false, message = "Validation failed!", errors });
				}

				var userId = GetUserIdFromClaims();
				if (userId == null)
				{
					return Unauthorized(new { success = false, message = "Invalid token" });
				}

				var result = await _eventService.CreateEventAsync(eventCreateDto, userId.Value);

				if (result.Success)
				{
					return CreatedAtAction(nameof(GetEvent), new { id = result.Data?.Id }, result);
				}

				return BadRequest(result);
			}

			[HttpPut("{id}")]
			[Authorize]
			public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventCreateDTO eventCreateDto)
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return BadRequest(new { success = false, message = "Validation failed!", errors });
				}

				var userId = GetUserIdFromClaims();
				if (userId == null)
				{
					return Unauthorized(new { success = false, message = "Invalid token" });
				}

				var result = await _eventService.UpdateEventAsync(id, eventCreateDto, userId.Value);

				if (result.Success)
				{
					return Ok(result);
				}

				if (result.Message == "Event not found")
				{
					return NotFound(result);
				}

				if (result.Message == "You can only update events you created")
				{
					return Forbid();
				}

				return BadRequest(result);
			}

			[HttpDelete("{id}")]
			[Authorize]
			public async Task<IActionResult> DeleteEvent(int id)
			{
				var userId = GetUserIdFromClaims();
				if (userId == null)
				{
					return Unauthorized(new { success = false, message = "Invalid token" });
				}

				var result = await _eventService.DeleteEventAsync(id, userId.Value);

				if (result.Success)
				{
					return Ok(result);
				}

				if (result.Message == "Event not found")
				{
					return NotFound(result);
				}

				if (result.Message == "You can only delete events you created")
				{
					return Forbid();
				}

				return BadRequest(result);
			}

			private int? GetUserIdFromClaims()
			{
				var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
				{
					return userId;
				}
				return null;
			}
		}
	}