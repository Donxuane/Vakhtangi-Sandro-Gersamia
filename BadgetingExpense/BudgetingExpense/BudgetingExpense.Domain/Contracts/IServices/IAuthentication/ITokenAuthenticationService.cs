namespace BudgetingExpense.Domain.Contracts.IServices.IAuthentication;

public interface ITokenAuthenticationService
{
    public Task<string> GenerateRefreshToken(string userId);
    public Task<string>? GenerateJwtTokenAsync(string userId, string userRole);
    public Task<bool> SaveTokensInMemory(string token, string refreshToken);
    public Task<bool> ValidateTokens(string token, string refreshToken);
    public Task DeleteTokens(string refreshToken);
}
