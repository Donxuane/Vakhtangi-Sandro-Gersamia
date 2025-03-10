using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports
{
    public interface IIncomeForecastService
    {
        public Task<IEnumerable<GetForecastCategory>> GetForecastCategoriesAsync( string userId);


    }
}
