using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models;

public class IncomeDetails
{
    [Key]
    public int Id { get; set; }
    [Required]
    public decimal IncomeAmount { get; set; }
    [Required]
    public DateOnly IncomeDate { get; set; }
    [Required]
    public int UserIncomeId { get; set; }
}
