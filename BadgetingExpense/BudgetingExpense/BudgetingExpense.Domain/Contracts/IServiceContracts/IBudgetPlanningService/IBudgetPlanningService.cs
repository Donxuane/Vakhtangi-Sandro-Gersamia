using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.IBudgetPlanningService
{
    public interface IBudgetPlanningService
    {
        public Task SendMessageAsync(string UserId);
        public Task AllExpenses(string UserId,int CategoryId);
    }
}
