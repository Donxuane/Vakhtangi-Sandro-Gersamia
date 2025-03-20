using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Reports;

public class IncomeForecastService : IForecastService<IncomeRecord>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<IncomeForecastService> _logger;

    public IncomeForecastService(IUnitOfWork unitOfWork, ILogger<IncomeForecastService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<IEnumerable<ForecastCategory>?> GetForecastCategoriesAsync(string userId)
    {
        try
        {
            var incomeRecords = await _unitOfWork.IncomeRecords.GetUserIncomeRecordsAsync(userId);
            var model = incomeRecords.GroupBy(x => new { x.Currency, x.CategoryName })
                .Select(x => new ForecastCategory
                {
                    CategoryName = x.Key.CategoryName,
                    Currency = x.Key.Currency.ToString(),
                    Expected = Math.Round(x.Sum(z => z.Amount) / x.Count(), 2)
                }).Where(x => x.Expected > 0);
            return model;
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return null;
        }
    }
}
