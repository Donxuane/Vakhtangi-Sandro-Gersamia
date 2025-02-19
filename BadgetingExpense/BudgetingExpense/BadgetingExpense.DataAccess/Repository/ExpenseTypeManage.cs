using BudgetingExpense.Domain.Contracts.IRepository;
using BudgetingExpense.Domain.Models;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Text;

namespace BudgetingExpense.DataAccess.Repository;

public class ExpenseTypeManage : IManageFinances<UserExpenses>
{
    private readonly DbConnection _connection;
    private readonly DbTransaction _transaction;

    public ExpenseTypeManage(DbConnection connection, DbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task Add(UserExpenses model)
    {
        var query = "INSERT INTO UserExpense(Currency,ExpenseType,UserId)" +
            "Values(@Currency,@ExpenseType,@UserId)";
        await _connection.ExecuteAsync(query, new {model.Currency, model.ExpenseType, 
            model.UserId},_transaction);
    }

    public async Task Delete(int Id)
    {
        var query = "DELETE FROM UserExpense WHERE Id = @Id";
        await _connection.ExecuteAsync(query, new { Id }, _transaction);
    }

    public async Task<IEnumerable<UserExpenses>> GetAll(string UserId)
    {
        var query = "SELECT FROM UserExpense WHERE UserId = @UserId";
        var collection = await _connection.QueryAsync<UserExpenses>(query, new {UserId}, _transaction);
        return collection;
    }

    public async Task Update(UserExpenses model)
    {
        if (model.UserId != null)
        {
            var updateValues = new StringBuilder();
            var parameteres = new DynamicParameters();
            if (model.Currency != null && model.Currency != string.Empty)
            {
                updateValues.Append("Currency = @Currency,");
                parameteres.Add("Currency", model.Currency);
            }
            if (model.ExpenseType != null && model.ExpenseType != string.Empty)
            {
                updateValues.Append("ExpenseType = @ExpenseType,");
                parameteres.Add("ExpenseType", model.ExpenseType);
            }
            if (updateValues != null && updateValues.Length > 0)
            {
                updateValues.Length -= 1;
                var query = $"UPDATE UserExpense SET {updateValues} WHERE UserId = @UserId AND Id = @Id";
                parameteres.Add("UserId", model.UserId);
                parameteres.Add("Id", model.Id);
                await _connection.ExecuteAsync(query, parameteres, _transaction);
            }
        }
    }
}
