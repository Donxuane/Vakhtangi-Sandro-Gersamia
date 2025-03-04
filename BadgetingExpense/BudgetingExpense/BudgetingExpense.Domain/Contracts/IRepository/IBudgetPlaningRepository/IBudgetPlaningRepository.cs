using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Contracts.IRepository.IBudgetPlaningRepository
{
    public interface IBudgetPlaningRepository
    {
        public Task GetAll();
    }
}
