using BudgetingExpense.Domain.Contracts.IRepository.IFinanceRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitsRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IReportsRepository.IExpenseReportsRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IReportsRepository.IIncomeReportsRepository;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using System.Data.Common;
using BudgetingExpense.Domain.Contracts.IRepository.IBudgetPlaningRepository;

namespace BudgetingExpense.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAuthentication _authentication;
    private readonly IManageFinancesRepository<Expense> _expenseManage;
    private readonly IManageFinancesRepository<Income> _incomeManage;
    private readonly IIncomeRecordsRepository _incomeRecords;
    private readonly IExpenseRecordsRepository _expenseRecords;
    private readonly ILimitsRepository _limits;
    private readonly IBudgetPlaningRepository _budgetPlaning;
    private readonly DbConnection _connection;
    private DbTransaction? _transaction;

    public UnitOfWork(IAuthentication authentication, DbConnection connection,
        IManageFinancesRepository<Expense> expenseManage, IManageFinancesRepository<Income> incomeManage,
        IIncomeRecordsRepository incomeRecords, ILimitsRepository limits,
        IExpenseRecordsRepository expenseRecords, IBudgetPlaningRepository budgetPlaning)
    {
        _expenseManage = expenseManage;
        _incomeManage = incomeManage;
        _authentication = authentication;
        _incomeRecords = incomeRecords;
        _limits = limits;
        _expenseRecords = expenseRecords;

        _connection = connection;
        _budgetPlaning = budgetPlaning;
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

    public IIncomeRecordsRepository IncomeRecords => _incomeRecords;

    public ILimitsRepository LimitsRepository { get
        {
            if (_transaction != null)
            {
                _limits.SetTransaction(_transaction);
            }
            return _limits;
        }
    }

    public IExpenseRecordsRepository ExpenseRecords => _expenseRecords;
    public IBudgetPlaningRepository BudgetPlanning => _budgetPlaning;

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
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }
            _transaction = await _connection.BeginTransactionAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new InvalidOperationException("Database Connection Lost");
        }
    }
}
