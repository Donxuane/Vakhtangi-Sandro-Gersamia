using BudgetingExpense.Domain.Contracts.IServices.IAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BudgetingExpenses.Service.Service.Authentication;

public class TokenAuthenticationService : ITokenAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenAuthenticationService> _logger;

    public TokenAuthenticationService(IConfiguration configuration, ILogger<TokenAuthenticationService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task<string>? GenerateJwtTokenAsync(string userId, string userRole)
    {
        try
        {
            var tokenConfiguration = _configuration.GetSection("Jwt");
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration["Key"]));

            var claims = new[]
            {
                    new Claim(ClaimTypes.Name, userId),
                    new Claim(ClaimTypes.Role, userRole)
            };
            var token = new JwtSecurityToken
            (
                issuer: tokenConfiguration["Issuer"],
                audience: tokenConfiguration["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(tokenConfiguration["ExpiryMinutes"])),
                signingCredentials: new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256)
            );
            _logger.LogInformation("Token generated for {userId}", userId);
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception {ex}", ex.Message);
            throw;
        }
    }

    public Task<string> GenerateRefreshToken(string userId)
    {
        try
        {
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt")["Key"]));
            var claim = new[] { new Claim(ClaimTypes.Name, userId) };
            var token = new JwtSecurityToken
            (
                claims: claim,
                signingCredentials: new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256)
            );
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));

        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }
}
