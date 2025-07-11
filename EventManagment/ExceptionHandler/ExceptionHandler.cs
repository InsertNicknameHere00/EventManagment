using EventManagmentBackend.Models.Response;
using System.Net;
using System.Text.Json;

namespace EventManagmentBackend.ExceptionHandler
{
	public class ExceptionHandler
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandler> _logger;

		public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An unhandled exception has occurred!");
				await HandleExceptionAsync(context, ex);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var response = ApiResponse.ErrorResponse(
				"Error! An unhandled exception occurred while processing your request!",
				new List<string> { exception.Message }
			);

			var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			});

			await context.Response.WriteAsync(jsonResponse);
		}
	}
}