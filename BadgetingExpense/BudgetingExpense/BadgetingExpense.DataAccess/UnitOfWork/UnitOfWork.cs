using BudgetingExpense.Domain.Contracts;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly IAuthentication _authentication;
    private readonly DbConnection _connection;
    private DbTransaction _transaction;
    public UnitOfWork(IAuthentication authentication, DbConnection connection)
    {
        _authentication = authentication;
        _connection = connection;
    }
    public IAuthentication Authentication { get => _authentication; }

    public async Task BeginTransactionAsync()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
        _transaction = await _connection.BeginTransactionAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if(_transaction != null ) 
            await _transaction.DisposeAsync();
        await _connection.DisposeAsync();
    }

    public async Task RollBackAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }

    public async Task SaveChangesAsync()
    {
        if (_transaction != null)
            await _transaction.CommitAsync();
    }
}
