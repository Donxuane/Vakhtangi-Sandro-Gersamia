using System.Data.Common;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitations;
using BudgetingExpense.Domain.Models.MainModels;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.LimitsRepository;

public  class LimitsRepository : IBudgetLimitsRepository
{
    private readonly DbConnection _connection;
    private DbTransaction? _transaction;

    public LimitsRepository(DbConnection connection)
    {
        _connection = connection;
    }
    public void SetTransaction(DbTransaction transaction)
    {
        _transaction ??= transaction;
    }

    public async Task DeleteLimitsAsync(int id, string userId)
    {
        var query = "DELETE FROM Limits WHERE Id = @Id AND UserId = @userId";
        await _connection.ExecuteAsync(query, new { id, userId }, _transaction);
    }

    public async Task UpdateLimitsAsync(Limits limits)
    {
        await _connection.ExecuteAsync("UpdateProcedure", new
        {
            TableName = "Limits",
            limits.Currency,
            limits.Amount,
            limits.CategoryId,
            Date = limits.DateAdded,
            limits.PeriodCategory,
            limits.Id,
            limits.UserId
        },
        _transaction, commandType: System.Data.CommandType.StoredProcedure);

    }

    public async Task AddLimitAsync(Limits limits)
    {
        var query = "INSERT INTO Limits (UserId,CategoryId,Amount,PeriodCategory,DateAdded,Currency)" +
                    "VALUES (@UserId,@CategoryId,@Amount,@PeriodCategory,@DateAdded,@Currency)";
        await _connection.ExecuteAsync(query, new
        {
            limits.UserId,
            limits.CategoryId,
            limits.Amount,
            limits.PeriodCategory,
            limits.DateAdded,
            limits.Currency
        },
        _transaction);
    }
}
