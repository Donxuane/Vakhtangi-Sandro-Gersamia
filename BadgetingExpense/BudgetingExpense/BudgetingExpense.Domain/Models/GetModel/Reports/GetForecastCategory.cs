using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Enums;

namespace BudgetingExpense.Domain.Models.GetModel.Reports
{
    public class GetForecastCategory
    {
   
        public decimal Amount { get; set; }
        public Currencies Currencies { get; set; }
        public string CategoryName {get; set; }

    }
}
