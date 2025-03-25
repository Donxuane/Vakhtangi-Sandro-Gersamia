using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Reports;

public class ExpenseForecastService : IForecastService<ExpenseRecord>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpenseForecastService> _logger;
    private readonly IConfiguration _configuration;

    public ExpenseForecastService(IUnitOfWork unitOfWork, ILogger<ExpenseForecastService> logger,
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
            var count = _configuration.GetSection("ConfigureForcastCounts")["ExpenseForecastCount"];
            var amount = int.TryParse(count, out int number);
            var expenseRecords = await _unitOfWork.ExpenseRecords.ExpenseRecordsAsync(userId);
            var model = expenseRecords.GroupBy(x => new { x.Currency, x.CategoryName })
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
