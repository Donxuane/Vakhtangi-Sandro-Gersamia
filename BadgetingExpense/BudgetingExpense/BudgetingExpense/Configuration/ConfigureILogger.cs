using Serilog.Events;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace BudgetingExpense.Api.Configuration;

public static class ConfigureILogger
{
    public static void AddILoggerConfiguration(this IServiceCollection services)
    {
        services.AddLogging();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: SystemConsoleTheme.Colored)
            .WriteTo.File("Logs/ErrorLogs.log", restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo.File("Logs/InformationLogs.log", restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();
    }
}
