using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BudgetingExpenses.Service.Configuration.BackgroundServices
{
    public class LimitsCleanupService: BackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LimitsCleanupService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpContext _httpContext;
        public LimitsCleanupService(IUnitOfWork unitOfWork,ILogger<LimitsCleanupService> logger,IServiceProvider serviceProvider,HttpContext httpContext)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _httpContext = httpContext;
        }
        
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Limits Cleanup Execution Started");
               
                    await CleanupLimitsAsync(_httpContext.Items["UserId"].ToString());

                    
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    _logger.LogInformation("Limits Cleanup Execution finished");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception ex:{ex}", ex.Message);
                }
            }
        }

        private async Task CleanupLimitsAsync(string userId)
        {
          

            var getBudgetPlaningViewAsync = await _unitOfWork.BudgetPlaningRepository.GetBudgetPlaningViewAsync(userId);

            foreach (var getBudgetPlanning in getBudgetPlaningViewAsync)
            {
                if (getBudgetPlanning.TotalExpenses > getBudgetPlanning.LimitAmount)
                {
                    await _unitOfWork.LimitsRepository.DeleteLimitsAsync(getBudgetPlanning.Id);
                }
            }
        }
    }
}
