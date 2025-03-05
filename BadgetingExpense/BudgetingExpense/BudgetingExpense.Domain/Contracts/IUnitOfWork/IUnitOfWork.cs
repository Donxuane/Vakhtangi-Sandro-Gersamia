using BudgetingExpense.Domain.Contracts.IRepository.IFinanceRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Contracts.IRepository.ILimitationsRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IReportsRepository;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IUnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    public Task SaveChangesAsync();
    public Task RollBackAsync();
    public Task BeginTransaction();
    public IAuthentication Authentication { get; }
    public IManageFinancesRepository<Expense> ExpenseManage { get; }
    public IManageFinancesRepository<Income> IncomeManage { get; }
    public IIncomeRecordsRepository IncomeRecords {  get; }
    public IBudgetLimitsRepository LimitsRepository { get; }
    public IExpenseRecordsRepository ExpenseRecords { get; }
    public IBudgetPlaningRepository BudgetPlanning { get; }
    
}
