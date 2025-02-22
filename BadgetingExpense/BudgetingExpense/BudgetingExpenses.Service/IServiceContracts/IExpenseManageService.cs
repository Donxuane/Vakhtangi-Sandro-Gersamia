using BudgetingExpense.Domain.Models;
using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpenses.Service.IServiceContracts;

public interface IExpenseManageService
{
    public Task<bool> AddExpenseAsync(ExpenseDto model);
    public Task<bool> DeleteExpenseAsync(int expenseId);
    public Task<bool> AddExpenseCategoryAsync(string categoryName);
    public Task<IEnumerable<Income>?> GetAllExpenseRecordsAsync(string userId);
    public Task<IEnumerable<Category>?> GetAllExpenseCategoryRecordsAsync(string userId);
}
