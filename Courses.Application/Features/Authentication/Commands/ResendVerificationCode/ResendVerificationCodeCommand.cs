namespace Courses.Application.Features.Authentication.Commands.ResendVerificationCode;

public record ResendVerificationCodeCommand(SendVerificationCodeRequestDto Dto, UserType UserType) : IRequest<bool>; 