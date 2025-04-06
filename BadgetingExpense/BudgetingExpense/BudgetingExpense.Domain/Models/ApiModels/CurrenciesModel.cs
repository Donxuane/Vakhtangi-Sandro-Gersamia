using System.Text.Json.Serialization;

namespace BudgetingExpense.Domain.Models.ApiModels;


public class CurrenciesModel
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
}

