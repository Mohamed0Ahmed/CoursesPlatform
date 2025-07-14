using Courses.Shared.DTOs.AuthDtos;
using MediatR;

namespace Courses.Application.Features.Authentication.Commands.Register;

public record AdminRegisterCommand(AdminRegisterRequestDto Dto) : IRequest<RegisterResponseDto>; 