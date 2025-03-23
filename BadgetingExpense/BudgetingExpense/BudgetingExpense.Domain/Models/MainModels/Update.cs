using BudgetingExpense.Domain.CustomAttributes;
using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.MainModels;

public class Update
{
    public int Id { get; set; }
    public Currencies? Currency { get; set; }
    public double? Amount { get; set; }
    public int? CategoryId { get; set; }
    public DateTime? Date { get; set; }
    public string UserId { get; set; }
}
