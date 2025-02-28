using BudgetingExpense.Domain.Contracts.IRepository.IReportsRepository;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using Dapper;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Repository.ReportsRepository;

public class IncomeReportsRepository : IIncomeRecordsRepository
{
    private readonly DbConnection _connection;

    public IncomeReportsRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<IncomeRecords>> GetUserIncomeRecords(string userId)
    {
        var query = "SELECT * FROM IncomeCategories WHERE UserId = @UserId";
        var reports = await _connection.QueryAsync<IncomeRecords>(query, new { userId });
        return reports;
    }
}
