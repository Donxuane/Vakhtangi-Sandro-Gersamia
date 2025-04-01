using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.MainModels;
using System.Data.Common;

namespace BudgetingExpense.Domain.Contracts.IRepository.ILimitations;

public interface IBudgetLimitsRepository
{
    public Task AddLimitAsync(Limits limits);
    public void SetTransaction(DbTransaction transaction);
    public Task DeleteLimitsAsync(int id, string userId);
    public Task UpdateLimitsAsync(Limits limits);
}
