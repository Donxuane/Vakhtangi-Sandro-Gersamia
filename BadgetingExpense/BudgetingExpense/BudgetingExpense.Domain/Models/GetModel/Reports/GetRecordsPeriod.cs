using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models.GetModel.Reports;

public class GetRecordsPeriod
{
    [Required]
    public string UserId {  get; set; }
    [Required]
    public int Period {  get; set; }
}
