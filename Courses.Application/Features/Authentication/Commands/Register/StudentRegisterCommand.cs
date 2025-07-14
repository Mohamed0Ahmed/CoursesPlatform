using Courses.Shared.DTOs.AuthDtos;
using MediatR;

namespace Courses.Application.Features.Authentication.Commands.Register;

public record StudentRegisterCommand(StudentRegisterRequestDto Dto) : IRequest<RegisterResponseDto>;