using BudgetingExpense.Domain.Contracts.IRepository;
using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Models;

namespace BudgetingExpense.Domain.Contracts;

public interface IUnitOfWork : IAsyncDisposable
{
    public Task SaveChangesAsync();
    public Task RollBackAsync();
    public IAuthentication Authentication { get; }
    public IManageFinances<UserExpenses> ExpenseTypeManage { get; }
    public IManageFinances<UserIncome> IncomeTypeManage {  get; }
}
