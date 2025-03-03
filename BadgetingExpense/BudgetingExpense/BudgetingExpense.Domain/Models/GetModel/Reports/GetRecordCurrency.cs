using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.GetModel.Reports;

public class GetRecordCurrency
{
    public string UserId { get; set; }
    public int Period { get; set; }
    [Required]
    public int Currency { get; set; }
}
