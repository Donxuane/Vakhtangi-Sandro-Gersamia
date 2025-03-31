using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Models.ApiModels
{
    
  public class CurrenciesModel
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }

}

