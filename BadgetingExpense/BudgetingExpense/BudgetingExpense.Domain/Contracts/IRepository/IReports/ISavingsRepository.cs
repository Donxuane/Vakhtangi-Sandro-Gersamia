using BudgetingExpense.Domain.Models.GetModel.Reports;
using System.Data.Common;

namespace BudgetingExpense.Domain.Contracts.IRepository.IReports;

public interface ISavingsRepository
{
    public Task<IEnumerable<SavingsPeriod>> GetSavingsAnalyticsAsync(string userId, int? period);
    public void SetTransaction(DbTransaction transaction);
}
