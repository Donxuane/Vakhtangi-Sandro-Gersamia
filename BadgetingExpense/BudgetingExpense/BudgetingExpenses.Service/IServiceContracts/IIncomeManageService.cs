using BudgetingExpense.Domain.Models;
using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpenses.Service.IServiceContracts;

public interface IIncomeManageService
{
    public Task<bool> AddIncomeType(IncomeTypeDTO model);
    public Task<bool> DeleteIncomeType(int incomeTypeId);
    public Task<bool> AddIncomeCategory(string CategoryName);
    public Task<IEnumerable<Income>?> GetAllIncomeRecords(string userId);
    public Task<IEnumerable<Category>?> GetAllIncomeCategoryRecords(string userId); 
}
