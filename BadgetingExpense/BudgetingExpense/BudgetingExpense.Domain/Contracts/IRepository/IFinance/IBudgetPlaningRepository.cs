using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IFinance;

public interface IBudgetPlaningRepository
{
    public Task<IEnumerable<BudgetPlanning>> GetAll(string UserId, int CategoryId);
    public Task<string> GetEmail(string UserId);
}
