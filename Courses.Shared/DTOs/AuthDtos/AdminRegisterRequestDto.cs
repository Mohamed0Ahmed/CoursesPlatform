namespace Courses.Shared.DTOs.AuthDtos
{
    public class AdminRegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? AdminCode { get; set; } // Optional admin registration code
    }
}
