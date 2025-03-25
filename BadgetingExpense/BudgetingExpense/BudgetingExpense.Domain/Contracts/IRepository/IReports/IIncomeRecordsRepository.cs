using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IReports;

public interface IIncomeRecordsRepository
{
    public Task<IEnumerable<IncomeRecord>> IncomeRecordsAsync(string userId);
    public Task<(IEnumerable<IncomeRecord>, int pageAmount)?> AllIncomeRecordsAsync(string userId, int page);
}
