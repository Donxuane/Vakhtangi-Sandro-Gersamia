using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.INotifications;

public interface IExpenseAddedNotificationService
{
    public Task<bool> SendEmailWhileExpenseAddedAsync(Expense record);
}
