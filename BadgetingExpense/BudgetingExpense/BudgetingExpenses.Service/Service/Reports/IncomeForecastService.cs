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
    public async Task<IEnumerable<GetForecastCategory>> GetForecastCategoriesAsync(string userId)
    {
        try
        {
            var incomeRecords = await _unitOfWork.IncomeRecords.GetUserIncomeRecordsAsync(userId);
            var filteredByCurrency = incomeRecords.DistinctBy(x => x.Currency).Select(x => new { x.Currency });
            var filteredByCategory = incomeRecords.DistinctBy(x => x.CategoryName).Select(x => new { x.CategoryName });
            List<GetForecastCategory> model = [];
            foreach (var currency in filteredByCurrency)
            {
                foreach (var category in filteredByCategory)
                {
                    var count = incomeRecords.Count(x => x.Currency == currency.Currency && x.CategoryName == category.CategoryName);
                    var amount = incomeRecords.Where(x => x.Currency == currency.Currency && x.CategoryName == category.CategoryName).Sum(x => x.Amount);
                    if (count > 0 && amount > 0)
                    {
                        model.Add(new GetForecastCategory
                        {
                            Expected = Math.Round(amount / count, 2),
                            CategoryName = category.CategoryName,
                            Currency = currency.Currency.ToString()
                        });
                    }
                }
            }
            return model;
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return null;
        }
    }
}
