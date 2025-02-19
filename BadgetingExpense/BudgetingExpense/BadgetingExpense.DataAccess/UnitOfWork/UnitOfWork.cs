using BudgetingExpense.DataAccess.Repository;
using BudgetingExpense.Domain.Contracts;
using BudgetingExpense.Domain.Contracts.IRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Models;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAuthentication _authentication;
    private IManageFinances<UserExpenses> _expenseManage;
    private IManageFinances<UserIncome> _incomeManage;
    private readonly DbConnection _connection;
    private DbTransaction _transaction;
    public UnitOfWork(IAuthentication authentication, DbConnection connection)
    {
        _authentication = authentication;
        _connection = connection;
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }
    public IAuthentication Authentication { get => _authentication; }

    public IManageFinances<UserExpenses> ExpenseTypeManage
    {
        get
        {
            return _expenseManage = new ExpenseTypeManage(_connection, _transaction);
        }
    }

    public IManageFinances<UserIncome> IncomeTypeManage
    {
        get
        {
            return _incomeManage = new IncomeTypeManage(_connection, _transaction);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_transaction != null)
                await _transaction.DisposeAsync();
        }
        finally
        {
            await _connection.DisposeAsync();
        }
    }

    public async Task RollBackAsync()
    {
        try
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
