using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.DatabaseViewModels;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Reports;

public class IncomeReportsService : IIncomeReportsService
{
    private readonly IUnitOfWork _unitOfWork; 
    private readonly ILogger<IncomeReportsService> _logger;

    public IncomeReportsService(IUnitOfWork unitOfWork, ILogger<IncomeReportsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<(IEnumerable<IncomeRecord> records, int pageAmount)?> GetAllRecordsAsync(string userId, int page)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.AllIncomeRecordsAsync(userId, page);
            if (records != null)
            {
                return records;
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<IncomeRecord>?> RecordsBasedCategoryPeriodAsync(RecordCategory model)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.IncomeRecordsAsync(model.UserId);
            if (records != null && records.Any())
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if (model.Period == 0 && model.Category != null)
                {
                    return records.Where(x => x.CategoryName == model.Category).OrderByDescending(x => x.IncomeDate);
                }
                return records.Where(x => x.CategoryName == model.Category && x.IncomeDate >= period).OrderByDescending(x => x.IncomeDate);
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<IncomeRecord>?> RecordsBasedCurrencyPeriodAsync(RecordCurrency model)
    {
        try
        {
            var records = await _unitOfWork.IncomeRecords.IncomeRecordsAsync(model.UserId);
            if (records != null)
            {
                var period = DateTime.UtcNow.AddMonths(-model.Period);
                if (model.Period == 0 && model.Currency >= 0)
                {
                    return records.Where(x => x.Currency == model.Currency).OrderByDescending(x => x.IncomeDate);
                }
                return records.Where(x => x.Currency == model.Currency && x.IncomeDate >= period).OrderByDescending(x => x.IncomeDate);
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
