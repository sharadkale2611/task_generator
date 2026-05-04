using System.Net;
using System.Text.Json;
using task_generator.Dto;

namespace task_generator.Middlewares
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string message) : base(message) { }
	}

	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (ArgumentException ex)
			{
				await HandleException(context, HttpStatusCode.BadRequest, ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				await HandleException(context, HttpStatusCode.Conflict, ex.Message);
			}
			catch (NotFoundException ex)
			{
				await HandleException(context, HttpStatusCode.NotFound, ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex); // log it
				await HandleException(context, HttpStatusCode.InternalServerError, "Something went wrong");
			}
		}

		private static async Task HandleException(HttpContext context, HttpStatusCode statusCode, string message)
		{
			if (context.Response.HasStarted)
				return;

			context.Response.Clear();
			context.Response.StatusCode = (int)statusCode;
			context.Response.ContentType = "application/json";

			var response = HttpResponseDto<object>.FailureResponse(
				"Request failed",
				new List<string> { message }
			);

			var json = JsonSerializer.Serialize(response);

			await context.Response.WriteAsync(json);
		}
	}
}