using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsServices;

public interface IExpenseReportsService
{
    public Task<IEnumerable<ExpenseRecord>?> RecordsBasedCurrencyPeriod(GetRecordCurrency model);
    public Task<IEnumerable<ExpenseRecord>?> RecordsBasedCategoryPeriod(GetRecordCategory model);
    public Task<IEnumerable<ExpenseRecord>?> BiggestExpensesBasedPeriod(GetRecordsPeriod model);
    public Task<IEnumerable<ExpenseRecord>?> GetAllRecords(string userId);
}