namespace BudgetingExpense.Domain.Contracts.IServices.INotifications;

public interface IOptionalNotificationService
{
    public Task<bool> NotifyLimitsStatusAsync(string userId);
}
