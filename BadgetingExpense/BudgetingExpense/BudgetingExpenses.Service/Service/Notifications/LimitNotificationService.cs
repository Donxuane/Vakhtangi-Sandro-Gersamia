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
        try
        {
            var getBudgetPlaningView = await _unitOfWork.BudgetPlaningRepository.GetBudgetPlaningViewAsync(userId);
            foreach (var budgetItem in getBudgetPlaningView)
            {
                if (budgetItem.DateAdded.AddMonths(budgetItem.LimitPeriod) >= DateTime.Now)
                {
                    var enabled = await _unitOfWork.GetRepository.GetNotificationActiveStatusAsync(userId);
                    if (enabled)
                    {
                        var check = int.TryParse(_configuration.GetSection("PercentageToNotifyLimits")["Percentage"], out int percentage);
                        double actualPercentage = check == true ? (percentage / 100.0) : 0.9;
                        if (budgetItem.TotalExpenses >= budgetItem.LimitAmount * actualPercentage)
                        {
                            var email = await _unitOfWork.GetRepository.GetEmailAsync(userId);
                            var categoryName = await _unitOfWork.GetRepository.GetCategoryNameAsync(budgetItem.CategoryId);
                            var (subject, templateMessage) = await GetEmailPattern();
                            var message = templateMessage
                                .Replace("{optional}","\n")
                                .Replace("{category}", string.IsNullOrEmpty(categoryName) ? "Undefined" : categoryName)
                                .Replace("{amount}", budgetItem.TotalExpenses.ToString())
                                .Replace("{currency}", budgetItem.Currency.ToString())
                                .Replace("{limitAmount}", budgetItem.LimitAmount.ToString())
                                .Replace("{LimitCurrency}", budgetItem.Currency.ToString())
                                .Replace("{date}", DateTime.Now.ToString());

                            if (budgetItem.TotalExpenses <= budgetItem.LimitAmount)
                            {
                                message = message.Replace("{optional}", $"your expense exceeded {percentage}% of of your limit");
                            }
                            await _emailService.SendEmailAsync(new EmailModel()
                            {
                                Email = email,
                                Message = message,
                                Subject = subject
                            });

                        }
                    }
                }
                if (budgetItem.TotalExpenses > budgetItem.LimitAmount || budgetItem.DateAdded.AddMonths(budgetItem.LimitPeriod) < DateTime.Now)
                {
                    await _unitOfWork.LimitsRepository.DeleteLimitsAsync(budgetItem.Id, budgetItem.UserId);
                }
            }
            return true;
        }catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
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
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }
}

