using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.IMessaging;

public interface IEmailService
{
    public Task SendEmailAsync(EmailModel emailModel);
}
