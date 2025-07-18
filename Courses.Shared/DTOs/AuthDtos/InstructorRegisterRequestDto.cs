﻿namespace Courses.Shared.DTOs.AuthDtos
{
    public class InstructorRegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Specialization { get; set; }
    }
}
