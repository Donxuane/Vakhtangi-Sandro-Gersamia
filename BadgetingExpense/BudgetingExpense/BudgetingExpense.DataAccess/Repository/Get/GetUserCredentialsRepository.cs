using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using Dapper;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.Repository.Get;

public class GetUserCredentialsRepository : IGetUserCredentials
{
    private readonly DbConnection _connection;
    public GetUserCredentialsRepository(DbConnection connection)
    {
        _connection = connection;
    }
    public async Task<IEnumerable<string>> GetAllUsersIdiesAsync()
    {
        var query = "SELECT Id From AspNetUsers";
        return await _connection.QueryAsync<string>(query);
    }
}
