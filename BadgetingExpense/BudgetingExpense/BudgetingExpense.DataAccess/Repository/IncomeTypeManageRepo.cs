using BudgetingExpense.Domain.Contracts.IRepository;
using BudgetingExpense.Domain.Models;
using Dapper;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Repository;

public class IncomeTypeManageRepo : IManageFinancesRepository<Income>
{
    private readonly DbConnection _connection;
    private DbTransaction? _transaction;

    public IncomeTypeManageRepo(DbConnection connection)
    {
        _connection = connection;
    }
    public void SetTransaction(DbTransaction transaction)
    {
        _transaction ??= transaction;
    }

    public async Task Add(Income model)
    {
        var query = "INSERT INTO Incomes(Currency,Amount,CategoryId,[Date],UserId)" +
            "Values(@Currency,@Amount,@CategoryId,@Date,@UserId)";
        await _connection.ExecuteAsync(query, new { model.Currency, model.Amount,
            model.CategoryId, model.Date, model.UserId }, _transaction);
    }

    public async Task Delete(int Id)
    {
        var query = "DELETE FROM Incomes Where Id = @Id";
        await _connection.ExecuteAsync(query, new { Id }, _transaction);
    }

    public async Task<IEnumerable<Income>> GetAll(string UserId)
    {
        var query = "SELECT FROM Incomes WHERE UserId = @UserId";
        var collection = await _connection.QueryAsync<Income>(query, new { UserId }, _transaction);
        return collection;
    }

    public Task<int> AddCategory(Category category)
    {
        var query = "INSERT INTO Categories(Name,Type)OUTPUT INSERTED.Id VALUES(@Name,@Type)";
        var id = _connection.QuerySingleAsync<int>(query, new { category.Name, Type = category.Type = 1 });
        return id;
    }

    public Task<IEnumerable<Category>> GetCategories(string userId)
    {
        var query = @"SELECT DISTINCT c.Id,c.Name,c.Type
                      From Categories c
                      JOIN Incomes i ON c.Id = i.CategoryId WHERE i.UserId = @userId";
        var incomeCategories = _connection.QueryAsync<Category>(query, new { userId }, _transaction);
        return incomeCategories;
    }
}
