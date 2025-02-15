using BudgetingExpense.Domain.Contracts.IRepository.IIdentity;
using BudgetingExpense.Domain.Models;

namespace BudgetingExpense.DataAccess.Repository.Identity;

public class Authentication : IAuthentication
{

    public Task<bool> LoginInAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RegisterUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}
