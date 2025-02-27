using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpenses.Service.IServiceContracts;

public interface IExpenseManageService
{
    public Task<bool> AddExpenseAsync(Expense model);
    public Task<bool> DeleteExpenseAsync(int expenseId);
    public Task<int> AddExpenseCategoryAsync(string categoryName);
    public Task<IEnumerable<Expense>?> GetAllExpenseRecordsAsync(string userId);
    public Task<IEnumerable<Category>?> GetAllExpenseCategoryRecordsAsync(string userId);
    public Task<bool> UpdateExpenseAsync(Expense expenseDto);
    public Task<bool> UpdateCategoryAsync(string categoryName);
}
