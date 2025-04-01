using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports;

public interface ISavingsAnalyticService
{
    public Task<(IEnumerable<SavingsPeriod>,SavingsPeriod)>? GetSavingsAnalyticsAsync(string userId, int? month);
}
