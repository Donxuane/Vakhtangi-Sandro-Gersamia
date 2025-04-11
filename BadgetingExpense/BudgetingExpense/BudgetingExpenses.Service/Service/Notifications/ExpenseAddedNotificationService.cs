using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IServices.INotifications;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Notifications;

public class ExpenseAddedNotificationService : IExpenseAddedNotificationService
{
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpenseAddedNotificationService> _logger;
    public ExpenseAddedNotificationService(IConfiguration configuration, IEmailService emailService
        ,ILogger<ExpenseAddedNotificationService> logger, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _emailService = emailService;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> SendEmailWhileExpenseAddedAsync(Expense record)
    {
        try
        {
            var userEmail = await _unitOfWork.GetRepository.GetEmailAsync(record.UserId);
            if (userEmail != null)
            {
                var incomeEmail = _configuration.GetSection("ExpenseEmail");
                string? subject = incomeEmail["Subject"].ToString();
                string? template = incomeEmail["Message"].ToString();
                var categoryName = await _unitOfWork.GetRepository.GetCategoryNameAsync((int)record.CategoryId);
                var message = template
                    .Replace("{category}", string.IsNullOrEmpty(categoryName) ? "Undefined" : categoryName)
                    .Replace("{amount}", record.Amount.ToString())
                    .Replace("{currency}", record.Currency.ToString())
                    .Replace("{date}", record.Date.ToString());
                await _emailService.SendEmailAsync(new EmailModel { Email = userEmail, Message = message, Subject = subject });
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }
}
