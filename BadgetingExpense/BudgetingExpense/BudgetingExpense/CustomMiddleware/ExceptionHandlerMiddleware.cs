﻿using Microsoft.Data.SqlClient;
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
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var StatusCode = context.Response.StatusCode;
            _logger.LogError("Sql Error Occurred: {error}", sqlEx.Message);
            await context.Response.WriteAsJsonAsync(new { 
                StatusCode,
                Error = sqlEx.Errors.Cast<SqlError>().Select(x=> new
                {
                    x.Number,
                    x.LineNumber,
                    x.State,
                    x.Server,
                    x.Procedure,
                    x.Message
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
            _logger.LogError("Status Code:{code}", StatusCode);
            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode,
                Error = ex.Message,
                Type = ex.Source.GetTypeCode()
            });
        }
    }
}
