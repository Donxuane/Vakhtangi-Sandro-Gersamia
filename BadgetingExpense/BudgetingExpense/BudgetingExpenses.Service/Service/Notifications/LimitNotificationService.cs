using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Notifications;

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
            var (subject, templateMessage) = await GetEmailPattern();

            foreach (var budgetItem in getBudgetPlaningView)
            {
                if (budgetItem.LimitAmount < budgetItem.TotalExpenses || budgetItem.TotalExpenses >= budgetItem.LimitAmount * 0.9)
                {
                    var categoryName = await _unitOfWork.GetRepository.GetCategoryNameAsync(budgetItem.CategoryId);

                 
                    var message = templateMessage
                        .Replace("{category}", string.IsNullOrEmpty(categoryName) ? "Undefined" : categoryName)
                        .Replace("{amount}", budgetItem.TotalExpenses.ToString())
                        .Replace("{currency}", budgetItem.Currency.ToString())
                        .Replace("{limitAmount}", budgetItem.LimitAmount.ToString())
                        .Replace("{LimitCurrency}", budgetItem.Currency.ToString());

                    if (budgetItem.TotalExpenses >= budgetItem.LimitAmount * 0.9)
                    {
                        message = message.Replace("{category}", "you expense exceeded 90% of of your limit");
                    }
                    await _emailService.SendEmailAsync(new EmailModel()
                    {
                        Email = email,
                        Message = message,
                        Subject = subject
                    });
                    
                }
            }
            return true;
        }

        return false;
    }

    private async Task<(string? subject,string? message)> GetEmailPattern()
    {
        
        try
        {
            return await Task.Run(() =>
            {
                var email = _configuration.GetSection("LimitEmail");
                string? subject = email["Subject"].ToString();
                string? message = email["Message"].ToString();
                return (subject, message);
            });
        }
        catch (Exception ex)
        {
           _logger.LogError("Exception ex:{ex}",ex.Message);
           return (null,null);
        }
    }
}

