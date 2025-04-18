﻿using BudgetingExpense.DataAccess.Repository.Identity;
using BudgetingExpense.DataAccess.SqlQueries;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Configuration;

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
        var scope = _scopeFactory.CreateScope();
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (!await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogInformation("Database Migration Execution Started");
                await dbContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Database Queries Execution Started");
                var result = AddDatabaseContentAccordingly();
                if (result)
                {
                    _logger.LogInformation("Database Setup Finished!");

                    var seedData = scope.ServiceProvider.GetRequiredService<ConfigureSeeding>();
                    if (seedData != null)
                    {
                        await seedData.SeedRoles();
                        await seedData.SeedData();
                    }
                }
            }
            else
            {
                _logger.LogInformation("Database Already Exists!");
            }
        }
        finally
        {
            if (scope is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else
            {
                scope.Dispose();
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private bool AddDatabaseContentAccordingly()
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("default");
            using (DbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var queries = GetDatabaseQueries();
                if (queries != null && queries.Count != 0)
                {
                    foreach (var query in queries)
                    {
                        DbTransaction transaction = connection.BeginTransaction();
                        try
                        {
                            

                                connection.Execute(query, null, transaction);
                                transaction.Commit();
                            
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }

                    }

                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }

    private List<string?>? GetDatabaseQueries()
    {
        try
        {
            List<string?> queries = [];
            var tables = new GetSqlData("Tables").GetData();
            queries.Add(tables);
            var views = new GetSqlData("Views").GetData();
            if (views != null)
            {
                var viewQueries = views.Split("Go", StringSplitOptions.RemoveEmptyEntries);
                foreach (var query in viewQueries)
                {
                    queries.Add(query);
                }
            }
            var procedures = new GetSqlData("Procedures").GetData();
            if (procedures != null)
            {
                var proceduresQueries = procedures.Split("Go", StringSplitOptions.RemoveEmptyEntries);
                foreach (var query in proceduresQueries)
                {
                    queries.Add(query);
                }
            }
            var triggers = new GetSqlData("Triggers").GetData();
            if(triggers != null)
            {
                var triggersQueries = triggers.Split("Go", StringSplitOptions.RemoveEmptyEntries);
                foreach(var query in triggersQueries)
                {
                    queries.Add(query);
                }
            }
            return queries;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return null;
        }
    }
}
