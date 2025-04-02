using System.Text.Json.Serialization;

namespace BudgetingExpense.Domain.Models.ApiModels;

public class CurrenciesRate
{
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    [JsonPropertyName("currencies")]
    public List<CurrenciesModel> Currencies { get; set; }

}
