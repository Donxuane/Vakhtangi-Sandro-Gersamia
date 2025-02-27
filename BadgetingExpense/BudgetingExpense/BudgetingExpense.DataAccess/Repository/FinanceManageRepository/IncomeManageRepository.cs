using BudgetingExpense.Domain.Contracts.IRepository.IFinanceRepository;
using BudgetingExpense.Domain.Models.MainModels;
using Dapper;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Repository.FinanceManageRepository;

public class IncomeManageRepository : IManageFinancesRepository<Income>
{
    private readonly DbConnection _connection;
    private DbTransaction? _transaction;

    public IncomeManageRepository(DbConnection connection)
    {
        _connection = connection;
    }
    public void SetTransaction(DbTransaction transaction)
    {
        _transaction ??= transaction;
    }

    public async Task AddAsync(Income model)
    {
        var query = "INSERT INTO Incomes(Currency,Amount,CategoryId,[Date],UserId)" +
            "Values(@Currency,@Amount,@CategoryId,@Date,@UserId)";
        await _connection.ExecuteAsync(query, new
        {
            model.Currency,
            model.Amount,
            model.CategoryId,
            model.Date,
            model.UserId
        }, _transaction);
    }

    public async Task DeleteAsync(int Id)
    {
        var query = "DELETE FROM Incomes Where Id = @Id";
        await _connection.ExecuteAsync(query, new { Id }, _transaction);
    }

    public async Task<IEnumerable<Income>> GetAllAsync(string UserId)
    {
        var query = "SELECT FROM Incomes WHERE UserId = @UserId";
        var collection = await _connection.QueryAsync<Income>(query, new { UserId }, _transaction);
        return collection;
    }

    public Task<int> AddCategoryAsync(Category category)
    {
        var query = "INSERT INTO Categories(Name,Type)OUTPUT INSERTED.Id VALUES(@Name,@Type)";
        var id = _connection.QuerySingleAsync<int>(query, new { category.Name, Type = category.Type = 1 }, _transaction);
        return id;
    }

    public Task<IEnumerable<Category>> GetCategoriesAsync(string userId)
    {
        var query = @"SELECT DISTINCT c.Id,c.Name,c.Type
                      From Categories c
                      JOIN Incomes i ON c.Id = i.CategoryId WHERE i.UserId = @userId";
        var incomeCategories = _connection.QueryAsync<Category>(query, new { userId }, _transaction);
        return incomeCategories;
    }

    public async Task UpdateCategoryAsync(Category category)
    {

      
        var query = "UPDATE Categories SET Name = @Name ,Type = @Type WHERE Id = @Id ";
        await _connection.ExecuteAsync(query, new { category.Name,Type =category.Type = 1,Id = category.Id }, _transaction);

    }

    public async Task UpdateAsync(Income model)
    {
        var query = "UPDATE Incomes SET Currency=@Currency,Amount=@Amount,Date=@Date WHERE UserId = @UserId AND Id = @Id";
        await _connection.ExecuteAsync(query, new { model.Currency, model.Amount, model.Date, model.UserId, model.Id }, _transaction);
    }
}
