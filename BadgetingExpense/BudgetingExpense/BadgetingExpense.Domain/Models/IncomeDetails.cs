using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Models
{
  public class IncomeDetails
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal IncomeAmount { get; set; }
        [Required]
        public DateOnly IncomeDate { get; set; }

    }
}
