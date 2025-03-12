using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.INotifications;

public interface IIncomeReceiveNotificationService
{
    public Task<bool> NotifyIncomeAsync(Income record);
}
