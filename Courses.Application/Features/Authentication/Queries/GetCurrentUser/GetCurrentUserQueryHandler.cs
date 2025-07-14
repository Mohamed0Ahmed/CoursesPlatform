using Courses.Shared.DTOs.AuthDtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Courses.Application.Features.Authentication.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserInfoDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetCurrentUserQueryHandler> _logger;

    public GetCurrentUserQueryHandler(UserManager<ApplicationUser> userManager, ILogger<GetCurrentUserQueryHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UserInfoDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            _logger.LogWarning("GetCurrentUser failed: User not found for ID {UserId}", request.UserId);
            throw new InvalidOperationException("User not found");
        }

        if (user.UserType != request.UserType)
        {
            _logger.LogWarning("GetCurrentUser failed: User {UserId} is not of type {UserType}", request.UserId, request.UserType);
            throw new UnauthorizedAccessException("Access denied");
        }

        return user.Adapt<UserInfoDto>();
    }
} 