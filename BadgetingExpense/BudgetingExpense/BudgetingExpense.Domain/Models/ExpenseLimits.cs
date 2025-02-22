using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models;

public class ExpenseLimits
{
 
    [Key]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public int LimitPeriod { get; set; }
    [Required]
    public string LimitExpenseType { get; set;}
    [Required]
    public decimal LimitAmount { get; set;}
}
