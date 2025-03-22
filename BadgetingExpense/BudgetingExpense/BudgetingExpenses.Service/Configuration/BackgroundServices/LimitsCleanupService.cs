using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

    public class LimitsCleanupService : BackgroundService
    {

        private readonly ILogger<LimitsCleanupService> _logger;
        private readonly IServiceProvider _serviceProvider;



        public LimitsCleanupService(ILogger<LimitsCleanupService> logger, IServiceProvider serviceProvider)
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

                        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                        
                        _logger.LogInformation($"Limits Cleanup Execution finished ");

                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception ex:{ex}", ex.Message);
                }
            }
        }

        private async Task CleanupLimitsAsync(string userId)
        {
            var scope = _serviceProvider.CreateScope();
            try
            {
                var notificationService = scope.ServiceProvider.GetService<ILimitNotificationService>();
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
               
            }
        }
    }
                      
                 
                    
                   
                   

       
        
    

