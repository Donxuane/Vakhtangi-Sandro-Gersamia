using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IReports;

public interface IIncomeRecordsRepository
{
    public Task<IEnumerable<IncomeRecord>> GetUserIncomeRecords(string userId);
}
