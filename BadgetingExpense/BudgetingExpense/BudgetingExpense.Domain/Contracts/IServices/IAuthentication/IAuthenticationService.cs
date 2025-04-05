using BudgetingExpense.Domain.Models.AuthenticationModels;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.IAuthentication;

public interface IAuthenticationService
{
    public Task<string?> LoginUserServiceAsync(Login user);
    public Task AddUserRolesAsync(string email, string role);
    public Task<bool> VerifyUserEmailAsync(string email, string verificationCode);
    public bool CacheNewUserCredentialsInMemory(Register user);
    public Task<string> GenerateRefreshToken(string userId);
    public Task<string>? GenerateJwtTokenAsync(string userId, string userRole);
}
