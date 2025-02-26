using BudgetingExpense.Domain.Models;
using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpenses.Service.IServiceContracts;

public interface IExpenseManageService
{
    public Task<bool> AddExpenseAsync(ExpenseDto model);
    public Task<bool> DeleteExpenseAsync(int expenseId);
    public Task<int> AddExpenseCategoryAsync(string categoryName);
    public Task<IEnumerable<Expense>?> GetAllExpenseRecordsAsync(string userId);
    public Task<IEnumerable<Category>?> GetAllExpenseCategoryRecordsAsync(string userId);
    public Task<bool> UpdateExpenseAsync(UpdateExpenseDto expenseDto);
    public Task<bool> UpdateCategoryAsync(string categoryName);
}
