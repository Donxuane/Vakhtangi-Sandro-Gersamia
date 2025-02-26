using BudgetingExpense.Domain.Models;
using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpenses.Service.IServiceContracts;

public interface IAuthenticationService
{
    public Task<bool> LoginUserServiceAsync(LoginDto user);
    public Task<bool> RegisterUserServiceAsync(RegisterDto user);
    public Task<string> GenerateJwtTokenAsync(string userId, string userRole);
    public Task AddUserRolesAsync(string email, string role);
    public Task<IList<string>?> GetRoleAsync(string email);
    public Task<User?> GetUserAsync(string email);
}
