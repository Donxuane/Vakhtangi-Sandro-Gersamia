using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IFinance;

public interface IBudgetPlaningRepository
{
    public Task<IEnumerable<BudgetPlanning>> GetAllAsync(string UserId, int CategoryId);
    public Task<string> GetEmailAsync(string UserId);
}
