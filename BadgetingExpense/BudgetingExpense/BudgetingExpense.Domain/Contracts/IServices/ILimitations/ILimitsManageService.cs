using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.ILimitations;

public interface ILimitsManageService
{
    Task<bool> SetLimitsAsync(Limits limits);
    Task<bool> DeleteLimitsAsync(int LimitId);
    Task<bool> UpdateLimitsAsync(Limits limits);
}
