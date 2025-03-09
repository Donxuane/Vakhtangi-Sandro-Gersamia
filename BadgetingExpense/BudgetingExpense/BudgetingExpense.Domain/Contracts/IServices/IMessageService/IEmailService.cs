using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.IMessageService;

public interface IEmailService
{
    public Task SendEmail(EmailModel emailModel);
}
