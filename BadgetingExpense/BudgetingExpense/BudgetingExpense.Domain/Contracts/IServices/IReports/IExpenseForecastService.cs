using BudgetingExpense.Domain.Models.GetModel.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Contracts.IServices.IReports
{
    public interface IExpenseForecastService
    {
        public Task<IEnumerable<GetForecastCategory>> GetForecastCategoriesAsync(string userId);
    }
}
