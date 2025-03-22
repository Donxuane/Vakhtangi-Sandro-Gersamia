using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Configuration.BackgroundServices;

public class LimitsCleanupService : BackgroundService
{

    private readonly ILogger<LimitsCleanupService> _logger;
    private readonly IServiceScopeFactory _serviceProvider;
    public LimitsCleanupService(ILogger<LimitsCleanupService> logger, 
        IServiceScopeFactory serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Limits Cleanup Execution Started");
                await CleanupLimitsAsync();
                _logger.LogInformation($"Limits Cleanup Execution finished ");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception ex:{ex}", ex.Message);
            }
        }
    }

    private async Task CleanupLimitsAsync()
    {
        var scope = _serviceProvider.CreateScope();
        try
        {
            var notificationService = scope.ServiceProvider.GetRequiredService<ILimitNotificationService>();
            var userCredentialsRepository = scope.ServiceProvider.GetRequiredService<IGetUserCredentials>();
            var collection = await userCredentialsRepository.GetAllUsersIdiesAsync();
            foreach(var record in collection)
            {
                var result = await notificationService.NotifyLimitExceededAsync(record);
                if (result == false)
                {
                    _logger.LogError("Service Provider ILimitsNotificationService Failed!!!!");
                }
            }
        }
        finally
        {
            if (scope is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else if(scope is IDisposable disposable)
            {
                disposable.Dispose();
            }
           
        }
    }
}
