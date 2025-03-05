using System.Security.Claims;

namespace BudgetingExpense.api.CustomMiddleware;

public class UserCredentialsMiddleware
{
    private readonly RequestDelegate _next;

    public UserCredentialsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var claims = context.User.Claims.ToList();
            foreach (var claim in claims)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\tType: {claim.Type} , Value {claim.Value}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            var userId = context.User.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                context.Items["UserId"] = userId;
            }
            await _next(context);
        }
        catch(Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(ex.Message);
        }
    } 
}
