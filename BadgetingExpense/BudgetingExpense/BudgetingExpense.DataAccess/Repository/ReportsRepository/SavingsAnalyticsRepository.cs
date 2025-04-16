using BudgetingExpense.Domain.Contracts.IRepository.IReports;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using Dapper;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Repository.ReportsRepository;

public class SavingsAnalyticsRepository : ISavingsRepository
{
    private readonly DbConnection _connection;
    private DbTransaction? _transaction; 

    public SavingsAnalyticsRepository(DbConnection connection)
    {
        _connection = connection;
    }
    public void SetTransaction(DbTransaction transaction)
    {
        _transaction = transaction;
    }
    public async Task<IEnumerable<SavingsPeriod>> GetSavingsAnalyticsAsync(string userId, int? period)
    {
        return await _connection.QueryAsync<SavingsPeriod>("SavingsAnalyticsProcedure", new { userId, period },
            _transaction, commandType: System.Data.CommandType.StoredProcedure);
    }
}
