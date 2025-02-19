using System.ComponentModel.DataAnnotations;

namespace BudgetingExpense.Domain.Models
{
    public class UserIncome
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Currency{ get; set; }
        [Required]
        [MaxLength(20)]
        public string IncomeType { get; set; }
}
