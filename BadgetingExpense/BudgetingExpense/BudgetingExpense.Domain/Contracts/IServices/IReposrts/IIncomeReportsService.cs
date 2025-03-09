using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReposrts;

public interface IIncomeReportsService
{
    public Task<IEnumerable<IncomeRecord>?> RecordsBasedCurrecncyPeriod(GetRecordCurrency model);
    public Task<IEnumerable<IncomeRecord>?> RecordsBasedCategoryPeriod(GetRecordCategory model);
    public Task<IEnumerable<IncomeRecord>?> GetAllRecords(string userId);
}
