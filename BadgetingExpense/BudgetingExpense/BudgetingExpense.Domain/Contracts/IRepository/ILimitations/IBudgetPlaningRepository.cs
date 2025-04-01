using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.ILimitations;

public interface IBudgetPlaningRepository
{
    public Task<IEnumerable<BudgetPlanning>> GetBudgetPlaningViewAsync(string userId);
    public Task<IEnumerable<LimitationsView>> GetLimitsInfo(string userId);
}