using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IReports;

public interface IExpenseRecordsRepository
{
    public Task<IEnumerable<ExpenseRecord>> GetUserExpenseRecordsAsync(string userId);
}
