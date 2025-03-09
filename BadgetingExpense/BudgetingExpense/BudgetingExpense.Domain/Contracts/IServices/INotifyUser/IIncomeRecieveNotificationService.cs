using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.INotifyUser;

public interface IIncomeRecieveNotificationService
{
    public Task<bool> NotifyIncomeAsync(Income record);
}
