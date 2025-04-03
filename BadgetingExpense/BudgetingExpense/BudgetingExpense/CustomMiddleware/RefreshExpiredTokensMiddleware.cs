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
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if (token != null)
        {
            
            var jwtHeandler = new JwtSecurityTokenHandler();
            var jwt = jwtHeandler.ReadJwtToken(token);
            if(jwt.ValidTo < DateTime.UtcNow)
            {
                var userId = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                var actualRefreshToken = context.Request.Cookies["refreshToken"];
                var scope = _service.CreateScope();
                if (scope != null)
                {
                    var manager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityModel>>();
                    var authentication = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
                    var refreshToken = authentication.GenerateRefreshToken(userId);
                    if (refreshToken == actualRefreshToken)
                    {
                        var user = await manager.FindByIdAsync(userId);
                        var role = await manager.GetRolesAsync(user);
                        var jwtToken = await authentication.GenerateJwtTokenAsync(userId, role.FirstOrDefault());
                        context.Response.Headers["Authorization"] = jwtToken;
                    }
                }
            }
        }
        await _next(context);
    }
}
