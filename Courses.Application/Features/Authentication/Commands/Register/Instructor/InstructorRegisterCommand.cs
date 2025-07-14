using Courses.Shared.DTOs.AuthDtos;
using MediatR;

namespace Courses.Application.Features.Authentication.Commands.Register.Instructor;

public record InstructorRegisterCommand(InstructorRegisterRequestDto Dto) : IRequest<RegisterResponseDto>; 