using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.ILimitsManageService;

public interface ILimitsManageService
{
    Task<bool> SetLimits(Limits limits);
    Task <bool> DeleteLimits(int LimitId); 
    Task<bool> UpdateLimits(Limits limits);
}
