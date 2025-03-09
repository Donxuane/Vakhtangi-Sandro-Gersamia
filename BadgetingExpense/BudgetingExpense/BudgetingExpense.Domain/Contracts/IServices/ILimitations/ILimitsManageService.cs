using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.ILimitations;

public interface ILimitsManageService
{
    Task<bool> SetLimits(Limits limits);
    Task<bool> DeleteLimits(int LimitId);
    Task<bool> UpdateLimits(Limits limits);
}
