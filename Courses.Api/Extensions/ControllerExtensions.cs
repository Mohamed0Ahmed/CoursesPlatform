using Courses.Shared.Exceptions;

namespace Courses.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static ActionResult<ApiResponse<T>> Success<T>(this ControllerBase controller, T data, string message = "Success")
        {
            return controller.Ok(ApiResponse<T>.Success(data, message));
        }

        public static ActionResult<ApiResponse<T>> BadRequest<T>(this ControllerBase controller, string message)
        {
            return controller.BadRequest(ApiResponse<T>.BadRequest(message));
        }

        public static ActionResult<ApiResponse<T>> Unauthorized<T>(this ControllerBase controller, string message = "Unauthorized")
        {
            return controller.Unauthorized(ApiResponse<T>.Unauthorized(message));
        }

        public static ActionResult<ApiResponse<T>> NotFound<T>(this ControllerBase controller, string message = "Resource not found")
        {
            return controller.NotFound(ApiResponse<T>.NotFound(message));
        }

        public static ActionResult<ApiResponse<T>> ValidationError<T>(this ControllerBase controller, List<string> errors)
        {
            return controller.BadRequest(ApiResponse<T>.ValidationError(errors));
        }

        public static ActionResult<ApiResponse<T>> ServerError<T>(this ControllerBase controller, string message = "Internal server error")
        {
            return controller.StatusCode(500, ApiResponse<T>.ServerError(message));
        }

        // Helper method to throw custom exceptions
        public static void ThrowIfNull<T>(T value, string message = "Resource not found")
        {
            if (value == null)
                throw new NotFoundException(message);
        }

        public static void ThrowIfInvalid(bool condition, string message = "Invalid request")
        {
            if (!condition)
                throw new BadRequestException(message);
        }
    }
}