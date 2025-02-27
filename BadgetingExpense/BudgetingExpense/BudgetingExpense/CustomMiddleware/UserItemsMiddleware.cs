using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BudgetingExpense.api.CustomMiddleware;

public class UserItemsMiddleware
{
    private readonly RequestDelegate _next;

    public UserItemsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var claims = context.User.Claims.ToList();
        foreach (var claim in claims)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\tType: {claim.Type} , Value {claim.Value}");
        }
        Console.ForegroundColor = ConsoleColor.Gray;
        var userId = context.User.FindFirst(ClaimTypes.Name)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            context.Items["UserId"] = userId;
        }
        else
        {
            context.Items["UserId"] = null;
        }
        await _next(context);
    } 
}
