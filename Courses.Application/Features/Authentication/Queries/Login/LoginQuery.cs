using Courses.Shared.DTOs.AuthDtos;
using MediatR;

namespace Courses.Application.Features.Authentication.Queries.Login;

public record LoginQuery(LoginRequestDto Dto, UserType UserType) : IRequest<LoginResponseDto>; 