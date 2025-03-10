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
   
        public double Expected { get; set; }
        public Currencies Currency { get; set; }
        public string CategoryName {get; set; }

    }
}
