using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpenses.Service.IServiceContracts;

public interface IAuthenticationService
{
    public Task<bool> LoginUserServiceAsync(LoginDto user);
    public Task<bool> RegisterUserServiceAsync(RegisterDto user);
    public Task<string> GenerateJwtTokenAsync(string userId, string userRole);
}
