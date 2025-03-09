namespace BudgetingExpense.Domain.Contracts.IRepository.IGet;

public interface IGetRepository
{
    public Task<string> GetEmail(string UserId);
    public Task<string?> GetCategoryName(int categoryId);
}
