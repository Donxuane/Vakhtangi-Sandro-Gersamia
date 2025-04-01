using System.Data.Common;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitations;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.LimitsRepository;

public class BudgetPlaningRepository : IBudgetPlaningRepository
{
    private readonly DbConnection _connection;

    public BudgetPlaningRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<BudgetPlanning>> GetBudgetPlaningViewAsync(string userId)
    {
        var query = "SELECT * FROM BudgetPlaning WHERE UserId =@UserId;";
        return await _connection.QueryAsync<BudgetPlanning>(query, new { userId });
    }

    public async Task<IEnumerable<LimitationsView>> GetLimitsInfo(string userId)
    {
        var query = "SELECT LimitAmount,LimitPeriod,DateAdded,CategoryId," +
            "TotalExpenses,ExpenseCount,Currency FROM BudgetPlaning WHERE UserId = @userId";
        return await _connection.QueryAsync<LimitationsView>(query, new { userId });
    }
}
