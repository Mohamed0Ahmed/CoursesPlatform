using Courses.Application.Abstraction;
using Courses.Domain.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;

namespace Courses.Application.Services
{
    public class TwoFactorService : ITwoFactorService
    {
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;
        private const string CodePrefix = "2FA_";
        private const int CodeExpirationMinutes = 10;

        public TwoFactorService(IEmailService emailService, IMemoryCache cache)
        {
            _emailService = emailService;
            _cache = cache;
        }

        public async Task<string> GenerateVerificationCodeAsync(ApplicationUser user)
        {
            // Generate a 6-digit verification code
            var code = GenerateRandomCode();

            // Store the code in cache with expiration
            var cacheKey = $"{CodePrefix}{user.Id}";
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CodeExpirationMinutes)
            };

            _cache.Set(cacheKey, code, cacheOptions);

            return code;
        }

        public async Task<bool> ValidateVerificationCodeAsync(ApplicationUser user, string code)
        {
            var cacheKey = $"{CodePrefix}{user.Id}";

            if (!_cache.TryGetValue(cacheKey, out string? storedCode))
            {
                return false; // Code expired or doesn't exist
            }

            if (string.IsNullOrEmpty(storedCode) || storedCode != code)
            {
                return false; // Invalid code
            }

            // Remove the code from cache after successful validation
            _cache.Remove(cacheKey);

            return true;
        }

        public async Task<bool> SendVerificationCodeAsync(ApplicationUser user)
        {
            try
            {
                var code = await GenerateVerificationCodeAsync(user);
                return await _emailService.SendVerificationCodeAsync(user.Email!, code);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsVerificationCodeExpiredAsync(ApplicationUser user)
        {
            var cacheKey = $"{CodePrefix}{user.Id}";
            return !_cache.TryGetValue(cacheKey, out _);
        }

        private string GenerateRandomCode()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);

            // Generate a 6-digit number
            var number = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000;
            return number.ToString("D6"); // Ensure 6 digits with leading zeros
        }
    }
}