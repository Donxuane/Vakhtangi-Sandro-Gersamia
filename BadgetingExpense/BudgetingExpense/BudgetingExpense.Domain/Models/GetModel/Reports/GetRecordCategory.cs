using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.GetModel.Reports;

public class GetRecordCategory
{
    public string UserId { get; set; }
    [Required]
    public string Category { get; set; } 
    public int Period { get; set; }
}
