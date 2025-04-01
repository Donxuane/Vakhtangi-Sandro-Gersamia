using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.ILimitations;

public interface ILimitsManageService
{
    public Task<bool> SetLimitsAsync(Limits limits);
    public Task<bool> DeleteLimitsAsync(int LimitId, string userId);
    public Task<bool> UpdateLimitsAsync(Limits limits);
    public Task<IEnumerable<(LimitationsView,string)>> GetLimitsDetails(string userId);
}
