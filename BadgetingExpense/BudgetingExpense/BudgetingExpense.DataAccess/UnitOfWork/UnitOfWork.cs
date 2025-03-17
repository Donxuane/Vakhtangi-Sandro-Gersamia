using BudgetingExpense.Domain.Contracts.IRepository.IFinance;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitations;
using BudgetingExpense.Domain.Contracts.IRepository.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;
using System.Data.Common;
using BudgetingExpense.Domain.Contracts.IRepository.INotifications;

namespace BudgetingExpense.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly Func<IAuthentication> _authenticationFactory;
    private readonly Func<IManageFinancesRepository<Expense>> _expenseManageFactory;
    private readonly Func<IManageFinancesRepository<Income>> _incomeManageFactory;
    private readonly Func<IIncomeRecordsRepository> _incomeRecordsFactory;
    private readonly Func<IExpenseRecordsRepository> _expenseRecordsFactory;
    private readonly Func<IBudgetLimitsRepository> _limitsFactory;
    private readonly Func<IGetRepository> _getRepositoryFactory;
    private readonly Func<IToggleNotificationsRepository> _toggleNotificationsFactory;
    private readonly Func<IBudgetPlaningRepository> _planingFactory;

    private IAuthentication? _authentication;
    private IManageFinancesRepository<Expense>? _expenseManage;
    private IManageFinancesRepository<Income>? _incomeManage;
    private IIncomeRecordsRepository? _incomeRecords;
    private IExpenseRecordsRepository? _expenseRecords;
    private IBudgetLimitsRepository? _limits;
    private IGetRepository? _getRepository;
    private IToggleNotificationsRepository? _toggleNotifications;
    private IBudgetPlaningRepository? _planing;

    private readonly DbConnection _connection;
    private DbTransaction? _transaction;

    public UnitOfWork(Func<IAuthentication> authentication, DbConnection connection,
        Func<IManageFinancesRepository<Expense>> expenseManage, Func<IManageFinancesRepository<Income>> incomeManage,
        Func<IIncomeRecordsRepository> incomeRecords, Func<IBudgetLimitsRepository> limits,
        Func<IExpenseRecordsRepository> expenseRecords,
        Func<IGetRepository> getRepository,Func<IToggleNotificationsRepository> toggleNotifications,
        Func<IBudgetPlaningRepository> planing)
    {
        _expenseManageFactory = expenseManage;
        _incomeManageFactory = incomeManage;
        _authenticationFactory = authentication;
        _incomeRecordsFactory = incomeRecords;
        _limitsFactory = limits;
        _expenseRecordsFactory = expenseRecords;
        _connection = connection;
        _getRepositoryFactory = getRepository;
        _toggleNotificationsFactory = toggleNotifications;
        _planingFactory = planing;
    }

    public IAuthentication Authentication => _authentication??=_authenticationFactory();

    public IManageFinancesRepository<Expense> ExpenseManage
    {
        get
        {
            if (_transaction != null)
            {
                _expenseManage ??= _expenseManageFactory();
                _expenseManage.SetTransaction(_transaction);
            }
            return _expenseManage??=_expenseManageFactory();
        }
    }
    public IManageFinancesRepository<Income> IncomeManage
    {
        get
        {
            if (_transaction != null)
            {
                _incomeManage ??= _incomeManageFactory();
                _incomeManage.SetTransaction(_transaction);
            }
            return _incomeManage??=_incomeManageFactory();
        }
    }

    public IIncomeRecordsRepository IncomeRecords => _incomeRecords??=_incomeRecordsFactory();

    public IBudgetLimitsRepository LimitsRepository { get
        {
            if (_transaction != null)
            {
                _limits ??= _limitsFactory();
                _limits.SetTransaction(_transaction);
            }
            return _limits??=_limitsFactory();
        }
    }

    public IExpenseRecordsRepository ExpenseRecords => _expenseRecords??=_expenseRecordsFactory();

    public IGetRepository GetRepository => _getRepository??=_getRepositoryFactory();
    public IToggleNotificationsRepository ToggleNotificationsRepository { get
        {
            if (_transaction != null)
            {
                _toggleNotifications ??= _toggleNotificationsFactory();
                _toggleNotifications.SetTransaction(_transaction);
            }
            return _toggleNotifications??=_toggleNotificationsFactory();
        } 
    }

    public IBudgetPlaningRepository BudgetPlaningRepository =>_planing??=_planingFactory();


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

    public async Task BeginTransactionAsync()
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
