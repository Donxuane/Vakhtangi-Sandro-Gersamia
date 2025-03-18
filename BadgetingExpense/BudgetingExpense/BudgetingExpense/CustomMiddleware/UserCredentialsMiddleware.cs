using System.Security.Claims;

namespace BudgetingExpense.api.CustomMiddleware;

public class UserCredentialsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserCredentialsMiddleware> _logger;

    public UserCredentialsMiddleware(RequestDelegate next, ILogger<UserCredentialsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
                await _next(context);
            }
            context.Response.StatusCode = 401;
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
        }
    } 
}
