
using System.Data;
using System.Data.Common;
using BudgetingExpense.Domain.Contracts.IRepository.INotifications;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.NotificationsRepository
{
   public class ToggleNotificationsRepository : IToggleNotificationsRepository
    {
        private readonly DbConnection _connection;
private  DbTransaction _transaction;
        public ToggleNotificationsRepository(DbConnection connection)
        {
            _connection = connection;
        }

       

        public async  Task ToggleNotification(string userId)
        {
            var query = "UPDATE AspNetUsers SET Notifications = @Notifications WHERE UserId = @UserId";
            await _connection.QueryAsync(query, new { userId});

        }
}
