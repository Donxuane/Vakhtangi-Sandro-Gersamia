using BudgetingExpense.Domain.Models.MainModels;
using System.Data.Common;

namespace BudgetingExpense.Domain.Contracts.IRepository.ILimitations;

public interface IBudgetLimitsRepository
{
    public Task SetLimitAsync(Limits limits);
    public void SetTransaction(DbTransaction transaction);
    public Task DeleteLimitsAsync(int id);
    public Task UpdateLimitsAsync(Limits limits);
}
