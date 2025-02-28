using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsService;

public interface IIncomeReportsService
{
    public Task<IEnumerable<IncomeRecord>?> RecordsBasedCurrecncyPeriod(GetIncomeRecord model);
}
