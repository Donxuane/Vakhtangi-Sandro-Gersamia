using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports;

public interface IIncomeReportsService
{
    public Task<IEnumerable<IncomeRecord>?> RecordsBasedCurrencyPeriodAsync(RecordCurrency model);
    public Task<IEnumerable<IncomeRecord>?> RecordsBasedCategoryPeriodAsync(RecordCategory model);
    public Task<IEnumerable<IncomeRecord>?> GetAllRecordsAsync(string userId);
}
