using BudgetingExpense.Domain.Contracts.IRepository;
using BudgetingExpense.Domain.Models;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Text;

namespace BudgetingExpense.DataAccess.Repository;

public class IncomeTypeManage : IManageFinances<UserIncome>
{
    private readonly DbConnection _connection;
    private readonly DbTransaction _transaction;

    public IncomeTypeManage(DbConnection connection, DbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }
    public async Task Add(UserIncome model)
    {
        var query = "INSERT INTO UserIncome(Currency,IncomeType,UserId)Values(@Currency,@IncomeType,UserId)";
        await _connection.ExecuteAsync(query, new { model.Currency, model.IncomeType, model.UserId }, _transaction);
    }

    public async Task Delete(int Id)
    {
        var query = "DELETE FROM UserIncome Where Id = @Id";
        await _connection.ExecuteAsync(query, new { Id }, _transaction);
    }

    public async Task<IEnumerable<UserIncome>> GetAll(string UserId)
    {
        var query = "SELECT FROM UserIncome WHERE UserId = @UserId";
        var collection = await _connection.QueryAsync<UserIncome>(query, new { UserId }, _transaction);
        return collection;
    }

    public async Task Update(UserIncome model)
    {
        if (model.UserId != null)
        {
            var updateValues = new StringBuilder();
            var parameteres = new DynamicParameters();
            if (model.Currency != null && model.Currency!=string.Empty)
            {
                updateValues.Append("Currency = @Currency,");
                parameteres.Add("Currency", model.Currency);
            }
            if (model.IncomeType != null && model.IncomeType!=string.Empty)
            {
                updateValues.Append("IncomeType = @IncomeType,");
                parameteres.Add("IncomeType", model.IncomeType);
            }
            if (updateValues != null && updateValues.Length>0)
            {
                updateValues.Length -= 1;
                var query = $"UPDATE UserIncome SET {updateValues} WHERE UserId = @UserId AND Id = @Id";
                parameteres.Add("UserId", model.UserId);
                parameteres.Add("Id", model.Id);
                await _connection.ExecuteAsync(query, parameteres, _transaction);
            }
        }
    }
}
