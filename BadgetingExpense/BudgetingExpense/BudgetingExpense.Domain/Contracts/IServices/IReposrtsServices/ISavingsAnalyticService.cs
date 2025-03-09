using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.IReposrtsServices
{
    public interface ISavingsAnalyticService
    {
        public Task<double> SavingsAnalyticByPeriodAsync(double expense, double income);
        public Task<double> SavingPercentageAsync(double expense,double income);
        Task<(double expense, double income)> FinanceRecords(string userId, int month);
    }
}
