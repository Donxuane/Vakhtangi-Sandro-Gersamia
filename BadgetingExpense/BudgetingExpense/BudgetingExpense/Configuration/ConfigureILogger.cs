using Serilog.Events;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace BudgetingExpense.Api.Configuration;

public static class ConfigureILogger
{
    public static IServiceCollection AddILoggerConfiguration(this IServiceCollection services)
    {
        services.AddLogging(options =>
        {
            options.ClearProviders();
            options.AddSerilog();
        });
        return services;
    }
}
