using BudgetingExpense.Domain.Models;

namespace BudgetingExpense.Domain.Contracts.IRepository.IIdentity;

public interface IAuthentication
{
    public Task<bool> LoginInAsync(User user);
    public Task<bool> RegisterUserAsync(User user);
}
