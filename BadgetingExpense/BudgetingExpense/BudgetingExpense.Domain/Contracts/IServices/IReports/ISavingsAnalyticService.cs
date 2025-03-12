using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports;

public interface ISavingsAnalyticService
{
    public Task<Savings?> SavingsAnalyticsAsync(string userId, int month);
}
