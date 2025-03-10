using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpense.Domain.Contracts.IServices.IReposrts
{
    public interface IIncomeForecastService
    {
        public Task<IEnumerable<GetForecastCategory>> GetForecastCategoriesAsync( string userId);


    }
}
