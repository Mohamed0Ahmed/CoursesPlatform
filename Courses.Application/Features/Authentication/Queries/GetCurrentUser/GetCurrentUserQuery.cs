using Courses.Shared.DTOs.AuthDtos;
using MediatR;

namespace Courses.Application.Features.Authentication.Queries.GetCurrentUser;

public record GetCurrentUserQuery(string UserId, UserType UserType) : IRequest<UserInfoDto>; 