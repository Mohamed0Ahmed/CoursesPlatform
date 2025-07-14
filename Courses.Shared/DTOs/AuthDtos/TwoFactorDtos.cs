namespace Courses.Shared.DTOs.AuthDtos
{
    public class SendVerificationCodeRequestDto
    {
        public string Email { get; set; } = string.Empty;
    }

    public class VerifyCodeRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }


}