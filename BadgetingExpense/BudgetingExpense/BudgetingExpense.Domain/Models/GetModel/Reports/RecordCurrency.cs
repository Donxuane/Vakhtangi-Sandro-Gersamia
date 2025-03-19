using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.GetModel.Reports;

public class RecordCurrency
{
    public string UserId { get; set; }
    public int Period { get; set; }
    [Required]
    public Currencies Currency { get; set; }
}
