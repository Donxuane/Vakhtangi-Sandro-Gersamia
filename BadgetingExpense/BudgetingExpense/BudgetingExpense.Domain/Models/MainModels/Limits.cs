using BudgetingExpense.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.MainModels;

public class Limits
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public double Amount { get; set; }
    [Required]
    public int PeriodCategory { get; set; }
    [Required]
    public DateTime DateAdded { get; set; }
    public Currencies Currency { get; set; }
}
