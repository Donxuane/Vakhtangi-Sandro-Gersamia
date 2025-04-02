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
            var savingPeriod = new SavingsPeriod
            {
                Currency = Currencies.GEL
            };

            foreach (var item in records)
            {

                if (currencies.Any(x => x.Key == item.Currency.ToString()))
                {
                    var currencyRate = currencies.FirstOrDefault(x => x.Key == item.Currency.ToString()).Value;
                    savingPeriod.AverageIncome +=Math.Round( (item.AverageIncome * (double)currencyRate),2);
                    savingPeriod.AverageExpense +=Math.Round( (item.AverageExpense * (double)currencyRate),2);
                  
                }
                else
                {
                    savingPeriod.AverageIncome +=Math.Round( item.AverageIncome,2);
                    savingPeriod.AverageExpense +=Math.Round( item.AverageExpense,2);
                } 
                savingPeriod.Percentage += Math.Round(item.Percentage,2);
                
            }
            savingPeriod.Savings =  savingPeriod.AverageIncome - savingPeriod.AverageExpense;
            return (records, savingPeriod);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            await _unitOfWork.RollBackAsync();
            throw;
        }
    }
}
