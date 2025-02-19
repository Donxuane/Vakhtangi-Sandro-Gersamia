using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models
{
   public class UserExpenses
    {

        [Required]

public int Id { get; set; }
        [Required]
        [MaxLength(10)]
public string  Currency{ get; set; }
        [Required]
        [MaxLength(50)]
public string ExpenseType { get; set; }

    }
