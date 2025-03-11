namespace BudgetingExpense.Domain.Contracts.IServices.INotifications;

public interface IToggleNotificationsService
{
    public Task<bool> ToggleNotificationsAsync(string userId,bool status);
}
