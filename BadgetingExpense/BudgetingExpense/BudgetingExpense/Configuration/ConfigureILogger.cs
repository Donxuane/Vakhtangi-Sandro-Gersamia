using Serilog.Events;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using BudgetingExpenses.Service.Live;
using Microsoft.AspNetCore.SignalR;

namespace BudgetingExpense.Api.Configuration;

public static class ConfigureILogger
{
    public static IServiceCollection AddILoggerConfiguration(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();
        services.AddLogging(options =>
        {
            options.ClearProviders();
            options.AddSerilog();
        });
        return services;
    }

    public static void AddLoggerDetails(this IServiceProvider service)
    {
        var hubContext = service.GetRequiredService<IHubContext<AppLiveHub>>();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .WriteTo.File("Logs/ErrorLogs.log", restrictedToMinimumLevel: LogEventLevel.Error)
            //.WriteTo.File("Logs/InformationLogs.log", restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.Sink(new SignalRSink(hubContext))
            .CreateLogger();
    }
}
