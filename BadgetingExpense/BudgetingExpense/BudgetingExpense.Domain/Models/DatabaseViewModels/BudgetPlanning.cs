using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Models.DatabaseViewModels
{
    public class BudgetPlanning
    {
        public string UserId { get; set; }

        public double limitAmount { get; set; }
        public int LimitPeriod { get; set; }
        public DateTime DateAdded { get; set; }
        public double ExpenseAmount { get; set; }
        public int Currency { get; set; }
        public DateTime Date { get; set; }

    }
}
