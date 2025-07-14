using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Courses.Application.Services.Jwt;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        _issuer = _configuration["JwtSettings:Issuer"] ?? "CoursesPlatform";
        _audience = _configuration["JwtSettings:Audience"] ?? "CoursesPlatform";
        _expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60");
    }

    public string GenerateToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Name, user.UserName ?? ""),
            new("UserType", user.UserType.ToString()),
            new("FirstName", user.FirstName ?? ""),
            new("LastName", user.LastName ?? ""),
            new("FullName", user.FullName ?? "")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GetUserIdFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return string.Empty;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = false, // Don't validate lifetime for getting user ID
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public DateTime GetExpirationDateFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return DateTime.MinValue;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return validatedToken.ValidTo;
        }
        catch
        {
            return DateTime.MinValue;
        }
    }
}