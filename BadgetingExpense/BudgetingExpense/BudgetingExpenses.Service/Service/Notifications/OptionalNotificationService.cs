using BudgetingExpense.Domain.Contracts.IServices.INotifications;

namespace BudgetingExpenses.Service.Service.Notifications;

public class OptionalNotificationService : IOptionalNotificationService
{
    public Task<bool> NotifyLimitsStatusAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
