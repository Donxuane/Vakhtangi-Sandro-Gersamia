using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models.ApiModels;
using Microsoft.Extensions.Configuration;

namespace BudgetingExpenses.Service.Service.ApiService
{
  public class CurrencyRateService
    {
        private readonly IConfiguration _configuration;

        public CurrencyRateService(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        public async Task<List<CurrenciesRate>> GetCurrencies()
        {
            var url = _configuration.GetSection("CurrencyRate")["Url"];
            using HttpClient client = new HttpClient();
            string json = await client.GetStringAsync(url);

            var data = JsonSerializer.Deserialize<List<CurrenciesRate>>(json);


            return data;
        } 
        
    }
}
