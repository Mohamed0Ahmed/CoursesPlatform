namespace Courses.Shared.DTOs.AuthDtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserInfoDto User { get; set; } = new();
    }

    public class UserInfoDto
    {
        //public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public string UserName { get; set; } = string.Empty;
        //public string FirstName { get; set; } = string.Empty;
        //public string LastName { get; set; } = string.Empty;
        //public string UserType { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        //public bool IsActive { get; set; }
        //public bool EmailConfirmed { get; set; }
        //public DateTime CreatedAt { get; set; }
    }

    public class RegisterResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public UserInfoDto User { get; set; } = new();
    }

    public class VerifyEmailResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public UserInfoDto User { get; set; } = new();
    }
} 