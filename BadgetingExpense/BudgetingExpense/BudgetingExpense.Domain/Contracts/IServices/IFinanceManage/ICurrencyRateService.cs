using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Contracts.IServices.IFinanceManage
{
    public interface ICurrencyRateService
    {
        public Task<Dictionary<string, decimal>> GetCurrencyRates();
    }
}
