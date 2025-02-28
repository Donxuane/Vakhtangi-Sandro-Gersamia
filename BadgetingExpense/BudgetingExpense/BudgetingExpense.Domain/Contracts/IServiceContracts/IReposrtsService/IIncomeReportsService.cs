using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsService;

public interface IIncomeReportsService
{
    public Task<IncomeRecords> RecordsBasedCurrecncyPeriod(int currency, int period);
}
 