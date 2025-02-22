using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models;

public class ExpenseLimits
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    [Required]
    public double Amount { get; set; }
    [Required]
    public int PeriodCategory {  get; set; }
    [Required]
    public DateTime DateAdded {  get; set; }
}
