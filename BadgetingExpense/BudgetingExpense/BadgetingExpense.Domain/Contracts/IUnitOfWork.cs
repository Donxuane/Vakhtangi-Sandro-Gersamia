using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;

namespace BudgetingExpense.Domain.Contracts;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
    public Task RollBackAsync();
    public IAuthentication Authentication { get; }
}
