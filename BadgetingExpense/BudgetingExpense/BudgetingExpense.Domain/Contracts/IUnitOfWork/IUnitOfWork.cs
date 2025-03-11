using BudgetingExpense.Domain.Contracts.IRepository.IFinance;
using BudgetingExpense.Domain.Contracts.IRepository.IGet;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitations;
using BudgetingExpense.Domain.Contracts.IRepository.INotifications;
using BudgetingExpense.Domain.Contracts.IRepository.IReports;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IUnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    public Task SaveChangesAsync();
    public Task RollBackAsync();
    public Task BeginTransactionAsync();
    public IAuthentication Authentication { get; }
    public IManageFinancesRepository<Expense> ExpenseManage { get; }
    public IManageFinancesRepository<Income> IncomeManage { get; }
    public IIncomeRecordsRepository IncomeRecords {  get; }
    public IBudgetLimitsRepository LimitsRepository { get; }
    public IExpenseRecordsRepository ExpenseRecords { get; }
    public IBudgetPlaningRepository BudgetPlanning { get; }
    public IGetRepository GetRepository { get; }
    public IToggleNotificationsRepository ToggleNotificationsRepository { get; }
}
