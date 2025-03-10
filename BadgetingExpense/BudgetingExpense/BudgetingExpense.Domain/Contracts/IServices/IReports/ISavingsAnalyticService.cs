namespace BudgetingExpense.Domain.Contracts.IServices.IReports;

public interface ISavingsAnalyticService
{
    public Task<double> SavingsAnalyticByPeriodAsync(double expense, double income);
    public Task<double> SavingPercentageAsync(double expense, double income);
    Task<(double expense, double income)> FinanceRecords(string userId, int month);
}
