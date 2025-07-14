using Courses.Shared.DTOs.AuthDtos;
using MediatR;

namespace Courses.Application.Features.Authentication.Commands.VerifyEmail;

public record VerifyEmailCommand(VerifyCodeRequestDto Dto, UserType UserType) : IRequest<VerifyEmailResponseDto>; 