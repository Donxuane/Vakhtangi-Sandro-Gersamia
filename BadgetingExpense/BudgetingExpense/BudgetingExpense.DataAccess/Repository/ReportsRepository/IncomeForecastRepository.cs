using System.Data.Common;
using BudgetingExpense.Domain.Contracts.IRepository.IReports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.ReportsRepository;

public class IncomeForecastRepository : IForecastRepository<IncomeRecord>
{
    private readonly DbConnection _connection;

    public IncomeForecastRepository(DbConnection connection)
    {
        _connection = connection;
    }
    public async Task<IEnumerable<IncomeRecord>> GetAll(string userId)
    {
        var query = "SELECT Expected,Currency FROM Incomes WHERE UserId = @UserId";
        var report = await _connection.QueryAsync<IncomeRecord>(query ,new {userId});
        return report;
    }
}
