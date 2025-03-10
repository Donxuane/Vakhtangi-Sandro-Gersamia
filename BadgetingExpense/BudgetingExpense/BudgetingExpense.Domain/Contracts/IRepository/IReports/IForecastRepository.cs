using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Contracts.IRepository.IReports
{
    public interface IForecastRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll(string userId);
    }
}
