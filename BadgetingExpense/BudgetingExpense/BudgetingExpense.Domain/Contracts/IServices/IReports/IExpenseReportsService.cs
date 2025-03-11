using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports;

public interface IExpenseReportsService
{
    public Task<IEnumerable<ExpenseRecord>?> RecordsBasedCurrencyPeriodAsync(GetRecordCurrency model);
    public Task<IEnumerable<ExpenseRecord>?> RecordsBasedCategoryPeriodAsync(GetRecordCategory model);
    public Task<IEnumerable<ExpenseRecord>?> BiggestExpensesBasedPeriodAsync(GetRecordsPeriod model);
    public Task<IEnumerable<ExpenseRecord>?> GetAllRecordsAsync(string userId);
}