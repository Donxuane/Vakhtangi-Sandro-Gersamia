using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;

public interface IIncomeManageService
{
    public Task<bool> AddIncomeAsync(Income model);
    public Task<bool> DeleteIncomeAsync(int incomeTypeId);
    public Task<int> AddIncomeCategoryAsync(string CategoryName);
    public Task<IEnumerable<Income>?> GetAllIncomeRecordsAsync(string userId);
    public Task<IEnumerable<Category>?> GetAllIncomeCategoryRecordsAsync(string userId);
    public Task<bool> UpdateIncomeAsync(Update income);
    public Task<bool> UpdateIncomeCategoryAsync(Category categoryDto);
}
