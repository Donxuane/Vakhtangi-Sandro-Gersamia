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
            var savingsInLocalCurrency = records.Select(x =>
            {
                var currencyRate = currencies
                .ContainsKey(x.Currency.ToString()) ? (double)currencies[x.Currency.ToString()] : 1.0;
                return new
                {
                    AverageIncome = Math.Round((x.AverageIncome * currencyRate), 2),
                    AverageExpense = Math.Round((x.AverageExpense * currencyRate), 2),
                    Percentage = Math.Round((x.Percentage / records.Count()), 2)
                };
            }).Aggregate(new SavingsPeriod { Currency = Currencies.GEL }, (savingsPeriod, y) =>
            {
                savingsPeriod.AverageIncome += y.AverageIncome;
                savingsPeriod.AverageExpense += y.AverageExpense;
                savingsPeriod.Percentage += y.Percentage / records.Count();
                return savingsPeriod;
            });
            savingsInLocalCurrency.Savings = savingsInLocalCurrency.AverageIncome - savingsInLocalCurrency.AverageExpense;
            return (records, savingsInLocalCurrency);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            await _unitOfWork.RollBackAsync();
            throw;
        }
    }
}
