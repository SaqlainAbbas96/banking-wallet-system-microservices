namespace IdentityService.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
        {
            return new ApiResponse<T> { Success = true, Data = data, Message = message };
        }

        public static ApiResponse<T> FailureResponse(IEnumerable<string> errors, string? message = null)
        {
            return new ApiResponse<T> { Success = false, Errors = errors, Message = message };
        }

        public static ApiResponse<T> FailureResponse(string error, string? message = null)
        {
            return new ApiResponse<T> { Success = false, Errors = new[] { error }, Message = message };
        }
    }
}
