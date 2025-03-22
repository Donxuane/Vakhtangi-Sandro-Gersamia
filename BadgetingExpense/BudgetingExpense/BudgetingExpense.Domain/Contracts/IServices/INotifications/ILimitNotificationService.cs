namespace BudgetingExpense.Domain.Contracts.IServices.INotifications;

public interface ILimitNotificationService
{
 public Task<bool> NotifyLimitExceededAsync(string userId);
}
