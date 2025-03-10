using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IServices.IReposrts;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.GetModel.Reports;

namespace BudgetingExpenses.Service.Service.Reports
{
   public class IncomeForecastService : IIncomeForecastService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncomeForecastService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<GetForecastCategory>> GetForecastCategoriesAsync(string userId)
        {
            var incomeRecords = await _unitOfWork.IncomeRecords.GetUserIncomeRecords(userId);
            var filteredByCurrency = incomeRecords.DistinctBy(x => x.Currency);
            var filteredByCategory = incomeRecords.DistinctBy(x => x.CategoryName);
            var finalFilter = incomeRecords;
            foreach (var incomeRecord in incomeRecords)
            {
              
            }
        }
    }
}
