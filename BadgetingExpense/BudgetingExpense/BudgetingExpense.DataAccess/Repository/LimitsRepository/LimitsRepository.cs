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

    public async Task DeleteLimitsAsync(int id)
    {
        var query = "DELETE FROM Limits WHERE Id = @Id";
        await _connection.ExecuteAsync(query, new { id }, _transaction);
    }

    public async Task UpdateLimitsAsync(Limits limits)
    {
        var query = @"UPDATE Limits SET CategoryId = @CategoryId,Amount = @Amount,PeriodCategory = @PeriodCategory WHERE UserId =@UserId AND Id=@Id";
        await _connection.ExecuteAsync(query, new { limits.CategoryId, limits.Amount, limits.PeriodCategory,limits.UserId,limits.Id },
            _transaction);

    }

    public async Task SetLimitAsync(Limits limits)
    {
        var query = "INSERT INTO Limits (UserId,CategoryId,Amount,PeriodCategory,DateAdded)" +
                    "VALUES (@UserId,@CategoryId,@Amount,@PeriodCategory,@DateAdded)";
        await _connection.ExecuteAsync(query, new
        {
            limits.UserId,
            limits.CategoryId,
            limits.Amount,
            limits.PeriodCategory,
            limits.DateAdded
        },_transaction);
    }
}
