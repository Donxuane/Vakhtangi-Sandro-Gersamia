using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpenses.Service.DtoModels
{
    public class UpdateLimitDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public double Amount { get; set; }
        public int PeriodCategory { get; set; }

     

    }
}
