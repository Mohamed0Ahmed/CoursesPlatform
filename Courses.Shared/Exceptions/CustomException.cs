namespace Courses.Shared.Exceptions
{
    public class CustomException : Exception
    {
        public int StatusCode { get; }

        public CustomException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }

        public CustomException(string message, Exception innerException, int statusCode = 400)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : CustomException
    {
        public NotFoundException(string message = "Resource not found") : base(message, 404)
        {
        }
    }

    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(string message = "Unauthorized access") : base(message, 401)
        {
        }
    }

    public class BadRequestException : CustomException
    {
        public BadRequestException(string message = "Bad request") : base(message, 400)
        {
        }
    }

    public class ValidationException : CustomException
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors) : base("Validation failed", 400)
        {
            Errors = errors;
        }

        public ValidationException(string message, List<string> errors) : base(message, 400)
        {
            Errors = errors;
        }
    }
}