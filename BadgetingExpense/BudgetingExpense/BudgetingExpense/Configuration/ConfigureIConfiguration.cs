namespace BudgetingExpense.Api.Configuration;

public static class ConfigureIConfiguration
{
    public static void AddMultipleJsonFileConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("seedData.json", optional: true, reloadOnChange: true)
            .AddJsonFile("emailSettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}
