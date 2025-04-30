
using System.Security.Claims;
using Data;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Models.Db;

public static class UserHelper
{
    public static async Task<Result<User>> GetCurrentUserAsync(
        ClaimsPrincipal userPrincipal,
        AppDbContext dbContext,
        Serilog.ILogger _logger)
    {
        var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            _logger.Error($"Invalid user claim: '{userIdClaim}'.");
            return Result.Fail("Invalid user token");
        }

        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            _logger.Error($"User not found with ID: '{userId}'.");
            return Result.Fail("User not found");
        }

        return user;
    }
}
