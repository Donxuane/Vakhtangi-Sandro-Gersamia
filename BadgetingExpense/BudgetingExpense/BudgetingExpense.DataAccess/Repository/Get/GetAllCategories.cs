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
    public int GetCategoryTypeAsync(int categoryId)
    {
        var query = "Select Type FROM Categories WHERE Id = @CategoryId";
        return _connection.QuerySingle<int>(query, new { categoryId });
    }
}
