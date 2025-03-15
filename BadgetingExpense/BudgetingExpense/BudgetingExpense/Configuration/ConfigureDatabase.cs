using BudgetingExpense.DataAccess.Repository.Identity;
using BudgetingExpense.DataAccess.SqlQueries;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace BudgetingExpense.Api.Configuration;

public class ConfigureDatabase : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ConfigureDatabase> _logger;
    private readonly IConfiguration _configuration;

    public ConfigureDatabase(IServiceScopeFactory scopeFactory,
        ILogger<ConfigureDatabase> logger, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (!await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogInformation("Database Migration Execution Started");
                await dbContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Database Queries Execution Started");
                AddDatabaseContentAccordingly();
                _logger.LogInformation("Database Setup Finished!");
            }
            else
            {
                _logger.LogInformation("Database Already Exists!");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void AddDatabaseContentAccordingly()
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("default");
            using (DbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    var queries = GetDatabaseQueries();
                    if (queries != null && queries.Count != 0)
                    {
                        foreach (var query in queries)
                        {
                            if (query != null)
                            {
                                connection.Execute(query, null, transaction);
                            }
                        }
                        transaction.Commit();
                    }
                }
                }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
        }
    }

    private List<string?>? GetDatabaseQueries()
    {
        try
        {
            List<string?> queries = [];
            var tables = new GetSqlData("TableQueries").GetData();
            queries.Add(tables);
            queries.Add(view);
            queries.Add(view1);
            queries.Add(view2);

            return queries;
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return null;
        }
    }
}
