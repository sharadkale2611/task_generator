using System.Text.Json;
using task_generator.Dto;

namespace task_generator.Middlewares
{
	public class ResponseWrapperMiddleware
	{
		private readonly RequestDelegate _next;

		public ResponseWrapperMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var originalBodyStream = context.Response.Body;

			using var newBody = new MemoryStream();
			context.Response.Body = newBody;

			await _next(context);

			context.Response.Body = originalBodyStream;

			newBody.Seek(0, SeekOrigin.Begin);
			var responseBody = await new StreamReader(newBody).ReadToEndAsync();

			if (context.Response.StatusCode == 200 && !string.IsNullOrWhiteSpace(responseBody))
			{
				var wrapped = HttpResponseDto<object>.SuccessResponse(
					JsonSerializer.Deserialize<object>(responseBody),
					"Success"
				);

				var json = JsonSerializer.Serialize(wrapped);

				await context.Response.WriteAsync(json);
			}
			else
			{
				await context.Response.WriteAsync(responseBody);
			}
		}
	}
}
