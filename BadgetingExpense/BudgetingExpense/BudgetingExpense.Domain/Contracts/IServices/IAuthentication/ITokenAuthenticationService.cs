namespace BudgetingExpense.Domain.Contracts.IServices.IAuthentication;

public interface ITokenAuthenticationService
{
    public Task<string> GenerateRefreshToken(string userId);
    public Task<string>? GenerateJwtTokenAsync(string userId, string userRole);
}
