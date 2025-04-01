using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Contracts.IServices.IReports;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Enums;
using BudgetingExpense.Domain.Models.GetModel.Reports;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Reports;

public class SavingsAnalyticService : ISavingsAnalyticService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SavingsAnalyticService> _logger;
    private readonly ICurrencyRateService _currencyRateService;
    public SavingsAnalyticService(IUnitOfWork unitOfWork, ILogger<SavingsAnalyticService> logger,ICurrencyRateService currencyRateService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currencyRateService = currencyRateService;
    }
    public async Task<(IEnumerable<SavingsPeriod>,SavingsPeriod)>? GetSavingsAnalyticsAsync(string userId, int? month)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var records = await _unitOfWork.SavingsRepository.GetSavingsAnalyticsAsync(userId, month);
            
            await _unitOfWork.SaveChangesAsync();
            var currencies = await _currencyRateService.GetCurrencyRates();
            var savingPeriod = new SavingsPeriod();

            foreach (var item in records)
            {
                
                if (item.Currency.ToString() == currencies.Select(x => new{x.Key}).ToString())
                {
                    savingPeriod.AverageIncome += item.AverageIncome * currencies
                        .Where(X => X.Key.ToString() == item.Currency.ToString()).Select(x => new { x.Value });
                }
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            await _unitOfWork.RollBackAsync();
            throw;
        }
    }
}
