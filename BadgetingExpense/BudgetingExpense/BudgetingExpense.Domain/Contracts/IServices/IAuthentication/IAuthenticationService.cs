using BudgetingExpense.Domain.Models.AuthenticationModels;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.IAuthentication;

public interface IAuthenticationService
{
    public Task<string?> LoginUserServiceAsync(Login user);
    public Task AddUserRolesAsync(string email, string role);
    public Task<User?> GetUserAsync(string email);
}
