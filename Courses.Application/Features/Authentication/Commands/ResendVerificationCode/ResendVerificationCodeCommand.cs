using Courses.Shared.DTOs.AuthDtos;
using MediatR;

namespace Courses.Application.Features.Authentication.Commands.ResendVerificationCode;

public record ResendVerificationCodeCommand(SendVerificationCodeRequestDto Dto, UserType UserType) : IRequest<bool>; 