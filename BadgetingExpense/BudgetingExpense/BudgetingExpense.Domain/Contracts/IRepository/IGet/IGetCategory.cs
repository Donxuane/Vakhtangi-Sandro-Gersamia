namespace BudgetingExpense.Domain.Contracts.IRepository.IGet;

public interface IGetCategory
{
   public Task<int> GetCategoryTypeAsync(int categoryId);
}
