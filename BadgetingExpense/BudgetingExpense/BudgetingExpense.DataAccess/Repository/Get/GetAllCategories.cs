using System.Data.Common;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.Get;

public class GetAllCategories : IGetCategory
{
    private readonly DbConnection _connection;

    public GetAllCategories(DbConnection connection)
    {
        _connection = connection;
    }
    public async Task<int> GetCategoryTypeAsync(int categoryId)
    {
        var query = "Select Type FROM Categories WHERE Id = @CategoryId";
        return  await _connection.QuerySingleAsync<int>(query, new { categoryId });
    }
}
