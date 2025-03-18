namespace BudgetingExpense.Api.CustomMiddleware;

public class ExceptionLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionLoggerMiddleware> _logger;

    public ExceptionLoggerMiddleware(RequestDelegate next, ILogger<ExceptionLoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}\nStatus Code:{code}", ex.Message,context.Response.StatusCode);
            await context.Response.WriteAsJsonAsync(new { ex.Message });
        }
    }
}
