namespace Courses.Application.Features.Authentication.Queries.GetCurrentUser;

public record GetCurrentUserQuery(string UserId, UserType UserType) : IRequest<UserInfoDto>; 