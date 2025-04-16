using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.BackgroundServices;

public class LimitsCleanupService : BackgroundService
{

    private readonly ILogger<LimitsCleanupService> _logger;
    private readonly IServiceScopeFactory _serviceFactory;
    public LimitsCleanupService(ILogger<LimitsCleanupService> logger,
        IServiceScopeFactory serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Waiting Background Service to Start Executing");
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                _logger.LogInformation("Limits Cleanup Execution Started");
                await CleanupLimitsAsync();
                _logger.LogInformation($"Limits Cleanup Execution finished ");
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception ex:{ex}", ex.Message);
            }
        }
    }

    private async Task CleanupLimitsAsync()
    {
        var scope = _serviceFactory.CreateScope();
        try
        {
            var notificationService = scope.ServiceProvider.GetRequiredService<ILimitNotificationService>();
            var userCredentialsRepository = scope.ServiceProvider.GetRequiredService<IGetUserCredentials>();
            var collection = await userCredentialsRepository.GetAllUsersIdiesAsync();
            foreach (var record in collection)
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
            else if (scope is IDisposable disposable)
            {
                disposable.Dispose();
            }

        }
    }
}
