namespace Courses.Application.Features.Authentication.Commands.Register.Student;

public record StudentRegisterCommand(StudentRegisterRequestDto Dto) : IRequest<RegisterResponseDto>;