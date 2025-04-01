using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IGet
{
    public interface IBudgetPlaningRepository
    {
        public Task<IEnumerable<BudgetPlanning>> GetBudgetPlaningViewAsync(string userId);
    }
