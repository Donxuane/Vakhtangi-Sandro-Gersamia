using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports;

public interface IForecastService<T> where T : class
{
    public Task<IEnumerable<ForecastCategory>> GetForecastCategoriesAsync( string userId);
}
