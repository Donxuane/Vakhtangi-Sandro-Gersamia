using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Contracts.IServices.INotifyUser;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Configuration;

namespace BudgetingExpenses.Service.Service.NotifyUser;

public class IncomeReceiveNotificationService : IIncomeReceiveNotificationService
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    public IncomeReceiveNotificationService(IEmailService emailService, 
        IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<bool> NotifyIncomeAsync(Income record)
    {
        try
        {
            var userEmail = await _unitOfWork.GetRepository.GetEmail(record.UserId);
            if (userEmail != null)
            {
                var incomeEmail = _configuration.GetSection("IncomeEmail");
                string? subject = incomeEmail["Subject"].ToString();
                string? message = incomeEmail["Message"].ToString();
                var categoryName = await _unitOfWork.GetRepository.GetCategoryName((int)record.CategoryId);
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
                await _emailService.SendEmail(new EmailModel { Email = userEmail, Message = message, Subject = subject });
                return true;
            }
            return false;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
