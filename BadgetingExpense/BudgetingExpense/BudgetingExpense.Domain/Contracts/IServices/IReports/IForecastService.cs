using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports;

public interface IForecastService
{
    public Task<IEnumerable<GetForecastCategory>> GetForecastCategoriesAsync( string userId);
}
