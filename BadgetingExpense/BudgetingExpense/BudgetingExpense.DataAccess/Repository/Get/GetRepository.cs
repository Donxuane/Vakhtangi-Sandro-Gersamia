using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using Dapper;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Repository.Get;

public class GetRepository : IGetRepository
{
    private readonly DbConnection _connection;

    public GetRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<string?> GetCategoryName(int categoryId)
    {
        var query = "SELECT Name FROM Categories WHERE Id = @categoryId";
        var categoryName = await _connection.QuerySingleAsync<string>(query, new {categoryId});
        return categoryName;
    }

    public async Task<string> GetEmail(string UserId)
    {
        var query = "SELECT Email FROM AspNetUsers Where Id =@UserId";
        return await _connection.QuerySingleAsync<string>(query, new { UserId });
    }
}
