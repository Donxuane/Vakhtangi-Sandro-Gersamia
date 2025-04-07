using Microsoft.Data.SqlClient;
using System.Net;

namespace BudgetingExpense.Api.CustomMiddleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var startTime = DateTime.Now;
            await _next(context);
            var endTime = DateTime.Now;
            _logger.LogInformation("Request To Pass Took: {time} s", endTime - startTime);
        }
        catch (SqlException sqlEx)
        {
            context.Response.StatusCode = sqlEx.Number;
            var StatusCode = context.Response.StatusCode;
            string sqlStringMessage = sqlEx.Number switch
            {
                2627 => "Duplicate record found. Please check your data.",
                547 => "This record cannot be deleted because it is referenced by other data.",
                1205 => "The transaction was deadlocked. Please try again.",
                _ => "An unexpected error occurred while interacting with the database."
            };
            _logger.LogError("Sql Error Occurred: {error}", sqlStringMessage);
            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode,
                Error = sqlEx.Errors.Cast<SqlError>().Select(x => new
                {
                    x.Number,
                    x.LineNumber,
                    x.State,
                    x.Server,
                    x.Procedure,
                    Message = sqlStringMessage
                })
            });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = ex switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };
            var StatusCode = context.Response.StatusCode;
            _logger.LogError("Status Code:{code}\n\tException: {ex}", StatusCode, ex.Message);
            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode,
                Error = ex.Message,
                Type = ex.Source.GetTypeCode()
            });
        }
    }
}
