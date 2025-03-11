namespace BudgetingExpense.Domain.Contracts.IRepository.IGet;

public interface IGetRepository
{
    public Task<string> GetEmailAsync(string UserId);
    public Task<string?> GetCategoryNameAsync(int categoryId);
    public Task<bool> GetNotificationActiveStatusAsync(string userId);
}
