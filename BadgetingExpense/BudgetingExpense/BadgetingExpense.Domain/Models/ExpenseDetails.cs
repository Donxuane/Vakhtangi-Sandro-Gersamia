using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Models
{
    public class ExpenseDetails
    {
        [Required]
        public int Id { get; set; }
        [Required]
        
        public decimal ExpenseAmount { get; set; }
        [Required]
        public DateOnly ExpenseDate { get; set; }

    }
}
