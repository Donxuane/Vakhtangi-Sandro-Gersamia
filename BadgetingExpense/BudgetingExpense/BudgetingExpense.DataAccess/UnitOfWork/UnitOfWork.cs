using BudgetingExpense.Domain.Contracts.IRepository.IFinanceRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IRepository.IReportsRepository;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using System.Data.Common;

namespace BudgetingExpense.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAuthentication _authentication;
    private readonly IManageFinancesRepository<Expense> _expenseManage;
    private readonly IManageFinancesRepository<Income> _incomeManage;
    private readonly IIncomeRecordsRepository _incomerecords;
    private readonly DbConnection _connection;
    private DbTransaction? _transaction;

    public UnitOfWork(IAuthentication authentication, DbConnection connection,
        IManageFinancesRepository<Expense> expenseManage, IManageFinancesRepository<Income> incomeManage,
        IIncomeRecordsRepository incomeRecords)
    {
        _expenseManage = expenseManage;
        _incomeManage = incomeManage;
        _authentication = authentication;
        _incomerecords = incomeRecords;

        _connection = connection;
    }

    public IAuthentication Authentication => _authentication;

    public IManageFinancesRepository<Expense> ExpenseManage 
    {
        get 
        {
            if (_transaction != null)
            {
                _expenseManage.SetTransaction(_transaction);
            }
            return _expenseManage;
        } 
    }
    public IManageFinancesRepository<Income> IncomeManage 
    {
        get
        {
            if (_transaction != null)
            {
                _incomeManage.SetTransaction(_transaction);
            }
            return _incomeManage;
        } 
    }

    public IIncomeRecordsRepository IncomeRecords => _incomerecords;

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

    public async Task BeginTransaction()
    {
        try
        {
            if(_connection != null) 
            {
                await _connection.OpenAsync();
                _transaction = await _connection.BeginTransactionAsync();
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new InvalidOperationException("Database Connection Lost");
        }
    }
}
