using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.INotifyUser;

public interface IIncomeReceiveNotificationService
{
    public Task<bool> NotifyIncomeAsync(Income record);
}
