using BudgetingExpense.Domain.Contracts.IRepository.IReports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Repository.ReportsRepository;

public class IncomeReportsRepository : IIncomeRecordsRepository
{
    private readonly DbConnection _connection;
    private readonly IConfiguration _configuration;

    public IncomeReportsRepository(DbConnection connection, IConfiguration configuration)
    {
        _connection = connection;
        _configuration = configuration;
    }

    public async Task<(IEnumerable<IncomeRecord>, int pageAmount)?> AllIncomeRecordsAsync(string userId, int page)
    {

        var rowsAmount = int.TryParse(_configuration.GetSection("PageSize")["rows"], out int pageSize);
        if (rowsAmount)
        {
            var offset = (page - 1) * pageSize;
            var query = "SELECT * FROM IncomeCategories WHERE UserId = @userId ORDER BY IncomeDate DESC " +
                "OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
            var countQuery = "SELECT COUNT(*) AS TotalCount FROM IncomeCategories WHERE UserId = @userId";
            var result = await _connection.QueryAsync<IncomeRecord>(query, new { userId, offset, pageSize });
            var amount = await _connection.QuerySingleAsync<int>(countQuery, new { userId });
            int pages = (int)Math.Ceiling((double)amount / pageSize);
            return (result, pages);
        }
        return null;
    }

    public async Task<IEnumerable<IncomeRecord>> IncomeRecordsAsync(string userId)
    {
        var query = "SELECT * FROM IncomeCategories WHERE UserId = @UserId";
        var reports = await _connection.QueryAsync<IncomeRecord>(query, new { userId });
        return reports;
    }
}
