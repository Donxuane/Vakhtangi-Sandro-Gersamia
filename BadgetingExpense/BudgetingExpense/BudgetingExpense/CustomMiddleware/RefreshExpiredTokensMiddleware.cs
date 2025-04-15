using BudgetingExpense.DataAccess.Repository.Identity;
using BudgetingExpense.Domain.Contracts.IServices.IAuthentication;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BudgetingExpense.Api.CustomMiddleware;

public class RefreshExpiredTokensMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _service;

    public RefreshExpiredTokensMiddleware(RequestDelegate next, IServiceScopeFactory service)
    {
        _next = next;
        _service = service;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        var refreshToken = context.Request.Cookies["refreshToken"];
        if (refreshToken != null)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (token != null)
            {
                var jwt = jwtHandler.ReadJwtToken(token);
                if (jwt.ValidTo < DateTime.UtcNow)
                {
                    var userId = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    var refreshTokenUserId = jwtHandler.ReadJwtToken(refreshToken).Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    var jwtToken = await GenerateToken(userId, refreshTokenUserId);
                    context.Response.Headers.Authorization = jwtToken;
                }
            }
            else
            {
                var refreshTokenUserId = jwtHandler.ReadJwtToken(refreshToken).Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                var jwtToken = await GenerateToken(refreshTokenUserId, refreshTokenUserId);
                context.Response.Headers.Authorization = jwtToken;
            }
        }
        if(token!=null && refreshToken == null)
        {
            throw new UnauthorizedAccessException();
        }
        await _next(context);
    }

    private async Task<string?> GenerateToken(string userId,string refreshTokenUserId)
    {
        var scope = _service.CreateScope();
        if (scope != null)
        {
            var manager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityModel>>();
            var tokenAuthentication = scope.ServiceProvider.GetRequiredService<ITokenAuthenticationService>();
            if (userId == refreshTokenUserId)
            {
                var user = await manager.FindByIdAsync(userId);
                var role = await manager.GetRolesAsync(user);
                var jwtToken = await tokenAuthentication.GenerateJwtTokenAsync(userId, role.FirstOrDefault());
                return jwtToken;
            }
        }
        return null;
    }
}
