using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
