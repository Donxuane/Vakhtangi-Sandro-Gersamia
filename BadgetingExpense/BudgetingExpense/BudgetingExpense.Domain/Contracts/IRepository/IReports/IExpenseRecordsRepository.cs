using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IReports;

public interface IExpenseRecordsRepository
{
    public Task<IEnumerable<ExpenseRecord>> ExpenseRecordsAsync(string userId);
    public Task<(IEnumerable<ExpenseRecord> records,int pageAmount)?> AllExpenseRecordsAsync(string userId, int page);
}