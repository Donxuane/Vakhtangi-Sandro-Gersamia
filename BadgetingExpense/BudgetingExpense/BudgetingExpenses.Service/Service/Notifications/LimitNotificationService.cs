using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Notifications
{
    public class LimitNotificationService : ILimitNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LimitNotificationService> _logger;
        
    

        public LimitNotificationService(IUnitOfWork unitOfWork, IEmailService emailService, IConfiguration configuration, ILogger<LimitNotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;   
        }
        public async Task<bool> NotifyLimitExceededAsync(string userId)
        {
            var enabledOrNot = await _unitOfWork.GetRepository.GetNotificationActiveStatusAsync(userId);
            var email = await _unitOfWork.GetRepository.GetEmailAsync(userId);
            if (enabledOrNot)
            {
                var getBudgetPlaningView = await _unitOfWork.BudgetPlaningRepository.GetBudgetPlaningViewAsync(userId);
              var (subject,message) =   await GetEmail();
                foreach (var VARIABLE in getBudgetPlaningView)
                {
                    if (VARIABLE.LimitAmount < VARIABLE.TotalExpenses)
                    {
                        var categoryName = await _unitOfWork.GetRepository.GetCategoryNameAsync(VARIABLE.CategoryId);
                        message = message.Replace("{category}",
                            string.IsNullOrEmpty(categoryName) ? "Undifined" : categoryName);
                        message = message.Replace("{amount}", VARIABLE.TotalExpenses.ToString());
                        message = message.Replace("{currency}", VARIABLE.Currency.ToString());
                        message = message.Replace("{limitAmount}", VARIABLE.LimitAmount.ToString());
                        message = message.Replace("{LimitCurrecy}", VARIABLE.Currency.ToString());
                        await _emailService.SendEmailAsync(new EmailModel()
                            { Email = email, Message = message, Subject = subject });
                      
                    }  
                }
                return true;
            }

            return false;
        }

        private async Task<(string subject,string message)> GetEmail()
        {
            try
            {
                var email = _configuration.GetSection("LimitEmail");
                string? subject = email["Subject"].ToString();
                string? message = email["Message"].ToString();
                return (subject, message);
            }
            catch (Exception ex)
            {
               _logger.LogError("Exseption ex:{ex}",ex.Message);
               return (null,null);
            }
        }
    }
    
}
