using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Reports;

public class ExpenseForecastService : IForecastService<ExpenseRecord>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpenseForecastService> _logger;

    public ExpenseForecastService(IUnitOfWork unitOfWork, ILogger<ExpenseForecastService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<IEnumerable<GetForecastCategory>> GetForecastCategoriesAsync(string userId)
    {
        try
        {
            var expenseRecords = await _unitOfWork.ExpenseRecords.GetUserExpenseRecordsAsync(userId);
            var filteredByCurrency = expenseRecords.DistinctBy(x => x.Currency).Select(x => new { x.Currency });
            var filteredByCategory = expenseRecords.DistinctBy(x => x.CategoryName).Select(x => new {x.CategoryName});
            List<GetForecastCategory> model = [];
            foreach (var currency in filteredByCurrency)
            {
                foreach (var category in filteredByCategory)
                {
                    var count = expenseRecords.Count(x => x.Currency == currency.Currency && x.CategoryName == category.CategoryName);
                    var amount = expenseRecords.Where(x => x.Currency == currency.Currency && x.CategoryName == category.CategoryName).Sum(x => x.Amount);
                    if (count > 0 && amount > 0)
                    {
                    model.Add(new GetForecastCategory()
                    {
                        Expected = Math.Round(amount / count,2),
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
