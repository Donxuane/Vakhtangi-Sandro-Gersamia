using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;

namespace BudgetingExpense.Domain.Contracts;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
    public Task RollBackAsync();
    public Task BeginTransactionAsync();
    public IAuthentication Authentication { get; }
}
