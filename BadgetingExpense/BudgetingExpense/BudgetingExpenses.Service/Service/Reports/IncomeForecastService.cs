using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace BudgetingExpenses.Service.Service.Reports;

public class IncomeForecastService : IForecastService<IncomeRecord>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<IncomeForecastService> _logger;
    private readonly IConfiguration _configuration;

    public IncomeForecastService(IUnitOfWork unitOfWork, ILogger<IncomeForecastService> logger,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _configuration = configuration;
    }
    public async Task<IEnumerable<ForecastCategory>?> GetForecastCategoriesAsync(string userId)
    {
        try
        {
            var count = _configuration.GetSection("ConfigureForcastCounts")["IncomeForecastCount"];
            var amount = int.TryParse(count, out int number);
            var incomeRecords = await _unitOfWork.IncomeRecords.IncomeRecordsAsync(userId);
            var model = incomeRecords.GroupBy(x => new { x.Currency, x.CategoryName })
                .Where(x => x.Count() >= number)
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
