using BudgetingExpense.DataAccess.Repository;
using BudgetingExpense.Domain.Contracts.IRepository.IFinanceRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAuthentication _authentication;
    private IManageFinancesRepository<Expense> _expenseManage;
    private IManageFinancesRepository<Income> _incomeManage;
    private readonly DbConnection _connection;
    private DbTransaction _transaction;

    public UnitOfWork(IAuthentication authentication, DbConnection connection,
        IManageFinancesRepository<Expense> expenseManage, IManageFinancesRepository<Income> incomeManage)
    {
        _expenseManage = expenseManage;
        _incomeManage = incomeManage;
        _authentication = authentication;
        _connection = connection;
        _connection.Open();
        _transaction = _connection.BeginTransaction();


        _expenseManage.SetTransaction(_transaction);
        _incomeManage.SetTransaction(_transaction);
    }

    public IAuthentication Authentication => _authentication;

    public IManageFinancesRepository<Expense> ExpenseManage => _expenseManage;
    public IManageFinancesRepository<Income> IncomeManage => _incomeManage;

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
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
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
