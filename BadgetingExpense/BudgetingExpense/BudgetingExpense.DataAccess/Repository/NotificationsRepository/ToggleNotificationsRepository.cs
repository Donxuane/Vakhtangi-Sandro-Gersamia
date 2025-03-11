using System.Data.Common;
using BudgetingExpense.Domain.Contracts.IRepository.INotifications;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.NotificationsRepository;

public class ToggleNotificationsRepository : IToggleNotificationsRepository
{
    private readonly DbConnection _connection;
    private DbTransaction? _transaction;
    public ToggleNotificationsRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public void SetTransaction(DbTransaction transaction)
    {
        _transaction = transaction;
    }

    public async  Task ToggleNotification(string userId, bool status)
    {
        var query = "UPDATE AspNetUsers SET Notifications = @status WHERE Id = @UserId";
        await _connection.QueryAsync(query, new { status, userId }, _transaction);
    }
}
