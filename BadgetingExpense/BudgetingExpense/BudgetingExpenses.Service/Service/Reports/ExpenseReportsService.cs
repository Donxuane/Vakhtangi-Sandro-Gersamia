using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Reports;

public class ExpenseReportsService : IExpenseReportsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExpenseReportsService> _logger;
    private readonly IConfiguration _configuration;

    public ExpenseReportsService(IUnitOfWork unitOfWork, ILogger<ExpenseReportsService> logger,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<IEnumerable<ExpenseRecord>?> BiggestExpensesBasedPeriodAsync(RecordsPeriod model)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.ExpenseRecordsAsync(model.UserId);
            if (records != null)
            {
                var check = int.TryParse(_configuration.GetSection("TopExpenseAmounts")["Amount"], out int amount);
                var period = DateTime.Now.AddMonths(-model.Period);
                var final = records.Where(x => x.Date >= period && x.Currency == model.Currency)
                    .OrderByDescending(x => x.Amount).Take(check ? amount : 10).ToList();
                return final;
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }

    public async Task<(IEnumerable<ExpenseRecord>? records, int pageAmount)?> GetAllRecordsAsync(string userId, int page)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.AllExpenseRecordsAsync(userId, page);
            if (records.Value.records != null && records.Value.records.Any())
            {
                return records;
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<ExpenseRecord>?> RecordsBasedCategoryPeriodAsync(RecordCategory model)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.ExpenseRecordsAsync(model.UserId);
            if (records != null)
            {
                var period = DateTime.Now.AddMonths(-model.Period);
                if (model.Period == 0 && model.Category != null)
                {
                    return records.Where(x => x.CategoryName == model.Category).OrderByDescending(x => x.Date);
                }
                return records.Where(x => x.CategoryName == model.Category && x.Date >= period).OrderByDescending(x => x.Date);
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<ExpenseRecord>?> RecordsBasedCurrencyPeriodAsync(RecordCurrency model)
    {
        try
        {
            var records = await _unitOfWork.ExpenseRecords.ExpenseRecordsAsync(model.UserId);
            if (records != null)
            {
                var period = DateTime.Now.AddMonths(-model.Period);
                if (model.Currency > 0 && model.Period == 0)
                {
                    return records.Where(x => x.Currency == model.Currency).OrderByDescending(x => x.Date);
                }
                return records.Where(x => x.Currency == model.Currency && x.Date >= period).OrderByDescending(x => x.Date);
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
       
      
    }
}
