namespace task_generator.Dto
{
	public class HttpResponseDto<T>
	{
		public bool Success { get; set; }
		public string Message { get; set; } = string.Empty;
		public T? Data { get; set; }
		public List<string>? Errors { get; set; }


		// ✅ Success helper
		public static HttpResponseDto<T> SuccessResponse(T data, string message = "")
		{
			return new HttpResponseDto<T>
			{
				Success = true,
				Message = message,
				Data = data
			};
		}

		// ✅ Failure helper
		public static HttpResponseDto<T> FailureResponse(string message, List<string>? errors = null)
		{
			return new HttpResponseDto<T>
			{
				Success = false,
				Message = message,
				Errors = errors
			};
		}

	}
}
