using BudgetingExpense.Domain.Models;
using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpenses.Service.IServiceContracts;

public interface IIncomeManageService
{
    public Task<bool> AddIncomeAsync(IncomeDto model);
    public Task<bool> DeleteIncomeAsync(int incomeTypeId);
    public Task<int> AddIncomeCategoryAsync(string CategoryName);
    public Task<IEnumerable<Income>?> GetAllIncomeRecordsAsync(string userId);
    public Task<IEnumerable<Category>?> GetAllIncomeCategoryRecordsAsync(string userId); 
}
