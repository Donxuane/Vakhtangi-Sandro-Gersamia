using BudgetingExpenses.Service.DtoModels;

namespace BudgetingExpenses.Service.IServiceContracts;

public interface IIncomeManageService
{
    public Task<bool> AddIncomeType(IncomeTypeDTO model);
    public Task<bool> DeleteIncomeType(int incomeTypeId);
}
