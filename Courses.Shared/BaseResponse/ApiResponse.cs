namespace Courses.Shared.BaseResponse
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; }

        public ApiResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public ApiResponse(T data, string message = "Success", int statusCode = 200)
        {
            IsSuccess = true;
            StatusCode = statusCode;
            Message = message;
            Data = data;
            Errors = null;
            Timestamp = DateTime.UtcNow;
        }

        public ApiResponse(string errorMessage, int statusCode = 400)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Message = errorMessage;
            Data = default;
            Errors = new List<string> { errorMessage };
            Timestamp = DateTime.UtcNow;
        }

        public ApiResponse(List<string> errors, int statusCode = 400)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Message = "Validation failed";
            Data = default;
            Errors = errors;
            Timestamp = DateTime.UtcNow;
        }

        // Static factory methods for common responses
        public static ApiResponse<T> Success(T data, string message = "Success")
        {
            return new ApiResponse<T>(data, message, 200);
        }

        public static ApiResponse<T> BadRequest(string message)
        {
            return new ApiResponse<T>(message, 400);
        }

        public static ApiResponse<T> Unauthorized(string message = "Unauthorized")
        {
            return new ApiResponse<T>(message, 401);
        }

        public static ApiResponse<T> NotFound(string message = "Resource not found")
        {
            return new ApiResponse<T>(message, 404);
        }

        public static ApiResponse<T> ServerError(string message = "Internal server error")
        {
            return new ApiResponse<T>(message, 500);
        }

        public static ApiResponse<T> ValidationError(List<string> errors)
        {
            return new ApiResponse<T>(errors, 400);
        }
    }

}