using System.Text.Json;
using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Models.ApiModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.ApiService;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CurrencyRateService> _logger;
    public CurrencyRateService(IMemoryCache memoryCache, ILogger<CurrencyRateService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _memoryCache  = memoryCache;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    private async Task<List<CurrenciesRate>>? GetCurrencies()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Api");
            var jsonData = await client.GetStringAsync(client.BaseAddress);
            var data = JsonSerializer.Deserialize<List<CurrenciesRate>>(jsonData);
            return data;
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }
  
    public async Task<Dictionary<string  ,decimal>> GetCurrencyRates()
    {
        try
        {
            string key = "Currencies";
            var time = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(2));
            if (!_memoryCache.TryGetValue(key, out Dictionary<string, decimal> currencies))
            {
                var getCurrencies = await GetCurrencies();
                var collection = getCurrencies.SelectMany(x => x.Currencies)
                    .ToDictionary(x => x.Code, x => x.Rate);
                _memoryCache.Set(key, collection);
            }
            var result = _memoryCache.TryGetValue(key, out Dictionary<string, decimal> currenciesValue);
            return currenciesValue;
        }
        catch (Exception e)
        {
            _logger.LogError("Exception ex:{ex}",e.Message);
            throw;
        }
    } 
}
