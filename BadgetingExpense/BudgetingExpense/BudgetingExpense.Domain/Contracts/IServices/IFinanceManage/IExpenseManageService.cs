using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;

public interface IExpenseManageService
{
    public Task<bool> AddExpenseAsync(Expense model);
    public Task<bool> DeleteExpenseAsync(int expenseId,string userId);
    public Task<int> AddExpenseCategoryAsync(string categoryName);
    public Task<IEnumerable<Expense>?> GetAllExpenseRecordsAsync(string userId);
    public Task<IEnumerable<Category>?> GetAllExpenseCategoryRecordsAsync(string userId);
    public Task<bool> UpdateExpenseAsync(Update expenseDto);
    public Task<bool> UpdateCategoryAsync(Category Category);

}
