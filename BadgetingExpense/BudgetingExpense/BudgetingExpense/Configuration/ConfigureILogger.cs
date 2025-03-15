using Serilog.Events;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Extensions.Logging;
using Serilog.Data;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace BudgetingExpense.Api.Configuration;

public static class ConfigureILogger
{
    public static IServiceCollection AddILoggerConfiguration(this IServiceCollection services)
    {
         Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .WriteTo.File("Logs/ErrorLogs.log", restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.File("Logs/InformationLogs.log", restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();
        services.AddLogging(options =>
        {
            options.ClearProviders();
            options.AddSerilog();
        });
        return services;
    }
}
