using BudgetingExpense.Domain.Contracts.IRepository.IReports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using Dapper;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Repository.ReportsRepository;

public class ExpenseReportsRepository : IExpenseRecordsRepository
{
    private readonly DbConnection _connection;

    public ExpenseReportsRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<ExpenseRecord>> GetUserExpenseRecordsAsync(string userId)
    {
        var query = "SELECT * FROM ExpenseCategories WHERE UserId = @userId";
        var reports = await _connection.QueryAsync<ExpenseRecord>(query, new { userId });
        return reports;
    }
}
