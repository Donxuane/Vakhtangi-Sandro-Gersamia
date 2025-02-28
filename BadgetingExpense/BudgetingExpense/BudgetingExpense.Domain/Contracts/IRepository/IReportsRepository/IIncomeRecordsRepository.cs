using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IReportsRepository;

public interface IIncomeRecordsRepository
{
    public Task<IEnumerable<IncomeRecords>> GetUserIncomeRecords(string userId);
}
