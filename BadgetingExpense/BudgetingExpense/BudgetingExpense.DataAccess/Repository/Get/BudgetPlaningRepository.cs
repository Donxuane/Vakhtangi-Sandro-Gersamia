using System.Data.Common;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using Dapper;

namespace BudgetingExpense.DataAccess.Repository.Get;

public class BudgetPlaningRepository:IBudgetPlaningRepository

{
  private readonly DbConnection _connection;

  public BudgetPlaningRepository(DbConnection connection)
  {
      _connection = connection;
  }

    public async Task<IEnumerable<BudgetPlanning>> GetBudgetPlaningViewAsync(string userId)
    {
        var query = "SELECT * FROM BudgetPlaning WHERE UserId =@UserId;";
        return await _connection.QueryAsync<BudgetPlanning>(query, new { userId });
    }
}
