using System.Data.Common;

namespace BudgetingExpense.Domain.Contracts.IRepository.INotifications;

public interface IToggleNotificationsRepository
{
    public Task ToggleNotification(string userId, bool status);
    public void SetTransaction(DbTransaction transaction);

}
