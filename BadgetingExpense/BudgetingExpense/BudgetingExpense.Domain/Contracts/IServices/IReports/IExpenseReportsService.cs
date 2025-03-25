using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports;

public interface IExpenseReportsService
{
    public Task<IEnumerable<ExpenseRecord>?> RecordsBasedCurrencyPeriodAsync(RecordCurrency model);
    public Task<IEnumerable<ExpenseRecord>?> RecordsBasedCategoryPeriodAsync(RecordCategory model);
    public Task<IEnumerable<ExpenseRecord>?> BiggestExpensesBasedPeriodAsync(RecordsPeriod model);
    public Task<(IEnumerable<ExpenseRecord>? records, int pageAmount)?> GetAllRecordsAsync(string userId, int page);
}