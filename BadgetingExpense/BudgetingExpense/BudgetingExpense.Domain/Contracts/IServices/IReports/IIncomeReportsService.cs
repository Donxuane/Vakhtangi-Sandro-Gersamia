using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports;

public interface IIncomeReportsService
{
    public Task<IEnumerable<IncomeRecord>?> RecordsBasedCurrencyPeriod(GetRecordCurrency model);
    public Task<IEnumerable<IncomeRecord>?> RecordsBasedCategoryPeriod(GetRecordCategory model);
    public Task<IEnumerable<IncomeRecord>?> GetAllRecords(string userId);
}
