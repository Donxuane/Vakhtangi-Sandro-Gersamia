using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models.DatabaseViewModels;

namespace BudgetingExpense.Domain.Contracts.IRepository.IBudgetPlaningRepository
{
    public interface IBudgetPlaningRepository
    {
        public Task<IEnumerable<BudgetPlanning>> GetAll(string UserId,int CategoryId);

        public Task<string> GetEmail(string UserId);
        

     
    }
}
