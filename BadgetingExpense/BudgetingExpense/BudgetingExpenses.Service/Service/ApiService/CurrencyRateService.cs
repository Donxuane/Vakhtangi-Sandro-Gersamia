using System.Text.Json;
using BudgetingExpense.Domain.Contracts.IServices.IFinanceManage;
using BudgetingExpense.Domain.Models.ApiModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace BudgetingExpenses.Service.Service.ApiService;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly IMemoryCache _memoryCache;
    public CurrencyRateService(IConfiguration configuration,IMemoryCache memoryCache)
    {
        _memoryCache  = memoryCache;
    }

  private  async Task<List<CurrenciesRate>> GetCurrencies()
    {
        var url = _configuration.GetSection("CurrencyRate")["Url"];
        using HttpClient client = new HttpClient();
        string json = await client.GetStringAsync(url);

        var data = JsonSerializer.Deserialize<List<CurrenciesRate>>(json);


        return data;
    }
  
    public async Task<Dictionary<string  ,decimal>> GetCurrencyRates()
    {
        try
        {
         string key = "Currencies";
        var time  = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(2));
       
        if (!_memoryCache.TryGetValue(key, out Dictionary<string,decimal> Currencies))
        {
            Dictionary<string,decimal> collection = new Dictionary<string,decimal>();
          var getCurrencies = await  GetCurrencies();
          foreach (var item in getCurrencies)
          {
              foreach (var VARIABLE in item.Currencies)
              {
                  collection.Add(VARIABLE.Code,VARIABLE.Rate);
              }

          }
          _memoryCache.Set(key, collection);
        }

        var result =  _memoryCache.TryGetValue(key, out Dictionary<string, decimal> currenciesValue);  
        return currenciesValue;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            throw;
        }
    } 
}
