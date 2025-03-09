using BudgetingExpense.Domain.Models.AuthenticationModels;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.IAuthenticationService;

public interface IAuthenticationService
{
    public Task<bool> LoginUserServiceAsync(Login user);
    public Task<bool> RegisterUserServiceAsync(Register user);
    public Task<string> GenerateJwtTokenAsync(string userId, string userRole);
    public Task AddUserRolesAsync(string email, string role);
    public Task<IList<string>?> GetRoleAsync(string email);
    public Task<User?> GetUserAsync(string email);
}
