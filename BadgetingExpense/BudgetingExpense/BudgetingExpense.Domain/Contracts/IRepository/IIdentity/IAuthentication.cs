using BudgetingExpense.Domain.Models;

namespace BudgetingExpense.Domain.Contracts.IRepository.IIdentity;

public interface IAuthentication
{
    public Task<bool> CheckUserAsync(string email, string password);
    public Task<bool> RegisterUserAsync(User user);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<IList<string>?> GetUserRolesAsync(string email);
    public Task AddUserRoleAsync(string email, string role);
}
