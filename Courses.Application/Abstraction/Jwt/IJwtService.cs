using Courses.Domain.Identity;

namespace Courses.Application.Abstraction.Jwt;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
    bool ValidateToken(string token);
    string GetUserIdFromToken(string token);
    DateTime GetExpirationDateFromToken(string token);
} 