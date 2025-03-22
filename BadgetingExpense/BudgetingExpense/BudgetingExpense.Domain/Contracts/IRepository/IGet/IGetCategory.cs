namespace BudgetingExpense.Domain.Contracts.IRepository.IGet;

public interface IGetCategory
{
   public int GetCategoryTypeAsync(int categoryId);
}
