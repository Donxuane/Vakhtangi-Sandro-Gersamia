using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BudgetingExpenses.Service.Configuration.BackgroundServices
{
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

                    string userId = "3ffa2bc5-6d7c-4cc7-a87f-24206c7570dd";
                
                    if (userId is not null)
                    {
                        _logger.LogInformation("Limits Cleanup Execution Started");

                        await CleanupLimitsAsync(userId);


                        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                        
                        _logger.LogInformation($"Limits Cleanup Execution finished ");

                    }

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


                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                var notificationService = scope.ServiceProvider.GetService<ILimitNotificationService>();
                var getBudgetPlaningViewAsync =
                    await unitOfWork.BudgetPlaningRepository.GetBudgetPlaningViewAsync(
                        "3ffa2bc5 - 6d7c - 4cc7 - a87f - 24206c7570dd");

                foreach (var getBudgetPlanning in getBudgetPlaningViewAsync)
                {
                    if (getBudgetPlanning.TotalExpenses > getBudgetPlanning.LimitAmount)
                    {
                        try
                        {


                            await unitOfWork.BeginTransactionAsync();
                            await unitOfWork.LimitsRepository.DeleteLimitsAsync(getBudgetPlanning.Id);
                            await unitOfWork.SaveChangesAsync();
                            await notificationService.NotifyLimitExceededAsync(userId);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e.Message);
                            await unitOfWork.RollBackAsync();
                        }

                    }





                }
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
}
                      
                 
                    
                   
                   

       
        
    

