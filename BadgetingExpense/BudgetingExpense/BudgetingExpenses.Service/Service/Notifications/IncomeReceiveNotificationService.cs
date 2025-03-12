using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Notifications;

public class IncomeReceiveNotificationService : IIncomeReceiveNotificationService
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<IncomeReceiveNotificationService> _logger;
    public IncomeReceiveNotificationService(IEmailService emailService,
        IUnitOfWork unitOfWork, IConfiguration configuration,
        ILogger<IncomeReceiveNotificationService> logger)
    {
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> NotifyIncomeAsync(Income record)
    {
        try
        {
            var userEmail = await _unitOfWork.GetRepository.GetEmailAsync(record.UserId);
            if (userEmail != null)
            {
                var incomeEmail = _configuration.GetSection("IncomeEmail");
                string? subject = incomeEmail["Subject"].ToString();
                string? message = incomeEmail["Message"].ToString();
                var categoryName = await _unitOfWork.GetRepository.GetCategoryNameAsync((int)record.CategoryId);
                if (categoryName != null)
                {
                    message = message.Replace("{category}", categoryName);
                }
                else
                {
                    message = message.Replace("{category}", "Undefined");
                }
                message = message.Replace("{amount}", record.Amount.ToString());
                message = message.Replace("{currency}", record.Currency.ToString());
                message = message.Replace("{date}", record.Date.ToString());
                await _emailService.SendEmailAsync(new EmailModel { Email = userEmail, Message = message, Subject = subject });
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return false;
        }
    }
}
