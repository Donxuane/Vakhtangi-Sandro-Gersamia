using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IGet
{
    public interface IBudgetPlaningRepository
    {
        public Task<IEnumerable<BudgetPlanning>> GetBudgetPlaningViewAsync(string userId);
    }
}
